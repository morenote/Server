using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Models.Entity.Leanote.User
{
	/// <summary>
	/// 用户实名认证信息
	/// </summary>
	[Table("real_name_information")]
	public class RealNameInformation : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("id_card_no")]

		public string? IdCardNo { get; set; }
		[Column("is_encryption")]
		public bool IsEncryption { get; set; }


		[Column("hmac")]
		public string? Hmac { get; set; }
		[NotMapped]
		public bool Verify { get; set; }
		public string ToStringNoMac()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Id=" + Id);
			stringBuilder.Append("UserId=" + UserId);
			stringBuilder.Append("RealName=" + IdCardNo);
			return stringBuilder.ToString();
		}



	}
}
