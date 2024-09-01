using LiteDB;

using Microsoft.EntityFrameworkCore;


using MoreNote.MSync.Services.FileSystem;
using MoreNote.MSync.Services.FileSystem.IMPL;
using MoreNote.SampleLibrary.Data;

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
        /// <summary>
        /// 基路径，可以认为是仓库文件夹的路径 带/
        /// </summary>
        public string? BasePath { get; set; }

        public VirtualFileSystem fileSystemServices = new LocalFileSystem();
        public string RepositoryConfigFile { get; set; } = "config";

        public string GetConfigFilePath()
        {
            return BasePath + RepositoryConfigFile;
        }

        public string DataDir
        { get { string path = Path.Combine(BasePath, "Data"); return path; } }
        public string HistoryDir
        { get { string path = Path.Combine(BasePath, "History"); return path; } }
        public string ConfigDir
        { get { string path = Path.Combine(BasePath, "Config"); return path; } }
        public string DataBase
        { get { string path = Path.Combine(DataDir, "sqlite3.db"); return path; } }

        public static LocalRepository Open(string basePath)
        {
            LocalRepository localRepository = new LocalRepository();
            localRepository.BasePath = basePath;
            return localRepository;
        }

        /// <summary>
        /// 初始化仓库
        /// </summary>
        public async void Init()
        {

            //首先根据配置文件判断是是否是空的
            if (fileSystemServices.File_Exists(this.GetConfigFilePath()))
            {
                return;
            }
            //如果不存在data文件夹，创建data文件夹
            fileSystemServices.Directory_CreateDirectory(DataDir);

            //如果不存在history文件夹，创建history文件夹
            fileSystemServices.Directory_CreateDirectory(HistoryDir);

            //如果不存在config文件夹，创建config文件夹
            fileSystemServices.Directory_CreateDirectory(ConfigDir);
            using (var db = new SQLiteContext(DataBase, ""))
            {
               var result= await db.Database.EnsureCreatedAsync();
               
            }
        }
    }
}