using MoreNote.Logic.Model;
using MoreNote.Models.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFile
{
   /// <summary>
   /// 全局设置
   /// </summary>
   public class GlobalConfig
    {
        /// <summary>
        /// 仅供演示用途警告
        /// </summary>
        public bool DemonstrationOnly{get;set;}
        /// <summary>
        /// 网站如何保存图片和附件
        /// </summary>
        public StorageTypeEnum StorageTypeEnum{get;set;}=StorageTypeEnum.LocalDisk;

    


    }
}
