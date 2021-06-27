using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.FileService
{
    /// <summary>
    /// 文件储存抽象接口
    /// </summary>
    public interface IFileStorageService
    {
        public void Remove(string path);
           
    }
}
