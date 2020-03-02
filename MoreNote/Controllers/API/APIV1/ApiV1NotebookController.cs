
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System.Collections.Generic;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Notebook/[action]")]
   // [ApiController]
    public class ApiV1NotebookController : ApiV1BaseController
    {
        public ApiV1NotebookController(IHttpContextAccessor accessor) : base(accessor)
        {

        }

        //获取同步的笔记本
        //[HttpPost]
        public JsonResult GetSyncNotebooks( string token,int afterUsn,int maxEntry)
        {
            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };
              
                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            Notebook[] notebook = NotebookService.GeSyncNotebooks(user.UserId, afterUsn, maxEntry);
                return Json(notebook, MyJsonConvert.GetOptions());
        }
        
        public ApiNotebook[] fixNotebooks(Notebook[] notebooks)
        {
            ApiNotebook[] apiNotebooks = null;
            if (notebooks!=null)
            {
                apiNotebooks= new ApiNotebook[notebooks.Length];
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
            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                Notebook[] notebooks = NotebookService.GetAll(user.UserId);
                ApiNotebook[] apiNotebooks = fixNotebooks(notebooks);
                return Json(apiNotebooks, MyJsonConvert.GetOptions());
            }
       
            return null;
        }

        //添加notebook
        public IActionResult AddNotebook(string token, string title,string parentNotebookId,int seq)
        {
            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                Notebook notebook = new Notebook()
                {
                    NotebookId = SnowFlake_Net.GenerateSnowFlakeID(),
                    Title = title,
                    Seq = seq,
                    UserId = user.UserId,
                    ParentNotebookId = MyConvert.HexToLong(parentNotebookId)
                    
                };
                if (NotebookService.AddNotebook( ref notebook))
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
        public IActionResult UpdateNotebook(string token, string notebookId,string title,string parentNotebookId, int seq ,int usn)
        {
            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            else
            {
                Notebook notebook;
                if (NotebookService.UpdateNotebookApi(user.UserId,MyConvert.HexToLong(notebookId),title, MyConvert.HexToLong(parentNotebookId), seq,usn,out notebook))
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
        public IActionResult DeleteNotebook(string token,string notebookId,int usn)
        {

            User user = TokenSerivce.GetUserByToken(token);
            if (user == null)
            {
                ApiRe apiRe = new ApiRe()
                {
                    Ok = false,
                    Msg = "Not logged in",
                };

                return Json(apiRe, MyJsonConvert.GetOptions());
            }
            if (NotebookService.DeleteNotebookForce(user.UserId, MyConvert.HexToLong(notebookId), usn))
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