using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    public class Notebook
    {
        [Key]
        public long NotebookId { get; set; } // 须要设置bson:"_id" 不然mgo不会认为是主键
        public long UserId { get; set; }
        public long ParentNotebookId { get; set; } // 上级 
        public int Seq { get; set; } // 排序 
        public string Title { get; set; } // 标题 
        public string UrlTitle { get; set; } // Url标题 
        public int NumberNotes { get; set; } // 笔记数 
        public bool IsTrash { get; set; } // 是否是trash, 默认是false
        public bool IsBlog { get; set; } // 是否是Blog 

        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        // 2015/1/15, 更新序号
        public bool IsWX { get; set; }//猜测 微信推送
        public int Usn { get; set; } // UpdateSequenceNum 
        public bool IsDeleted { get; set; }
        //[Column("Subs", TypeName = "Notebook[]")]
        [NotMapped]
        public List<Notebook> Subs { get; set; }

    }
    public class NoteBookTree:Notebook
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
    public class Notebooks
    {
        public Notebook notebook { get; set; }
        SubNotebooks subNotebooks;
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


}
