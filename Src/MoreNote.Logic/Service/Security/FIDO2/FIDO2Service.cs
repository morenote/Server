using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fido2NetLib;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Entity.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.Models.Entity.ConfigFile;
using MoreNote.Models.Model.FIDO2;

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
        private IDistributedCache cache;//缓存数据库

        public Fido2User GetFido2User(User user)
        {
            return null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="configFileService"></param>
        public FIDO2Service(DataContext dataContext, IDistributedCache distributedCache, ConfigFileService configFileService)
        {
            this.dataContext = dataContext;
            this.cache = distributedCache;
            this.config = configFileService.WebConfig;
            this.fido2Config = config.SecurityConfig.FIDO2Config;
            _fido2 = new Fido2(new Fido2Configuration()
            {
                ServerDomain = fido2Config.ServerDomain,
                ServerName = fido2Config.ServerName,
                Origin = fido2Config.Origin
            }, FIDO2Conformance.MetadataServiceInstance(
                System.IO.Path.Combine(fido2Config.MDSCacheDirPath, @"Conformance"), fido2Config.Origin));
        }

        /// <summary>
        /// <para>创建证明选项</para>
        /// <param name="opts"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public CredentialCreateOptions MakeCredentialOptions(MakeCredentialParams opts)
        {
            var attType = opts.Attestation;

            var username = Encoding.UTF8.GetBytes(opts.Username);

            var userId = opts.Username.ToLongByHex();

            // Get user from DB by username (in our example, auto create missing users)
            var user = dataContext.User.Where(u => u.UserId == userId).FirstOrDefault();
            if (user == null)
            {
                throw new Exception();
            }
            var fidoUser = GetFido2User(user);
            // Create options
            var options = _fido2.RequestNewCredential(fidoUser, new List<PublicKeyCredentialDescriptor>(), opts.AuthenticatorSelection, opts.Attestation);

            cache.SetString(user.UserId.ToString() + "attestationOptions", options.ToJson(), 120);
            return options;
        }

        /// <summary>
        /// 注册用户凭证
        /// <para>当客户端返回响应时，我们验证并注册凭据。</para>
        /// </summary>
        /// <param name="attestationResponse"></param>
        public async void MakeCredentialResult(long userId, AuthenticatorAttestationRawResponse attestationResponse)
        {
            // 1. get the options we sent the client
            var jsonOptions = cache.GetString(userId.ToString() + "attestationOptions");
            var options = CredentialCreateOptions.FromJson(jsonOptions);

            // 2. Create callback so that lib can verify credential id is unique to this user
            IsCredentialIdUniqueToUserAsyncDelegate callback = async (IsCredentialIdUniqueToUserParams args) =>
            {
                //var users = await DemoStorage.GetUsersByCredentialIdAsync(args.CredentialId);
                //if (users.Count > 0)
                //    return false;

                return true;
            };
            // 2. Verify and make the credentials
            var success = await _fido2.MakeNewCredentialAsync(attestationResponse, options, callback);
            // 3. Store the credentials in db


        }

        /// <summary>
        /// 创建断言选项
        /// <para> 当用户想要登录时，我们会根据注册的凭据进行断言。</para>
        /// </summary>
        public void AssertionOptions()
        {
        }

        /// <summary>
        /// 验证断言响应
        /// <para>当客户端返回响应时，我们对其进行验证并接受登录。</para>
        /// </summary>
        public void MakeCredentialParams()
        {
        }
    }
}