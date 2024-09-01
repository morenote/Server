using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MSync.Services.FileSystem
{
    /// <summary>
    /// 虚拟文件系统，用于屏蔽各种文件系统的差异。
    /// 
    /// </summary>
    public interface VirtualFileSystem
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool File_Exists(string path);
        public void Directory_CreateDirectory(string dir);
    }
}
