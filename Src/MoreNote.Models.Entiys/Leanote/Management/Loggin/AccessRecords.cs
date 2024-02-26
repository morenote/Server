using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;


namespace MoreNote.Models.Entity.Leanote.Management.Loggin
{
	/// <summary>
	/// 访问记录
	/// </summary>
	[Table("access_records"), Index(nameof(IP), nameof(X_Real_IP), nameof(X_Forwarded_For), nameof(AccessTime), nameof(URL))]
	public class AccessRecords : BaseEntity
	{


		[Column("ip")]
		public string? IP { get; set; }
		[Column("x_real_ip")]
		public string? X_Real_IP { get; set; }
		[Column("x_forwarded_for")]
		public string? X_Forwarded_For { get; set; }
		[Column("referrer")]
		public string? Referrer { get; set; }//来源 从哪个网站来的
		[Column("request_header")]
		public string? RequestHeader { get; set; }//http header
		[Column("access_time")]
		public DateTime AccessTime { get; set; }
		[Column("unix_time")]
		public long? UnixTime { get; set; }
		[Column("time_interval")]
		public long? TimeInterval { get; set; }//距离上一次访问的时间间隔 如果没有上次 -1
		[Column("url")]
		public string? URL { get; set; }
		[Column("remote_ip_address")]
		public string? RemoteIPAddress { get; set; }
		[Column("remote_port")]
		public string? RemotePort { get; set; }

	}
	/// <summary>
	/// 白名单
	/// </summary>
	[Table("black_list")]
	public class Blacklist : BaseEntity
	{

		[Column("ip")]
		public string IP { get; set; }
	}

	[Table("backgroud_image_file")]
	public class BackgroudImageFile : BaseEntity
	{

		//如果 不进行FixContent处理，那么FileId=LocalFileId
		//public long? LocalFileId { get;set;}//客户端首次提交文件时的客户端定义的文件ID  
		[Column("album_id")]
		public long? AlbumId { get; set; }
		[Column("name")]
		public string Name { get; set; } // file name
		[Column("title")]
		public string Title { get; set; } // file  name or user defind for search
		[Column("size")]
		public long? Size { get; set; } // file  size (byte)
		[Column("type")]
		public string Type { get; set; } // file  type ""=image "doc"=word
		[Column("path")]
		public string Path { get; set; } // the file path
										 //0 public 1 protected 2 private
										 //公开 所有人可以访问
										 //保护 任何允许访问笔记的人可以允许访问
										 //私有 仅允许笔记拥有者访问
										 //public int AccessPermission { get;set; }
		[Column("is_default_album")]
		public bool IsDefaultAlbum { get; set; }
		[Column("created_time")]
		public DateTime CreatedTime { get; set; }
		[Column("access_number")]
		//自定义
		public int AccessNumber { get; set; }//文件访问数 
		[Column("sha1")]
		public string SHA1 { get; set; }
		[Column("md5")]
		public string MD5 { get; set; }
	}
}
