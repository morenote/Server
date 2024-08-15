using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using MoreNote.Logic.Models.Enum;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Notes
{
	/// <summary>
	/// 分享笔记 这个的共享仅仅是只读性质
	/// </summary>
	[Table("share_note"), Index(nameof(Id), nameof(NoteId))]
	public class ShareNote : BaseEntity
	{


		[Column("note_id")]
		public long? NoteId { get; set; }

		[Column("start_share_time")]
		public DateTime? StartShareTime { get; set; }//开始共享的时间

		[Column("end_Share_time")]
		public DateTime? EndShareTime { get; set; }//结束共享的时间

		[Column("share_password")]
		public string SharePassword { get; set; }//分享口令 也就是提取码

		[Column("share_type")]
		public ShareTypeEnum ShareType { get; set; } = ShareTypeEnum.Note;//分享类型 笔记

		[Column("download_count")]
		public int? DownloadCount { get; set; }//累计已经下载次数

		[Column("max_download_count")]
		public int? MaxDownloadCount { get; set; }//最大累计已经下载次数 也就是限制下载次数

		[Column("must_login")]
		public bool MustLogin { get; set; }//是否要求下载者必须登录

		[Column("allow_downloading_attachments")]
		public bool AllowDownloadingAttachments { get; set; }//是否允许下载与本笔记关联的附件

		[Column("allow_pull")]
		public bool AllowPull { get; set; }//允许导入下载者的笔记 否则只能阅读

	}
}