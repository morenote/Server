using MoreNote.Common.Helper;
using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using MoreNote.Logic.ExtensionMethods.DI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class TagService
    {
        private DependencyInjectionService dependencyInjectionService;

        public TagService(DependencyInjectionService dependencyInjectionService)
        {
            this.dependencyInjectionService = dependencyInjectionService;
        }

        public bool AddTags(long userId, string[] tags)
        {
            using (var sc = dependencyInjectionService.GetServiceScope())
            {
                using (var dataContext = sc.GetDataContext())
                {
                    foreach (var item in tags)
                    {
                        //解决item引起的bug
                        //todo:继续实现这个方法，维护与leanote的兼容性
                        var result= dataContext.NoteTag.Where(tag => tag.Tag.Equals(item));
                        if (item!=null&&result!=null&&result.Any())
                        {
                            dataContext.NoteTag.Add(new NoteTag(){ 
                                TagId=SnowFlakeNet.GenerateSnowFlakeID(),
                                UserId=userId,
                                Tag=item,
                                Usn=0,
                                Count=0,
                                CreatedTime=DateTime.Now,
                                UpdatedTime=DateTime.Now,
                                IsDeleted=false
                                });;
                        }
                    }
                    dataContext.SaveChanges();
                }
            }

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
        public NoteTag AddOrUpdateTag(long userId, string tag)
        {
            var noteService = dependencyInjectionService.GetNoteService();
            var userService = dependencyInjectionService.GetUserService();
            NoteTag noteTag = GetTag(userId, tag);
            // 存在, 则更新之
            if (noteTag != null && noteTag.TagId != 0)
            {
                // 统计note数
                int count = noteService.CountNoteByTag(userId, tag);
                noteTag.Count = count;
                noteTag.UpdatedTime = DateTime.Now;
                // 之前删除过的, 现在要添加回来了
                if (noteTag.IsDeleted)
                {
                    noteTag.Usn = userService.IncrUsn(userId);
                    noteTag.IsDeleted = false;
                    UpdateByIdAndUserId(noteTag.TagId, userId, noteTag);
                }
                return noteTag;
            }
            // 不存在, 则创建之
            var timeNow = DateTime.Now;
            noteTag = new NoteTag()
            {
                TagId = SnowFlakeNet.GenerateSnowFlakeID(),
                Count = 1,
                Tag = tag,
                UserId = userId,
                CreatedTime = timeNow,
                UpdatedTime = timeNow,
                Usn = userService.IncrUsn(userId),
                IsDeleted = false
            };
            AddNoteTag(noteTag);
            return noteTag;
        }

        public bool UpdateByIdAndUserId(long tagId, long userId, NoteTag noteTag)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var noteT = dataContext.NoteTag
                           .Where(b => b.TagId == tagId
                           && b.UserId == userId).FirstOrDefault();
                noteT.Tag = noteTag.Tag;
                noteT.Usn = noteTag.Usn;
                noteT.Count = noteTag.Count;
                noteT.UpdatedTime = noteTag.UpdatedTime;
                noteT.IsDeleted = noteTag.IsDeleted;
                return dataContext.SaveChanges() > 0;
            }
        }

        // 得到标签, 按更新时间来排序
        public NoteTag[] GetTags(long userId)
        {
            using (var sc = dependencyInjectionService.GetServiceScope())
            using (var dataContext = sc.GetDataContext())
            {
                return dataContext.NoteTag.Where(tag => tag.UserId == userId && tag.IsDeleted == false)
                      .OrderBy(tag => tag.UpdatedTime).ToArray();
            }
        }

        // 删除标签
        // 也删除所有的笔记含该标签的
        // 返回noteId => usn
        public HashSet<string> DeleteTag(long userId, string tag)
        {
            //todo:DeleteTag未实现
            throw new Exception();
        }

        // 删除标签, 供API调用
        public bool DeleteTagApi(long userId, string tag, int usn, out int toUsn, out string msg)
        {
            NoteTag noteTag = GetTag(userId, tag);
            if (noteTag == null)
            {
                toUsn = 0;
                msg = "notExists";
                return false;
            }
            if (noteTag.Usn > usn)
            {
                toUsn = noteTag.Usn;
                msg = "conflict";
                return false;
            }

            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                UserService userService = dependencyInjectionService.GetUserService();
                var result = dataContext.NoteTag
                     .Where(b => b.UserId == userId && b.Tag.Equals(tag
                     )).FirstOrDefault();
                result.IsDeleted = true;
                toUsn = userService.IncrUsn(userId);
                //todo:这里应该进行事务控制，失败的话应该回滚事务
                msg = "success";
                return dataContext.SaveChanges() > 0;
            }
        }

        // 重新统计标签的count
        public void reCountTagCount(long userId, string[] tags)
        {
            if (tags == null || tags.Length == 0)
            {
                return;
            }
            foreach (var tag in tags)
            {
                this.AddOrUpdateTag(userId, tag);
            }
        }

        // 同步用
        public NoteTag[] GeSyncTags(long userId, int afterUsn, int maxEntry)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteTag.
                    Where(b => b.UserId == userId && b.Usn > afterUsn).Take(maxEntry);
                return result.ToArray();
            }
        }

        public NoteTag GetTag(long tagId)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteTag
                          .Where(b => b.TagId == tagId);
                if (result == null)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }
        }

        public NoteTag GetTag(long userId, string tag)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.NoteTag
                        .Where(b => b.UserId == userId && b.Tag.Equals(tag));
                if (result == null)
                {
                    return null;
                }
                return result.FirstOrDefault();
            }
        }

        public bool AddNoteTag(NoteTag noteTag)
        {
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                if (noteTag.TagId == 0)
                {
                    noteTag.TagId = SnowFlakeNet.GenerateSnowFlakeID();
                }

                var result = dataContext.NoteTag.Add(noteTag);
                return dataContext.SaveChanges() > 0;
            }
        }

        public string[] GetBlogTags(long userid)
        {
            //todo:这里需要性能优化，获得blog标签
            using (var dataContext = dependencyInjectionService.GetDataContext())
            {
                var result = dataContext.Note.Where(note => note.UserId == userid && note.IsBlog == true && note.IsDeleted == false && note.IsTrash == false && note.Tags != null && note.Tags.Length > 0).DistinctBy(p => new { p.Tags }).ToArray();
                HashSet<string> hs = new HashSet<string>();
                foreach (var item in result)
                {
                    if (item.Tags != null && item.Tags.Length > 0)

                        foreach (var tag in item.Tags)
                        {
                            if (tag != null && !hs.Contains(tag))
                            {
                                hs.Add(tag);
                            }
                        }
                }
                return hs.ToArray<string>();
            }
        }
    }
}