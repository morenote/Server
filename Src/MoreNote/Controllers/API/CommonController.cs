﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.DTO.Leanote;

namespace MoreNote.Controllers.API.APIV1
{
	[Route("api/Common/[action]")]

	// [ApiController]
	public class CommonController : APIBaseController
	{
		private AttachService attachService;
		private NoteService noteService;
		private TokenSerivce tokenSerivce;
		private NoteContentService noteContentService;
		private NotebookService notebookService;
		private TrashService trashService;
		private IHttpContextAccessor accessor;
		private RepositoryService noteRepositoryService;

		public CommonController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NoteService noteService,
			NoteContentService noteContentService,
			NotebookService notebookService,
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
		public IActionResult GetHexId()
		{
			var re = new ApiResponseDTO();
			re.Ok = true;
			re.Data = this.idGenerator.NextHexId();
			return LeanoteJson(re);

		}




	}
}