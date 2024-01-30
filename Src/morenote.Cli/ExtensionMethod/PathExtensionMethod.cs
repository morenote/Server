using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace morenote_sync_cli.ExtensionMethod
{
    public static class PathExtensionMethod
    {
        /// <summary>
        /// 跨平台路径转换
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string PathConvert(this string path)
        {
            if (RuntimeEnvironment.IsWindows)
            {
                return path.Replace("/", "\\");
            }
            else
            {
                return path;
            }
        }
    }
}