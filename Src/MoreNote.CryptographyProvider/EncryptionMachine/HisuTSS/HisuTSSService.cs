using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.CryptographyProvider.EncryptionMachine.HisuTSS
{

    /// <summary>
    /// 加密服务平台的实现
    /// </summary>
    public class HisuTSSService: ICryptographyProvider
    {
        
        public HisuTSSService()
        {

        }
         private IHisuTSSApi api { get; set; }
        public HisuTSSService(IHisuTSSApi hisuTSS)
        {
            this.api = hisuTSS;
        }

        public Task<string> hmac(string data)
        {
            
            return api.hmac(data);
        }

        public Task<bool> verifyHmac(string data, string mac)
        {
            return api.verifyHmac(data, mac);
        }
        /// <summary>
        /// 转加密 
        /// </summary>
        /// <param name="cipherBase64">SM2密文Base64编码</param>
        /// <returns>base64结果</returns>
        public async Task<string> TransEncrypted(string cipherBase64)
        {
    
            var result= await api.transPinBlockFromPKToZPK("MBNK", "myself", "sm2",
                                               "MBNK", "myself", "RZPK", 0, cipherBase64, "1234567890123456");
            if (result.Ok)
            {
                return result.Data;
            }
            throw new Exception("转加密失败");
        }
    }
}
