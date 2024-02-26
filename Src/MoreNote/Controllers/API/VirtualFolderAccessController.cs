using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Property;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.FileStoreService;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.Enums;

namespace MoreNote.Controllers.API
{
	/// <summary>
	/// 笔记仓库
	/// </summary>

	[Route("api/VirtualFolderAccess/[action]")]
	public class VirtualFolderAccessController : APIBaseController
	{
		private NotebookService notebookService;
		private RepositoryService repositoryService;

		private OrganizationService organizationService;
		private EPassService ePassService;
		private DataSignService dataSignService;

		[Autowired]
		private VirtualFileAccessService VirtualFileAccessService { get; set; }
		[Autowired]
		private VirtualFolderAccessService VirtualFolderAccessService { get; set; }

		public VirtualFolderAccessController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NotebookService notebookService,
			RepositoryService noteRepositoryService,
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
		public IActionResult GetRootVirtualFolderInfos(string token, string repositoryHexId)
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
			var verify = repositoryService.Verify(repositoryHexId.ToLongByHex(), user.Id, RepositoryAuthorityEnum.DeleteRepository);
			if (verify == false)
			{
				return LeanoteJson(apiRe);
			}
			var repository = repositoryService.GetRepository(repositoryHexId.ToLongByHex());
			var list = this.VirtualFolderAccessService.GetRootVFolder(repository);
			apiRe.Ok = true;
			apiRe.Data = list;
			return LeanoteJson(apiRe);
		}
	}
}