using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.CryptographyProvider.EncryptionMachine.HisuTSS
{
    public class EncryptedResult
    {
        public bool Ok { get; set; }
        public string Data { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}
