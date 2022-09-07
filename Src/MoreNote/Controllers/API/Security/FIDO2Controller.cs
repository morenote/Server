using Fido2NetLib;
using Fido2NetLib.Objects;
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
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Model.FIDO2;
using System;
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

        public FIDO2Controller(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            , AuthService authService
            , FIDO2Service fIDO2Service
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.fido2Service = fIDO2Service;
        }
        /// <summary>
        /// 获取fido2列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public IActionResult GetFido2List(string userId, string token)
        {
            var user = userService.GetUserByUserId(userId.ToLongByHex());
            var verify= tokenSerivce.VerifyToken(userId.ToLongByHex(),token);
            if (!verify)
            {
               
                return SimpleJson(new ApiRe() { Msg="TokenIsError"});

            }
            var list = fido2Service.GetFido2List(user.Id);

            var re = new ApiRe()
            {
                Ok = true,
                Data = list != null
            };
            return SimpleJson(re);
        }

        /// <summary>
        /// 请求fido2注册选项
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MakeCredentialOptions(string token, string authType)
        {
            var tokenVerify = tokenSerivce.VerifyToken(token);
            if (!tokenVerify)
            {
                var apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "注册失败,token无效"
                };
                return Json(apiRe, MyJsonConvert.GetSimpleOptions());
            }
            var user = userService.GetUserByToken(token);

            var attachment = AuthenticatorAttachment.Platform;
            var ok = Enum.TryParse<AuthenticatorAttachment>(authType, true, out attachment);

            //注册选项
            var opts = new MakeCredentialParams(user.Username, user.Id);
            if (ok)
            {
                opts.AuthenticatorSelection.AuthenticatorAttachment = attachment;
            }
            var credentialCreateOptions = fido2Service.MakeCredentialOptions(user, opts);

            return Json(credentialCreateOptions);
        }

        /// <summary>
        /// 验证并注册FIDO2令牌
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public async Task<IActionResult> RegisterCredentials(string token, string keyName, string data)
        {
            try
            {
                var tokenVerify = tokenSerivce.VerifyToken(token);
                if (!tokenVerify)
                {
                    var apiRe = new ApiRe()
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
        [HttpPost]
        public async Task<IActionResult> CreateAssertionOptions(string email)
        {
            string error = "";

            try
            {
                var user = userService.GetUserByEmail(email);

                var assertionClientParams = new AssertionClientParams();

                var success = await fido2Service.AssertionOptionsPost(user, assertionClientParams);
                // 4. return "ok" to the client
                return Json(success);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }
        [HttpPost]
        public async Task<IActionResult> VerifyTheAssertionResponse(string email, string signData)
        {
            try
            {
                var clientRespons = JsonSerializer.Deserialize<AuthenticatorAssertionRawResponse>(signData);
                var user = userService.GetUserByEmail(email);

                var success = await fido2Service.MakeAssertionAsync(user, clientRespons);

                var re = new ApiRe();
                if (success.Status.Equals("success"))
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
               
                re.Data = success;
                return LeanoteJson(re);
            }
            catch (Exception ex)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(ex), result: null));
            }
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }
    }
}