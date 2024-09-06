using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MauiLib.Utils
{
    public class MyPathUtil
    {
        /// <summary>
        /// 基路径，可以认为是仓库文件夹的路径 带/
        /// </summary>
        public static string? BasePath { get; set; } = FileSystem.Current.AppDataDirectory;
        /// <summary>
        /// 数据存储
        /// </summary>
        public static string DataDir
        { get { string path = Path.Combine(BasePath, "Data"); return path; } }
        
        public static string HistoryDir
        { get { string path = Path.Combine(BasePath, "History"); return path; } }
        public static string ConfigFile
        { get { string path = Path.Combine(BasePath, "Config.json"); return path; } }
        public static string SQLiteFile
        { get { string path = Path.Combine(DataDir, "sqlite3.db"); return path; } }
    }
}
