using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.ConfigFile.Security
{
    /// <summary>
    /// 人脸识别配置
    /// </summary>
    public class FaceConfig
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = false;
        /// <summary>
        /// 人脸模型地址
        /// </summary>
        public string ModelFilesDirectory { get; set; }
        /// <summary>
        /// 容差阈值。如果比较面部的差距小于容差阈值则相同,否则不一样
        /// </summary>
        public double Tolerance { get; set; } = 0.2d;


    }
}
