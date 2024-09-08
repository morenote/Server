
using github.hyfree.GM;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.ApiRequest;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote.User;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/User/[action]")]
	public class UserAPIController : APIBaseController
	{
		private AuthService authService;
		private UserService userService;
		private TokenSerivce tokenSerivce;
		
		private EPassService ePassService;
		private GMService gMService;
		private DataSignService dataSignService;

		public UserAPIController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, GMService gMService
			
			, IHttpContextAccessor accessor, AuthService authService
			, EPassService ePass
			, DataSignService dataSignService
		   ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.authService = authService;
			this.userService = userService;
			this.tokenSerivce = tokenSerivce;
		
			this.ePassService = ePass;
			this.gMService = gMService;
			this.dataSignService = dataSignService;
		}

		/// <summary>
		/// 通过token获得用户本人的详情
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetUserInfoByToken(string token)
		{
			var user = tokenSerivce.GetUserByToken(token);
			var re = new ApiResponseDTO();
			if (user == null)
			{
				re.Msg = "NOTLOGIN";
			}
			re.Ok = true;
			re.Data = user;
			return LeanoteJson(re);
		}
		/// <summary>
		/// 通过邮箱获得用户详情
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetUserInfoByEmail(string email)
		{
			var user = userService.GetUserByEmail(email);
			var re = new ApiResponseDTO();
			if (user == null)
			{
				re.Msg = "NOTLOGIN";
			}
			re.Ok = true;
			re.Data = user;
			return LeanoteJson(re);
		}
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetUserInfoByUserId(string userId)
		{
			var user = userService.GetUserByUserId(userId.ToLongByHex());
			var re = new ApiResponseDTO();
			if (user == null)
			{
				re.Msg = "NOTLOGIN";
			}
			re.Ok = true;
			re.Data = user;
			return LeanoteJson(re);
		}

		[HttpPost]
		public IActionResult UpdateUsername(string token, string username)
		{
			var re = new ApiResponseDTO();
			var message = string.Empty;

			if (string.IsNullOrEmpty(username) || username.Length < 4)
			{
				re.Msg = "用户名长度必须大于4";
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
			if (username.Length > 16)
			{
				re.Msg = "Name string length>16";
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}

			UserInfo user = tokenSerivce.GetUserByToken(token);

			if (user == null)
			{
				re.Msg = "Unable to obtain user information through Session ";
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
			if (user.Username.Equals(config.SecurityConfig.DemoUsername))
			{
				re.Msg = "cannotUpdateDemo";
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
			if (userService.VDUserName(username, out message))
			{
				re.Ok = userService.UpdateUsername(user.Id, username);
				re.Msg = message;
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
			else
			{
				re.Msg = "Incorrect username format or conflict";
				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
		}

		//获取用户的登录策略

		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public JsonResult GetUserLoginSecurityStrategy(string UserName)
		{

			var ss = userService.GetGetUserLoginSecurityStrategy(UserName);
			ApiResponseDTO apiRe = new ApiResponseDTO()
			{
				Ok = (ss != null),
				Msg = "",
				Data = ss

			};
			return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
		}


		//todo:修改密码

		[HttpPost]
		public async Task<IActionResult> UpdatePwd(string token, string oldPwd, string pwd)
		{
			ApiResponseDTO re = new ApiResponseDTO();
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				re.Msg = "NOTLOGIN";

				return Json(re, MyJsonConvert.GetLeanoteOptions());
			}
			try
			{
				var result = await userService.UpdatePwd(user.Id, oldPwd, pwd);
				re.Ok = result;
				if (!result)
				{
					re.Msg = "更新密码失败";

				}
			}
			catch (Exception ex)
			{
				re.Msg = ex.Message;
				re.Ok = false;
			}


			return LeanoteJson(re);
		}
		//获得同步状态
		//[HttpPost]

		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public JsonResult GetSyncState(string token)
		{

			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};


				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
			ApiGetSyncState apiGetSyncState = new ApiGetSyncState()
			{
				LastSyncUsn = user.Usn,
				LastSyncTime = UnixTimeUtil.GetTimeStampInLong(DateTime.Now)
			};

			return Json(apiGetSyncState, MyJsonConvert.GetSimpleOptions());
		}

		//todo:头像设置

		[HttpPost]
		public IActionResult UpdateLogo()
		{
			return null;
		}
		//todo:上传图片

		[HttpPost]
		public IActionResult uploadImage()
		{
			return null;
		}

		[HttpPost, HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> GetRealNameInformation(string token, string digitalEnvelopeJson, string dataSignJson)
		{

			var re = new ApiResponseDTO();
			DigitalEnvelope digitalEnvelope = null;
			var verify = false;
			//数字信封
			if (this.config.SecurityConfig.ForceDigitalEnvelope)
			{
				digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);

			}
			if (this.config.SecurityConfig.ForceDigitalSignature)
			{
				//验证签名
				var dataSign = DataSignDTO.FromJSON(dataSignJson);
				verify = await this.ePassService.VerifyDataSign(dataSign);
				if (!verify)
				{
					return LeanoteJson(re);
				}
				verify = dataSign.SignData.Operate.Equals("/api/User/GetRealNameInformation");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签字签名和数字信封数据

				//签名存证
				this.dataSignService.AddDataSign(dataSign, "GetRealNameInformation");
			}


			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};
				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
		
			re.Ok = true;
			
			return LeanoteJson(re);
		}
		[HttpPost]
		public async Task<IActionResult> SetRealNameInformation(string token, string sfz, string digitalEnvelopeJson, string dataSignJson)
		{
			var re = new ApiResponseDTO();
			DigitalEnvelope digitalEnvelope = null;

			var verify = false;
			//数字信封
			if (this.config.SecurityConfig.ForceDigitalEnvelope)
			{

				digitalEnvelope = DigitalEnvelope.FromJSON(digitalEnvelopeJson);
				var data = digitalEnvelope.GetPayLoadValue(this.gMService, this.config.SecurityConfig.PrivateKey);
				if (data == null)
				{
					throw new Exception("数字信封解密失败");
				}
				//赋予解密的数字信封
				sfz = data;
			}
			if (this.config.SecurityConfig.ForceDigitalSignature)
			{

				//验证签名
				var dataSign = DataSignDTO.FromJSON(dataSignJson);
				verify = await this.ePassService.VerifyDataSign(dataSign);
				if (!verify)
				{
					return LeanoteJson(re);
				}
				verify = dataSign.SignData.Operate.Equals("/api/User/SetRealNameInformation");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签字签名和数字信封数据
				if (dataSign != null)
				{
					var dataSM3 = gMService.SM3String(sfz);
					var signSM3 = dataSign.SignData.Hash;
					if (!dataSM3.ToUpper().Equals(signSM3.ToUpper()))
					{
						re.Msg = "SM3 is error";
						re.Ok = false;
						return LeanoteJson(re);
					}
				}

				//签名存证
				this.dataSignService.AddDataSign(dataSign, "SetRealNameInformation");
			}
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};
				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}

		
			re.Ok = true;
			return LeanoteJson(re);
		}

		[HttpPost]
		public IActionResult SetMDEditorPreferences(string mdOption)
		{
			var re = new ResponseMessage();
			var mdHashSet = new HashSet<string>();
			mdHashSet.Add("ace");
			mdHashSet.Add("vditor");

			var rthashSet = new HashSet<string>();
			rthashSet.Add("tinymce");
			rthashSet.Add("textbus");
			//参数判断
			if (string.IsNullOrEmpty(mdOption) || !mdHashSet.Contains(mdOption))
			{
				re.Msg = "Parameter error ";
				re.Ok = false;
				return Json(re, MyJsonConvert.GetSimpleOptions());
			}
			var user = GetUserBySession();
			//设置编辑器偏好
			userService.SetMDEditorPreferences(user.Id, mdOption);

			re.Ok = true;
			return Json(re, MyJsonConvert.GetSimpleOptions());
		}
		[HttpPost]
		public IActionResult SetRTEditorPreferences(string rtOption)
		{
			var re = new ResponseMessage();
			var mdHashSet = new HashSet<string>();
			mdHashSet.Add("ace");
			mdHashSet.Add("vditor");

			var rthashSet = new HashSet<string>();
			rthashSet.Add("tinymce");
			rthashSet.Add("textbus");
			//参数判断
			if (string.IsNullOrEmpty(rtOption) || !rthashSet.Contains(rtOption))
			{
				re.Msg = "Parameter error ";
				re.Ok = false;
				return Json(re, MyJsonConvert.GetSimpleOptions());
			}
			var user = GetUserBySession();
			//设置编辑器偏好
			userService.SetRTEditorPreferences(user.Id, rtOption);
			re.Ok = true;
			return Json(re, MyJsonConvert.GetSimpleOptions());
		}

	}
}