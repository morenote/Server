using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    public class Suggestion
    {
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Addr { get; set; }

    }
}
