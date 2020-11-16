using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    /// <summary>
    /// 机器学习配置
    /// </summary>
    public class MachineLearningConfig
    {
        /// <summary>
        /// 垃圾评论的机器学习模型文件的存放地址
        /// </summary>
        public string SpamModelPath { get; set; }

    }
}
