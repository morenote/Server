using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiClient;
using WebApiClient.Attributes;

namespace MoreNote.CryptographyProvider.EncryptionMachine.HisuTSS
{
    public interface IHisuKMSApi:IHttpApi
    {
        /// <summary>
        /// SM2加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/hisuUniveralEncryptByPk")]
        public Task<EncryptedResult> HisuUniveralEncryptByPk(string data);
        /// <summary>
        /// SM2解密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("/hisuUniveralDecryptByVk")]
        public Task<EncryptedResult> HisuUniveralDecryptByVk(string data);

    }
}
