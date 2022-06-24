using Org.BouncyCastle.Crypto.Digests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.GM
{
    public class SM3Util
    {
        public  string Hash(string dataHex)
        {
            byte[] md = new byte[32];
            byte[] msg1 = HexUtil.HexToByteArray(dataHex);
            //计算SM3
            SM3Digest sm3 = new SM3Digest();
            sm3.BlockUpdate(msg1, 0, msg1.Length);
            sm3.DoFinal(md, 0);

            string hex = HexUtil.ByteArrayToHex(md);
            return hex;

        }
    }
}
