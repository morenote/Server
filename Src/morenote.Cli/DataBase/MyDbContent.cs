using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.DataBase
{
    public class MyDbContent : DbContext
    {
        private string connectionString;

        public MyDbContent(string sqliteFilePath)
        {
            this.connectionString =$"Data Source={sqliteFilePath}";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}