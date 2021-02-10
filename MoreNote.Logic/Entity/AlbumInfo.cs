
using MoreNote.Logic.Model;
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
    [Table("album")]
    public class Album
    {
        [Key]
        [Column("album_id")]
       // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public long?  AlbumId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }// album name
        [Column("type")]
        public int Type { get; set; }// type, the default is image: 0
        [Column("seq")]
        public int SEQ { get; set; }
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
       
       
    }
}
