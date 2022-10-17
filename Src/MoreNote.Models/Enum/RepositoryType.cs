using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enum
{
    /// <summary>
    /// 仓库拥有者类型
    /// </summary>
    public enum RepositoryType
    {
        //笔记仓库
        NoteRepository = 0x01,
        //文件仓库
        FileRepository = 0x02
    }
}
