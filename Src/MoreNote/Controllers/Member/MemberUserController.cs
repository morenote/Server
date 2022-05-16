using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;
using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.Value;

namespace MoreNote.Controllers.Member
{
    [Route("/member/user/{action=Username}")]
    [Authorize(Roles = "Admin,SuperAdmin,User")]
    public class MemberUserController : BaseController
    {
        public ConfigService ConfigService { get;set;}
        public MemberUserController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , ConfigService configService
            , IHttpContextAccessor accessor
           , ILoggingService loggingService) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor, loggingService)
        {
            this.ConfigService=configService;

        }
      //  [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Username()
        {
         
            User user = GetUserBySession();
            ViewBag.user = user;
            SetUserInfo();
            SetLocale();
            ViewBag.title = GetLanguageResource().GetMember()["Username"];
            return View("Views/Member/user/Username.cshtml");
        }
        //[Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Email()
        {
         
            User user = GetUserBySession();
            ViewBag.user = user;
            SetLocale();
            SetUserInfo();
            ViewBag.title = "电子邮箱";
            return View("Views/Member/user/email.cshtml");
        }
        //[Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Password()
        {
            User user = GetUserBySession();
            ViewBag.user = user;
            SetLocale();
             SetUserInfo();
            ViewBag.title = "密码";
            return View("Views/Member/user/password.cshtml");
        }

   

         public IActionResult Editor()
        {
            User user = GetUserBySession();
            ViewBag.user = user;
            SetLocale();
            SetUserInfo();
            ViewBag.md=user.MarkdownEditorOption;
            ViewBag.rt=user.RichTextEditorOption;
            ViewBag.title = "Select Editor Preferences";
            return View("Views/Member/user/editor.cshtml");
        }
       // [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }
       // [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Avatar()
        {
          

            User user = GetUserBySession();
            ViewBag.user = user;
            SetLocale();
             SetUserInfo();
            ViewBag.title= GetLanguageResource().GetMember()["Avatar"];
            ViewBag.globalConfigs= ConfigService.GetGlobalConfigForUser();

            return View("Views/Member/user/avatar.cshtml");
        }

             public IActionResult FIDO2()
        {
            User user = GetUserBySession();
            ViewBag.user = user;
          
            SetLocale();
             SetUserInfo();
            ViewBag.title = "FIDO2 Setting Options";
            userService.InitFIDO2Repositories(user.UserId);
            ViewBag.fido2Repositories=user.FIDO2Items;
            return View("Views/Member/user/fido2.cshtml");
        }


            public IActionResult AddTestFIDO2()
        {
            var user=this.GetUserBySession();
            var fido=new FIDO2Item()
            {
                Id=idGenerator.NextId(),
                UserId=user.UserId,
                CredentialId=System.Text.Encoding.Default.GetBytes ( "1111" ),
                UserHandle=System.Text.Encoding.Default.GetBytes ( "1111" ),
                SignatureCounter=0,
                CredType="TPM",

                PublicKey= System.Text.Encoding.Default.GetBytes ( "1111" ),
                RegDate=DateTime.Now,
                AaGuid=new Guid()

                
            };
            userService.AddFIDO2Repository(user.UserId, fido);

            return Ok("success");
            
        }
    }
}