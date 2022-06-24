namespace MoreNote.CryptographyProvider
{
    /// <summary>
    /// 加密服务接口
    /// </summary>
    public interface ICryptographyProvider
    {
      
        public Task<string> hmac(string data);
        public Task<bool> verifyHmac(string data, string mac);

        public Task<string> TransEncrypted(string cipherBase64);


        public Task<string> SM2Encrypt(string DataStr);
        public Task<string> SM2Decrypt(string base64Data);
    }
}