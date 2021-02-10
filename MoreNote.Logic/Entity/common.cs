using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    [Table("page")]
    public class Page
    {
        [Key]
        [Column("page_id")]
        public long? PageId { get; set; }
        [Column("cur_page")]
        public int CurPage { get; set; } // 当前页码 
        [Column("total_page")]
        public int TotalPage { get; set; } // 总页 
        [Column("per_page_size")]
        public int PerPageSize { get; set; }
        [Column("count")]
        public int Count { get; set; } // 总记录数 

    }
}
