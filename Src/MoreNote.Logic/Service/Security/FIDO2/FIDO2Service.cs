using Fido2NetLib;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.ConfigFile;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.Models.Model.FIDO2;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Fido2NetLib.Fido2;

namespace MoreNote.Logic.Security.FIDO2.Service
{
	/// <summary>
	/// 无密码安全密钥登录服务
	/// </summary>
	public class FIDO2Service
	{
		private readonly IFido2 _fido2;

		private DataContext dataContext;
		private WebSiteConfig config;
		private FIDO2Config fido2Config;
		private IDistributedCache distributedCache;//分布式缓存
		private IDistributedIdGenerator IdGenerator;
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="dataContext"></param>
		/// <param name="configFileService"></param>
		public FIDO2Service(DataContext dataContext,
			IDistributedCache distributedCache,
			ConfigFileService configFileService,
			IFido2 fido2,
			IDistributedIdGenerator IdGenerator)
		{
			this.IdGenerator = IdGenerator;
			this.dataContext = dataContext;
			this.distributedCache = distributedCache;
			this.config = configFileService.WebConfig;
			this.fido2Config = config.SecurityConfig.FIDO2Config;
			this._fido2 = fido2;
		}



		/// <summary>
		/// <para>注册：创建证明选项</para>
		/// <param name="opts"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public CredentialCreateOptions MakeCredentialOptions(UserInfo user, MakeCredentialParams opts)
		{
			var fidoUser = opts.GetFido2UserByUser();
			// Create options
			var exts = new AuthenticationExtensionsClientInputs()
			{
				Extensions = true,
				UserVerificationMethod = true,
			};
			var existingKeys = GetPublicKeyCredentialDescriptors(user.Id);

			var options = _fido2.RequestNewCredential(
				fidoUser,
				existingKeys,
				opts.AuthenticatorSelection,
				opts.Attestation,
				exts);

			distributedCache.SetString(user.Id.ToString() + "attestationOptions", options.ToJson(), 120);
			return options;
		}

		public List<PublicKeyCredentialDescriptor> GetPublicKeyCredentialDescriptors(long? userId)
		{
			var existingCredentials = this.dataContext.Fido2Items
				.Where(b => b.UserId == userId)
				.Select(p => p.GetDescriptor()).ToList<PublicKeyCredentialDescriptor>();
			return existingCredentials;
		}

		public List<FIDO2Item> GetFIDO2ItemByUserId(long? userId)
		{
			var existingFIDO2Items = this.dataContext.Fido2Items
			   .Where(b => b.UserId == userId).ToList<FIDO2Item>();
			return existingFIDO2Items;
		}

		/// <summary>
		/// 注册：验证用户凭证
		/// <para>当客户端返回响应时，我们验证并注册凭据。</para>
		/// </summary>
		/// <param name="attestationResponse"></param>
		public async Task<CredentialMakeResult> RegisterCredentials(UserInfo user, string fido2Name, AuthenticatorAttestationRawResponse attestationResponse)
		{
			// 1. get the options we sent the client
			var jsonOptions = distributedCache.GetString(user.Id.ToString() + "attestationOptions");
			if (string.IsNullOrEmpty(jsonOptions))
			{
				return null;
			}
			var options = CredentialCreateOptions.FromJson(jsonOptions);

			// 2. Create callback so that lib can verify credential id is unique to this user
			IsCredentialIdUniqueToUserAsyncDelegate callback = async (args, cancellationToken) =>
			{
				//var users = await DemoStorage.GetUsersByCredentialIdAsync(args.CredentialId);
				//if (users.Count > 0)
				//    return false;
				var argUserId = args.CredentialId;
				if (this.dataContext.Fido2Items.Where(b => b.CredentialId.Equals(argUserId)).Any())
				{
					return false;
				}
				return true;
			};
			// 2. Verify and make the credentials
			var success = await _fido2.MakeNewCredentialAsync(attestationResponse, options, callback);
			// 3. Store the credentials in db
			//var user = dataContext.User.Where(b => b.UserId == userId).FirstOrDefault();
			if (user == null)
			{
				return null;
			}
			var fido2 = new FIDO2Item()
			{
				Id = IdGenerator.NextId(),
				UserId = user.Id,
				FIDO2Name = fido2Name,
				CredentialId = success.Result.CredentialId,
				PublicKey = success.Result.PublicKey,
				UserHandle = success.Result.User.Id,
				SignatureCounter = success.Result.Counter,
				CredType = success.Result.CredType,
				RegDate = DateTime.Now,
				AaGuid = success.Result.Aaguid
			};

			dataContext.Fido2Items.Add(fido2);

			dataContext.SaveChanges();

			return success;
		}

		/// <summary>
		/// 断言：创建断言选项
		/// <para> 当用户想要登录时，我们会根据注册的凭据进行断言。</para>
		/// </summary>
		public async Task<AssertionOptions> AssertionOptionsPost(UserInfo user, AssertionClientParams assertionClientParams)
		{
			string error = "";

			// 1. Get user from DB
			if (user == null)
			{
				error = "username was not registered";
				var ass = new AssertionOptions()
				{
					Status = "bad",
					ErrorMessage = error
				};
				return ass;
			}
			// 2. Get registered credentials from database

			var existingCredentials = GetPublicKeyCredentialDescriptors(user.Id);

			var options = _fido2.GetAssertionOptions(
				existingCredentials,
				assertionClientParams.UserVerification,
				assertionClientParams.Extensions
			);
			distributedCache.SetString(user.Id.ToString() + "assertionOptions", options.ToJson(), 120);

			return options;
		}

		/// <summary>
		/// 断言：验证断言响应
		/// <para>当客户端返回响应时，我们对其进行验证并接受登录。</para>
		/// </summary>
		public async Task<AssertionVerificationResult> MakeAssertionAsync(UserInfo user, AuthenticatorAssertionRawResponse clientRespons)
		{
			if (user == null)
			{
				return new AssertionVerificationResult()
				{
					Status = "bad"
				};
			}
			// 1. Get the assertion options we sent the client
			var jsonOptions = distributedCache.GetString(user.Id.ToString() + "assertionOptions");
			if (string.IsNullOrEmpty(jsonOptions))
			{
				return new AssertionVerificationResult()
				{
					Status = "fail",
					ErrorMessage = "time out"
				};
			}
			var options = AssertionOptions.FromJson(jsonOptions);
			// 2. Get registered credential from database
			var storedCredential = this.GetFIDO2ItemByUserId(user.Id);
			// 3. Get credential counter from database

			var creds = dataContext.Fido2Items.Where(b => b.CredentialId.Equals(clientRespons.Id)).FirstOrDefault();

			var storedCounter = creds.SignatureCounter;
			// 4. Create callback to check if userhandle owns the credentialId

			IsUserHandleOwnerOfCredentialIdAsync callback = async (args, cancellationToken) =>
			  {
				  return storedCredential.Exists(c => c.CredentialId.Equals(args.CredentialId));
			  };
			// 5. Make the assertion
			var res = await _fido2.MakeAssertionAsync(clientRespons, options, creds.PublicKey, storedCounter, callback);
			// 6. Store the updated counter
			creds.SignatureCounter = res.Counter;
			dataContext.SaveChanges();
			return res;
		}
	}
}