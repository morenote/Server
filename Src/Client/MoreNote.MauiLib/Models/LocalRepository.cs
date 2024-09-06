using LiteDB;

using Microsoft.EntityFrameworkCore;

using MoreNote.MauiLib.Utils;
using MoreNote.MSync.Services.FileSystem;
using MoreNote.MSync.Services.FileSystem.IMPL;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MauiLib.Models
{
    // 创建你的 POCO 类
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }

    public class LocalRepository
    {
        

        public VirtualFileSystem fileSystemServices = new LocalFileSystem();
        public string RepositoryConfigFile { get; set; } = "config";

       

        public static LocalRepository Open()
        {
           return new LocalRepository();
        }

        /// <summary>
        /// 初始化仓库
        /// </summary>
        public  void Init()
        {

          
            //如果不存在data文件夹，创建data文件夹
            fileSystemServices.Directory_CreateDirectory(MyPathUtil.DataDir);

            //如果不存在history文件夹，创建history文件夹
            fileSystemServices.Directory_CreateDirectory(MyPathUtil.HistoryDir);

            //如果不存在config文件夹，创建config文件夹
            fileSystemServices.Directory_CreateDirectory(MyPathUtil.ConfigFile);
           
        }
    }
}