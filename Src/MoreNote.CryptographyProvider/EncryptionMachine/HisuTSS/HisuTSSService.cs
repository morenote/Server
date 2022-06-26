using MoreNote.Common.Utils;
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
        private IHisuKMSApi kmsAPi { get; set; }
        public HisuTSSService(IHisuTSSApi hisuTSS,IHisuKMSApi hisuKMSApi)
        {
            this.api = hisuTSS;
            this.kmsAPi = hisuKMSApi;
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
         
        public async Task<string> SM2Encrypt(string data)
        {
            var utf8=Encoding.UTF8.GetBytes(data);
            var base64 = Convert.ToBase64String(utf8); 
            var result = await kmsAPi.HisuUniveralEncryptByPk(base64);
            if (!result.Ok)
            {
                throw new Exception("SM2Encrypt is Error");
            }
            return result.Data;
        }

        public async Task<string> SM2Decrypt(string data)
        {
            var result = await kmsAPi.HisuUniveralDecryptByVk(data);
            if (!result.Ok)
            {
                throw new Exception("SM2Encrypt is Error");
            }
            var base64= result.Data;
            var utf8=Convert.FromBase64String(base64);
            var str = Encoding.UTF8.GetString(utf8);
            return str;

        }


    }
}
