﻿using Morenote.Models.Models.Entity;

using MoreNote.Models.Enums;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{

	/// <summary>
	/// 附件
	/// </summary>
	[Table("attach_info")]
	public class AttachInfo : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("note_id")]
		public long? NoteId { get; set; }
		[Column("upload_user_id")]
		public long? UploadUserId { get; set; } // 可以不是note owner, 协作者userId
		[Column("name")]
		public string Name { get; set; } // file name, MD5, such as 13232312.doc
		[Column("title")]
		public string Title { get; set; } // raw file name
		[Column("size")]
		public Int64 Size { get; set; } // file size (byte)
		[Column("type")]
		public string Type { get; set; }   // file type, "doc" = word
		[Column("path")]
		public string Path { get; set; }  // the file path such as: files/userId/attachs/adfadf.doc
										  // public  StorageTypeEnum StorageType { get; set; }//附件储存方式
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }
		[Column("storage_type")]
		public StorageTypeEnum StorageType { get; set; } //储存方式 本地？又拍云 对象储存？

	}
}
