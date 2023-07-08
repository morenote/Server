using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Enum
{
    /// <summary>
    /// 密钥类型
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// 秘密（例如token、密钥生成的参与材料、共享秘密值）
        /// </summary>
        Secrets, 
        /// <summary>
        /// 对称密钥
        /// </summary>
        SymmetricKey,
        /// <summary>
        /// SM2私钥
        /// </summary>
        SM2PrivateKey
    }
}
