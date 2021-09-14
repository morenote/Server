using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Logic.DB;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers.Admin
{
    public class AdminBlogController : AdminBaseController
    {


          public AdminBlogController(AttachService attachService
              , TokenSerivce tokenSerivce
              , NoteFileService noteFileService
              , UserService userService
              , ConfigFileService configFileService
              , IHttpContextAccessor accessor,
              AccessService accessService
    

              ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {

        }
    }
}