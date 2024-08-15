using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	// 评论
	[Table("blog_comment")]
	public class BlogComment : BaseEntity
	{

		[Column("note_id")]
		public long? NoteId { get; set; }
		[Column("user_id")]
		public long? UserId { get; set; }// UserId回复ToUserId
		[Column("content")]
		public string Content { get; set; } // 评论内容
		[Column("to_comment_id")]
		public long? ToCommentId { get; set; }// 对某条评论进行回复
		[Column("to_user_id")]
		public long? ToUserId { get; set; } // 为空表示直接评论, 不回空表示回复某人
		[Column("like_num")]
		public int LikeNum { get; set; }// 点赞次数, 评论也可以点赞
		[Column("like_user_ids")]
		public long?[] LikeUserIds { get; set; }// 点赞的用户ids
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }
		/// <summary>
		/// 评论是否允许公开显示，评论经过批准允许后才可以公开显示
		/// </summary>
		[Column("allow")]
		public bool Allow { get; set; }

	}
}
