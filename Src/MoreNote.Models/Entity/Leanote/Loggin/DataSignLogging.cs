using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Loggin
{
    /// <summary>
    /// 数据签名记录表
    /// </summary>
    [Table("data_sign"), Index(nameof(Id),nameof(DataId))]
    public class DataSignLogging:BaseEntity
    {
        [Column("data_id")]
        public long? DataId { get; set; }
        [Column("tag")]
        public string Tag { get; set; }
        [Column("data_sign_json")]
        public string DataSignJson { get; set; }



    }
}
