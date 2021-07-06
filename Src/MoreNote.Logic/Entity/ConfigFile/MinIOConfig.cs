using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    public  class MinIOConfig
    {
        /// <summary>
        /// MinIo的访问URL
        /// </summary>
        public string URL { get;set;}
        public string MINIO_ACCESS_KEY { get;set;}
        public string MINIO_SECRET_KEY { get;set;}
    }
}
