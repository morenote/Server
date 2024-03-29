﻿

using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
	/// <summary>
	/// 提交树，版本树
	/// </summary>
	[Table("submit_tree")]
	public class SubmitTree : BaseEntity
	{
		[Column("owner")]
		public long? Owner { get; set; }//拥有者
		[Column("height")]
		public int Height { get; set; } = 0;//当前高度
		[Column("root")]
		public long? Root { get; set; }//树根Id
		[Column("top")]
		public long? Top { get; set; } //树顶Id
	}
}
