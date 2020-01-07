using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 配置, 每一个配置一行记录
    public class Config
    {
        [Key]
        public long ConfigId { get; set; }
        public long UserId { get; set; }
        public string Key { get; set; }
        public string ValueStr { get; set; }
        
      

    }
}
