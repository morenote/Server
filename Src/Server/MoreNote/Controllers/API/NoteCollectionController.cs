using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Morenote.Framework.Filter.Global;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Logic.Service.MyOrganization;

using MoreNote.Logic.Service.Security.USBKey.CSP;
using MoreNote.Models.DTO.Leanote;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.Models.Enums;

using System;
using System.Threading.Tasks;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/NoteCollection/[action]")]
	// [ApiController]
	[ServiceFilter(typeof(CheckTokenFilter))]
	public class NoteCollectionController : APIBaseController
	{
		private NoteCollectionService noteCollectionService;
		private NotebookService noteRepositoryService;
		private OrganizationMemberRoleService repositoryMemberRoleService;
		private EPassService ePassService;
		private DataSignService dataSignService;
		private NoteService noteService;
		public NoteCollectionController(AttachService attachService
			, TokenSerivce tokenSerivce
			, NoteFileService noteFileService
			, UserService userService
			, ConfigFileService configFileService
			, IHttpContextAccessor accessor,
			NoteCollectionService notebookService,
			NoteService noteService,
			 EPassService ePassService,
			 OrganizationMemberRoleService repositoryMemberRoleService,
			 DataSignService dataSignService,
			 NotebookService noteRepositoryService
		   ) :
			base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
			this.noteCollectionService = notebookService;
			this.noteRepositoryService = noteRepositoryService;
			this.repositoryMemberRoleService = repositoryMemberRoleService;
			this.noteService = noteService;
			this.ePassService = ePassService;
			this.dataSignService = dataSignService;
		}

		//获取同步的笔记本
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public JsonResult GetSyncNotebooks(string token, int afterUsn, int maxEntry)
		{
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};

				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
			if (maxEntry == 0)
			{
				maxEntry = 100;
			}
			NoteCollection[] notebook = noteCollectionService.GeSyncNotebooks(user.Id, afterUsn, maxEntry);
			return Json(notebook, MyJsonConvert.GetLeanoteOptions());
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		private ApiNotebook[] fixNotebooks(NoteCollection[] notebooks)
		{
			ApiNotebook[] apiNotebooks = null;
			if (notebooks != null)
			{
				apiNotebooks = new ApiNotebook[notebooks.Length];
				for (int i = 0; i < notebooks.Length; i++)
				{
					apiNotebooks[i] = fixNotebook(notebooks[i]);
				}
			}
			return apiNotebooks;
		}
		[ApiExplorerSettings(IgnoreApi = true)]
		private ApiNotebook fixNotebook(NoteCollection notebook)
		{
			return new ApiNotebook()
			{
				NotebookId = notebook.Id,
				UserId = notebook.UserId,
				ParentNotebookId = notebook.ParentCollectionId,
				Seq = notebook.Seq,
				Title = notebook.Title,
				UrlTitle = notebook.UrlTitle,
				IsBlog = notebook.IsBlog,
				CreatedTime = notebook.CreatedTime,
				UpdatedTime = notebook.UpdatedTime,
				Usn = notebook.Usn,
				IsDeleted = notebook.IsDeleted,
			};
		}

		//得到用户的所有笔记本
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetNotebooks(string token)
		{
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};

				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
			else
			{
				NoteCollection[] notebooks = noteCollectionService.GetAll(user.Id);
				ApiNotebook[] apiNotebooks = fixNotebooks(notebooks);
				return Json(apiNotebooks, MyJsonConvert.GetLeanoteOptions());
			}

			return null;
		}

		//添加notebook
		[HttpPost]
		public IActionResult AddNotebook(string token, string title, string parentNotebookId, int seq)
		{
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};

				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
			else
			{
				NoteCollection notebook = new NoteCollection()
				{
					Id = idGenerator.NextId(),
					Title = title,
					Seq = seq,
					UserId = user.Id,
					ParentCollectionId = parentNotebookId.ToLongByHex()
				};
				if (noteCollectionService.AddNoteCollection(ref notebook))
				{
					ApiNotebook apiNotebook = fixNotebook(notebook);

					return Json(apiNotebook, MyJsonConvert.GetLeanoteOptions());
				}
				else
				{
					ApiResponseDTO apiRe = new ApiResponseDTO()
					{
						Ok = false,
						Msg = "AddNotebook is error",
					};

					return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
				}
			}
		}

		//修改笔记
		[HttpPost]
		public IActionResult UpdateNotebook(string token, string notebookId, string title, string parentNotebookId, int seq, int usn)
		{
			UserInfo user = tokenSerivce.GetUserByToken(token);
			if (user == null)
			{
				ApiResponseDTO apiRe = new ApiResponseDTO()
				{
					Ok = false,
					Msg = "NOTLOGIN",
				};

				return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
			}
			else
			{
				NoteCollection notebook;
				if (noteCollectionService.UpdateNoteCollectionApi(user.Id, notebookId.ToLongByHex(), title, parentNotebookId.ToLongByHex(), seq, usn, out notebook))
				{
					ApiNotebook apiNotebook = fixNotebook(notebook);

					return Json(apiNotebook, MyJsonConvert.GetLeanoteOptions());
				}
				else
				{
					ApiResponseDTO apiRe = new ApiResponseDTO()
					{
						Ok = false,
						Msg = "UpdateNotebook is error",
					};

					return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
				}
			}
		}


		/// <summary>
		/// 删除笔记本
		/// </summary>
		/// <param name="token"></param>
		/// <param name="notebookId">仓库id</param>
		/// <param name="noteCollectionId">笔记本id</param>
		/// <param name="recursion">是否递归删除，非空文件夹</param>
		/// <param name="force">系统会忽略错误检查，强制删除笔记本和里面的笔记</param>
		/// <returns></returns>
		[HttpPost, HttpDelete]
		public async Task<IActionResult> DeleteNoteCollection(string token, string notebookId, string noteCollectionId, bool recursively, bool force, string dataSignJson)
		{
			UserInfo user = tokenSerivce.GetUserByToken(token);
			var verify = false;
			ApiResponseDTO re = new ApiResponseDTO()
			{
				Ok = false,
				Msg = "NOTLOGIN",
			};

			if (user == null)
			{
				re.Msg = "NOTLOGIN";
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
				verify = dataSign.SignData.Operate.Equals("/api/Notebook/DeleteNoteCollection");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签名存证
				this.dataSignService.AddDataSign(dataSign, "DeleteNoteCollection");

			}

			var message = "";
			var notebook = noteCollectionService.GetNoteCollectionById(noteCollectionId.ToLongByHex());
			var repositoryId = notebook.NotesRepositoryId;
			if (repositoryId != notebookId.ToLongByHex())
			{
				return LeanoteJson(re);
			}
			//鉴别用户是否有权限
			verify = noteRepositoryService.Verify(repositoryId, user.Id, NotebookAuthorityEnum.Write);
			if (verify == false)
			{
				return LeanoteJson(re);
			}
			//增加usn
			var usn = noteRepositoryService.IncrUsn(repositoryId);

			if (recursively)
			{

				re.Ok = noteCollectionService.DeleteNotebookRecursively(noteCollectionId.ToLongByHex(), usn);
			}
			else
			{
				re.Ok = noteCollectionService.DeleteNoteCollection(noteCollectionId.ToLongByHex(), usn);
			}

			return LeanoteJson(re);
		}

		//获得笔记本的第一层文件夹
		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetRootNoteCollection(string token, string notebookId)
		{
			var apiRe = new ApiResponseDTO();

			var user = tokenSerivce.GetUserByToken(token);
			if (user != null)
			{
				//var repository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());

				//var memerRole = noteRepositoryService.GetRepositoryMemberRole(repositoryId.ToLongByHex());

				//检查用户是否对仓库具有读权限
				if (noteRepositoryService.Verify(notebookId.ToLongByHex(), user.Id, NotebookAuthorityEnum.Read))
				{
					var books = noteCollectionService.GetRootNoteCollections(notebookId.ToLongByHex());
					apiRe.Ok = true;
					apiRe.Data = books;
				}
			}
			return LeanoteJson(apiRe);
		}


		[HttpGet]
		[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult GetNoteCollectionChildren(string token, string notebookId)
		{
			var apiRe = new ApiResponseDTO();

			var user = tokenSerivce.GetUserByToken(token);



			if (user != null)
			{
				//var repository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());

				//var memerRole = noteRepositoryService.GetRepositoryMemberRole(repositoryId.ToLongByHex());

				var book = noteCollectionService.GetNoteCollectionById(notebookId.ToLongByHex());
				if (book == null)
				{
					return LeanoteJson(apiRe);

				}
				//检查用户是否对仓库具有读权限
				if (noteRepositoryService.Verify(book.NotesRepositoryId, user.Id, NotebookAuthorityEnum.Read))
				{
					var note = noteCollectionService.GetNotebookChildren(notebookId.ToLongByHex());
					apiRe.Ok = true;
					apiRe.Data = note;
				}
			}
			return LeanoteJson(apiRe);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNoteCollection(string token, string notebookId, string noteCollectionTitle, string parentNoteCollectionId, string dataSignJson)
		{
			var re = new ApiResponseDTO();
			var user = tokenSerivce.GetUserByToken(token);
			long? parentId = null;
			bool verify = false;
			long? repositoryId = null;

			//验证签名
			if (this.config.SecurityConfig.ForceDigitalSignature)
			{
				var dataSign = DataSignDTO.FromJSON(dataSignJson);
				verify = await this.ePassService.VerifyDataSign(dataSign);
				if (!verify)
				{
					return LeanoteJson(re);
				}
				verify = dataSign.SignData.Operate.Equals("/api/Notebook/CreateNoteCollection");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签名存证
				this.dataSignService.AddDataSign(dataSign, "CreateNoteCollection");

			}



			if (string.IsNullOrEmpty(notebookId))
			{
				return LeanoteJson(re);
			}

			if (string.IsNullOrEmpty(parentNoteCollectionId))
			{

				verify = noteRepositoryService.Verify(notebookId.ToLongByHex(), user.Id, NotebookAuthorityEnum.Write);
				repositoryId = notebookId.ToLongByHex();
			}
			else
			{
				var parentNotebook = noteCollectionService.GetNoteCollectionById(parentNoteCollectionId.ToLongByHex());
				if (user == null || parentNotebook == null)
				{
					return LeanoteJson(re);
				}
				repositoryId = parentNotebook.NotesRepositoryId;
				if (repositoryId != notebookId.ToLongByHex())
				{
					return LeanoteJson(re);
				}
				verify = noteRepositoryService.Verify(repositoryId, user.Id, NotebookAuthorityEnum.Write);
				parentId = parentNotebook.Id;
			}


			if (!verify)
			{
				return LeanoteJson(re);
			}
			var notebook = new NoteCollection()
			{
				Id = idGenerator.NextId(),
				NotesRepositoryId = repositoryId,
				Seq = 0,
				UserId = user.Id,

				CreatedTime = DateTime.Now,
				Title = noteCollectionTitle,
				ParentCollectionId = parentId,
			};

			noteCollectionService.AddNoteCollection(notebook);
			re.Ok = true;
			re.Data = notebook;

			return LeanoteJson(re);


		}
		[HttpPost]
		public async Task<IActionResult> UpdateNoteCollectionTitle(string token, string noteCollectionId, string noteCollectionTitle, string dataSignJson)
		{
			var re = new ApiResponseDTO();
			var user = tokenSerivce.GetUserByToken(token);
			var notebook = noteCollectionService.GetNoteCollectionById(noteCollectionId.ToLongByHex());

			if (user == null || notebook == null)
			{
				return LeanoteJson(re);
			}
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
				verify = dataSign.SignData.Operate.Equals("/api/Notebook/UpdateNoteBookTitle");
				if (!verify)
				{
					re.Msg = "Operate is not Equals ";
					return LeanoteJson(re);
				}
				//签名存证
				this.dataSignService.AddDataSign(dataSign, "UpdateNoteBookTitle");
			}



			var repositoryId = notebook.NotesRepositoryId;
			verify = noteRepositoryService.Verify(repositoryId, user.Id, NotebookAuthorityEnum.Write);
			if (!verify)
			{
				return LeanoteJson(re);
			}


			noteCollectionService.UpdateNoteCollectionTitle(notebook.Id, noteCollectionTitle);
			re.Ok = true;
			re.Data = notebook;

			return LeanoteJson(re);


		}
	}

}