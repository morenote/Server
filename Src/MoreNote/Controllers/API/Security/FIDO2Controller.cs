using Fido2NetLib;
using Fido2NetLib.Objects;

using Masuit.Tools;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.HySystem;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.Security.FIDO2;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.Auth;
using MoreNote.Models.Model.FIDO2;
using System;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Fido2NetLib.Fido2;

namespace MoreNote.Controllers.API
{
    [Route("api/fido2/[action]")]
    public class FIDO2Controller : APIBaseController

    {
        private AuthService authService;
        private FIDO2Service fido2Service;
        private Fido2ManagerService fido2Manager;

        public FIDO2Controller(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            , AuthService authService
            , FIDO2Service fIDO2Service
            , Fido2ManagerService fido2ManagerService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.fido2Service = fIDO2Service;
            this.fido2Manager=fido2ManagerService;
        }
       

        /// <summary>
        /// 请求fido2注册选项
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MakeCredentialOptions(string token )
        {
            var tokenVerify = tokenSerivce.VerifyToken(token);
            if (!tokenVerify)
            {
                var apiRe = new ApiReDTO()
                {
                    Ok = false,
                    Msg = "注册失败,token无效"
                };
                return Json(apiRe, MyJsonConvert.GetSimpleOptions());
            }
            var user = userService.GetUserByToken(token);

            var attachment = AuthenticatorAttachment.Platform;
           

            //注册选项
            var opts = new MakeCredentialParams(user.Username,user.Email, user.Id);
           
           // opts.AuthenticatorSelection.AuthenticatorAttachment = attachment;

            
            var credentialCreateOptions = fido2Service.MakeCredentialOptions(user, opts);

            return Json(credentialCreateOptions);
        }

        /// <summary>
        /// 验证并注册FIDO2令牌
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> RegisterCredentials(string token, string keyName, string data)
        {
            try
            {
                var tokenVerify = tokenSerivce.VerifyToken(token);
                if (!tokenVerify)
                {
                    var apiRe = new ApiReDTO()
                    {
                        Ok = false,
                        Msg = "注册失败,token无效"
                    };
                    return Json(apiRe, MyJsonConvert.GetSimpleOptions());
                }
                JsonSerializerOptions options = new System.Text.Json.JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Converters = {
                    new JsonStringEnumMemberConverter(),
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                options.Converters.Add(new Base64UrlConverter());

                var attestationResponse = JsonSerializer.Deserialize<AuthenticatorAttestationRawResponse>(data, options);

                var user = userService.GetUserByToken(token);
                if (string.IsNullOrEmpty(keyName) || !MyStringUtil.IsNumAndEnCh(keyName))
                {
                    keyName = "key";
                }
                var success = await fido2Service.RegisterCredentials(user, keyName, attestationResponse);
                // 4. return "ok" to the client
                return Json(success);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }
        
        /// <summary>
        /// 请求FIDO2断言
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAssertionOptions(string email)
        {
            string error = "";

            try
            {
                var user = userService.GetUserByEmail(email.ToLower());

                var assertionClientParams = new AssertionClientParams();

                var option = await fido2Service.AssertionOptionsPost(user, assertionClientParams);
                // 4. return "ok" to the client
                return Json(option);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }
        
        /// <summary>
        /// 客户端挑战服务器断言
        /// </summary>
        /// <param name="email"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> VerifyTheAssertionResponse(string email, string data)
        {
            var re = new ApiReDTO();
            try
            {
                data= System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));
                var clientRespons = JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(data);
                var user = userService.GetUserByEmail(email);

                var result = await fido2Service.MakeAssertionAsync(user, clientRespons);

                
                if (result.Status.Equals("ok"))
                {
                    //颁发token
                    var token = tokenSerivce.GenerateToken(user.Id,user.Email);
                    tokenSerivce.SaveToken(token);

                    UserToken authOk = new UserToken()
                    {
                        Token = token.TokenStr,
                        UserId = user.Id,
                        Email = user.Email,
                        Username = user.Username
                    };
                    re.Ok = true;
                    
                    re.Data = authOk;
                    return LeanoteJson(re);
                }
                else
                {
                    re.Data=result;
                }
              
                return LeanoteJson(re);
            }
            catch (Exception ex)
            {
                re.Ok=false;
                re.Msg = FormatException(ex);
                return   LeanoteJson(re);
                
            }
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }

        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> List(string userId)
        {
            ApiReDTO apiReDTO = new ApiReDTO();
            var list = this.fido2Manager.ListAllFido2(userId.ToLongByHex());
            apiReDTO.Data = list;
            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }
        [HttpDelete]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Delete(string keyId, string token)
        {
            ApiReDTO apiReDTO = new ApiReDTO();
            var verify = this.tokenSerivce.VerifyToken(token);
            if (!verify)
            {
                apiReDTO.Ok = false;
                apiReDTO.Msg = "this.tokenSerivce.VerifyToken(token) == false";
                return LeanoteJson(apiReDTO);
            }
            var user = this.userService.GetUserByToken(token);
           await this.fido2Manager.DeleteFido2(keyId.ToLongByHex());
            if (!this.fido2Manager.IsExist(user.Id))
            {
                //如果没有注册USBKEY，恢复默认的口令登录状态
                this.userService.SetUserLoginSecurityPolicyLevel(user.Id, LoginSecurityPolicyLevel.unlimited);
            }


            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Find(string keyId)
        {
            ApiReDTO apiReDTO = new ApiReDTO();
            var key = this.fido2Manager.ListAllFido2(keyId.ToLongByHex());
            apiReDTO.Data = key;
            apiReDTO.Ok = true;
            return LeanoteJson(apiReDTO);
        }
    }
}