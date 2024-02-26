using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.User
{
	[Table("user_account")]
	public class UserAccount : BaseEntity
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
	// note主页需要
}
