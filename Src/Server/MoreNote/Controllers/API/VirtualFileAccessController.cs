﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Controllers.API.APIV1;
using MoreNote.AutoFac.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Enums;
using MoreNote.Logic.Service.Notes;


namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 笔记仓库
    /// </summary>

    [Route("api/VirtualFileAccess/[action]")]
	public class VirtualFileAccessController : APIBaseController
	{
		private NoteCollectionService notebookService;
		private NotebookService repositoryService;

		private OrganizationService organizationService;
		private EPassService ePassService;
		private DataSignService dataSignService;

		[Autowired]
		private VirtualFileAccessService VirtualFileAccessService { get; set; }

		public VirtualFileAccessController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NoteCollectionService notebookService,
			NotebookService noteRepositoryService,
			 OrganizationService organizationService,
			   EPassService ePassService,
			   DataSignService dataSignService
		   ) :
		 base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.notebookService = notebookService;
			this.repositoryService = noteRepositoryService;
			this.ePassService = ePassService;
			this.dataSignService = dataSignService;
		}

		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetRootVirtualFileInfos(string token, string repositoryHexId)
		{
			var apiRe = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				apiRe.Msg = "";
				return LeanoteJson(apiRe);
			}
			var verify = repositoryService.Verify(repositoryHexId.ToLongByHex(), user.Id, NotebookAuthorityEnum.DeleteRepository);
			if (verify == false)
			{
				return LeanoteJson(apiRe);
			}
			var repository = repositoryService.GetNotebook(repositoryHexId.ToLongByHex());
			var list = this.VirtualFileAccessService.GetRootVFiles(repository);
			apiRe.Ok = true;
			apiRe.Data = list;
			return LeanoteJson(apiRe);
		}
	}
}