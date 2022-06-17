using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MoreNote.CryptographyProvider.EncryptionMachine.HisuTSS
{
    /// <summary>
    /// 加密机webapi rpc调用
    /// </summary>
    public interface IHisuTSSApi: IHttpApi
    {
        [HttpPost("/hmac")]
        public Task<string> hmac(string data);

        [HttpPost("/verifyHmac")]
        public Task<bool> verifyHmac(string data, string mac);

        [HttpPost("/transPinBlockFromPKToZPK")]
        public Task<EncryptedResult> transPinBlockFromPKToZPK(string designID, string nodeID, string keyModelID,
            string designID01, string nodeID01, string keyModelID01,
            int pinENcMode, string cipherBase64,string accNo);
    }
}
