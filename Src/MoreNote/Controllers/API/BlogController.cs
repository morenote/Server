using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Blog/[action]")]
    // [ApiController]
    public class BlogController : APIBaseController
    {
        private TokenSerivce tokenSerivce;
        private TagService tagService;
        public BlogController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            TagService tagService
            ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
           
            this.tokenSerivce= tokenSerivce;
            this.tagService= tagService;
        }
        /// <summary>
        /// 生成VuePress
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateVuePress(string repositoryId)
        {

            return Content("hello world");
        }
       


    }
}