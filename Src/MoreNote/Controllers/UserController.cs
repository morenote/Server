using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class UserController: BaseController
    {
        public UserController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {


        }

        public IActionResult Account(int tab)
        {
            //todo:Account
            return null;
        }
        [HttpPost]
        public IActionResult UpdateUsername(string username)
        {
            var re = new ResponseMessage();
            if (string.IsNullOrEmpty(username)||username.Length<4)
            {
                re.Msg = "Unable to obtain user information through Session ";
                return Json(re, MyJsonConvert.GetOptions());
            }
            if (username.Length>16)
            {
                re.Msg = "Name string length>16";
                return Json(re, MyJsonConvert.GetOptions());

            }
            var message=string.Empty;
            User user=GetUserBySession();
            if (user == null)
            {
                re.Msg= "Unable to obtain user information through Session ";
                return Json(re,MyJsonConvert.GetOptions());
            }
            if (user.Username.Equals(config.SecurityConfig.DemoUsername))
            {
                re.Msg= "cannotUpdateDemo";
                return Json(re, MyJsonConvert.GetOptions());

            }
            if (userService.VDUserName(username,out message))
            {
                re.Ok=userService.UpdateUsername(user.UserId,username);
                re.Msg=message;
                return Json(re, MyJsonConvert.GetOptions());
            }
            else
            {
                re.Msg = "Incorrect username format or conflict";
                return Json(re, MyJsonConvert.GetOptions());
            }
        }
        [HttpPost]
        public IActionResult UpdatePwd(string oldPwd,string pwd)
        {
            var re = new ResponseMessage();
            if (string.IsNullOrEmpty(oldPwd) ||string.IsNullOrEmpty(pwd)|| pwd.Length < 6)
            {
                re.Msg = "Password length is too short";
                return Json(re, MyJsonConvert.GetOptions());
            }

            User user = GetUserBySession();
            if (user == null)
            {
                re.Msg = "Unable to obtain user information through Session ";
                return Json(re, MyJsonConvert.GetOptions());
            }

            if (user.Username.Equals(config.SecurityConfig.DemoUsername))
            {
                re.Msg = "cannotUpdateDemo";
                return Json(re, MyJsonConvert.GetOptions());

            }
            string error;

            if (userService.VDPassWord(pwd, user.UserId, out error))
            {
                re.Ok = userService.UpdatePwd(user.UserId, oldPwd, pwd);
                re.Msg= "The update password is wrong, please check the password you provided ";
                return Json(re, MyJsonConvert.GetOptions());
            }
            else
            {
                re.Msg=error;
                re.Ok=false;
                return Json(re, MyJsonConvert.GetOptions());
            }
        }
        [HttpPost]
        public IActionResult SetEditorPreferences(string mdOption,string rtOption)
        {

            var re = new ResponseMessage();
            var mdHashSet=new HashSet<string>();
            mdHashSet.Add("ace");
            mdHashSet.Add("vditor");

            var rthashSet=new HashSet<string>();
            rthashSet.Add("tinymce");
            rthashSet.Add("textbus");
            //参数判断
            if (string.IsNullOrEmpty(mdOption)||!mdHashSet.Contains(mdOption)||string.IsNullOrEmpty(rtOption)||!rthashSet.Contains(rtOption))
            {
                re.Msg="Parameter error ";
                re.Ok=false;
                return Json(re,MyJsonConvert.GetSimpleOptions());
            }
            var user=GetUserBySession();
            //设置编辑器偏好
            userService.SetEditorPreferences(user.UserId, mdOption,rtOption);

             re.Ok=true;
            return Json(re,MyJsonConvert.GetSimpleOptions());
        }

    }
}
