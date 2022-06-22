using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Morenote.Framework.Filter.Global;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.Enum;

using System;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Common/[action]")]
    
    // [ApiController]
    public class CommonController : APIBaseController
    {
        private AttachService attachService;
        private NoteService noteService;
        private TokenSerivce tokenSerivce;
        private NoteContentService noteContentService;
        private NotebookService notebookService;
        private TrashService trashService;
        private IHttpContextAccessor accessor;
        private NoteRepositoryService noteRepositoryService;

        public CommonController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor,
            NoteService noteService,
            NoteContentService noteContentService,
            NotebookService notebookService,
            NoteRepositoryService noteRepositoryService,
            TrashService trashService
           ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
            this.attachService = attachService;
            this.noteService = noteService;
            this.tokenSerivce = tokenSerivce;
            this.noteContentService = noteContentService;
            this.trashService = trashService;
            this.accessor = accessor;
            this.notebookService = notebookService;
            this.noteRepositoryService = noteRepositoryService;
        }
        /// <summary>
        /// 获得Hex形式的ID
        /// </summary>
        /// <returns></returns>
       
       public IActionResult GetHexId()
        {
            var re = new ApiRe();
            re.Ok = true;
            re.Data = this.idGenerator.NextHexId();
            return LeanoteJson(re);

        }




    }
}