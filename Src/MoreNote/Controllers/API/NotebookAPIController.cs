using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Collections.Generic;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Notebook/[action]")]
    // [ApiController]
    public class NotebookAPIController : APIBaseController
    {
        private NotebookService notebookService;

        public NotebookAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.notebookService = notebookService;
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

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            if (maxEntry == 0)
            {
                maxEntry = 100;
            }
            Notebook[] notebook = notebookService.GeSyncNotebooks(user.UserId, afterUsn, maxEntry);
            return Json(notebook, MyJsonConvert.GetOptions());
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

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                Notebook[] notebooks = notebookService.GetAll(user.UserId);
                ApiNotebook[] apiNotebooks = fixNotebooks(notebooks);
                return Json(apiNotebooks, MyJsonConvert.GetOptions());
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

                return Json(apiRe, MyJsonConvert.GetOptions());
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

                    return Json(apiNotebook, MyJsonConvert.GetOptions());
                }
                else
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "AddNotebook is error",
                    };

                    return Json(apiRe, MyJsonConvert.GetOptions());
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

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                Notebook notebook;
                if (notebookService.UpdateNotebookApi(user.UserId, notebookId.ToLongByHex(), title, parentNotebookId.ToLongByHex(), seq, usn, out notebook))
                {
                    ApiNotebook apiNotebook = fixNotebook(notebook);

                    return Json(apiNotebook, MyJsonConvert.GetOptions());
                }
                else
                {
                    ApiRe apiRe = new ApiRe()
                    {
                        Ok = false,
                        Msg = "UpdateNotebook is error",
                    };

                    return Json(apiRe, MyJsonConvert.GetOptions());
                }
            }
        }

        //todo:删除笔记本
        public IActionResult DeleteNotebook(string token, string notebookId, int usn)
        {
            User user = tokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "NOTLOGIN",
                };

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            if (notebookService.DeleteNotebookForce(user.UserId, notebookId.ToLongByHex(), usn))
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = true,
                    Msg = "success",
                };
                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "conflict",
                };
                return Json(apiRe, MyJsonConvert.GetOptions());
            }
        }
    }
}