﻿using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Management
{
	/// <summary>
	/// 举报管理
	/// </summary>
	[Table("report_info")]
	public class ReportInfo : BaseEntity
	{

		[Column("note_id")]
		public long? NoteId { get; set; }
		[Column("user_id")]
		public long? UserId { get; set; } // UserId回复ToUserId 
		[Column("reason")]
		public string Reason { get; set; } // 评论内容 
		[Column("comment_id")]
		public int CommentId { get; set; } // 对某条评论进行回复 

	}
}
