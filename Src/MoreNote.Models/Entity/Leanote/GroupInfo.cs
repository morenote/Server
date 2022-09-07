using System;
using System.Collections.Generic;
using System.Text;
using MoreNote.Logic.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Morenote.Models.Models.Entity;

namespace MoreNote.Logic.Entity
{
    // 分组
    [Table("group_team")]
    public class GroupTeam: BaseEntity
    {
        
        [Column("user_id")]
        public long? UserId { get; set; } // 所有者Id 
        [Column("title")]
        public string Title { get; set; } // 标题 
        [Column("user_count")]
        public int UserCount { get; set; } // 用户数 
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        //[Column("users")]
       // public User[] Users { get; set; }// 分组下的用户, 不保存, 仅查看
    }

    // 分组好友
    [Table("group_team_user")]
    public class GroupTeamUser: BaseEntity
    {
       
        [Column("group_id")]
        public long? GroupId { get; set; } // 分组 
        [Column("user_id")]
        public long? UserId { get; set; } //  用户 
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

    }
}
