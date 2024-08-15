using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.Models.Enums;
using MoreNote.Models.Enums.Common.Editor;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace MoreNote.Models.Entity.Leanote.User
{

	[Table("user_info"), Index(nameof(Email), nameof(Verified), nameof(Username), nameof(UsernameRaw), nameof(Role)
		, nameof(ThirdUserId), nameof(FromUserId))]
	public class UserInfo : BaseEntity
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
		///         Argon2（支持） bcrypt（支持） scrypt   pbkdf2（支持） pbkdf2SM3
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

		/// <summary>
		/// 客户端哈希算法（口令派生密钥算法）
		/// </summary>

		[Column("client_password_hash_algorithm")]
		public string? ClientPasswordHashAlgorithm { get; set; }
		/// <summary>
		/// 客户端软模块哈希加密迭代次数，
		/// </summary>
		[Column("client_password_hash_iterations")]

		public int? ClinetPasswordHashIterations { get; set; }

		/// <summary>
		/// 客户端软模块哈希加密时CPU线程限制，
		/// 当启用加密机时，用户口令由加密机保护，此参数无效
		/// </summary>
		[Column("client_password_degree_of_parallelism")]

		public int? ClientPasswordDegreeOfParallelism { get; set; }

		/// <summary>
		/// 客户端软模块哈希加密时内存限制，
		/// 当启用加密机时，用户口令由加密机保护，此参数无效
		/// </summary>
		[Column("client_password_memory_size")]

		public int? ClientPasswordMemorySize { get; set; }
		/// <summary>
		/// 客户端盐
		/// </summary>

		[Column("client_salt")]

		public string? ClientSalt { get; set; }//盐 盐的长度默认是32字节,当启用加密机时此参数无效



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
		public string? Profile { get; set; }//个人简介
		[Column("country")]
		public string? Country { get; set; }//国家

		[Column("address")]
		public string? Address { get; set; }//地址
		[Column("phone")]
		public string? Phone { get; set; }//地址

		[Column("Avatar")]
		public string? Avatar { get; set; }//头像

		[Column("geographic_province")]
		public string? GeographicProvince { get; set; }//地理位置


		[Column("geographic_city")]
		public string? GeographicCity { get; set; }//地理位置

		[Column("signature")]
		public string? Signature { get; set; }//个人名片签名
		[Column("title")]
		public string? Title { get; set; }//个人名片标题
		[Column("group")]
		public string? Group { get; set; }//个人名片归属团体

		[Column("tags")]
		public string? Tags { get; set; }//个人名片标签

		//==================================编辑器偏好======================================================
		[Column("markdown_option")]
		public MarkdownEditorOption? PreferredMarkdownEditor { get; set; } = MarkdownEditorOption.vditor;//富文本编辑器选项

		[Column("rich_text_option")]
		public RichTextEditorOption? PreferredRichTextEditor { get; set; } = RichTextEditorOption.textbus;//markdown编辑器选项

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
			return Role.ToLower().Equals("admin");
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
			stringBuilder.Append("Id=" + Id);
			stringBuilder.Append("Email=" + Email);
			stringBuilder.Append("Username=" + Username);
			stringBuilder.Append("Verified=" + Verified);
			stringBuilder.Append("Pwd=" + Pwd);
			stringBuilder.Append("PasswordHashAlgorithm=" + PasswordHashAlgorithm);
			stringBuilder.Append("PasswordHashIterations=" + PasswordHashIterations);
			stringBuilder.Append("PasswordDegreeOfParallelism=" + PasswordDegreeOfParallelism);
			stringBuilder.Append("PasswordMemorySize=" + PasswordMemorySize);
			stringBuilder.Append("Salt=" + Salt);
			stringBuilder.Append("Role=" + Role);
			return stringBuilder.ToString();
		}



		public bool IsSuperAdmin()
		{
			return Role.ToLower().Equals("superadmin");
		}
	}

}
