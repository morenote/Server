using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 配置, 每一个配置一行记录
    [Table("config")]
    public class Config
    {
        [Key]
        [Column("config_id")]
        public long? ConfigId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("key")]
        public string Key { get; set; }
        [Column("value_str")]
        public string ValueStr { get; set; }
        
      

    }
}
