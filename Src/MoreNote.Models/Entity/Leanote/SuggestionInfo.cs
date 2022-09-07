using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

namespace MoreNote.Logic.Entity
{
    [Table("suggestion")]
    public class Suggestion: BaseEntity
    {
      
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("addr")]
        public string Addr { get; set; }

    }
}
