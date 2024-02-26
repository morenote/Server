namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
	public enum EncryptionAlgorithm
	{
		/// <summary>
		/// 明文
		/// </summary>
		Plaintext,
		/// <summary>
		/// 对称加密算法AES256
		/// </summary>
		AES256,
		/// <summary>
		/// 国密SM4
		/// </summary>
		SM4

	}
}
