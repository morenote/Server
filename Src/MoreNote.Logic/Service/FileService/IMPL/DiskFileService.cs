using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL
{
    /// <summary>
    /// 本地文件
    /// </summary>
    public class DiskFileService : IFileStorageService
    {
        /// <summary>
        /// 被删除文件的相对路径
        /// </summary>
        /// <param name="path"></param>
        public void Remove(string path)
        {
            File.Delete(path);
        }
    }
}
