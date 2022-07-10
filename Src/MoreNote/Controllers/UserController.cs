using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Framework.Controllers;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoreNote.Controllers
{
    public class UserController : BaseController
    {
        public UserController(AttachService attachService
            , TokenSerivce tokenSerivce
            , NoteFileService noteFileService
            , UserService userService
            , ConfigFileService configFileService
            , IHttpContextAccessor accessor
             ) :
            base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
        {
        }

        public IActionResult Account(int tab)
        {
            //todo:Account
            return null;
        }

        

        [HttpPost]
        public async Task<IActionResult> UpdatePwd(string oldPwd, string pwd)
        {
            var re = new ResponseMessage();
            if (string.IsNullOrEmpty(oldPwd) || string.IsNullOrEmpty(pwd) || pwd.Length < 6)
            {
                re.Msg = "Password length is too short";
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }

            User user = GetUserBySession();
            if (user == null)
            {
                re.Msg = "Unable to obtain user information through Session ";
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }

            if (user.Username.Equals(config.SecurityConfig.DemoUsername))
            {
                re.Msg = "cannotUpdateDemo";
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
            string error;

            if (userService.VDPassWord(pwd, user.UserId, out error))
            {
                re.Ok =  await userService.UpdatePwd(user.UserId, oldPwd, pwd);
                re.Msg = "The update password is wrong, please check the password you provided ";
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
            else
            {
                re.Msg = error;
                re.Ok = false;
                return Json(re, MyJsonConvert.GetLeanoteOptions());
            }
        }

 
        /// <summary>
        /// 设置宽度
        /// </summary>
        /// <param name="notebookWidth">笔记本div的宽度</param>
        /// <param name="noteListWidth">笔记div的宽度</param>
        /// <param name="mdEditorWidth">编辑器div的宽度</param>
        /// <returns></returns>
        public IActionResult UpdateColumnWidth(int notebookWidth, int noteListWidth, int mdEditorWidth)
        {
            var re = new ResponseMessage();

            var userId = GetUserIdBySession();
            re.Ok = userService.UpdateColumnWidth(userId, notebookWidth, noteListWidth, mdEditorWidth);

            return Json(re, MyJsonConvert.GetLeanoteOptions());
        }
        /// <summary>
        /// 设置是否隐藏笔记本div
        /// </summary>
        /// <param name="leftIsMin"></param>
        /// <returns></returns>
        public IActionResult UpdateLeftIsMin(bool leftIsMin)
        {
            var re = new ResponseMessage();

            var userId = GetUserIdBySession();
            re.Ok = userService.UpdateLeftIsMin(userId, leftIsMin);

            return Json(re, MyJsonConvert.GetLeanoteOptions());
        }
    }
}