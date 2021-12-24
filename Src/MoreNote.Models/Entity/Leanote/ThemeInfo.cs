using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{
    // 主题, 每个用户有多个主题, 这里面有主题的配置信息
    // 模板, css, js, images, 都在路径Path下
    [Table("theme")]
    public class Theme
    {
        [Key]
        [Column("theme_id")]
        public long? ThemeId { get; set; } // 必须要设置bson:"_id" 
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("author")]
        public string Author { get; set; }
        [Column("author_url")]
        public string AuthorUrl { get; set; }
        [Column("theme_path")]
        public string Path { get; set; } // 文件夹路径, 
        [Column("info")]
        public string[] Info { get; set; }// 所有信息
        [Column("is_active")]
        public bool IsActive { get; set; } // 是否在用 
        [Column("is_default")]
        public bool IsDefault { get; set; } // leanote默认主题, 如果用户修改了默认主题, 则先copy之. 也是admin用户的主题
        [Column("style")]
        public string Style { get; set; } //之前的, 只有default的用户才有blog_default, blog_daqi, blog_left_fixed
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        //public FriendLinks[] FriendLinksArray { get; set; }//友链
    }
    [Table("friend_links")]
    public class FriendLinks
    {
        [Key]
        [Column("friend_links_id")]
        public long? FriendLinksId { get; set; }
        [Column("theme_id")]
        public long? ThemeId { get; set; }
        [Column("title")]
        public string? Title { get; set; }
        [Column("url")]
        public string? Url { get; set; }
       
    }
}
