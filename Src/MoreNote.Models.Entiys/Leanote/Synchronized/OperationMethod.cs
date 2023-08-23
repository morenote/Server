using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.Synchronized
{
    public enum OperationMethod
    {
        NOP=0x00,//无效操作
        Post=0x01,//创建
        Put=0x02,//更新
        PATCH=0x03,//更新某些字段
        DELETE=0x04//删除
    }
}
