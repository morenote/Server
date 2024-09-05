using MoreNote.MauiLib.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MauiLib.Data
{
   public   class SQLiteManager
    {

        public SQLiteContext InstanceSQLiteContext()
        {
            return new SQLiteContext(MyPathUtil.SQLiteFile);
        }

        /// <summary>
        /// 将创建数据库（如果不存在）并初始化数据库架构。 
        /// 如果存在任何表（包括其他 DbContext 类的表），则不会初始化该架构。
        /// </summary>
        public  static async Task EnsureCreatedAsync()
        {
            using (var sqlite=new SQLiteContext(MyPathUtil.SQLiteFile))
            {
               await sqlite.Database.EnsureCreatedAsync();
            }
        }

        /// <summary>
        /// EnsureDeleted 方法将删除数据库（如果存在）。
        /// 如果没有适当的权限，则会引发异常。
        /// </summary>
        public static async Task EnsureDeletedAsync()
        {
            using (var sqlite = new SQLiteContext(MyPathUtil.SQLiteFile))
            {
                await sqlite.Database.EnsureDeletedAsync();
            }
        }
    }
}
