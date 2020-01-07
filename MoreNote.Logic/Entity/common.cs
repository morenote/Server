using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
   public class Page
    {
        [Key]
        public long PageId { get; set; }
        public int CurPage { get; set; } // 当前页码 
        public int TotalPage { get; set; } // 总页 
        public int PerPageSize { get; set; }
        public int Count { get; set; } // 总记录数 

    }
}
