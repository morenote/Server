using MoreNote.Common.ExtensionMethods;
using MoreNote.Common.Helper;
using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service
{
    // blog
    /*
    note, notebook都可设为blog
    关键是, 怎么得到blog列表? 还要分页

    ??? 不用新建, 直接使用notes表, 添加IsBlog字段. 新建表 blogs {NoteId, UserId, CreatedTime, IsTop(置顶)}, NoteId, UserId 为unique!!

    // 设置一个note为blog
    添加到blogs中

    // 设置/取消notebook为blog
    创建一个note时, 如果其notebookId已设为blog, 那么添加该note到blog中.
    设置一个notebook为blog时, 将其下所有的note添加到blogs里 -> 更新其IsBlog为true
    取消一个notebook不为blog时, 删除其下的所有note -> 更新其IsBlog为false

    */

    public class BlogService
    {
        private DataContext dataContext;
        public NoteService NoteService { get; set; }
        public NoteContentService NoteContentService { get; set; }
        public UserService UserService { get; set; }
        public ConfigService ConfigService { get; set; }
        public CommonService CommonService { get; set; }

        public BlogService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public BlogStat GetBlogStat(long? noteId)
        {
            var note = NoteService.GetBlogNote(noteId);
            var stat = new BlogStat()
            {
                NodeId = note.NoteId,
                ReadNum = note.ReadNum,
                LikeNum = note.LikeNum,
                CommentNum = note.CommentNum
            };
            return stat;
        }

        /// <summary>
        /// 统计网站上公开的Post的数量
        /// </summary>
        public int CountTheNumberForBlogs(long? userId)
        {
            var count = dataContext.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId).Count();
            return count;
        }

        public int CountTheNumberForBlogTags(long? userId, string tag)
        {
            var count = dataContext.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId && b.Tags.Contains(tag)).Count();
            return count;
        }

        public Note[] GetNotes(long? userid)
        {
            var result =
                dataContext.Note.Where(note => note.IsBlog == true && note.IsDeleted == false && note.IsTrash == false && note.UserId == userid).OrderByDescending(note => note.PublicTime).ToArray();
            return result;
        }

        public int CountTheNumberForBlogsOfNoteBookId(long? userId, long? notebookId)
        {
            var count = dataContext.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId && b.NotebookId == notebookId).Count();
            return count;
        }

        public int CountTheNumberForBlogsOfTag(long? userId, string tag)
        {
            var count = dataContext.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId && b.Tags.Contains(tag)).Count();
            return count;
        }

        public BlogItem GetBlogByIdAndUrlTitle(long? userId, string noteIdOrUrlTitle)
        {
            if (Util.IsObjectId(noteIdOrUrlTitle))
            {
                return GetBlog(noteIdOrUrlTitle.ToLongByHex());
            }
            else
            {
                var note = dataContext.Note.Where(b => b.UserId == userId && b.Title == noteIdOrUrlTitle
                              && b.IsBlog == true
                              && b.IsTrash == false
                              && b.IsDeleted == false).FirstOrDefault();
                return GetBlogItem(note);
            }
        }

        public BlogItem GetBlog(long? noteId)
        {
            var note = dataContext.Note.Where(b => b.NoteId == noteId).FirstOrDefault();
            return GetBlogItem(note);
        }

        public BlogItem GetBlogItem(Note note)
        {
            if (note == null || !note.IsBlog)
            {
                return new BlogItem();
            }

            //内容
            var noteContent = NoteContentService.GetNoteContent(note.NoteId, note.UserId);
            // 组装成blogItem
            User user = UserService.GetUserByUserId(note.UserId);
            var blog = new BlogItem()
            {
                Note = note,
                Abstract = noteContent.Abstract,
                Content = noteContent.Content,
                HasMore = false,
                User = user
            };
            return blog;
        }

        public Notebook[] ListBlogNotebooks(long? userId)
        {
            var noteBooks = dataContext.Notebook.Where(b => b.UserId == userId && b.IsBlog == true).ToArray();
            return noteBooks;
        }

        /// <summary>
        /// 博客列表
        /// userId 表示谁的blog
        /// </summary>
        /// <param name="uerId"></param>
        /// <param name="noteBookId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortField"></param>
        /// <param name="isAsc"></param>
        /// <param name="pageObj"></param>
        /// <param name="blogItem"></param>
        public void ListBlogs(long? userId, long? noteBookId, int page, int pageSize, string sortField, bool isAsc, out Page pageObj, out BlogItem blogItem)
        {
            

            var notes = NoteService.ListNotes(userId, noteBookId, false, page, pageSize, sortField, isAsc, true);
            if (notes == null || notes.Count == 0)
            {
                pageObj = new Page();
                blogItem = null;
                return;
            }
            throw new Exception();
        }

        public BlogInfo GetBlogInfo(UserBlog userBlog, User userinfo)
        {
            BlogInfo blogInfo = new BlogInfo()
            {
                UserId = userinfo.UserId.ToHex24(),
                Username = userinfo.Username,
                UserLogo = userinfo.Logo,
                Title = userBlog.Title,
                SubTitle = userBlog.SubTitle,
                Logo = userBlog.Logo,
                OpenComment = userBlog.CanComment,
                CommentType = userBlog.CommentType,// leanote, or disqus
                DisqusId = userBlog.DisqusId,
                ThemeId = userBlog.ThemeId.ToHex24(),
                SubDomain = userBlog.SubDomain,
                Domain = userBlog.Domain,
            };
            return blogInfo;
        }

        public TagCount GetBlogTags(long? userId)
        {
            throw new Exception();
        }
        // 重新计算博客的标签
        // 在设置设置/取消为博客时调用
        public bool ReCountBlogTags(long? userId)
        {
            //todo 需要完成此功能 感觉性能很差劲
            var notes=dataContext.Note.Where(b=>b.UserId==userId&&b.IsTrash==false&&b.IsDeleted==false&&b.IsBlog==true);
            if (notes==null||!notes.Any())
            {
                dataContext.TagCount.Where(b=>b.UserId==null&&b.IsBlog==true).DeleteFromQuery();
                dataContext.SaveChanges();
            }
            else
            {

            }
            return true;
        }

        public Archive[] ListBlogsArchive(long? userId, long? noteBookId, int year, int month, string sortField, bool isAec)
        {
            throw new Exception();
        }

        public void SearchBlogByTags(string[] tags, long? userId, int pageNumber, int pageSize, string sortField, bool isAsc, out Page pageInfo, BlogItem[] bolgs)
        {
            throw new Exception();
        }

        public BlogItem[] notes2BlogItems(Note[] notes)
        {
            throw new Exception();
        }

        public void SearchBlog(string key, long? userid, int page, int pageSize, string sortField, bool isAsc, out Page pageObj, out BlogItem[] blogItems)
        {
            throw new Exception();
        }

        public Post PreNextBlog(long? userid, string sortField, bool isAsc, long? noteId, string baseTime)
        {
            //what is baseTime???
            throw new Exception();
        }

        public void ListAllBlogs(long? userId, string tag, string keywords, bool
             isRecommend, int pageSize, string sorterField, bool isAsc)
        {
            throw new Exception();
        }

        public void fixUserBlog(UserBlog[] userBlog)
        {
            throw new Exception();
        }

        public UserBlog GetUserBlog(long? userId)
        {
            var result = dataContext.UserBlog.Where(b => b.UserId == userId).FirstOrDefault();
            return result;
        }

        public bool UpdateUserBlog(UserBlog userBlog)
        {
            throw new Exception();
        }

        public bool UpdateUserBlogBase(long? userId, UserBlogBase userBlogBase)
        {
            throw new Exception();
        }

        public bool UpdateUserBlogPaging(long? userId, int perPageSize, string sortField, bool isAsc)
        {
            throw new Exception();
        }

        public UserBlog GetUserBlogBySubDomain(string subDomain)
        {
            throw new Exception();
        }

        public UserBlog GetUserBlogByDomain(string domain)
        {
            throw new Exception();
        }

        //---------------------
        // 后台管理
        public bool SetRecommend(long? noteIdm, bool isRecommend)
        {
            throw new Exception();
        }

        /// <summary>
        /// 返回所有liked用户, bool是否还有
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="isALL"></param>
        /// <param name="hasMoreLikedUser"></param>
        /// <returns></returns>
        public UserAndBlog[] ListLikedUsers(long? noteId, bool isALL, out bool hasMoreLikedUser)
        {
            var pageSize = 5;//默认前5
            int skipNum;
            string sortFieldR;
            CommonService.parsePageAndSort(1, pageSize, "CreatedTime", false, out skipNum, out sortFieldR);
            var result = dataContext.BlogLike.Where(b => b.NoteId == noteId).OrderBy(a=>a.CreatedTime);
            if (result == null || !result.Any())
            {
                hasMoreLikedUser = false;
                return null;
            }
            List<BlogLike> allResult = null;
            var likes = result.ToList<BlogLike>();
            // 总记录数
            var count= likes.Count();
            if (count<1)
            {
                hasMoreLikedUser=false;
                return null;
            }



            if (isALL)
            {
                allResult = (from a in likes
                             orderby a.CreatedTime ascending
                             select a).Skip(skipNum).Take(pageSize).ToList<BlogLike>();
            }
            else
            {
                allResult = (from a in likes
                             orderby a.CreatedTime ascending
                             select a).ToList<BlogLike>();
            }
            // 得到所有userIds
            List<long?> userIds=new List<long?>();
            foreach (var like in likes)
            {
                userIds.Add(like.UserId);
            }
            // 得到用户信息
            var userMap= UserService.MapUserAndBlogByUserIds(userIds.ToArray());
            UserAndBlog[] users =new UserAndBlog[likes.Count];
            for (int i = 0; i < likes.Count; i++)
            {
                users[i]=userMap[likes[i].UserId];
            }

            //todo:ListLikedUsers
            hasMoreLikedUser =( count > pageSize);
            return users;
        }

        public bool IsILikeIt(long? noteId, long? userId)
        {
            if (noteId == null || userId == null)
            {
                return false;
            }
            bool result = dataContext.BlogLike.Where(u => u.NoteId == noteId && u.UserId == userId).Any();
            return result;
        }

        public bool IncReadNum(long? noteId)
        {
            try
            {
                var result = dataContext.Note.Where(b => b.NoteId == noteId).FirstOrDefault();
                result.ReadNum++;
                return dataContext.SaveChanges() == 1;
            }
            catch (Exception)
            {
                //bug
                return false;
            }
        }

        public bool LikeBlog(long? noteId, long? userId)
        {
            throw new Exception();
        }

        public BlogComment Comment(long? noteId, long? toCommentId, long? uerId, string connect)
        {
            throw new Exception();
        }

        public void sendEmail(Note note, BlogComment comment, long? userId, string Content)
        {
            throw new Exception();
        }

        public bool DeleteComment(long? noteId, long? CommentId, long? userId)
        {//实际上只提供评论的数据库Id就可以删除
            //Id是唯一的
            throw new Exception();
        }

        public bool LikeComment(long? commentId, long? userId)
        {
            throw new Exception();
        }
        /// <summary>
        /// 评论列表
        /// userId主要是显示userId是否点过某评论的赞
        ///  还要获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="noteId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageObj"></param>
        /// <param name="blogCommentPublics"></param>
        /// <param name="userAndBlog"></param>
        /// <returns></returns>
        public bool ListComments(long? userId, long? noteId, int page, int pageSize, out Page pageObj, out BlogCommentPublic[] blogCommentPublics, out Dictionary<string,UserAndBlog> userAndBlog)
        {
            var pageInfo=page;
            var comments2=new List<BlogComment>();
            CommonService.parsePageAndSort(page,pageSize,"CreatedTime",false,out int skipNum,out string sortFieldR);
            var result=dataContext.BlogComment.Where(b=>b.NoteId==noteId);
            // 总记录数
            var count=result.Count();
            comments2=(from a in result.ToList<BlogComment>()
                      
                      select a).Skip(skipNum).Take(pageSize).ToList<BlogComment>();

            if (comments2.Count==0)
            {
                pageObj=null;
                blogCommentPublics=null;
                userAndBlog=null;
                return false;

            }
            var comments=new  BlogCommentPublic[comments2.Count];
            // 我是否点过赞呢?


            // 得到用户信息

           
            // 得到用户信息
            throw new Exception();
        }

        public bool Report(long? noteId, long? commentId, string reason, long? userId)
        {
            throw new Exception();
        }

        public bool UpateCateIds(long? uerId, long[] cateIds)
        {
            throw new Exception();
        }

        public bool UpateCateUrlTitle(long? userid, long? cateId, string urlTitle)
        {
            throw new Exception();
        }

        public bool UpateBlogUrlTitle(long? userId, long? noteId, string urltitle)
        {
            throw new Exception();
        }

        public bool UpateBlogAbstract(long? userId, long? noteId, string desc, string abstractStr)
        {
            throw new Exception();
        }

        public HashSet<string> GetSingles(long? userId)
        {
            throw new Exception();
        }

        public BlogSingle GetSingle(long? singleId)
        {
            throw new Exception();
        }

        public BlogSingle GetSingleByUserIdAndUrlTitle(long? userId, string singleIdOrUrlTitle)
        {
            throw new Exception();
        }

        public bool updateBlogSingles(long? userId, bool isDelete, bool isAdd, long? singleId, string tiltle, string urlTitle)
        {
            throw new Exception();
        }

        public bool DeleteSingle(long? userId, long? singleId)
        {
            throw new Exception();
        }

        public bool UpdateSingleUrlTitle(long? userID, long? singleId, string urlTitle)
        {
            throw new Exception();
        }

        public bool AddOrUpdateSingle(long? userId, long? singleId, string title, string content)
        {
            throw new Exception();
        }

        public bool SortSingles(long? userId, long[] sinaleIds)
        {
            throw new Exception();
        }

        public string GetUserBlogUrl(UserBlog userBlog, string userName)
        {
            //todo:GetUserBlogUrl
            return ConfigService.GetBlogUrl() + "/" + userName;
        }

        public BlogUrls GetBlogUrls(UserBlog userBlog, User
             userInfo)
        {
            string indexUrl, postUrl, searchUrl, cateUrl, singleUrl, tagsUrl, archiveUrl, tagPostsUrl;
            string blogUrl = ConfigService.GetBlogUrl();
            var userIdOrEmail = "";
            if (!string.IsNullOrEmpty(userInfo.Username))
            {
                userIdOrEmail = userInfo.Username;
            }
            else if (!string.IsNullOrEmpty(userInfo.Email))
            {
                userIdOrEmail = userInfo.Email;
            }
            else
            {
                userIdOrEmail = userInfo.UserId.ToHex();
            }
            indexUrl = blogUrl + "/" + userIdOrEmail;

            cateUrl = blogUrl + "/cate/" + userIdOrEmail;        // /username/notebookId

            postUrl = blogUrl + "/post/" + userIdOrEmail;        // /username/xxxxx

            searchUrl = blogUrl + "/search/" + userIdOrEmail;   // blog.leanote.com/search/username

            singleUrl = blogUrl + "/single/" + userIdOrEmail;   // blog.leanote.com/single/username/singleId

            archiveUrl = blogUrl + "/archives/" + userIdOrEmail; // blog.leanote.com/archive/username

            tagsUrl = blogUrl + "/tags/" + userIdOrEmail;

            tagPostsUrl = blogUrl + "/tag/" + userIdOrEmail; // blog.leanote.com/archive/username
            BlogUrls blogUrls = new BlogUrls()
            {
                IndexUrl = indexUrl,
                CateUrl = cateUrl,
                SearchUrl = searchUrl,
                SingleUrl = singleUrl,
                PostUrl = postUrl,
                ArchiveUrl = archiveUrl,
                TagsUrl = tagsUrl,
                TagPostsUrl = tagPostsUrl
            };
            return blogUrls;
        }

        public Post[] FixBlogs(BlogItem[] blogs)
        {
            var posts = new Post[blogs.Length];
            for (int i = 0; i < blogs.Length; i++)
            {
                posts[i] = FixBlog(blogs[i]);
            }
            return posts;
        }

        public Post FixBlog(BlogItem blogItem)
        {
            throw new Exception();
        }

        public Post FixNote(Note note)
        {
            throw new Exception();
        }

        public Cate[] GetCateArrayForBlog(long? userId)
        {
            var result = (from _note in dataContext.Note
                          join _noteBook in dataContext.Notebook on _note.NotebookId equals _noteBook.NotebookId
                          where _note.IsBlog == true && _note.IsTrash == false && _note.IsDeleted == false
                          select new Cate
                          {
                              CateId = _note.NotebookId,
                              Title = _noteBook.Title
                          }).DistinctBy(p => new { p.CateId }).OrderByDescending(b => b.Title).ToArray();
            return result;
        }
    }
}