namespace MoreNote.CryptographyProvider
{
	/// <summary>
	/// 加密服务接口
	/// </summary>
	public interface ICryptographyProvider
	{

		public byte[] Hmac(byte[] data);
		public bool VerifyHmac(byte[] data, byte[] mac);

		public byte[] TransEncrypted(byte[] cipher, byte[] salt);
		public byte[] SM2Encrypt(byte[] data);
		public byte[] SM2Decrypt(byte[] data);

		public byte[] SM4Encrypt(byte[] data, byte[] iv);
		public byte[] SM4Decrypt(byte[] data, byte[] iv);

		public byte[] SM4Encrypt(byte[] data);
		public byte[] SM4Decrypt(byte[] data);

	}
}