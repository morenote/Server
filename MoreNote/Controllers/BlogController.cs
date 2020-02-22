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
        private User ActionInitBlogUser(string blogUserName)
        {
            if (string.IsNullOrEmpty(blogUserName))
            {
                //默认账号
                blogUserName = "hyfree";
            }
            User blogUser = UserService.GetUserByUserName(blogUserName);
            ViewBag.blogUser = blogUser;
            if (blogUser == null)
            {
                return null;
            }
            ViewBag.CateArray = BlogService.GetCateArrayForBlog(blogUser.UserId);
            return blogUser;

        }
        [Route("{controller=Blog}/{action=Index}/{blogUserName?}/{archiveHex?}")]
        public IActionResult Archive(string blogUserName,string archiveHex)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser==null)
            {
                return Content("查无此人");
            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            ViewBag.blog = blog;
            return View();
        }
        [Route("{controller=Blog}/{action=Index}/{blogUserName?}/{cateHex?}/")]
        public IActionResult Cate(string blogUserName,string cateHex, int page)
        {
            long notebookId = MyConvert.HexToLong(cateHex);
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            if (page < 1)
            {
                //页码
                page = 1;
            }
            ViewBag.page = page;

            ViewBag.postCount = BlogService.CountTheNumberForBlogsOfNoteBookId(blogUser.UserId,notebookId);
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentForBlogOfNoteBookId(page,notebookId);
            ViewBag.noteAndContent = noteAndContent;

            if (blogUser == null)
            {
                return Content("查无此人");
            }
            ViewBag.CateArray = BlogService.GetCateArrayForBlog(blogUser.UserId);

            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();


        }

        [Route("{controller=Blog}/{action=Index}/{blogUserName?}")]
        public IActionResult Index(string blogUserName,int page)
        {
            if (page<1)
            {
                //页码
                page = 1;
            }
            ViewBag.page = page;
            if (string.IsNullOrEmpty(blogUserName))
            {
                //默认账号
                blogUserName = "hyfree";
            }
            User blogUser = UserService.GetUserByUserName(blogUserName);
            ViewBag.blogUser = blogUser;
            if (blogUser==null)
            {
                return Content("查无此人");
            }
            ViewBag.postCount = BlogService.CountTheNumberForBlogs(blogUser.UserId);
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentForBlog(page);
            ViewBag.noteAndContent = noteAndContent;
            ViewBag.CateArray = BlogService.GetCateArrayForBlog(blogUser.UserId);
            
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "更多笔记,moreote.top");
            blog.Add("keywords", "搜索");
            ViewBag.blog = blog;

            return View();
        }

        [Route("{controller=Blog}/{action=Post}/{blogUserName?}/{noteIdHex?}/")]
        public IActionResult Post(string blogUserName,string noteIdHex)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            long noteId = MyConvert.HexToLong(noteIdHex);
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
        [Route("{controller=Blog}/{action=Post}/{blogUserName?}/{keywords?}/")]
        public IActionResult Search(string blogUserName,string keywords)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        [Route("{controller=Blog}/{action=Post}/{blogUserName?}/{SingleIdHex?}/")]
        public IActionResult Single(string blogUserName,string SingleIdHex)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        [Route("{controller=Blog}/{action=Post}/{blogUserName?}/{tag?}/")]
        public IActionResult Tags_Posts(string blogUserName, string tag,  int page)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标题");
            blog.Add("keywords", "关键字");
            ViewBag.blog = blog;
            return View();
        }
        [Route("{controller=Blog}/{action=Post}/{blogUserName?}/")]
        public IActionResult Tags(string blogUserName)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
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