using leanote.Common.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    /// <summary>
    /// 专辑
    /// </summary>
    public class Album
    {
        [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long  AlbumId { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }// album name
        public int Type { get; set; }// type, the default is image: 0
        public int seq { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
