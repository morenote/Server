using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.GM
{
    class SM4_Context
    {
        public int mode;

        public long[] sk;

        public bool isPadding;

        public SM4_Context()
        {
            this.mode = 1;
            this.isPadding = true;
            this.sk = new long[32];
        }
    }
}
