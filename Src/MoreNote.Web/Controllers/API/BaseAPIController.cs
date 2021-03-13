using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Utils;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MoreNote.Logic.Entity.ConfigFile;
using UpYunLibrary;

namespace MoreNote.Controllers.API.APIV1
{

    /**
     * 源代码基本是从GO代码直接复制过来的
     * 
     * 只是简单的实现了API的功能
     * 
     * 2020年01月27日
     * */
    public class BaseAPIController : BaseController
    {
   

        public BaseAPIController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
          
        }
    }
}