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
        /// <summary>
        /// 获得服务器挑战
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult GetLoginChallenge(string email,string sessionCode)
        {
            ApiRe apiRe = new ApiRe();
            var user = userService.GetUserByEmail(email);
            if (user == null)
            {
                return LeanoteJson(apiRe);
            }
            var challenge = ePass.GenServerChallenge("LoginChallenge", sessionCode, user.Id);
            apiRe.Ok = true;
            apiRe.Data = challenge;
            return LeanoteJson(apiRe);
        }




        /// <summary>
        /// 对服务器的挑战给出响应
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
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
                   // var token = tokenSerivce.GenerateToken(user.UserId, user.Email);

                    //tokenSerivce.SaveToken(token);
              
                    //登录日志
                    logg.UserId = user.Id;
                    logg.IsLoginSuccess = true;
                    apiRe.Ok = true;
                    this.distributedCache.SetBool("USBKEY"+ challenge.RequestNumber,true);

                    //apiRe.Data = userToken;
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