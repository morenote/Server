using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Logic.Service.Security.USBKey;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Entity.Leanote.Management.Loggin;
using MoreNote.Models.Enums;

using System;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Auth/[action]")]
	public class AuthAPIController : APIBaseController
	{
		private AuthService authService;
		private UsbKeyManagerService usbKeyManagerService;
		public AuthAPIController(AttachService attachService,
			 TokenSerivce tokenSerivce,
			 NoteFileService noteFileService,
			 UserService userService,
			 ConfigFileService configFileService,
			 UsbKeyManagerService usbKeyManagerService,
			 IHttpContextAccessor accessor,

			 AuthService authService
			) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.authService = authService;
			this.usbKeyManagerService = usbKeyManagerService;
		}
		/// <summary>
		/// 取号-用于维持会话状态
		/// SessionCode在提交后失效，再次认证需要再次取号
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Session()
		{
			var re = new ApiResponseDTO();
			//产生一个序号
			var id = idGenerator.NextHexId();//序号
			var random = RandomTool.CreatSafeRandomHex(16);
			var data = cryptographyProvider.SM3(HexUtil.HexToByteArray(id+random));
			re.Data = data;
			re.Ok = true;
			return LeanoteJson(re);
		}

		/// <summary>
		/// 使用口令挑战
		///  成功返回 {Ok: true, Item: token }
		///  失败返回 {Ok: false, Msg: ""}
		/// </summary>
		/// <param name="email"></param>
		/// <param name="pwd"></param>
		/// 
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> PasswordChallenge(string email, string pwd, string sessionCode)
		{
		

			var re = new ApiResponseDTO();
			try
			{
				//使用认证服务鉴别口令
				var tokenStr = await authService.LoginByPWD(email, pwd);
				if (!string.IsNullOrEmpty(tokenStr))
				{
					var user = userService.GetUserByEmail(email);

					var userToken = new UserToken()
					{
						Token = tokenStr,
						UserId = user.Id,
						Email = user.Email,
						Username = user.Username
					};
					re.Ok = true;
					//re.Data = userToken;
					this.distributedCache.SetBool("Password" + sessionCode, true);
				
					return LeanoteJson(re);
				}
				else
				{
					re.Msg = "用户名或密码有误";
					
					//口令重试计数器
					//todo:增加用户口令重试计数器
					var errorCount = distributedCache.GetInt("SessionErrorCount");
					if (errorCount == null)
					{
						distributedCache.SetInt("SessionErrorCount", 1);
					}
					else
					{
						distributedCache.SetInt("SessionErrorCount", errorCount.Value + 1);

					}
					return LeanoteJson(re);
				}
			}
			catch (Exception ex)
			{
				re.Msg = ex.Message;
				re.Ok = false;
				return LeanoteJson(re);
			}
		}
		/// <summary>
		/// 提交登录信息
		/// </summary>
		/// <param name="email"></param>
		/// <param name="session"></param>
		/// <returns></returns>
		[HttpGet, HttpPost]
		public IActionResult SubmitLogin(string email, string sessionCode)
		{

			var re = new ApiResponseDTO();
			var user = userService.GetUserByEmail(email);
			if (user == null)
			{
				return LeanoteJson(re);
			}
			var Passwrod_Check = this.distributedCache.GetBool("Password" + sessionCode, false);
			var USBKEY_CHECK = this.distributedCache.GetBool("USBKEY" + sessionCode, false);

			var result = false;

			if (user.LoginSecurityPolicyLevel == LoginSecurityPolicyLevel.compliant)
			{
				result = Passwrod_Check && USBKEY_CHECK;
			}
			if (user.LoginSecurityPolicyLevel == LoginSecurityPolicyLevel.unlimited || user.LoginSecurityPolicyLevel == LoginSecurityPolicyLevel.loose)
			{
				result = Passwrod_Check || USBKEY_CHECK;
			}



			if (result)
			{
				var token = tokenSerivce.GenerateToken(user.Id, user.Email);

				tokenSerivce.SaveToken(token);
				var userToken = new UserToken()
				{
					Token = token.TokenStr,
					UserId = user.Id,
					Email = user.Email,
					Username = user.Username
				};


				re.Ok = true;
				re.Data = userToken;
				return LeanoteJson(re);
			}
			return LeanoteJson(re);


		}



		[HttpDelete]
		//todo:注销函数
		public JsonResult Logout()
		{
			//ex:API当前不使用cookie和session判断用户身份，
			//API调用必须显式的提供token字段，以证明身份

			var apiRe = new ApiResponseDTO()
			{
				Ok = true,
				Msg = "未提供注销功能"
			};
			return Json(apiRe, MyJsonConvert.GetSimpleOptions());
		}

		//todo:注册
		[HttpPost]
		public async Task<IActionResult> Register(string email, string pwd)
		{
			//ex:API当前不使用cookie和session判断用户身份，
			//API调用必须显式的提供token字段，以证明身份
			//API调用者必须是管理员身份或者超级管理员身份，否则调用无效
			//如果用户设置二次验证必须显示提供二次验证码
			var re = new ApiResponseDTO();
			if (!this.config.SecurityConfig.OpenRegister)
			{
				re.Msg = "服务器管理员已经禁止用户注册功能";
				return LeanoteJson(re);
			}

			if (await authService.Register(email, pwd, 0))
			{
				re = new ApiResponseDTO()
				{
					Ok = true,
					Msg = "注册成功"
				};
			}
			else
			{
				re = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "注册失败"
				};
			}
			return Json(re, MyJsonConvert.GetSimpleOptions());
		}
		[HttpPost]
		public async Task<IActionResult> ChangeUserPassword(string token, string password)
		{
			return null;
		}


		//[HttpGet]
		//[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		//public async Task<IActionResult> GetUserLoginLogging(string token)
		//{
		//	var re = new ApiResponseDTO()
		//	{
		//		Ok = false,
		//		Data = null
		//	};
		//	var user = tokenSerivce.GetUserByToken(token);
		//	if (!(user.IsAdmin() || user.IsSuperAdmin()))
		//	{
		//		re.Msg = "只有Admin或SuperAdmin账户才可以访问";
		//		return LeanoteJson(re);
		//	}
		//	var data = log.GetAllUserLoggingLogin();


		//	re.Data = data;
		//	re.Ok = true;
		//	return LeanoteJson(re);


		//}

		/// <summary>
		/// 获得用户登录设置
		/// </summary>
		/// <returns></returns>
		[HttpGet, HttpPost]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public async Task<IActionResult> UserLoginSettings(string email)
		{

			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = userService.GetUserByEmail(email);
			if (user == null)
			{
				return LeanoteJson(re);
			}
			re.Ok = true;
			re.Data = user.LoginSecurityPolicyLevel;
			return LeanoteJson(re);

		}
		/// <summary>
		/// 设置用户登录设置
		/// </summary>
		/// <param name="token"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> SetUserLoginSettings(string token, LoginSecurityPolicyLevel level)
		{

			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				return LeanoteJson(re);
			}
			if (level == LoginSecurityPolicyLevel.compliant)
			{
				var existUsbKey = this.usbKeyManagerService.IsExist(user.Id);
				if (!existUsbKey)
				{
					re.Msg = "No USBKEY registration";
					return LeanoteJson(re);

				}

			}

			userService.SetUserLoginSecurityPolicyLevel(user.Id, level);

			re.Ok = true;
			return LeanoteJson(re);


		}
	}
}