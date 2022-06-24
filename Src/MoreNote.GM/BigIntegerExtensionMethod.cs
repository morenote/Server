using Org.BouncyCastle.Math;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.GM
{
    public static class BigIntegerExtensionMethod
    {
        public static byte[] ToByteArray32(this BigInteger bigInteger)
        {
            //0098025f5dcbbb67ac06951c86d31db9ab983ceb6d57cc615d54398d66a15a7600
            var byteData = bigInteger.ToByteArray();
            if (byteData==null)
            {
                throw new ArgumentNullException("bigInteger is null");
            }
            
            if (byteData.Length==33 && byteData[0]==0x00)
            {
                var result=byteData.Skip(1).ToArray();
                return result;
            }
            
            return byteData;
        }
    }
}
