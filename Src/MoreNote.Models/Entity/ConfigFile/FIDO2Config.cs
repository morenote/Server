using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.ConfigFile
{
    public class FIDO2Config
    {
        public string ServerDomain{get;set;}
        public string ServerName{get;set;}
        public string Origin{get;set;}
        public int TimestampDriftTolerance{get;set;}=300000;
        public string MDSCacheDirPath{get;set;}=string.Empty;
    }
}
