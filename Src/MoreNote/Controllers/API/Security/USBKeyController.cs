using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote.Loggin;

using System;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.Security
{
    [Route("api/USBKey/[action]")]
    public class USBKeyController : APIBaseController
    {
        private AuthService authService;
        private EPassService ePass;

        public USBKeyController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
            , AuthService authService
            , EPassService ePassService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.authService = authService;
            this.ePass = ePassService;
        }

        public IActionResult GetLoginChallenge(string email)
        {
            ApiRe apiRe = new ApiRe();
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return LeanoteJson(apiRe);
            }
            var challenge = ePass.GenServerChallenge("LoginChallenge", user.UserId);
            apiRe.Ok = true;
            apiRe.Data = challenge;
            return LeanoteJson(apiRe);
        }

        public async Task<IActionResult> LoginResponse( string data)
        {
            var clinetResponse = ClientResponse.FromJSON(data);
            var challenge = ePass.GetServerChallenge(clinetResponse.Id);

            var result = await ePass.VerifyClientResponse(clinetResponse,false);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in Request.Headers)
            {
                stringBuilder.Append(item.Key + "=" + item.Value.ToString() + "\r\n");

            }
            LoggingLogin logg = new LoggingLogin()
            {
                Id = this.idGenerator.NextId(),
                LoginDateTime = DateTime.Now,
                LoginMethod = "SmartToken",
                Ip = Request.Host.Host,
                BrowserRequestHeader = stringBuilder.ToString(),
            };

            ApiRe apiRe = new ApiRe();

            try
            {
                if (result)
                {
                    var userId = challenge.UserId;
                    var user = userService.GetUserByUserId(userId);
                    var token = tokenSerivce.GenerateToken(user.UserId, user.Email);

                    tokenSerivce.SaveToken(token);
                    var userToken = new UserToken()
                    {

                        Token = token.TokenStr,
                        UserId = user.UserId,
                        Email = user.Email,
                        Username = user.Username
                    };
                    //登录日志
                    logg.UserId = user.UserId;
                    logg.IsLoginSuccess = true;

                    apiRe.Ok = true;
                    apiRe.Data = userToken;
                    return LeanoteJson(apiRe);
                }
                else
                {
                    return LeanoteJson(apiRe);
                }

            }
            finally
            {
                await logg.AddMac(this.cryptographyProvider);
                this.logging.Save(logg);
            }
            
        }
    }
}