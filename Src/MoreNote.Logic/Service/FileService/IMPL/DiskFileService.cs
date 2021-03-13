using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService.IMPL
{
    public class DiskFileService : IFileService
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
