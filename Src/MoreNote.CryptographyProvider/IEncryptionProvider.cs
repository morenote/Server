namespace MoreNote.CryptographyProvider
{
    /// <summary>
    /// 加密服务接口
    /// </summary>
    public interface ICryptographyProvider
    {
      
        public Task<string> hmac(string data);
        public Task<bool> verifyHmac(string data, string mac);
    }
}