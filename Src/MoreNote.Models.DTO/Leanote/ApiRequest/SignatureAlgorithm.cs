namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
	public enum SignatureAlgorithm
	{
		/// <summary>
		/// 匿名
		/// </summary>
		Anonymous,
		/// <summary>
		/// 使用token验证身份
		/// </summary>
		Token,
		/// <summary>
		/// HMAC using SHA-256 
		/// </summary>
		HS256,
		/// <summary>
		/// RSA-4096 SHA256
		/// </summary>
		RS256,

		/// <summary>
		/// 
		/// </summary>
		ES256,
		/// <summary>
		/// 国密sm2 sm3
		/// </summary>
		SM2
	}
}
