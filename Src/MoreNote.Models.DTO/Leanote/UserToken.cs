namespace MoreNote.Models.DTO.Leanote
{
	public class UserToken
	{
		public string Token { get; set; }
		public long? UserId { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
	}
}
