using MoreNote.Logic.Entity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Logic.Entity
{
    
    public  class UserInfo
    {
        public  const string ThirdGithub = "ThirdQQ";
        public  string ThirdQQ;
    }
    [Table("authorization")]
    public class Authorization
    {
        public Authorization()
        {

        }
        public Authorization(long? authorizationId, string type, string value)
        {
            AuthorizationId = authorizationId;
            AuthorizationType = type;
            AuthorizationValue = value;
        }

        [Key]
        [Column("authorization_id")]
        public long? AuthorizationId { get; set; }
        [Column("authorization_type")]
        public string AuthorizationType { get; set; }
        [Column("authorization_value")]
        public string AuthorizationValue { get; set; }
        
        
    }

    [Table("user_info")]
    public class User
    {
        [Key]
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("email")]
        public string Email { get; set; }//全是小写
        [Column("verified")]
        public bool Verified { get; set; }//Email是否已验证过?
        [Column("username")]
        public string Username { get; set; }//不区分大小写, 全是小写
        [Column("username_raw")]
        public string UsernameRaw { get; set; }//// 可能有大小写
        [Column("pwd")]
        public string Pwd { get; set; }
        [Column("salt")]
        public string Salt { get; set; }//MD5 盐
        [Column("google_authenticator_secret_key")]
        public string GoogleAuthenticatorSecretKey { get; set; }//谷歌身份验证密码
        [Column("pwd_cost")]
        public int Cost { get; set; }//加密强度--》迭代次数
        [Column("user_role")]
        public string Role { get; set; }//角色 用户组
        [Column("jurisdiction")]
        public List<Authorization> Jurisdiction { get;set;} //授权 拥有的权限

        [Column("created_time")]
        public DateTime CreatedTime{ get; set; }
        [Column("logo")]

        public string Logo{ get; set; }//9-24
        // 主题
        [Column("theme")]
        public string Theme{ get; set; }//使用的主题
        // 用户配置
        [Column("notebook_width")]
        public int NotebookWidth{ get; set; }// 笔记本宽度
        [Column("note_list_width")]
        public int NoteListWidth{ get; set; }// 笔记列表宽度
        [Column("md_editor_width")]
        public int MdEditorWidth{ get; set; }// markdown 左侧编辑器宽度
        [Column("left_is_min")]
        public bool LeftIsMin{ get; set; }// 左侧是否是隐藏的, 默认是打开的
        // 这里 第三方登录
        [Column("third_user_id")]
        public string ThirdUserId{ get; set; } // 用户Id, 在第三方中唯一可识别
        [Column("third_username")]
        public string ThirdUsername{ get; set; } // 第三方中username, 为了显示
        [Column("third_type")]
        public int ThirdType{ get; set; }// 第三方类型
        // 用户的帐户类型
        [Column("image_num")]
        public int ImageNum{ get; set; }//图片数量
        [Column("image_size")]
        public int ImageSize{ get; set; }//图片大小
        [Column("attach_num")]
        public int AttachNum{ get; set; }//附件数量
        [Column("attach_size")]
        public int AttachSize{ get; set; }//附件大小
        [Column("from_user_id")]
        public long? FromUserId{ get; set; }//邀请的用户
        [Column("account_type")]
        public int AccountType{ get; set; }// // normal(为空), premium
        [Column("account_start_time")]
        public DateTime AccountStartTime{ get; set; }//开始日期
        [Column("account_end_time")]
        public DateTime AccountEndTime{ get; set; }//结束日期

        //阈值
        [Column("max_image_num")]
        public int MaxImageNum{ get; set; }// 图片数量
        [Column("max_image_size")]
        public int MaxImageSize{ get; set; }//图片大小
        [Column("max_attach_num")]
        public int MaxAttachNum{ get; set; }//附件数量
        [Column("max_attach_size")]
        public int MaxAttachSize{ get; set; }//附件大小
        [Column("max_per_attach_size")]
        public int MaxPerAttachSize{ get; set; }//单个附件大小
          //更新序号
        [Column("usn")]
        public int Usn{ get; set; }//UpdateSequenceNum , 全局的
        [Column("full_sync_before")]
        public DateTime FullSyncBefore{ get; set; }//需要全量同步的时间, 如果 > 客户端的LastSyncTime, 则需要全量更新
    }
    [Table("user_account")]
    public class UserAccount
    {
        [Key]
        [Column("user_id")]
        public long? UserId{ get; set; }
        [Column("account_type")]
        public string AccountType{ get; set; } //normal(为空), premium
        [Column("account_start_time")]
        public DateTime AccountStartTime{ get; set; }//开始日期
        [Column("account_end_time")]
        public DateTime AccountEndTime{ get; set; }// 结束日期
       // 阈值
        [Column("max_image_num")]
        public int MaxImageNum{ get; set; }// 图片数量
        [Column("max_image_size")]
        public int MaxImageSize{ get; set; } // 图片大小
        [Column("max_attach_Num")]
        public int MaxAttachNum{ get; set; }    // 图片数量
        [Column("max_attach_size")]
        public int MaxAttachSize{ get; set; }   // 图片大小
        [Column("max_per_attach_size")]
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
[Table("user_and_blog")]
public class UserAndBlog
    {
    [Key]
    [Column("user_id")]
    public long? UserId{ get; set; }// 必须要设置bson:"_id" 不然mgo不会认为是主键
    [Column("email")]
    public string Email{ get; set; } // 全是小写
    [Column("username")]
    public string Username{ get; set; }// 不区分大小写, 全是小写
    [Column("logo")]
    public string Logo{ get; set; }
    [Column("blog_title")]
    public string BlogTitle{ get; set; }// 博客标题
    [Column("blog_logo")]
    public string BlogLogo{ get; set; }// 博客Logo
    [Column("blog_url")]
    public string BlogUrl{ get; set; } // 博客链接, 主页
    public BlogUrls BlogUrls { get;set;}
}





