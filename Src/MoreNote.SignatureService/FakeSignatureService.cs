using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.SignatureService
{
    /// <summary>
    /// 虚假签名服务（仅供测试用途）
    /// </summary>
    public class FakeSignatureService : ISignatureService
    {
        public Task<string> rawSignature(string data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> rawVerify(string data, string sign, string cer, bool usbKey, string pubKeyModulusInHex, string pubKeyExpInHex)
        {
            throw new NotImplementedException();
        }
    }
}
