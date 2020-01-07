using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 主题, 每个用户有多个主题, 这里面有主题的配置信息
    // 模板, css, js, images, 都在路径Path下
   public class Theme
    {

        [Key]
        public long ThemeId { get; set; } // 必须要设置bson:"_id" 
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string AuthorUrl { get; set; }
        public string Path { get; set; } // 文件夹路径, 
        public string[] Info { get; set; }// 所有信息
       
        public bool IsActive { get; set; } // 是否在用 
        public bool IsDefault { get; set; } // leanote默认主题, 如果用户修改了默认主题, 则先copy之. 也是admin用户的主题
        public string Style { get; set; } //之前的, 只有default的用户才有blog_default, blog_daqi, blog_left_fixed
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        //public FriendLinks[] FriendLinksArray { get; set; }//友链
    }
    public class FriendLinks
    {
        [Key]
        public long FriendLinksId { get; set; }
        public long ThemeId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
       
    }
}
