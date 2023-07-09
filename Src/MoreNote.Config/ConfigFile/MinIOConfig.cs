using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFile
{
    public  class MinIOConfig
    {
        /// <summary>
        /// MinIo的访问URL
        /// </summary>
        public string Endpoint { get;set;}
        public bool WithSSL { get; set; }
        public string CDNEndpoint { get;set;}
        public bool CDNWithSSL { get;set;}

        public string NoteFileBucketName { get;set;}
        public string RandomImagesBucketName { get;set;}


        public string MINIO_ACCESS_KEY { get;set;}
        public string MINIO_SECRET_KEY { get;set;}
        public int BrowserDownloadExpiresInt { get;set;}=3600;




    }
}
