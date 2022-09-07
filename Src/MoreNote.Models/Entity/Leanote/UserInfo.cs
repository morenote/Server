using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Entity;
using MoreNote.Models.DTO.Leanote.Auth;
using MoreNote.Models.Entity.Security.FIDO2;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace MoreNote.Logic.Entity
{
    public class UserInfo
    {
        public const string ThirdGithub = "ThirdQQ";
        public string ThirdQQ;
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

    [Table("user_info"), Index(nameof(Email), nameof(Verified), nameof(Username), nameof(UsernameRaw), nameof(Role)
        , nameof(ThirdUserId), nameof(FromUserId))]
    public class User: BaseEntity
    {
       

        [Column("email")]
        public string? Email { get; set; }//全是小写

        [Column("verified")]
        public bool Verified { get; set; }//Email是否已验证过?

        [Column("username")]
        public string? Username { get; set; }//不区分大小写, 全是小写

        [Column("username_raw")]
        public string? UsernameRaw { get; set; }//// 可能有大小写

        [Column("pwd")]
        [JsonIgnore]
        public string? Pwd { get; set; }

        /// <summary>
        /// 加密用户登录口令使用的哈希算法
        /// <para>
        /// 密码算法
        ///   哈希算法
        ///         sha256（支持）  sha512
        ///         HMAC算法
        ///             hmac-sha256 hmac-sha512
        ///   慢哈希算法
        ///         Argon2（支持） bcrypt（支持） scrypt   pbkdf2（支持）
        /// 由安全芯片或加密设备实现
        ///   sjj1962（支持）
        /// </para>
        /// </summary>
        [Column("password_hash_algorithm")]
        public string PasswordHashAlgorithm { get; set; }

        /// <summary>
        /// 软模块哈希加密迭代次数，
        /// 当启用加密机时，用户口令由加密机保护，此参数无效
        /// </summary>
        [Column("password_hash_iterations")]
        [JsonIgnore]
        public int PasswordHashIterations { get; set; }

        /// <summary>
        /// 软模块哈希加密时CPU线程限制，
        /// 当启用加密机时，用户口令由加密机保护，此参数无效
        /// </summary>
        [Column("password_degree_of_parallelism")]
        [JsonIgnore]
        public int PasswordDegreeOfParallelism { get; set; }

        /// <summary>
        /// 软模块哈希加密时内存限制，
        /// 当启用加密机时，用户口令由加密机保护，此参数无效
        /// </summary>
        [Column("password_memory_size")]
        [JsonIgnore]
        public int PasswordMemorySize { get; set; }

        [Column("salt")]
        [JsonIgnore]
        public string? Salt { get; set; }//盐 盐的长度默认是32字节,当启用加密机时此参数无效

        [Column("google_authenticator_secret_key")]
        [JsonIgnore]
        public string? GoogleAuthenticatorSecretKey { get; set; }//谷歌身份验证密码

        [Column("user_role")]
        public string Role { get; set; }//角色 用户组  Admin,SuperAdmin,User

        [Column("jurisdiction")]
        public List<Authorization> Jurisdiction { get; set; } //授权 拥有的权限

        [Column("created_time")]
        public DateTime CreatedTime { get; set; }

        [Column("logo")]
        public string? Logo { get; set; }//9-24

        // 主题
        [Column("theme")]
        public string? Theme { get; set; }//使用的主题

        // 用户配置
        [Column("notebook_width")]
        public int NotebookWidth { get; set; }// 笔记本宽度

        [Column("note_list_width")]
        public int NoteListWidth { get; set; }// 笔记列表宽度

        [Column("md_editor_width")]
        public int MdEditorWidth { get; set; }// markdown 左侧编辑器宽度

        [Column("left_is_min")]
        public bool LeftIsMin { get; set; }// 左侧是否是隐藏的, 默认是打开的

        // 这里 第三方登录
        [Column("third_user_id")]
        public string? ThirdUserId { get; set; } // 用户Id, 在第三方中唯一可识别

        [Column("third_username")]
        public string? ThirdUsername { get; set; } // 第三方中username, 为了显示

        [Column("third_type")]
        public int ThirdType { get; set; }// 第三方类型

        // 用户的帐户类型
        [Column("image_num")]
        public int ImageNum { get; set; }//图片数量

        [Column("image_size")]
        public int ImageSize { get; set; }//图片大小

        [Column("attach_num")]
        public int AttachNum { get; set; }//附件数量

        [Column("attach_size")]
        public int AttachSize { get; set; }//附件大小

        [Column("from_user_id")]
        public long? FromUserId { get; set; }//邀请的用户

        [Column("account_type")]
        public int AccountType { get; set; }// // normal(为空), premium

        [Column("account_start_time")]
        public DateTime AccountStartTime { get; set; }//开始日期

        [Column("account_end_time")]
        public DateTime AccountEndTime { get; set; }//结束日期

        //阈值
        [Column("max_image_num")]
        public int MaxImageNum { get; set; }// 图片数量

        [Column("max_image_size")]
        public int MaxImageSize { get; set; }//图片大小

        [Column("max_attach_num")]
        public int MaxAttachNum { get; set; }//附件数量

        [Column("max_attach_size")]
        public int MaxAttachSize { get; set; }//附件大小

        [Column("max_per_attach_size")]
        public int MaxPerAttachSize { get; set; }//单个附件大小

        //更新序号
        [Column("usn")]
        public int Usn { get; set; }//UpdateSequenceNum , 全局的

        [Column("full_sync_before")]
        public DateTime FullSyncBefore { get; set; }//需要全量同步的时间, 如果 > 客户端的LastSyncTime, 则需要全量更新

        [Column("blog_url")]
        public string? BlogUrl { get; set; }

        [Column("post_url")]
        public string? PostUrl { get; set; }
        //------------------------------------联系信息----------------------------------------------------
        [Column("profile")]
        public string Profile { get;set;}//个人简介
        [Column("country")]
        public string Country { get;set;}//国家

        [Column("address")]
        public string Address { get; set; }//地址
        [Column("phone")]
        public string Phone { get; set; }//地址

        [Column("Avatar")]
        public string Avatar { get; set; }//头像

        [Column("geographic_province")]
        public string GeographicProvince { get; set; }//地理位置


        [Column("geographic_city")]
        public string GeographicCity { get; set; }//地理位置

        [Column("signature")]
        public string Signature { get; set; }//个人名片签名
        [Column("title")]
        public string Title { get; set; }//个人名片标题
        [Column("group")]
        public string Group { get; set; }//个人名片归属团体

        [Column("tags")]
        public string Tags { get; set; }//个人名片标签

        //==================================编辑器偏好======================================================
        [Column("markdown_option")]
        public string? MarkdownEditorOption { get; set; } = "ace";//富文本编辑器选项

        [Column("rich_text_option")]
        public string? RichTextEditorOption { get; set; } = "tinymce";//markdown编辑器选项

        //==================================安全密钥  FIDO2 yubikey=========================================
        [Column("fido2_items")]
        public List<FIDO2Item>? FIDO2Items { get; set; }

        [Column("password_expired")]
        public int? PasswordExpired { get; set; }//密码过期时间

        [Column("change_password_reminder")]
        public bool? ChangePasswordReminder { get; set; }//修改密码提醒

        [Column("login_security_policy_level")]
        public LoginSecurityPolicyLevel LoginSecurityPolicyLevel { get; set; } = LoginSecurityPolicyLevel.unlimited;//登录安全策略级别

        //=======================================================================================================
        public bool IsAdmin()
        {
            return this.Role.ToLower().Equals("admin");
        }

        [NotMapped]
        public bool Verify { get; set; }

        [Column("hmac")]
        public string? Hmac { get; set; }

        /// <summary>
        /// 用于计算hmac的材料，hmac用于防止数据库被篡改
        /// </summary>
        /// <returns></returns>
        public string ToStringNoMac()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Id=" + this.Id);
            stringBuilder.Append("Email=" + this.Email);
            stringBuilder.Append("Username=" + this.Username);
            stringBuilder.Append("Verified=" + this.Verified);
            stringBuilder.Append("Pwd=" + this.Pwd);
            stringBuilder.Append("PasswordHashAlgorithm=" + this.PasswordHashAlgorithm);
            stringBuilder.Append("PasswordHashIterations=" + this.PasswordHashIterations);
            stringBuilder.Append("PasswordDegreeOfParallelism=" + this.PasswordDegreeOfParallelism);
            stringBuilder.Append("PasswordMemorySize=" + this.PasswordMemorySize);
            stringBuilder.Append("Salt=" + this.Salt);
            stringBuilder.Append("Role=" + this.Role);
            return stringBuilder.ToString();
        }

        public User AddMac(ICryptographyProvider cryptographyProvider)
        {
            
                var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
              
                this.Hmac =  cryptographyProvider.Hmac(bytes).ByteArrayToBase64();
                return this;
        }

        public User  VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            if (string.IsNullOrEmpty(this.Hmac))
            {
                this.Verify = false;
                return this;
            }
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
           
            var result =  cryptographyProvider.VerifyHmac(bytes, this.Hmac.Base64ToByteArray());
            this.Verify = result;

            return this;
        }

        public bool IsSuperAdmin()
        {
            return this.Role.ToLower().Equals("superadmin");
        }
    }

    [Table("user_account")]
    public class UserAccount: BaseEntity
    {
        

        [Column("account_type")]
        public string AccountType { get; set; } //normal(为空), premium

        [Column("account_start_time")]
        public DateTime AccountStartTime { get; set; }//开始日期

        [Column("account_end_time")]
        public DateTime AccountEndTime { get; set; }// 结束日期

        // 阈值
        [Column("max_image_num")]
        public int MaxImageNum { get; set; }// 图片数量

        [Column("max_image_size")]
        public int MaxImageSize { get; set; } // 图片大小

        [Column("max_attach_Num")]
        public int MaxAttachNum { get; set; }    // 图片数量

        [Column("max_attach_size")]
        public int MaxAttachSize { get; set; }   // 图片大小

        [Column("max_per_attach_size")]
        public int MaxPerAttachSize { get; set; }// 单个附件大小
    }
}    // note主页需要

//public class UserAndBlogUrl
//{
//    public User User { get; set; }
//    public string BlogUrl{ get; set; }
//    public string PostUrl{ get; set; }
//}

// 用户与博客信息结合, 公开
[Table("user_and_blog")]
public class UserAndBlog
{
    [Key]
    [Column("user_id")]
    public long? UserId { get; set; }// 必须要设置bson:"_id" 不然mgo不会认为是主键

    [Column("email")]
    public string Email { get; set; } // 全是小写

    [Column("username")]
    public string Username { get; set; }// 不区分大小写, 全是小写

    [Column("logo")]
    public string Logo { get; set; }

    [Column("blog_title")]
    public string BlogTitle { get; set; }// 博客标题

    [Column("blog_logo")]
    public string BlogLogo { get; set; }// 博客Logo

    [Column("blog_url")]
    public string BlogUrl { get; set; } // 博客链接, 主页

    public BlogUrls BlogUrls { get; set; }
}