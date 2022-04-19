using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Morenote.Framework.Filter.Global;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyOrganization;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.Enum;

using System;
using System.Collections.Generic;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Notebook/[action]")]
    // [ApiController]
    [ServiceFilter(typeof(CheckTokenFilter))]
    public class NotebookAPIController : APIBaseController
    {
        private NotebookService notebookService;
        private NoteRepositoryService noteRepositoryService;
        private OrganizationMemberRoleService repositoryMemberRoleService;
      
        private NoteService noteService;
        public NotebookAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService,
            NoteService noteService,


            ILoggingService loggingService
            , OrganizationMemberRoleService repositoryMemberRoleService
            , NoteRepositoryService noteRepositoryService)
            :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
            this.repositoryMemberRoleService = repositoryMemberRoleService;
            this.noteService = noteService;
        }

        //获取同步的笔记本
        //[HttpPost]
        public JsonResult GetSyncNotebooks(string token, int afterUsn, int maxEntry)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
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
            Notebook[] notebook = notebookService.GeSyncNotebooks(user.UserId, afterUsn, maxEntry);
            return Json(notebook, MyJsonConvert.GetLeanoteOptions());
        }

        public ApiNotebook[] fixNotebooks(Notebook[] notebooks)
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

        public ApiNotebook fixNotebook(Notebook notebook)
        {
            return new ApiNotebook()
            {
                NotebookId = notebook.NotebookId,
                UserId = notebook.UserId,
                ParentNotebookId = notebook.ParentNotebookId,
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
        public IActionResult GetNotebooks(string token)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };

                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                Notebook[] notebooks = notebookService.GetAll(user.UserId);
                ApiNotebook[] apiNotebooks = fixNotebooks(notebooks);
                return Json(apiNotebooks, MyJsonConvert.GetLeanoteOptions());
            }

            return null;
        }

        //添加notebook
        public IActionResult AddNotebook(string token, string title, string parentNotebookId, int seq)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };

                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                Notebook notebook = new Notebook()
                {
                    NotebookId = SnowFlakeNet.GenerateSnowFlakeID(),
                    Title = title,
                    Seq = seq,
                    UserId = user.UserId,
                    ParentNotebookId = parentNotebookId.ToLongByHex()
                };
                if (notebookService.AddNotebook(ref notebook))
                {
                    ApiNotebook apiNotebook = fixNotebook(notebook);

                    return Json(apiNotebook, MyJsonConvert.GetLeanoteOptions());
                }
                else
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "AddNotebook is error",
                    };

                    return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
                }
            }
        }

        //修改笔记
        public IActionResult UpdateNotebook(string token, string notebookId, string title, string parentNotebookId, int seq, int usn)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };

                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                Notebook notebook;
                if (notebookService.UpdateNotebookApi(user.UserId, notebookId.ToLongByHex(), title, parentNotebookId.ToLongByHex(), seq, usn, out notebook))
                {
                    ApiNotebook apiNotebook = fixNotebook(notebook);

                    return Json(apiNotebook, MyJsonConvert.GetLeanoteOptions());
                }
                else
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "UpdateNotebook is error",
                    };

                    return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
                }
            }
        }

        //todo:删除笔记本
        public IActionResult DeleteNotebook(string token, string notebookId)
        {
            User user = tokenSerivce.GetUserByToken(token);
            ApiRe re = new ApiRe()
            {
                Ok = false,
                Msg = "NOTLOGIN",
            };

            if (user == null)
            {
                re.Msg = "NOTLOGIN";
                
              return LeanoteJson(re);
            }
            var message="";
            var notebook=notebookService.GetNotebookById(notebookId.ToLongByHex());
            var notesRepositoryId = notebook.NotesRepositoryId;
            var verify=noteRepositoryService.Verify(notesRepositoryId,user.UserId,RepositoryAuthorityEnum.Write);
            if (verify == false)
            {
                return LeanoteJson(verify);
            }
            
            var usn=noteRepositoryService.IncrUsn(notesRepositoryId);

            if (notebookService.DeleteNotebook( notebookId.ToLongByHex(), usn,out message))
            {

                ApiRe apiRe = new ApiRe()
                {
                    Ok = true,
                    Msg = "success",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "conflict",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
        }
        /// <summary>
        /// 递归删除笔记本
        /// </summary>
        /// <param name="token"></param>
        /// <param name="notebookId"></param>
        /// <returns></returns>
        public IActionResult DeleteNotebookRecursively(string token, string notebookId)
        {
            User user = tokenSerivce.GetUserByToken(token);
            ApiRe re = new ApiRe()
            {
                Ok = false,
                Msg = "NOTLOGIN",
            };

            if (user == null)
            {
                re.Msg = "NOTLOGIN";

                return LeanoteJson(re);
            }
            var message = "";
            var notebook = notebookService.GetNotebookById(notebookId.ToLongByHex());
            var notesRepositoryId = notebook.NotesRepositoryId;
            var verify = noteRepositoryService.Verify(notesRepositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (verify == false)
            {
                return LeanoteJson(verify);
            }

            var usn = noteRepositoryService.IncrUsn(notesRepositoryId);
            if (notebookService.DeleteNotebook(notebookId.ToLongByHex(), usn, out message))
            {

                ApiRe apiRe = new ApiRe()
                {
                    Ok = true,
                    Msg = "success",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "conflict",
                };
                return Json(apiRe, MyJsonConvert.GetLeanoteOptions());
            }

        }

        //获得笔记本的第一层文件夹
        public IActionResult GetRootNotebooks(string token, string repositoryId)
        {
            var apiRe=new ApiRe();

            var user = tokenSerivce.GetUserByToken(token);
            if (user != null)
            {
                //var repository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());

                //var memerRole = noteRepositoryService.GetRepositoryMemberRole(repositoryId.ToLongByHex());

                //检查用户是否对仓库具有读权限
                if (noteRepositoryService.Verify(repositoryId.ToLongByHex(),user.UserId,RepositoryAuthorityEnum.Read))
                {
                    var books = notebookService.GetRootNotebooks(repositoryId.ToLongByHex());
                    apiRe.Ok = true;
                    apiRe.Data=books;
                }
            }
            return  LeanoteJson(apiRe);
        }
        


        public IActionResult GetNotebookChildren(string token, string notebookId)
        {
            var apiRe = new ApiRe();

            var user = tokenSerivce.GetUserByToken(token);

            

            if (user != null)
            {
                //var repository = noteRepositoryService.GetNotesRepository(repositoryId.ToLongByHex());

                //var memerRole = noteRepositoryService.GetRepositoryMemberRole(repositoryId.ToLongByHex());

                var book=notebookService.GetNotebookById(notebookId.ToLongByHex());
                if (book==null)
                {
                    return LeanoteJson(apiRe);

                }
                //检查用户是否对仓库具有读权限
                if (noteRepositoryService.Verify(book.NotesRepositoryId, user.UserId, RepositoryAuthorityEnum.Read))
                {
                    var note = notebookService.GetNotebookChildren(notebookId.ToLongByHex());
                    apiRe.Ok = true;
                    apiRe.Data = note;
                }
            }
            return LeanoteJson(apiRe);
        }
        public IActionResult CreateNoteBook(string token,string noteRepositoryId, string notebookTitle, string parentNotebookId)
        {
            var re = new ApiRe();
            var user = tokenSerivce.GetUserByToken(token);
            long? parentId=null;
            bool  verify=false;
            long? repositoryId=null;

            if (string.IsNullOrEmpty(noteRepositoryId))
            {
                return LeanoteJson(re);
            }

            if (string.IsNullOrEmpty(parentNotebookId))
            {

                verify = noteRepositoryService.Verify(noteRepositoryId.ToLongByHex(), user.UserId, RepositoryAuthorityEnum.Write);
                repositoryId=noteRepositoryId.ToLongByHex();
            }
            else
            {
                var parentNotebook = notebookService.GetNotebookById(parentNotebookId.ToLongByHex());
                if (user == null || parentNotebook == null)
                {
                    return LeanoteJson(re);
                }
                repositoryId = parentNotebook.NotesRepositoryId;
                if (repositoryId != noteRepositoryId.ToLongByHex())
                {
                    return LeanoteJson(re);
                }
                verify = noteRepositoryService.Verify(repositoryId, user.UserId, RepositoryAuthorityEnum.Write);
                parentId=parentNotebook.NotebookId;
            }

         
            if (!verify)
            {
                return LeanoteJson(re);
            }
            var notebook = new Notebook()
            {
                NotebookId = SnowFlakeNet.GenerateSnowFlakeID(),
                NotesRepositoryId = repositoryId,
                Seq = 0,
                UserId = user.UserId,
               
                CreatedTime = DateTime.Now,
                Title = notebookTitle,
                ParentNotebookId = parentId,
            };

            notebookService.AddNotebook(notebook);
            re.Ok=true;
            re.Data = notebook; 

            return LeanoteJson(re);


        }
        public IActionResult UpdateNoteBookTitle(string token, string notebookId,string notebookTitle)
        {
            var re = new ApiRe();
            var user = tokenSerivce.GetUserByToken(token);
            var notebook = notebookService.GetNotebookById(notebookId.ToLongByHex());

            if (user == null || notebook == null)
            {
                return LeanoteJson(re);
            }
            var repositoryId = notebook.NotesRepositoryId;
            var verify = noteRepositoryService.Verify(repositoryId, user.UserId, RepositoryAuthorityEnum.Write);
            if (!verify)
            {
                return LeanoteJson(re);
            }
           

            notebookService.UpdateNotebookTitle(notebook.NotebookId, notebookTitle);
            re.Ok = true;
            re.Data = notebook;

            return LeanoteJson(re);


        }
    }
 
}