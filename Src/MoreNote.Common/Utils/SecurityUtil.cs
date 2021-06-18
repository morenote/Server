using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Common.Utils
{
    public  class SecurityUtil
    {
        public static bool ComparePassword(byte[] a1,byte[] a2)
        {
            if (a1==null||a2==null||a2.Length!=a2.Length)
            {
                return false;
            }
            //防止计时攻击
            int result = 0;

            for (int i = 0; i < a1.Length; i++)
            {
                result |= a1[i] ^ a2[i];
            }
            return result == 0;
        }

    }
}
