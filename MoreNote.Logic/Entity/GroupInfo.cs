using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.Entity;
using System.ComponentModel.DataAnnotations;

namespace MoreNote.Logic.Entity
{
    // 分组
    public class Group
    {
        [Key]
        public long GroupId { get; set; } // 谁的 
        public long UserId { get; set; } // 所有者Id 
        public string Title { get; set; } // 标题 
        public int UserCount { get; set; } // 用户数 

        public DateTime CreatedTime { get; set; }
        public User[] Users { get; set; }// 分组下的用户, 不保存, 仅查看
    }

    // 分组好友
    public class GroupUser
    {
        [Key]
        public long GroupUserId { get; set; } // 谁的 
        public long GroupId { get; set; } // 分组 
        public long UserId { get; set; } //  用户 

        public DateTime CreatedTime { get; set; }

    }
}
