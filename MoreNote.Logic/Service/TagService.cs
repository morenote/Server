using MoreNote.Common.Utils;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TagService
    {
      
        public static bool AddTags(long userId,string[] tags)
        {
            using (var db = new DataContext())
            {
             
            }
                //todo 
                return true;
        }
        //---------------------------
        // v2
        // 第二版标签, 单独一张表, 每一个tag一条记录

        // 添加或更新标签, 先查下是否存在, 不存在则添加, 存在则更新
        // 都要统计下tag的note数
        // 什么时候调用? 笔记添加Tag, 删除Tag时
        // 删除note时, 都可以调用
        // 万能
        public static NoteTag AddOrUpdateTag(long userId,string tag)
        {
            NoteTag noteTag=GetTag(userId,tag);
            // 存在, 则更新之
            if (noteTag!=null&&noteTag.TagId!=0)
            {
                // 统计note数
                int count=NoteService.CountNoteByTag(userId,tag);
                noteTag.Count=count;
                noteTag.UpdatedTime=DateTime.Now;
                // 之前删除过的, 现在要添加回来了
                if (noteTag.IsDeleted)
                {
                    noteTag.Usn = UserService.IncrUsn(userId);
                    noteTag.IsDeleted=false;
                    UpdateByIdAndUserId(noteTag.TagId,userId,noteTag);
                }
                return noteTag;
            }
            // 不存在, 则创建之
            var timeNow =DateTime.Now;
            noteTag = new NoteTag()
            {
                TagId = SnowFlake_Net.GenerateSnowFlakeID(),
                Count = 1,
                Tag = tag,
                UserId = userId,
                CreatedTime = timeNow,
                UpdatedTime = timeNow,
                Usn = UserService.IncrUsn(userId),
                IsDeleted = false
            };
            AddNoteTag(noteTag);
            return noteTag;

        }
        public static bool UpdateByIdAndUserId(long tagId,long userId,NoteTag noteTag)
        {
            using (var db = new DataContext())
            {
                var noteT = db.NoteTag
                    .Where(b => b.TagId==tagId
                    &&b.UserId==userId).FirstOrDefault();
                noteT.Tag = noteTag.Tag;
                noteT.Usn = noteTag.Usn;
                noteT.Count = noteTag.Count;
                noteT.UpdatedTime = noteTag.UpdatedTime;
                noteT.IsDeleted = noteTag.IsDeleted;
                return db.SaveChanges()>0;
            }

        }

        // 得到标签, 按更新时间来排序
        public static NoteTag[] GetTags(long userId)
        {
            throw new Exception();
        }
        // 删除标签
        // 也删除所有的笔记含该标签的
        // 返回noteId => usn
        public static HashSet<string> DeleteTag(long userId,string tag)
        {
            throw new Exception();
        }
        // 删除标签, 供API调用
        public static bool DeleteTagApi(long userId,string tag,int usn,out int toUsn,out string msg)
        {
            NoteTag noteTag=GetTag(userId,tag);
            if (noteTag==null)
            {
                toUsn=0;
                msg= "notExists";
                return false;
            }
            if (noteTag.Usn>usn)
            {
                toUsn = noteTag.Usn;
                msg = "conflict";
                return false;
            }
           
            using (var db = new DataContext())
            {
                var result = db.NoteTag
                    .Where(b => b.UserId==userId&&b.Tag.Equals(tag
                    )).FirstOrDefault();
                result.IsDeleted=true;
                toUsn = UserService.IncrUsn(userId);
                //todo:这里应该进行事务控制，失败的话应该回滚事务
                msg="success";
                return db.SaveChanges()>0;
            }
           
        }
        // 重新统计标签的count
        public static void reCountTagCount(long userId,string tags)
        {
            throw new Exception();
        }
        // 同步用
        public static NoteTag[] GeSyncTags(long userId, int afterUsn, int maxEntry)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteTag.
                    Where(b => b.UserId == userId && b.Usn > afterUsn).Take(maxEntry);
                return result.ToArray();
            }
        }
        public static NoteTag GetTag(long tagId)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteTag
                    .Where(b => b.TagId==tagId);
                if (result==null)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }
        }
        public static NoteTag GetTag(long userId,string tag)
        {
            using (var db = new DataContext())
            {
                var result = db.NoteTag
                    .Where(b =>b.UserId==userId&& b.Tag.Equals(tag));
                if (result == null)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }
        }
        public static bool AddNoteTag(NoteTag noteTag)
        {
            if (noteTag.TagId==0)
            {
                noteTag.TagId=SnowFlake_Net.GenerateSnowFlakeID();
            }
            using (var db = new DataContext())
            {
                var result = db.NoteTag.Add(noteTag);
                return db.SaveChanges()>0;
            }

        }
        public static string[] GetBlogTags(long userid)
        {

            using (var db = new DataContext())
            {
                var result = db.Note.Where(note => note.IsBlog == true && note.IsDeleted == false && note.IsTrash == false && note.Tags != null && note.Tags.Length > 0).ToArray();
                HashSet<string> hs = new HashSet<string>();
                foreach (var item in result)
                {
                    if (item.Tags!=null&& item.Tags.Length>0)
               
                    foreach (var tag in item.Tags)
                    {
                        if (hs.Contains(tag))
                        {
                                hs.Add(tag);
                        }
                    }
                }
                return hs.ToArray<string>();

                //var result2 = (from _note in db.Note
                //              join _noteBook in db.Notebook on _note.NotebookId equals _noteBook.NotebookId
                //              where _note.IsBlog == true && _note.IsTrash == false && _note.IsDeleted == false
                //              select new Cate
                //              {
                //                  CateId = _note.NotebookId,
                //                  Title = _noteBook.Title
                //              }).DistinctBy(p => new { p.CateId }).OrderByDescending(b => b.Title).ToArray();
                //return result2;
            }

        }
    }
}
