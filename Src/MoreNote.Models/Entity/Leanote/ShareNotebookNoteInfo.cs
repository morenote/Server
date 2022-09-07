using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

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
        public Dictionary<long,EachSharedNote> DefaultNotebook;// noteId => 共享的note
        public Dictionary<long,EachSharedNotebook> Notebooks;// notebookId => 共享的notebook
    }
    public class SharedNotebookAndNotes
    {
        public long? userId { get;set;}
      public Dictionary<long,EachSharedNotebookAndNotes> Shared { get;set;}
    }
    public class SharingNotebookAndNotes
    {
        public long? UserId { get;set;}
        public Dictionary<long,long[]> Notes { get;set;}
        public Dictionary<long,long[]> NOtebooks { get;set;}

    }
    [Table("share_notebook")]
    public class ShareNotebook: BaseEntity
    {
       
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("to_user_id")]
        public long? ToUserId { get; set; }
        [Column("to_group_id")]
        public long? ToGroupId { get; set; } // 分享给的用户组 
     
        public GroupTeam ToGroup { get; set; } // 仅仅为了显示, 不存储, 分组信息
        [Column("notebook_id")]
        public long? NotebookId { get; set; }
        [Column("seq")]
        public int Seq { get; set; } // 排序 
        [Column("perm")]
        public int Perm { get; set; } // 权限, 其下所有notes 0只读, 1可修改
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("is_default")]
        public bool IsDefault { get; set; } //是否是默认共享notebook, perm seq=-9999999, NotebookId=null

    }

    public class ShareNotebooks
    {




    }

}
