using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Pay
{
    public class PayEntity
    {
        [Key]//主键 
             // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Chapter { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime PayTime { get; set; }
        public int Money { get; set; }

    }
}
