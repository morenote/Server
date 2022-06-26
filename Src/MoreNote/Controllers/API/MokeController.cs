using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Logic.Service;
using MoreNote.Logic.Service.MyRepository;
using MoreNote.Models.DTO.ng;

using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MoreNote.Controllers.API.APIV1
{
    [Route("api/Common/[action]")]
    public class MokeController : APIBaseController
    {
        private AttachService attachService;
        private NoteService noteService;
        private TokenSerivce tokenSerivce;
        private NoteContentService noteContentService;
        private NotebookService notebookService;
        private TrashService trashService;
        private IHttpContextAccessor accessor;
        private NoteRepositoryService noteRepositoryService;

        public MokeController(AttachService attachService
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

            int[] temp = { 7, 5, 4, 2, 4, 7, 5, 6, 5, 9, 6, 3, 1, 5, 3, 6, 5 };
            int[] fakeY2temp = { 1, 6, 4, 8, 3, 7, 2 };
            fakeY = temp;
            fakeY2 = fakeY2temp;
            for (int i = 0; i < temp.Length; i++)
            {
                visitData.Add(new VisitData()
                {
                    x = DateTime.Now,
                    y = fakeY[i]
                });
            }

            for (int i = 0; i < fakeY2.Length; i++)
            {
                visitData2.Add(new VisitData()
                {
                    x = DateTime.Now,
                    y = fakeY2[i]
                });
            }

        }

        private DateTime beginDay = DateTime.Now;
        private int[] fakeY = null;
        private int[] fakeY2 = null;
        private List<VisitData> visitData;
        private List<VisitData> visitData2;
    }
        
  
}