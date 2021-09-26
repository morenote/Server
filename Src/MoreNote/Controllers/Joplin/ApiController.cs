using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Models.DTO.Joplin;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoreNote.Controllers.Joplin
{
    public class ApiController : JoplinBaseController
    {
        private AuthService AuthService { get; set; }

        public ApiController(AttachService attachService
          , TokenSerivce tokenSerivce
          , NoteFileService noteFileService
          , UserService userService
          , ConfigFileService configFileService
          , IHttpContextAccessor accessor
            , AuthService authService
          ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
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

        [Route("/api/items/root:/testing.txt:/content")]
        [HttpPut]
        public IActionResult PutTestingContent(string what)
        {
            //http://joplin.morenote.top/api/items/root:/testing.txt:/content 
            //what=testing.txt:/content 

            var response=new PutContextResponseDto()
            {
                name="testing.txt",
                id="{AC66705E-090C-4AE6-8933-77A7BAC256E8}",
                created_time=UnixTimeUtil.GetUnixTimeMillisecondsInLong(),
                updated_time=UnixTimeUtil.GetUnixTimeMillisecondsInLong()
            };
             return Json(response, MyJsonConvert.GetOptions());
        }
        
        [Route("/api/items/root:/testing.txt:/content")]
        [HttpGet]
        public IActionResult GetTestingContent(string what)
        {
           return Content("testing");
        }

        [Route("/api/items/root:/testing.txt:")]
        [HttpDelete]
        public IActionResult DeleteTestingContent(string what)
        {
           return Ok();
        }

        [Route("/api/items/root:/{**what}")]
        [HttpGet]
        public IActionResult GetItems(string what)
        {
            return Content(what);
        }

        [Route("/api/items/root:/{**what}")]
        [HttpDelete]
        public IActionResult DeleteItems(string what)
        {
            return Content(what);
        }

        [Route("/api/items/root:/{**what}")]
        [HttpPost]
        public IActionResult PostItems(string what)
        {
            return Content(what);
        }
    }
}