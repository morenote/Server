using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public  class NoteService
    {
  
        public static bool AddNote(Note note)
        {
            using (var db = new DataContext())
            {
                var result = db.Note.Add(note);

                return db.SaveChanges() > 0;
            }
        }

  
        // fileService调用
        // 不能是已经删除了的, life bug, 客户端删除后, 竟然还能在web上打开
        //我也是？？？？
        public static Note GetNoteById(long NoteId)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.NoteId == NoteId).FirstOrDefault();
                return result;
            }
        }
        public static Note SelectNoteByTag(String Tag)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.Tags.Contains(Tag)).FirstOrDefault();
               
                return result;
            }
        }
        public static NoteAndContent[] GetNoteAndContent(bool isBlog)
        {
            using (var db = new DataContext())
            {
                var result = (from _note in db.Note
                              join _content in db.NoteContent on _note.ContentId equals _content.NoteId
                              where _note.IsBlog == isBlog
                              select new NoteAndContent
                              {
                                  note = _note,
                                  noteContent = _content
                              }).ToArray();
                return result;
            }
        }
        public static NoteAndContent GetNoteAndContent(long noteId)
        {
            using (var db = new DataContext())
            {
                var result = (from _note in db.Note
                              join _content in db.NoteContent on _note.ContentId equals _content.NoteId
                              where _note.NoteId==noteId
                              select new NoteAndContent
                              {
                                  note = _note,
                                  noteContent = _content
                              }).FirstOrDefault();
                return result;
            }
        }
   

        // getDirtyNotes, 把note的图片, 附件信息都发送给客户端
        // 客户端保存到本地, 再获取图片, 附件

        // 得到所有图片, 附件信息
        // 查images表, attachs表
        // [待测]
        public NoteFile getFiles(long noteId)
        {
            return null;
        }
        public static Note GetNote(long noteId,long userId)
        {
            throw new Exception();
        }
        // 得到blog, blogService用
        // 不要传userId, 因为是公开的
        public static Note GetBlogNote(long noteId)
        {
            throw new Exception();
        }
        public static  NoteAndContent GetNoteAndContent(long noteId,long userId)
        {
            throw new Exception();
        }
        public static Note GetNoteBySrc(string src,string userId)
        {
            throw new Exception();
        }
        public static void GetNoteAndContentBySrc(string url,long userId,out long noteId,out NoteAndContentSep noteAndContent)
        {
            throw new Exception();
        }
        // 获取同步的笔记
        // > afterUsn的笔记
        public static ApiNote[] GetSyncNotes(long userid, int afterUsn, int maxEntry)
        {
            using (var db = new DataContext())
            {
                var result = db.Note.
                    Where(b => b.UserId == userid && b.Usn > afterUsn).Take(maxEntry);
                return ToApiNotes(result.ToArray());
            }
        }
        // note与apiNote的转换
        public static ApiNote[] ToApiNotes(Note[] notes)
        {
            // 2, 得到所有图片, 附件信息
            // 查images表, attachs表

            if (notes != null && notes.Length > 0)
            {
                ApiNote[] apiNotes = new ApiNote[notes.Length];
                //得到所有文件
                long[] noteIds=new long[notes.Length];
                for (int i = 0; i < notes.Length; i++)
                {
                    noteIds[i]=notes[i].NoteId;
                }
              // Dictionary<long,NoteFile[]> noteFilesMap=getFiles(noteIds);
                for (int i = 0; i < notes.Length; i++)
                {
                    apiNotes[i] = ToApiNote(ref notes[i], null);
                }

                return apiNotes;
            }
            else
            {
                return new ApiNote[0];
            }

        }
        // note与apiNote的转换
        public static ApiNote ToApiNote(ref Note note, APINoteFile[] files)
        {
      
            ApiNote apiNote = new ApiNote()
            {
                NoteId = note.NoteId.ToString("x"),

                NotebookId = note.NotebookId.ToString("x"),
                UserId = note.UserId.ToString("x"),
                Title = note.Title,
                Tags = note.Tags,
                IsMarkdown = note.IsMarkdown,
                IsBlog = note.IsBlog,
                IsTrash = note.IsTrash,
                IsDeleted = note.IsDeleted,
                Usn = note.Usn,
                CreatedTime = note.CreatedTime,
                UpdatedTime = note.UpdatedTime,
                PublicTime = note.PublicTime,
                Files = files

            };
            return apiNote;
        }
        // getDirtyNotes, 把note的图片, 附件信息都发送给客户端
        // 客户端保存到本地, 再获取图片, 附件

        /// <summary>
        /// 得到所有图片, 附件信息
        /// 查images表, attachs表
        /// [待测]
        /// </summary>
        /// <param name="noteIds"></param>
        /// <returns></returns>
        public static Dictionary<long,NoteFile[]> getFiles(long[] noteIds)
        {
            var noteImages = NoteImageService.getImagesByNoteIds(noteIds);
            var noteAttachs= AttachService.getAttachsByNoteIds(noteIds);

            throw new Exception();
        }
        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public static Note[] ListNotes(long userId,
          long notebookId,
          bool isTrash,
          int pageNumber,
          int pageSize,
          string sortField,
          bool isAsc,
          bool isBlog)
        {
            using (var db = new DataContext())
            {
                var result = db.Note
                    .Where(b => b.NotebookId == notebookId && b.UserId == userId);
                return result.ToArray();
            }
        }

        public static Note[] ListNotesByNoteIdsWithPageSort(long[] noteIds,long userId,int pageNumber,int pageSize,string sortField,bool isAsc,bool isBlog)
        {
            throw new Exception();
        }
        // shareService调用
        public static Note[] ListNotesByNoteIds(long[] noteIds)
        {
            
            throw new Exception();
        }
        // blog需要
        public static Note[] ListNoteContentsByNoteIds(long[] noteIds)
        {
            throw new Exception();
        }
        // 只得到abstract, 不需要content
        public static Note[] ListNoteAbstractsByNoteIds(long[] noteIds)
        {
            throw new Exception();
        }
        public static NoteContent[] ListNoteContentByNoteIds(long noteIds)
        {
            throw new Exception();
        }
        // 添加笔记
        // 首先要判断Notebook是否是Blog, 是的话设为blog
        // [ok]
        public static Note AddNote(Note note,bool fromAPI)
        {
            throw new Exception();
        }
        // 添加共享d笔记
        public static Note AddSharedNote(Note note,long muUserId)
        {
            throw new Exception();
        }
        // 添加笔记和内容
        // 这里使用 info.NoteAndContent 接收?
        public static Note AddNoteAndContentForController(Note note,NoteContent noteContent ,long updatedUserId)
        {
            throw new Exception();
        }
        public static Note AddNoteAndContent(Note note,NoteContent noteContent,long myUserId)
        {
            throw new Exception();
        }
        public static Note AddNoteAndContentApi(Note note,NoteContent noteContent,long myUserId)
        {
            throw new Exception();
        }
        // 修改笔记
        // 这里没有判断usn
        public static bool UpdateNote(long updateUserId,long noteId,Note needUpdate,int usb)
        {
            throw new Exception();
        }
        // 当设置/取消了笔记为博客
        public static bool UpdateNoteContentIsBlog(long noteId,bool isBlog)
        {
            throw new Exception();
        }
        // 附件修改, 增加noteIncr
        public static int IncrNoteUsn(long noteId,long userId)
        {
            throw new Exception();
        }
        // 这里要判断权限, 如果userId != updatedUserId, 那么需要判断权限
        // [ok] TODO perm还没测 [del]
        public static bool UpdateNoteTitle(long userId,long updateUserId,long noteId,string title)
        {
            throw new Exception();
        }
        // ?????
        // 这种方式太恶心, 改动很大
        // 通过content修改笔记的imageIds列表
        // src="http://localhost:9000/file/outputImage?fileId=541ae75499c37b6b79000005&noteId=541ae63c19807a4bb9000000"
        //哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈歇一会哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈哈
        public static bool updateNoteImages(long noteId, string content)
        {
            throw new Exception();
        }
        // 更新tags
        // [ok] [del]
        public static bool UpdateTags(long noteId,long userId,string[] tags)
        {
            throw new Exception();
        }
        public static bool ToBlog(long userId,long noteId,bool isBlog,bool isTop)
        {
            throw new Exception();
        }
        // 移动note
        // trash, 正常的都可以用
        // 1. 要检查下notebookId是否是自己的
        // 2. 要判断之前是否是blog, 如果不是, 那么notebook是否是blog?
        public static Note MoveNote(long noteId,long notebookId,long userId)
        {
            throw new Exception();
        }
        // 如果自己的blog状态是true, 不用改变,
        // 否则, 如果notebookId的blog是true, 则改为true之
        // 返回blog状态
        // move, copy时用
        public static bool updateToNotebookBlog(long noteId,long notebookId,long userId)
        {
            throw new Exception();
        }
        // 判断是否是blog
        public static bool IsBlog(long noteId)
        {
            throw new Exception();
        }
        // 复制note
        // 正常的可以用
        // 先查, 再新建
        // 要检查下notebookId是否是自己的
        public static Note CopyNote(long noteId,long userId)
        {
            throw new Exception();
        }
        // 复制别人的共享笔记给我
        // 将别人可用的图片转为我的图片, 复制图片
        public static Note CopySharedNote(long noteId,long fromUserId,long myUserId)
        {
            throw new Exception();
        }
        // 通过noteId得到notebookId
        // shareService call
        // [ok]
        public static long GetNotebookId(long noteId)
        {
            throw new Exception();
        }
        //------------------
        // 搜索Note, 博客使用了
        public static Note[] SearchNote(string key,long userId,int pageNumber,int pageSize,string sortField,bool isAsc,bool isBlog)
        {
            throw new Exception();
        }
        // 搜索noteContents, 补集pageSize个
        public static Note[] searchNoteFromContent(Note[] notes,long userId,string key,int pagSize,string sortField,bool isBlog)
        {
            throw new Exception();
        }
        //----------------
        // tag搜索
        public static Note[] SearchNoteByTags(string[] tags,long userId,int pageNumber,int pageSize,string sortField,bool isAsc)
        {
            throw new Exception();
        }
        //------------
        // 统计
        public static int CountNote(long userId)
        {
            throw new Exception();
        }
        public static int CountBlog(long usrId)
        {
            throw new Exception();
        }
        // 通过标签来查询
        public static int CountNoteByTag(long userId,string tag)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteTag
                    .Where(b => b.UserId == userId && b.Tag.Equals(tag)).Count();
                
                return result;
            }
           
        }

        // 删除tag
        // 返回所有note的Usn
        public static Dictionary<string,int> UpdateNoteToDeleteTag(long userId,string targetTag)
        {
            throw new Exception();
        }
        // api

        // 得到笔记的内容, 此时将笔记内的链接转成标准的Leanote Url
        // 将笔记的图片, 附件链接转换成 site.url/file/getImage?fileId=xxx,  site.url/file/getAttach?fileId=xxxx
        public static  string FixContentBad(string content,bool isMarkdown)
        {
            throw new Exception();
        }
        // 得到笔记的内容, 此时将笔记内的链接转成标准的Leanote Url
        // 将笔记的图片, 附件链接转换成 site.url/file/getImage?fileId=xxx,  site.url/file/getAttach?fileId=xxxx
        // 性能更好, 5倍的差距
        public static string FixContent(string content,bool isMarkdown)
        {
           throw new Exception();
        }





    }
}
