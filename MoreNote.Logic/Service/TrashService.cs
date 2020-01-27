using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class TrashService
    {
        // 回收站
        // 可以移到noteSerice中

        // 不能删除notebook, 如果其下有notes!
        // 这样回收站里只有note

        // 删除笔记后(或删除笔记本)后入回收站
        // 把note, notebook设个标记即可!
        // 已经在trash里的notebook, note不能是共享!, 所以要删除共享


        // 删除note
        // 应该放在回收站里
        // 有trashService
        public static bool DeleteNote(long noteId,long userId)
        {
            throw new Exception();
        }
        // 删除别人共享给我的笔记
        // 先判断我是否有权限, 笔记是否是我创建的
        public static bool DeleteSharedNote(long noteId,long myUserId)
        {
            throw new Exception();
        }
        // recover
        public static bool  recoverNote(long noteId,long notebookId,long userId)
        {
            throw new Exception();
        }
        // 删除trash
        public static bool DeleteTrash(long noteId,long userId)
        {
            throw new Exception();
        }
        public static bool DeleteTrashApi(long noteId,long userId,int usn)
        {
            throw new Exception();
        }
        // 列出note, 排序规则, 还有分页
        // CreatedTime, UpdatedTime, title 来排序
        public static Note[] ListNotes(long userId,int pageNumber,int pageSize,string sortField,bool isAsc)
        {
            throw new Exception();
        }


    }
}
