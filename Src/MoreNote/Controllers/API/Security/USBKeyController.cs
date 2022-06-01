using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Security.FIDO2.Service;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote.USBKey;

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

        public async Task<IActionResult> LoginResponse(string data)
        {
            var clinetResponse = ClientResponse.FromJSON(data);
            var challenge = ePass.GetServerChallenge(clinetResponse.Id);

            var result = await ePass.VerifyClientResponse(clinetResponse,false);

            ApiRe apiRe = new ApiRe();
            if (result)
            {
                var userId = challenge.UserId;
                var user = userService.GetUserByUserId(userId);
                var token = tokenSerivce.GenerateToken();
                tokenSerivce.AddToken(token);
                AuthOk authOk = new AuthOk()
                {
                    Ok = true,
                    Token = token.TokenStr,
                    UserId = user.UserId.ToHex24(),
                    Email = user.Email,
                    Username = user.Username
                };
                apiRe.Ok = true;
                apiRe.Data = authOk;
                return LeanoteJson(apiRe);
            }
            else
            {
                return LeanoteJson(apiRe);
            }
        }
    }
}