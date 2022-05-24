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
    }
}
