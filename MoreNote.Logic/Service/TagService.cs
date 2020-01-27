using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TagService
    {
      
        public static bool AddTags(long userId,string[] tags)
        {
            throw new Exception();
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
            throw new Exception();
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
        public static bool DeleteTagApi(long userId,string tag,int usn,out int toUsn)
        {
            throw new Exception();
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
    }
}
