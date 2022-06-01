using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.SignatureService.RSASign
{
    public class RSASignService : ISignatureService
    {
        public Task<string> rawSignature(string data)
        {
            throw new NotImplementedException();
        }

        public Task<bool> rawVerify(String data, String sign, String cer, bool usbKey, String pubKeyModulusInHex, String pubKeyExpInHex)
        {
            throw new NotImplementedException();
        }
    }
}
