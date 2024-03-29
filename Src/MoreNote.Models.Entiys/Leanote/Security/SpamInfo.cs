﻿using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.Security
{
	[Table("spam_info")]
	public class SpamInfo : BaseEntity
	{

		[Column("spam_input")]
		public string Input { get; set; }
		[Column("prediction")]
		public bool Prediction { get; set; }//机器识别结果
		[Column("score")]
		public float Score { get; set; }//机器识别可靠性
		[Column("manual_check")]
		public bool ManualCheck { get; set; }//是否经过人工复核
		[Column("manual_result")]
		public bool ManualResult { get; set; }//人工识别结果
		[Column("creat_data")]
		public DateTime CreatData { get; set; }//输入时间
	}
}
