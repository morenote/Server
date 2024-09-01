using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Security.Face
{
	/// <summary>
	/// 存储人脸数据
	/// </summary>
	[Table("face_sample"), Index(nameof(Id), IsUnique = true)]
	public class FaceSample : BaseEntity
	{

		/// <summary>
		/// 用户id
		/// </summary>
		[Column("user_id")]
		public long? UserId { get; set; }

		/// <summary>
		/// 人脸样本存储地址
		/// </summary>
		[Column("face_image_path")]
		public string FaceImagePath { get; set; }




	}
}
