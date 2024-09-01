using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MSync.Services.FileSystem.IMPL
{
    public class LocalFileSystem : VirtualFileSystem
    {
        public bool File_Exists(string path)
        {
          return  File.Exists(path);
        }
        public void Directory_CreateDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
           
        }

    }

}
