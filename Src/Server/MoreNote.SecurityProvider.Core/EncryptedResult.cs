using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoreNote.SecurityProvider.Core
{
    public class EncryptedResult
    {
        public bool Ok { get; set; }
        public string Data { get; set; }
        public int ErrorCode {  get; set; }
        public string ErrorMessage {  get; set; }
    }
}
