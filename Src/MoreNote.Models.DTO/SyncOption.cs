using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO
{
    public enum SyncOption
    {
        /// <summary>
        /// 返回相等的
        /// </summary>
        equal=0x01,
        /// <summary>
        /// 小于或相等
        /// </summary>
        less=0x02,
        /// <summary>
        /// 返回最新的
        /// </summary>
        last=0x03
     
    }
}
