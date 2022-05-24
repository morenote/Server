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
    /// 登录测试服务器 login Testing
    /// </summary>
    public class JoplinTestingController : JoplinBaseController
    {
        private AuthService AuthService { get; set; }

        public JoplinTestingController(AttachService attachService
          , TokenSerivce tokenSerivce
          , NoteFileService noteFileService
          , UserService userService
          , ConfigFileService configFileService
          , IHttpContextAccessor accessor
            , AuthService authService
          ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.AuthService = authService;
        }

       
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
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
             return Json(response, MyJsonConvert.GetLeanoteOptions());
        }
        /// <summary>
        ///  删除测试信息
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        [Route("/api/items/root:/testing.txt:/content")]
        [HttpGet]
        public IActionResult GetTestingContent(string what)
        {
           return Content("testing");
        }
        
        /// <summary>
        /// 删除测试信息
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        [Route("/api/items/root:/testing.txt:")]
        [HttpDelete]
        public IActionResult DeleteTestingContent(string what)
        {
           return Ok();
        }

       
    }
}