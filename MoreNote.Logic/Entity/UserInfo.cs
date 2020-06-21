using MoreNote.Logic.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MoreNote.Logic.Entity
{
    
    public static class UserInfo
    {
        public  const string ThirdGithub = "ThirdQQ";
        public static string ThirdQQ;
    }
    public class Authorization
    {
        [Key]
        public long AuthorizationId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class User
    {
        [Key]
        public long UserId { get; set; }
        public string Email { get; set; }//全是小写
        public bool Verified { get; set; }//Email是否已验证过?
        public string Username { get; set; }//不区分大小写, 全是小写
        public string UsernameRaw { get; set; }//// 可能有大小写
        public string Pwd { get; set; }
        public string Salt { get; set; }//MD5 盐
       
        public int Cost { get; set; }//加密强度--》迭代次数
        public string Role { get; set; }//角色 用户组
        public Authorization[] Jurisdiction { get;set;} //授权 拥有的权限

        public DateTime CreatedTime{ get; set; }

        public string Logo{ get; set; }//9-24
        // 主题
        public string Theme{ get; set; }//使用的主题
        // 用户配置
        public int NotebookWidth{ get; set; }// 笔记本宽度
        public int NoteListWidth{ get; set; }// 笔记列表宽度
        public int MdEditorWidth{ get; set; }// markdown 左侧编辑器宽度
        public bool LeftIsMin{ get; set; }// 左侧是否是隐藏的, 默认是打开的
        // 这里 第三方登录
        public string ThirdUserId{ get; set; } // 用户Id, 在第三方中唯一可识别
        public string ThirdUsername{ get; set; } // 第三方中username, 为了显示
        public int ThirdType{ get; set; }// 第三方类型
        // 用户的帐户类型
        public int ImageNum{ get; set; }//图片数量
        public int ImageSize{ get; set; }//图片大小
        public int AttachNum{ get; set; }//附件数量
        public int AttachSize{ get; set; }//附件大小
        public long FromUserId{ get; set; }//邀请的用户

        public int AccountType{ get; set; }// // normal(为空), premium
        public DateTime AccountStartTime{ get; set; }//开始日期
        public DateTime AccountEndTime{ get; set; }//结束日期

        //阈值

        public int MaxImageNum{ get; set; }// 图片数量
        public int MaxImageSize{ get; set; }//图片大小
        public int MaxAttachNum{ get; set; }//附件数量
        public int MaxAttachSize{ get; set; }//附件大小
        public int MaxPerAttachSize{ get; set; }//单个附件大小
        //更新序号
        public int Usn{ get; set; }//UpdateSequenceNum , 全局的
        public DateTime FullSyncBefore{ get; set; }//需要全量同步的时间, 如果 > 客户端的LastSyncTime, 则需要全量更新


    }
    public class UserAccount
    {
        [Key]
        public long UserId{ get; set; }
        public string AccountType{ get; set; } //normal(为空), premium
        public DateTime AccountStartTime{ get; set; }//开始日期
        public DateTime AccountEndTime{ get; set; }// 结束日期
        // 阈值
        public int MaxImageNum{ get; set; }// 图片数量
        public int MaxImageSize{ get; set; } // 图片大小
        public int MaxAttachNum{ get; set; }    // 图片数量
        public int MaxAttachSize{ get; set; }   // 图片大小
        public int MaxPerAttachSize{ get; set; }// 单个附件大小
    }
}    // note主页需要
    public class UserAndBlogUrl
    {

        public User user { get; set; }
        public string BlogUrl{ get; set; }
        public string PostUrl{ get; set; }
    }

// 用户与博客信息结合, 公开
public class UserAndBlog
    {
    [Key]
    public long UserId{ get; set; }// 必须要设置bson:"_id" 不然mgo不会认为是主键
    public string Email{ get; set; } // 全是小写
    public string Username{ get; set; }// 不区分大小写, 全是小写
    public string Logo{ get; set; }
    public string BlogTitle{ get; set; }// 博客标题
    public string BlogLogo{ get; set; }// 博客Logo
    public string BlogUrl{ get; set; } // 博客链接, 主页
}





