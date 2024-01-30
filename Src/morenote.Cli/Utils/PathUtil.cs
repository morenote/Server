using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.Utils
{
    public class PathUtil
    {
        public static readonly char dsc=Path.DirectorySeparatorChar;
        public static string GetCurDir()
        {
            string dir = System.Environment.CurrentDirectory;
            return dir;
        }

        public static string GetExeDir()
        {
            string dir =  System.AppDomain.CurrentDomain.BaseDirectory;
            return dir;
        }
    }
}