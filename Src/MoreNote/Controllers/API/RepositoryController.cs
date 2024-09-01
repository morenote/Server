using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API
{
	/// <summary>
	/// 笔记仓库
	/// </summary>

	[Route("api/Repository/[action]")]
	public class RepositoryController : APIBaseController
	{
		private NotebookService notebookService;
		private RepositoryService noteRepositoryService;
		private OrganizationService organizationService;
		private EPassService ePassService;
		private DataSignService dataSignService;
		private NoteService noteService;
		private TokenSerivce tokenSerivce;
		private NoteContentService noteContentService;

		public RepositoryController(AttachService attachService,
			 TokenSerivce tokenSerivce,
				  NoteService noteService,

		 NoteContentService noteContentService
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
			this.noteRepositoryService = noteRepositoryService;
			this.ePassService = ePassService;
			this.dataSignService = dataSignService;
			this.noteService = noteService;
			this.noteContentService = noteContentService;
			this.tokenSerivce = tokenSerivce;

		}
		/// <summary>
		/// 检索我的仓库
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="token"></param>
		/// <param name="repositoryType"></param>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetMyRepository(string userId, string token, RepositoryType repositoryType)
		{
			var apiRe = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);
			if (user != null)
			{
				var rep = noteRepositoryService.GetRepositoryList(user.Id, repositoryType);
				apiRe = new ApiResponseDTO()
				{
					Ok = true,
					Data = rep
				};
			}
			apiRe.Msg = "";
			return LeanoteJson(apiRe);
		}

		/// <summary>
		/// 读取仓库信息
		/// </summary>
		/// <param name="repositoryId">仓库id</param>
		/// <param name="token">访问者token</param>
		/// <returns></returns>
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult RepositoryInfo(string repositoryId, string token)
		{
			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);

			var verify = noteRepositoryService.Verify(repositoryId.ToLongByHex(), user.Id, RepositoryAuthorityEnum.Read);
			if (!verify && user != null)
			{
				re.Msg = "Operate is not Equals ";
				return LeanoteJson(re);
			}
			var rep = noteRepositoryService.GetRepository(repositoryId.ToLongByHex());
			re.Ok = (rep != null);
			re.Data = rep;
			re.Msg = "";
			return LeanoteJson(re);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRepository(string token, string data, string dataSignJson)
		{
			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var verify = false;
			if (this.config.SecurityConfig.ForceDigitalSignature)
			{
				//验证签名
				var dataSign = DataSignDTO.FromJSON(dataSignJson);
				verify = await this.ePassService.VerifyDataSign(dataSign);
				if (!verify)
				{
					return LeanoteJson(re);
				}
				verify = dataSign.SignData.Operate.Equals("/api/Repository/CreateRepository");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签名存证
				this.dataSignService.AddDataSign(dataSign, "CreateRepository");
			}

			var user = tokenSerivce.GetUserByToken(token);
			var repository = JsonSerializer.Deserialize<Repository>(data, MyJsonConvert.GetLeanoteOptions());
			if (repository.RepositoryOwnerType == RepositoryOwnerType.Organization)
			{
				var orgId = repository.OwnerId;
				verify = organizationService.Verify(orgId, user.Id, OrganizationAuthorityEnum.AddRepository);
				if (verify == false)
				{
					re.Msg = "您没有权限创建这个仓库";

					return LeanoteJson(re);
				}
			}
			if (repository.RepositoryOwnerType == RepositoryOwnerType.Personal)
			{
				if (repository.OwnerId != user.Id)
				{
					re.Msg = "您没有权限创建这个仓库";
					return LeanoteJson(re);
				}
			}
			//if (!MyStringUtil.IsNumAndEnCh(notesRepository.Name))
			//{
			//    apiRe.Msg = "仓库路径仅允许使用英文大小写、数字，不允许特殊符号";
			//    return LeanoteJson(apiRe);
			//}
			if (noteRepositoryService.ExistRepositoryByName(repository.OwnerId, repository.Name))
			{
				re.Msg = "仓库名称冲突";
				return LeanoteJson(re);
			}

			var result = noteRepositoryService.CreateRepository(repository);
			if (repository.RepositoryType == RepositoryType.NoteRepository)
			{
				var list = new List<string>(4) { "life", "study", "work", "tutorial" };
				foreach (var item in list)
				{
					// 添加笔记本, 生活, 学习, 工作
					var userId = user.Id;
					var notebook = new NoteCollection()
					{
						Id = idGenerator.NextId(),
						NotesRepositoryId = result.Id,
						Seq = 0,
						UserId = userId,
						CreatedTime = DateTime.Now,
						Title = item,
						ParentCollectionId = null,
					};
					notebookService.AddNotebook(notebook);
				}
			}

			if (result == null)
			{
				re.Msg = "数据库创建仓库失败";
				return LeanoteJson(re);
			}
			re.Ok = true;
			re.Data = result;
			return LeanoteJson(re);
		}

		/// <summary>
		/// 删除仓库
		/// </summary>
		/// <param name="token"></param>
		/// <param name="repositoryId">仓库ID</param>
		/// <param name="dataSignJson">签名文件</param>
		/// <returns></returns>
		[HttpPost, HttpDelete]
		public async Task<IActionResult> DeleteRepository(string token, string repositoryId, string dataSignJson)
		{
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

			if (this.config.SecurityConfig.ForceDigitalSignature)
			{
				//验证签名
				var dataSign = DataSignDTO.FromJSON(dataSignJson);
				verify = await this.ePassService.VerifyDataSign(dataSign);
				if (!verify)
				{
					return LeanoteJson(re);
				}
				verify = dataSign.SignData.Operate.Equals("/api/repository/Deleterepository");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签名存证
				this.dataSignService.AddDataSign(dataSign, "DeleteRepository");
			}

			verify = noteRepositoryService.Verify(repositoryId.ToLongByHex(), user.Id, RepositoryAuthorityEnum.DeleteRepository);
			if (!verify)
			{
				return LeanoteJson(re);
			}

			this.noteRepositoryService.DeleteRepository(repositoryId.ToLongByHex());
			re.Ok = true;
			return LeanoteJson(re);
		}
		/// <summary>
		///重建笔记索引
		/// </summary>
		/// <param name="repositoryId"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> RebuildNotesIndex(string repositoryId)
		{

			var books = notebookService.GetRootNotebooks(repositoryId.ToLongByHex());
			foreach (var book in books)
			{
				await RebuildNotesIndex2(book.Id);
			}

			return LeanoteJson(new ApiResponseDTO() { Ok = true });

		}
		private async Task RebuildNotesIndex2(long? bookId)
		{

			var books = notebookService.GetNotebookChildren(bookId);
			foreach (var book in books)
			{
				await RebuildNotesIndex2(book.Id);
			}
			var notes = noteService.GetNoteChildrenByNotebookId(bookId);
			foreach (var note in notes)
			{
				var context = noteContentService.GetNoteContentByNoteId(note.Id);
				this.noteService.SetNoteContextId(note.Id, context.Id);

			}


		}
	}
}