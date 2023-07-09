using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enums
{
    /// <summary>
    /// 身份证明方案
    /// </summary>
    public enum IdentificationSolutions
    {
        /// <summary>
        /// 未配置
        /// </summary>
        None=0x01, 
        /// <summary>
        /// 预共享秘密
        /// </summary>
        PreSharedSecrets=0x02

    }
}
