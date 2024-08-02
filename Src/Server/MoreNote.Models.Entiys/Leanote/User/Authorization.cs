using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Models.Entity.Leanote.User
{
	[Table("authorization")]
	public class Authorization
	{
		public Authorization()
		{
		}

		public Authorization(long? authorizationId, string type, string value)
		{
			AuthorizationId = authorizationId;
			AuthorizationType = type;
			AuthorizationValue = value;
		}

		[Key]
		[Column("authorization_id")]
		public long? AuthorizationId { get; set; }

		[Column("authorization_type")]
		public string AuthorizationType { get; set; }

		[Column("authorization_value")]
		public string AuthorizationValue { get; set; }
	}

}
