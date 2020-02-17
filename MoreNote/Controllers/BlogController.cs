using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
 

    public class BlogController : Controller
    {
        public IActionResult Archive()
        {

            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult Cate()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();

        }

        [Route("{controller=Blog}/{action=Index}/{blogUserName?}")]
        public IActionResult Index(string userName,int page)
        {
            if (page<1)
            {
                //页码
                page = 1;
            }
            ViewBag.page = page;
            if (string.IsNullOrEmpty(userName))
            {
                //默认账号
                userName = "hyfree";
            }
            User blogUser = UserService.GetUserByUserName(userName);
            ViewBag.blogUser = blogUser;
            if (blogUser==null)
            {
                return Content("查无此人");
            }
            ViewBag.postCount = BlogService.CountTheNumberOfBlogs(blogUser.UserId);
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentForBlog(page);
            ViewBag.noteAndContent = noteAndContent;
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "更多笔记,moreote.top");
            blog.Add("keywords", "搜索");
            ViewBag.blog = blog;

            return View();
        }
        public IActionResult Post(string id)
        {
            long noteId = MyConvert.HexToLong(id);
            if (noteId==0)
            {
                return Content("未找到");

            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            NoteAndContent noteAndContent= NoteService.GetNoteAndContent(noteId);
            if (noteAndContent==null)
            {
                return Content("未经授权的访问");
            }
            if (!noteAndContent.note.IsBlog)
            {
                return Content("未经授权的访问");
            }
            ViewBag.noteAndContent = noteAndContent;
            blog.Add("Title", noteAndContent.note.Title);
            blog.Add("NoteTitle", noteAndContent.note.Title);
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult Search()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult Single()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult Tags_Posts()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult Tags()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult NoFound()
        {
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        public IActionResult IncReadNum()
        {
            return Content("");

        }  
        public IActionResult getLikesAndComments()
        {
            return Content("");

        }  
        
    }
}