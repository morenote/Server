namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
	public class Header
	{
		/// <summary>
		/// 签名算法
		/// </summary>
		public SignatureAlgorithm SignatureAlgorithm { get; set; } = SignatureAlgorithm.SM2;
		/// <summary>
		/// 加密算法
		/// </summary>
		public EncryptionAlgorithm EncryptionAlgorithm { get; set; } = EncryptionAlgorithm.SM4;
		/// <summary>
		/// 时间戳
		/// </summary>
		public long Timestamp { get; set; }
		/// <summary>
		/// 计数器
		/// </summary>
		public long Counter { get; set; }


	}
}
