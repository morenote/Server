using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Models.DTO.Joplin;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Joplin
{
    /// <summary>
    /// Sesssion API
    /// </summary>
    public class JoplinSessionsController : JoplinBaseController
    {
        private AuthService AuthService { get; set; }

        public JoplinSessionsController(AttachService attachService
          , TokenSerivce tokenSerivce
          , NoteFileService noteFileService
          , UserService userService
          , ConfigFileService configFileService
          , IHttpContextAccessor accessor
          , AuthService authService
         , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.AuthService = authService;
        }

        [Route("api/sessions")]
        public IActionResult sessions([FromBody] SessionRequestDto sessionRequest)
        {
            string token = string.Empty;
            User user = null;
            var loginReult = AuthService.LoginByPWD(sessionRequest.email, sessionRequest.password, out token, out user);

            if (loginReult)
            {
                var response = new SessionResponseDto
                {
                    id = token,
                    user_id = user.UserId.ToHex24()
                };
                return Json(response, MyJsonConvert.GetOptions());
            }
            else
            {
                var response = new SessionResponseDto
                {
                    error = "Invalid username or password"
                };
                //（禁止） 服务器拒绝请求。
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return Json(response, MyJsonConvert.GetOptions());
            }
        }
    }
}