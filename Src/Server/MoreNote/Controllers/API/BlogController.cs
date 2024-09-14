using Autofac;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.AutoFac.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.BlogBuilder;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Enums;

using System.Threading.Tasks;
using MoreNote.Logic.Service.Notes;


namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Blog/[action]")]
	// [ApiController]
	public class BlogController : APIBaseController
	{
		private TokenSerivce tokenSerivce;
		private TagService tagService;
		[Autowired]
		private NotebookService notebookService { get; set; }


		public BlogController(AttachService attachService,
			 TokenSerivce tokenSerivce,
			 NoteFileService noteFileService,
			 UserService userService,
			 ConfigFileService configFileService,
			  NotebookService noteRepositoryService,
			 IHttpContextAccessor accessor,
			TagService tagService
			) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{

			this.tokenSerivce = tokenSerivce;
			this.tagService = tagService;
		}
		/// <summary>
		/// 生成VuePress
		/// </summary>
		/// <param name="repositoryId"></param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> InitBlogBuilder(string repositoryId, string token, BlogBuilderType blogBuilderType)
		{

			var blogBuilder = this.componentContext.ResolveKeyed<BlogBuilderInterface>(blogBuilderType);
			var verify = false;
			var user = tokenSerivce.GetUserByToken(token);
			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			if (user == null)
			{
				return LeanoteJson(re);
			}
			verify = notebookService.Verify(repositoryId.ToLongByHex(), user.Id, NotebookAuthorityEnum.ManagementMember);
			if (!verify)
			{
				return LeanoteJson(re);
			}
			var tempNotebook = notebookService.GetNotebook(repositoryId.ToLongByHex());
			await blogBuilder.WriteNotesRepository(tempNotebook);

			re.Ok = true;
			return LeanoteJson(re);


		}



	}
}