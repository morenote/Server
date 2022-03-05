using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    [Table("notebook")]
    public class Notebook
    {
        [Key]
        [Column("notebook_id")]
        public long? NotebookId { get; set; } // 须要设置bson:"_id" 不然mgo不会认为是主键

        [Column("user_id")]
        public long? UserId { get; set; }

        [Column("notes_repository_id")]
        public long? NotesRepositoryId { get; set; }//仓库id

        [Column("parent_notebook_Id")]
        public long? ParentNotebookId { get; set; } // 上级

        [Column("seq")]
        public int Seq { get; set; } // 排序

        [Column("title")]
        public string Title { get; set; } // 标题

        [Column("url_title")]
        public string UrlTitle { get; set; } // Url标题

        [Column("number_notes")]
        public int NumberNotes { get; set; } // 笔记数

        [Column("is_trash")]
        public bool IsTrash { get; set; } // 是否是trash, 默认是false

        [Column("is_blog")]
        public bool IsBlog { get; set; } // 是否是Blog

        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }

        // 2015/1/15, 更新序号
        [Column("is_wx")]
        public bool IsWX { get; set; }//猜测 微信推送

        [Column("usn")]
        public int Usn { get; set; } // UpdateSequenceNum

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        //[Column("Subs", TypeName = "Notebook[]")]
        [NotMapped]
        public List<Notebook> Subs { get; set; }
    }

    public class NoteBookTree : Notebook
    {
        public new List<NoteBookTree> Subs { get; set; }

        public NoteBookTree()
        {
            this.Subs = new List<NoteBookTree>();
        }

        // 定义的显示类型转换,返回一个 MyClassA 类型的对象
    }

    // 仅仅是为了返回前台
    public class SubNotebooks
    {
        public Notebook[] Notebooks { get; set; }
    }

    public struct Notebooks
    {
        public Notebook notebook { get; set; }
        SubNotebooks SubNotebooks { get; set; }
    }

    // SubNotebook sort

    /*
修改方案, 因为要共享notebook的问题, 所以还是每个notebook一条记录
{
    notebookId,
    title,
    seq,
    parentNoteBookId, // 上级
    userId
}

得到所有该用户的notebook, 然后组装成tree返回之
更新顺序
添加notebook
更新notebook
删除notebook
*/

    public class DragNotebooksInfo
    {
        public long? curNotebookId { get; set; }
        public long? parentNotebookId { get; set; }
        public long?[] siblings { get; set; }
    }
}