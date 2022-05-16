using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Service;
using MoreNote.Logic.Service.DistributedIDGenerator;
using Snowflake.Core;

namespace MoreNote.Common.Utils
{
    public class SnowFlakeNetService:IDistributedIdGenerator
    {
        private  IdWorker worker = null;
        
        // 定义私有构造函数，使外界不能创建该类实例
        public SnowFlakeNetService(ConfigFileService configFileService)
        {
            var config = configFileService.WebConfig.IdGeneratorConfig;
            if (worker == null)
            {
                worker = new IdWorker(config.WorkerId, config.DatacenterId);
            }
        }
        public  IdWorker GetInstance()
        {
            worker = new IdWorker(1, 1);
            return worker;
        }
        /// <summary>
        /// 产生全局唯一的long类型ID
        /// </summary>
        /// <returns></returns>
        public long NextId()
        {
            return   GetInstance().NextId();
        }
        /// <summary>
        /// 生成全局唯一的hex字符串
        /// </summary>
        /// <returns></returns>
        public  string NextHexId()
        {
            return NextId().ToHex();
        }
    }
}
