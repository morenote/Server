using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.ConfigFile
{
    /// <summary>
    /// 分布式缓存设置
    /// </summary>
    public class RedisConfig
    {

        public bool IsEnable{get; set;}=false;

        public string Configuration {get; set;}="localhost";

        public string InstanceName {get; set;}="RedisDistributedCache";

    }
}
