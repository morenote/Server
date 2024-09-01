using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.DTO.Leanote;

namespace MoreNote.Controllers.API.APIV1
{
	[Route("api/System/[action]")]

	// [ApiController]
	public class SystemController : APIBaseController
	{
		private AttachService attachService;
		private NoteService noteService;
		private TokenSerivce tokenSerivce;
		private NoteContentService noteContentService;
		private NoteCollectionService notebookService;
		private TrashService trashService;
		private IHttpContextAccessor accessor;
		private RepositoryService noteRepositoryService;

		public SystemController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NoteService noteService,
			NoteContentService noteContentService,
			NoteCollectionService notebookService,
			RepositoryService noteRepositoryService,
			TrashService trashService
		   ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.attachService = attachService;
			this.noteService = noteService;
			this.tokenSerivce = tokenSerivce;
			this.noteContentService = noteContentService;
			this.trashService = trashService;
			this.accessor = accessor;
			this.notebookService = notebookService;
			this.noteRepositoryService = noteRepositoryService;
		}
		/// <summary>
		/// 获得Hex形式的ID
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Version()
		{
			
			return Content("V0.0.1");

		}




	}
}