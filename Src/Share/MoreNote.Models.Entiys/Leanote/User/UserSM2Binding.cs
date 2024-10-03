using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MoreNote.Models.Entity.Leanote.User
{
	[Table("user_sm2_binding"), Index(nameof(UserId))]
	public class UserSM2Binding : BaseEntity
	{

		[Column("user_id")]
		public long? UserId { get; set; }
		[Column("certificate")]
		public string Certificate { get; set; }
		[Column("public_key")]
		public string PublicKey { get; set; } = "SHA1";
		[Column("hmac")]
		public string Hmac { get; set; }

		public byte[] GetBytes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Id=" + Id);
			stringBuilder.Append("Certificate=" + Certificate);
			stringBuilder.Append("PublicKey" + PublicKey);
		
			return Encoding.UTF8.GetBytes(stringBuilder.ToString());
		}

	}
}
