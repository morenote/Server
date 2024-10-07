namespace MoreNote.SecurityProvider.Core
{
	/// <summary>
	/// 加密服务接口
	/// </summary>
	public interface ICryptographyProvider
	{

        public Task<byte[]> Hmac(byte[] data);
        public Task<bool> VerifyHmac(byte[] data, byte[] mac);

		public Task<byte[]> TransEncrypted(byte[] cipher, byte[] salt);
		public Task<byte[]> SM2Encrypt(byte[] data);
		public Task<byte[]> SM2Decrypt(byte[] data);

        public  Task<byte[]> SM4Encrypt(byte[] data, byte[] iv);
        public  Task<byte[]> SM4Decrypt(byte[] data, byte[] iv);

        public  Task<byte[]> SM4Encrypt(byte[] data);
        public Task<byte[]>  SM4Decrypt(byte[] data);

		public Task<byte[]> SM3(byte[] data);

	}
}