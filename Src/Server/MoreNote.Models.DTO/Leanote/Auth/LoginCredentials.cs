using MoreNote.Models.DTO.Leanote.USBKey;

namespace MoreNote.Models.DTO.Leanote.Auth
{
	/// <summary>
	/// 登录凭证
	/// </summary>
	public class LoginCredentials
	{
		/// <summary>
		/// 口令凭证
		/// </summary>
		public PasswordCredentials? passwordCredentials { get; set; }
		/// <summary>
		/// 安全令牌凭证
		/// </summary>
		public ClientResponse? SmartToken { get; set; }

	}
}
