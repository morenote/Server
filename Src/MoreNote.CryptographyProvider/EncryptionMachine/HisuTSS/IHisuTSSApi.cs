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
    }
}
