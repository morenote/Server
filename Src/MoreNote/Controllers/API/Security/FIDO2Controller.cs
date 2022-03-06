using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Fido2NetLib;
using Fido2NetLib.Objects;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.HySystem;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Models.Model.FIDO2;

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
            , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.authService = authService;
            this.fido2Service = fIDO2Service;
        }
        /// <summary>
        /// 请求fido2注册选项
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MakeCredentialOptions(string token,string authType)
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

            var attachment=AuthenticatorAttachment.Platform;
            var ok=Enum.TryParse< AuthenticatorAttachment >(authType,true,out attachment);


            //注册选项
            var opts = new MakeCredentialParams(user.Username, user.UserId);
            if (ok)
            {
                opts.AuthenticatorSelection.AuthenticatorAttachment = attachment;
            }
            var credentialCreateOptions = fido2Service.MakeCredentialOptions(user,opts);

            return Json(credentialCreateOptions);

        }
        /// <summary>
        /// 验证并注册FIDO2令牌
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RegisterCredentials(string token,string keyName, string data)
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

                var attestationResponse= JsonSerializer.Deserialize< AuthenticatorAttestationRawResponse >(data,options);

                var user = userService.GetUserByToken(token);
                if (string.IsNullOrEmpty(keyName)|| !HyString.IsNumAndEnCh(keyName))
                {
                    keyName="key";
                }
                var success=await fido2Service.RegisterCredentials(user, keyName, attestationResponse);
                // 4. return "ok" to the client
                return Json(success);
            }
            catch (Exception e)
            {
                return Json(new CredentialMakeResult(status: "error", errorMessage: FormatException(e), result: null));
            }
        }

        public IActionResult CreateAssertionOptions()
        {
            return View();
        }

        public IActionResult VerifyTheAssertionResponse()
        {
            return View();
        }

        private string FormatException(Exception e)
        {
            return string.Format("{0}{1}", e.Message, e.InnerException != null ? " (" + e.InnerException.Message + ")" : "");
        }
    }
}