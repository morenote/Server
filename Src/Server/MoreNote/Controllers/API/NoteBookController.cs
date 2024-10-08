﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Notes;
using MoreNote.Logic.Service.MyOrganization;

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
using MoreNote.Models.Entiys.Leanote.Notes;
using Morenote.Framework.Filter.Global;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 笔记仓库
    /// </summary>

    [Route("api/Notebook/[action]")]
	public class NoteBookController : APIBaseController
	{
		private NoteCollectionService notebookService;
		private NotebookService notebookRepositoryService;
		private OrganizationService organizationService;
		private EPassService ePassService;
		private DataSignService dataSignService;
		private NoteService noteService;
		private TokenSerivce tokenSerivce;
		private NoteContentService noteContentService;

		public NoteBookController(AttachService attachService,
			 TokenSerivce tokenSerivce,
				  NoteService noteService,

		 NoteContentService noteContentService
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
			this.notebookRepositoryService = noteRepositoryService;
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
		public IActionResult GetMyNotebook(string userId, string token, NotebookType repositoryType)
		{
			var apiRe = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);
			if (user != null)
			{
				var rep = notebookRepositoryService.GetNotebookList(user.Id, repositoryType);
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
		public IActionResult NotebookInfo(string repositoryId, string token)
		{
			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var user = tokenSerivce.GetUserByToken(token);

			var verify = notebookRepositoryService.Verify(repositoryId.ToLongByHex(), user.Id, NotebookAuthorityEnum.Read);
			if (!verify && user != null)
			{
				re.Msg = "Operate is not Equals ";
				return LeanoteJson(re);
			}
			var rep = notebookRepositoryService.GetNotebook(repositoryId.ToLongByHex());
			re.Ok = (rep != null);
			re.Data = rep;
			re.Msg = "";
			return LeanoteJson(re);
		}

		[HttpPost]
		[ServiceFilter(typeof(MessageSignFilter))]
		//[MessageSignFilter()]
		public async Task<IActionResult> CreateNotebook(string token, string data, string digitalEnvelopeJson)
		{
			var re = new ApiResponseDTO()
			{
				Ok = false,
				Data = null
			};
			var verify = false;
			

			var user = tokenSerivce.GetUserByToken(token);
			var notebook = JsonSerializer.Deserialize<Notebook>(data, MyJsonConvert.GetLeanoteOptions());
			if (notebook.NotebookOwnerType == NotebookOwnerType.Organization)
			{
				var orgId = notebook.OwnerId;
				verify = organizationService.Verify(orgId, user.Id, OrganizationAuthorityEnum.AddRepository);
				if (verify == false)
				{
					re.Msg = "您没有权限创建这个仓库";

					return LeanoteJson(re);
				}
			}
			if (notebook.NotebookOwnerType == NotebookOwnerType.Personal)
			{
				if (notebook.OwnerId != user.Id)
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
			if (notebookRepositoryService.ExistNotebookByName(notebook.OwnerId, notebook.Name))
			{
				re.Msg = "仓库名称冲突";
				return LeanoteJson(re);
			}

			var result = notebookRepositoryService.CreateNotebook(notebook);
			if (notebook.NotebookType == NotebookType.NoteRepository)
			{
				var list = new List<string>(4) { "life", "study", "work", "tutorial" };
				foreach (var item in list)
				{
					// 添加笔记本, 生活, 学习, 工作
					var userId = user.Id;
					var noteCollection = new NoteCollection()
					{
						Id = idGenerator.NextId(),
						NotesRepositoryId = result.Id,
						Seq = 0,
						UserId = userId,
						CreatedTime = DateTime.Now,
						Title = item,
						ParentCollectionId = null,
					};
					notebookService.AddNoteCollection(noteCollection);
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
		[ServiceFilter(typeof(MessageSignFilter))]
		public async Task<IActionResult> DeleteNotebook(string token, string id)
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

			await Task.Delay(0);

			verify = notebookRepositoryService.Verify(id.ToLongByHex(), user.Id, NotebookAuthorityEnum.DeleteRepository);
			if (!verify)
			{
				return LeanoteJson(re);
			}

			this.notebookRepositoryService.DeleteNotebook(id.ToLongByHex());
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

			var books = notebookService.GetRootNoteCollections(repositoryId.ToLongByHex());
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
				var context =await noteContentService.GetNoteContentByNoteId(note.Id);
				this.noteService.SetNoteContextId(note.Id, context.Id);

			}


		}
	}
}