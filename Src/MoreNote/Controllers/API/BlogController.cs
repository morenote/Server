using Autofac;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.BlogBuilder;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.Enum;

using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Blog/[action]")]
    // [ApiController]
    public class BlogController : APIBaseController
    {
        private TokenSerivce tokenSerivce;
        private TagService tagService;
        [Autowired]
        private NoteRepositoryService noteRepositoryService { get;set;}

       
        public BlogController(AttachService attachService,
             TokenSerivce tokenSerivce,
             NoteFileService noteFileService,
             UserService userService,
             ConfigFileService configFileService,
              NoteRepositoryService noteRepositoryService,
             IHttpContextAccessor accessor,
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
        public async Task<IActionResult> InitBlogBuilder(string repositoryId, string token, BlogBuilderType blogBuilderType)
        {

            var  blogBuilder=this.componentContext.ResolveKeyed< BlogBuilderInterface >(blogBuilderType);
            var verify = false;
            var user = tokenSerivce.GetUserByToken(token);
            var re = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            if (user == null)
            {
                return LeanoteJson(re);
            }
            verify = noteRepositoryService.Verify(repositoryId.ToLongByHex(), user.Id, RepositoryAuthorityEnum.Read);
            if (!verify)
            {
                return LeanoteJson(re);
            }
            var noteRepository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());
            await  blogBuilder.WriteNotesRepository(noteRepository);

            re.Ok=true;
            return LeanoteJson(re);


        }
       


    }
}