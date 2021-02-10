using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Entity.ConfigFile
{
    /// <summary>
    /// Postgresql 连接配置
    /// </summary>
    public class PostgreSqlConfig
    {
        public string Connection { get; set; }

        public static PostgreSqlConfig GenerateTemplate()
        {
            PostgreSqlConfig postgreSqlConfig=new PostgreSqlConfig()
            {
                Connection= "Host=127.0.0.1;Port=5432;Database=postgres; User ID=postgres;Password=postgres;"
            };
            return postgreSqlConfig;
        }
    }
}
