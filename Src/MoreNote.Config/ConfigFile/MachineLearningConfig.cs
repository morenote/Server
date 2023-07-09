using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFile
{
    /// <summary>
    /// 机器学习配置
    /// </summary>
    public class MachineLearningConfig
    {
        /// <summary>
        /// 是否开启机器学习反垃圾评论
        /// 请注意 这将额外消耗2GB的内存 4GB以下的服务器请不要开启
        /// </summary>
        public bool CanMachineLearning { get; set; }=false;

        /// <summary>
        /// 垃圾评论的机器学习模型文件的存放地址
        /// </summary>
        public string SpamModelPath { get; set; }
     
        public static MachineLearningConfig GenerateTemplate()
        {
            MachineLearningConfig machineLearningConfig=new MachineLearningConfig()
            {
                CanMachineLearning=false,
                SpamModelPath= "垃圾评论的机器学习模型文件的存放地址"

            };
            return machineLearningConfig;
        }
    }
}
