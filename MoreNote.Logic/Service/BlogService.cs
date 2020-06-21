using MoreNote.Common.Helper;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class BlogService
    {
        public static BlogStat GetBlogStat(long noteId)
        {
            throw new Exception();
        }
        /// <summary>
        /// 统计网站上公开的Post的数量
        /// </summary>
        public static int CountTheNumberForBlogs(long userId)
        {
            using(var db=new DataContext())
            {
                var count = db.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId).Count();
                return count;
           
            }
        }
        public static int CountTheNumberForBlogTags(long userId,string tag)
        {
            using (var db = DataContext.getDataContext())
            {
                var count = db.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId&&b.Tags.Contains(tag)).Count();
                return count;

            }
        }
        public static Note[] GetNotes(long userid)
        {
            using (var db = DataContext.getDataContext())
            {
                var result =
                    db.Note.Where(note => note.IsBlog == true && note.IsDeleted == false && note.IsTrash == false && note.UserId == userid).OrderByDescending(note=>note.PublicTime).ToArray();
                return result;
            }
        }
        public static int CountTheNumberForBlogsOfNoteBookId(long userId,long notebookId)
        {
            using (var db = DataContext.getDataContext())
            {
                var count = db.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId&&b.NotebookId== notebookId).Count();
                return count;

            }
        }
        public static int CountTheNumberForBlogsOfTag(long userId, string tag)
        {
            using (var db = DataContext.getDataContext())
            {
                var count = db.Note.Where(b => b.IsBlog == true && b.IsDeleted == false && b.IsTrash == false && b.UserId == userId && b.Tags.Contains(tag)).Count();
                return count;
            }
        }
        public static BlogItem GetBlogByIdAndUrlTitle(long userId,string noteIdOrUrlTitle)
        {
            throw new Exception();
        }
        public static BlogItem GetBlog(long noteId)
        {
            throw new Exception();
        }
        public static BlogItem GetBlogItem(Note note)
        {
            throw new Exception();
        }
        public static Notebook[] ListBlogNotebooks(long userId)
        {
            throw new Exception();
        }
        public static void ListBlogs(long uerId,long noteBookId ,int page,int pageSize,string sortField,bool isAsc,out Page pageObj,out BlogItem blogItem)
        {
            throw new Exception();

        }
        public static TagCount GetBlogTags(long userId)
        {
            throw new Exception();
        }
        public static bool ReCountBlogTags(long userId)
        {
            //todo 需要完成此功能
            return true;
        }
        public static Archive[] ListBlogsArchive(long userId,long noteBookId,int year,int month,string sortField ,bool isAec)
        {
            throw new Exception();
        }
        public static void SearchBlogByTags(string [] tags,long userId,int pageNumber,int pageSize,string sortField,bool isAsc,out Page pageInfo,BlogItem[] bolgs)
        {
            throw new Exception();
        }
        public static BlogItem[] notes2BlogItems(Note[] notes)
        {
            throw new Exception();
        }
        public static void SearchBlog(string key,long userid,int page ,int pageSize,string sortField,bool isAsc,out Page pageObj,out BlogItem[] blogItems)
        {
            throw new Exception();
        }
        public static Post PreNextBlog(long userid,string sortField,bool isAsc,long noteId ,string baseTime)
        {
            //what is baseTime???
            throw new Exception();
        }
        public static void ListAllBlogs(long userId,string tag,string keywords,bool
             isRecommend,int pageSize,string sorterField,bool isAsc)
        {
            throw new Exception();
        }
        public static void fixUserBlog(UserBlog[] userBlog)
        {
            throw new Exception();
        }
        public static UserBlog GetUserBlog(long userId)
        {
            using (var db=new DataContext())
            {
                var result = db.UserBlog.Where(b => b.UserId == userId).FirstOrDefault();
                return result;
            }
        }
        public static bool UpdateUserBlog(UserBlog userBlog)
        {
            throw new Exception();
        }
        public static bool UpdateUserBlogBase(long userId,UserBlogBase userBlogBase)
        {
            throw new Exception();
        }
        public static bool UpdateUserBlogPaging(long userId,int perPageSize ,string sortField,bool isAsc)
        {
            throw new Exception();
        }
        public static UserBlog GetUserBlogBySubDomain(string subDomain)
        {
            throw new Exception();
        }
        public static UserBlog GetUserBlogByDomain(string domain)
        {
            throw new Exception();
        }
        //---------------------
        // 后台管理
        public static bool SetRecommend(long noteIdm,bool isRecommend)
        {
            throw new Exception();
        }
        public static UserAndBlog[] ListLikedUsers(long noteId,bool isALL)
        {
            throw new Exception();
        }
        public static bool IsILikeIt(long noteId,long UserId)
        {
            throw new Exception();
        }
        public static bool IncReadNum(long noteId)
        {
            using (var db = DataContext.getDataContext())
            {
                try
                {
                    var result = db.Note.Where(b => b.NoteId == noteId).FirstOrDefault();
                    result.ReadNum++;
                    return db.SaveChanges()==1;
                    
                }
                catch (Exception)
                {
                    //bug
                    return false;
                }
              
               
            }
        }
        public static bool LikeBlog(long noteId,long userId)
        {
            throw new Exception();
        }
        public static BlogComment Comment(long noteId,long toCommentId,long uerId,string connect)
        {
            throw new Exception();
        }
        public static void sendEmail(Note note ,BlogComment comment,long userId,string Content)
        {
            throw new Exception();
        }
        public static bool DeleteComment(long noteId,long CommentId,long userId)
        {//实际上只提供评论的数据库Id就可以删除
            //Id是唯一的
            throw new Exception();
        }
        public bool LikeComment(long commentId,long userId)
        {
            throw new Exception();
        }
        public bool ListComments(long userId,long noteId,int page,int pageSize,out Page pageObj,out BlogCommentPublic[] blogCommentPublics,out HashSet<string> vs)
        {
            throw new Exception();
        }
        public static bool Report (long noteId,long commentId,string reason,long userId)
        {
            throw new Exception();
        }
        public static bool UpateCateIds(long uerId,long[] cateIds)
        {
            throw new Exception();
        }

        public static bool UpateCateUrlTitle(long userid,long cateId,string urlTitle)
        {
            throw new Exception();
        }
        public bool UpateBlogUrlTitle(long userId,long noteId,string urltitle)
        {
            throw new Exception();
        }
        public static bool UpateBlogAbstract(long userId,long noteId,string desc,string abstractStr)
         {
            throw new Exception();
          }
          public static HashSet<string> GetSingles(long userId)
        {
            throw new Exception();

        }
        public static BlogSingle GetSingle(long singleId)
        {
            throw new Exception();
        }
        public static BlogSingle GetSingleByUserIdAndUrlTitle(long userId,string singleIdOrUrlTitle)
        {
            throw new Exception();
        }
        public static bool updateBlogSingles(long userId,bool isDelete,bool isAdd,long singleId,string tiltle,string urlTitle)
        {
            throw new Exception();
        }
        public static bool DeleteSingle(long userId,long singleId)
        {
            throw  new Exception();
        }
        public static bool  UpdateSingleUrlTitle(long userID,long singleId,string urlTitle)
        {
            throw new Exception();
        }
        public static bool AddOrUpdateSingle(long userId,long singleId,string title,string content)
        {
            throw new Exception();
        }
        public static bool SortSingles(long userId,long[] sinaleIds)
        {
            throw new Exception();
        }
        public static string GetUserBlogUrl(UserBlog userBlog,string userName)
        {
            throw new Exception();
        }
        public static BlogUrls GetBlogUrls(UserBlog userBlog,User
             userInfo)
        {
            throw new Exception();
        }
        public Post[] FixBlogs(BlogItem[] blogs)
        {
            throw new Exception();
        }
        public Post FixBlog(BlogItem blogItem)
        {
            throw new Exception();
        }
        public static Post FixNote(Note note)
        {
            throw new Exception();
        }
        public static Cate[] GetCateArrayForBlog(long userId)
        {
            using (var db = DataContext.getDataContext())
            {
                var result = (from _note in db.Note
                              join _noteBook in db.Notebook on _note.NotebookId equals _noteBook.NotebookId
                              where _note.IsBlog == true &&_note.IsTrash==false&&_note.IsDeleted==false
                              select new Cate
                              {
                                  CateId = _note.NotebookId,
                                  Title = _noteBook.Title
                              }).DistinctBy(p => new { p.CateId }).OrderByDescending(b => b.Title).ToArray();
                return result;
            }

        }
    



    }
}
