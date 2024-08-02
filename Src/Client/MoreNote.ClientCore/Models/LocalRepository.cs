using MoreNote.MSync.Services.FileSystem;
using MoreNote.MSync.Services.FileSystem.IMPL;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MSync.Models
{
    public class LocalRepository
    {
        /// <summary>
        /// 基路径，可以认为是仓库文件夹的路径 带/
        /// </summary>
        public string? BasePath { get; set; }
        public VirtualFileSystem fileSystemServices=new LocalFileSystem();
        public string RepositoryConfigFile { get; set; }="config";

        public string GetConfigFilePath()
        {
            return BasePath + RepositoryConfigFile;
        }
        public static LocalRepository Open(string path)
        {
            return null;
        }
        /// <summary>
        /// 初始化仓库
        /// </summary>
        public void Init()
        {
            //首先根据配置文件判断是是否是空的
            if (fileSystemServices.File_Exists(this.GetConfigFilePath()))
            {
                return;
            }
            //如果不存在data文件夹，创建data文件夹
            fileSystemServices.Directory_CreateDirectory(BasePath + "Data");

            //如果不存在history文件夹，创建history文件夹
            fileSystemServices.Directory_CreateDirectory(BasePath + "History");

            //如果不存在config文件夹，创建config文件夹
            fileSystemServices.Directory_CreateDirectory(BasePath + "Config");

            //如果不存在images文件夹，创建images文件夹
            fileSystemServices.Directory_CreateDirectory(BasePath + "Images");

            //如果不存在HugeFiles文件夹，创建HugeFiles文件夹
            fileSystemServices.Directory_CreateDirectory(BasePath + "HugeFiles");





        }

    }
}