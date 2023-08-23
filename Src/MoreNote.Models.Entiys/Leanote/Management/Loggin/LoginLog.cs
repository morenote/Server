using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Management.Loggin
{
    public class LoginLog
    {
        public long? ID { get; set; }

        public string IP { get; set; }

        public string RequestHeader { get; set; }

    }
}
