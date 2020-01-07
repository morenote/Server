using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class ShareNotebookNoteInfo
    {



    }
    // 以后可能含有其它信息
    public class EachSharedNote
    {
        public int Seq;
    }
    public class EachSharedNotebook
    {
        public int Seq;
    }
    // 每一个用户共享给的note, notebook
    public class EachSharedNotebookAndNotes
    {
        public int Seq { get; set; }// // 共享给谁的所在序号
        public HashSet<string> DefaultNotebook;// noteId => 共享的note
        public HashSet<string> Notebooks;// notebookId => 共享的notebook
    }
    public class ShareNotebook
    {
        [Key]
        public int ShareNotebookId { get; set; } // 必须要设置bson:"_id" 不然mgo不会认为是主键
        public long UserId { get; set; }
        public long ToUserId { get; set; }
        public long ToGroupId { get; set; } // 分享给的用户组 
        public Group ToGroup { get; set; } // 仅仅为了显示, 不存储, 分组信息
        public long NotebookId { get; set; }
        public int Seq { get; set; } // 排序 
        public int Perm { get; set; } // 权限, 其下所有notes 0只读, 1可修改
        public DateTime CreatedTime { get; set; }
        public bool IsDefault { get; set; } //是否是默认共享notebook, perm seq=-9999999, NotebookId=null

    }
    //未完成待续
}
