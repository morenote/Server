using MoreNote.Common.Cryptography;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enums
{
    /// <summary>
    /// 对称加密算法
    /// </summary>
    public enum SymmetricEncryptionAlgorithm
    {
        AES256,SM4
    }
}
