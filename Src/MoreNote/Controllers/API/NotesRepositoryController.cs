using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.HySystem;
using MoreNote.Common.Utils;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.Entity.Leanote;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 笔记仓库
    /// </summary>

    [Route("api/NotesRepository/[action]")]
    public class NotesRepositoryController : APIBaseController
    {
        private NotebookService notebookService;
        private NoteRepositoryService noteRepositoryService;
        private OrganizationService organizationService;

        public NotesRepositoryController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
             OrganizationService organizationService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetMyNoteRepository(string userId, string token)
        {
            var apiRe = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            if (user != null)
            {
                var rep = noteRepositoryService.GetNoteRepositoryList(user.UserId);
                apiRe = new ApiRe()
                {
                    Ok = true,
                    Data = rep
                };
            }
            apiRe.Msg = "";
            return LeanoteJson(apiRe);
        }

        public IActionResult CreateNoteRepository(string token, string data)
        {
            var apiRe = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            var user = tokenSerivce.GetUserByToken(token);
            var notesRepository = JsonSerializer.Deserialize<NotesRepository>(data, MyJsonConvert.GetLeanoteOptions());
            if (notesRepository.RepositoryOwnerType == RepositoryOwnerType.Organization)
            {
                var orgId = notesRepository.OwnerId;
                var verify = organizationService.Verify(orgId, user.UserId, OrganizationAuthorityEnum.AddRepository);
                if (verify == false)
                {
                    apiRe.Msg = "您没有权限创建这个仓库";

                    return LeanoteJson(apiRe);
                }
            }
            if (notesRepository.RepositoryOwnerType == RepositoryOwnerType.Personal)
            {
                if (notesRepository.OwnerId != user.UserId)
                {
                    apiRe.Msg = "您没有权限创建这个仓库";
                    return LeanoteJson(apiRe);
                }
            }
            //if (!MyStringUtil.IsNumAndEnCh(notesRepository.Name))
            //{
            //    apiRe.Msg = "仓库路径仅允许使用英文大小写、数字，不允许特殊符号";
            //    return LeanoteJson(apiRe);
            //}
            if (noteRepositoryService.ExistNoteRepositoryByName(notesRepository.OwnerId, notesRepository.Name))
            {
                apiRe.Msg = "仓库名称冲突";
                return LeanoteJson(apiRe);
            }

            var result = noteRepositoryService.CreateNoteRepository(notesRepository);

            var list = new List<string>(4) { "life", "study", "work", "tutorial" };
            foreach (var item in list)
            {
                // 添加笔记本, 生活, 学习, 工作
                var userId = user.UserId;
                var notebook = new Notebook()
                {
                    NotebookId = idGenerator.NextId(),
                    NotesRepositoryId=result.Id,
                    Seq = 0,
                    UserId = userId,
                    CreatedTime = DateTime.Now,
                    Title = item,
                    ParentNotebookId = null,
                };
                notebookService.AddNotebook(notebook);
            }


            if (result == null)
            {
                apiRe.Msg = "数据库创建仓库失败";
                return LeanoteJson(apiRe);
            }
            apiRe.Ok = true;
            apiRe.Data = result;
            return LeanoteJson(apiRe);
        }
        /// <summary>
        /// 删除仓库
        /// </summary>
        /// <param name="token"></param>
        /// <param name="noteRepositoryId"></param>
        /// <returns></returns>
        public IActionResult DeleteNoteRepository(string token, string noteRepositoryId)
        {
            var user = tokenSerivce.GetUserByToken(token);
            var apiRe = new ApiRe()
            {
                Ok = false,
                Data = null
            };
            if (user == null)
            {
                return LeanoteJson(apiRe);
            }
            var verify = noteRepositoryService.Verify(noteRepositoryId.ToLongByHex(), user.UserId, RepositoryAuthorityEnum.DeleteRepository);
            if (!verify)
            {
                return LeanoteJson(apiRe);
            }

            this.noteRepositoryService.DeleteNoteRepository(noteRepositoryId.ToLongByHex());
            apiRe.Ok = true;
            return LeanoteJson(apiRe);

        }
    }
}