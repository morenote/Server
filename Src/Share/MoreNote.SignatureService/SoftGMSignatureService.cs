using github.hyfree.GM;

using MoreNote.SecurityProvider.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.SignatureService
{
    public class SoftGMSignatureService : ISignatureService
    {
        GMService gm = new GMService();
        public bool GMT0009_VerifySign(byte[] m, byte[] signData, byte[] pubkey, byte[] userId)
        {
         return  gm.GMT0009_VerifySign(m,signData, pubkey, userId);
        }

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
