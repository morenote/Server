using System;
using System.Collections.Generic;
using System.Text;

namespace UpYunLibrary.OSS
{
    public class UPYunOSSOptions
    {
        public string bucket { get; set; }
        public string save_key { get; set; }
        public int expiration { get; set; }

    }
}
