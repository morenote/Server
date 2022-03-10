using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Controllers.API.APIV1;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

namespace MoreNote.Controllers.API
{
    /// <summary>
    /// 笔记仓库
    /// </summary>
    public class NotesRepositoryController : APIBaseController
    {
        private NotebookService notebookService;
        private NoteRepositoryService noteRepositoryService;
        public NotesRepositoryController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
            ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetNoteRepository(string token)
        {
            var user = GetUserByToken(token);
            var rep = noteRepositoryService.GetNoteRepositoryList(user.UserId);
            var apiRe = new ApiRe()
            {
                Ok = true,
                Data = rep
            };
            return SimpleJson(apiRe);

        }
    }
}
