﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;
using Microsoft.AspNetCore.Http;
using MoreNote.Logic.Service.Logging;
// ReSharper disable All
namespace MoreNote.Controllers.Member
{
    public class MemberBaseController : BaseController
    {
        public MemberBaseController(AttachService attachService
          , TokenSerivce tokenSerivce
          , NoteFileService noteFileService
          , UserService userService
          , ConfigFileService configFileService
          , IHttpContextAccessor accessor
            
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {

        }
    }
}