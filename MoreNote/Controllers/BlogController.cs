using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.Service;

namespace MoreNote.Controllers
{
 

    public class BlogController : Controller
    {
        private async Task InsertLogAsync(string url)
        {
            var headers = Request.Headers;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in headers)
            {
                stringBuilder.Append(item.Key + "---" + item.Value + "\r\n");
            }
            string RealIP = headers["X-Forwarded-For"].ToString().Split(",")[0];
            
            AccessRecords accessRecords = new AccessRecords()
            {
                AccessId = SnowFlake_Net.GenerateSnowFlakeID(),
                IP = RealIP,
                X_Real_IP = headers["X-Real-IP"],
                X_Forwarded_For = headers["X-Forwarded-For"],
                Referrer = headers["Referer"],
                RequestHeader = stringBuilder.ToString(),
                AccessTime = DateTime.Now,
                UnixTime = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                TimeInterval = -1,
                url = url
            };
            await AccessService.InsertAccessAsync(accessRecords).ConfigureAwait(false);
        }
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
        /// <summary>
        /// 博客归档
        /// </summary>
        /// <param name="blogUserName"></param>
        /// <param name="archiveHex"></param>
        /// <returns></returns>
        [Route("{controller=Blog}/{action=Index}/{blogUserName?}/{archiveHex?}")]
        public IActionResult Archive(string blogUserName,string archiveHex)
        {
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser==null)
            {
                return Content("查无此人");
            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            var list = BlogService.GetNotes(blogUser.UserId);
            IOrderedEnumerable<IGrouping<int, Note>> queryArchiveList =
                                from note in list
                                group note by note.PublicTime.Year into newGroup
                                orderby newGroup.Key descending
                                select newGroup;
            ViewBag.queryArchiveList = queryArchiveList;
            ViewBag.blogUser = blogUser;
            blog.Add("Title", "标题");
            ViewBag.blog = blog;
            return View();
        }
     
        [Route("{controller=Blog}/{action=Cate}/{blogUserName?}/{cateHex?}/")]
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
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentForBlogOfNoteBookId(page,notebookId, blogUser.UserId);
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

        [Route("Blog/{blogUserIdHex}")]
        [Route("{controller=Blog}/{action=Index}/{blogUserIdHex?}")]
        public IActionResult Index(string blogUserIdHex, int page)
        {
            if (page<1)
            {
                //页码
                page = 1;
            }
            ViewBag.page = page;
            User blogUser=null;
            if (string.IsNullOrEmpty(blogUserIdHex))
            {

                //默认账号
               string blogUserName = "hyfree";
                blogUser = UserService.GetUserByUserName(blogUserName);
            }
            else
            {
                blogUser = UserService.GetUserByUserId(MyConvert.HexToLong(blogUserIdHex));
            }

            if (blogUser == null)
            {
                blogUser = UserService.GetUserByUserName(blogUserIdHex);
                if (blogUser == null)
                {
                    return Content("查无此人");
                }

            }
            ViewBag.blogUser = blogUser;
           
            ViewBag.postCount = BlogService.CountTheNumberForBlogs(blogUser.UserId);
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentForBlog(page,blogUser.UserId);
            ViewBag.noteAndContent = noteAndContent;
            ViewBag.CateArray = BlogService.GetCateArrayForBlog(blogUser.UserId);
            
            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "moreote云笔记");
            blog.Add("keywords", "搜索");
            ViewBag.blog = blog;

            return View();
        }
        [Route("Blog/Post/{noteIdHex}/")]
        public IActionResult Post1(string noteIdHex)
        {
            long noteId = MyConvert.HexToLong(noteIdHex);
            Note note = NoteService.GetNoteById(noteId);
            User user = UserService.GetUserByUserId(note.UserId);
            return Redirect($"/Blog/Post/{user.Username}/{noteIdHex}");
        }
        [Route("Blog/Post/{blogUserName}/{noteIdHex}/")]
        public async Task<IActionResult> PostAsync(string blogUserName,string noteIdHex)
        {
            //添加访问日志
            await InsertLogAsync($"Blog/Post/{blogUserName}/{noteIdHex}/").ConfigureAwait(false);

            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("查无此人");
            }
            long noteId = MyConvert.HexToLong(noteIdHex);
            if (noteId==0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("未找到");

            }
            Dictionary<string, string> blog = new Dictionary<string, string>();
            NoteAndContent noteAndContent= NoteService.GetNoteAndContent(noteId);
            NoteService.AddReadNum(noteId);
            if (noteAndContent==null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("未经授权的访问");
            }
            if (noteAndContent.note.IsDeleted || noteAndContent.note.IsTrash)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Content("这篇文章已经被删除");
            }
            if (!noteAndContent.note.IsBlog)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return Content("这篇文章已经被取消分享");
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
                Response.StatusCode = (int)HttpStatusCode.NotFound;
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
            if (page < 1)
            {
                //页码
                page = 1;
            }
            ViewBag.page = page;
          
            User blogUser = ActionInitBlogUser(blogUserName);
            if (blogUser == null)
            {
                return Content("查无此人");
            }
            ViewBag.blogUser = blogUser;

            ViewBag.postCount = BlogService.CountTheNumberForBlogTags(blogUser.UserId,tag);
            NoteAndContent[] noteAndContent = NoteService.GetNoteAndContentByTag(page, blogUser.UserId,tag);
            ViewBag.noteAndContent = noteAndContent;
            ViewBag.CateArray = BlogService.GetCateArrayForBlog(blogUser.UserId);

            Dictionary<string, string> blog = new Dictionary<string, string>();
            blog.Add("Title", "标签检索");
            blog.Add("keywords", "搜索");
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
            string[] tags = TagService.GetBlogTags(blogUser.UserId);
            ViewBag.tags = tags;
            ViewBag.blogUser = blogUser;
            blog.Add("Title", "标签云");
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
            string json = "jsonpCallback({\"Ok\":true," +
                "\"Code\":0,\"Msg\":\"\",\"Id\":\"\",\"List\":null,\"Item\":{\"commentUserInfo\":null,\"comments\":null,\"hasMoreLikedUser\":false,\"isILikeIt\":false,\"likedUsers\":null,\"pageInfo\":{\"CurPage\":1,\"TotalPage\":0,\"PerPageSize\":0,\"Count\":0,\"List\":null}}});";
            return Content(json);

        }  
        
    }
}