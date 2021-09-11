using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity
{
    /// <summary>
    /// 密码处理器接口
    /// </summary>
     public interface IPasswordStore
    {
    

        /// <summary>
        /// 加密口令,必须单向的
        /// </summary>
        /// <param name="pass">口令</param>
        /// <param name="salt">盐值</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns></returns>
        public byte[] Encryption(byte[] pass, byte[] salt,int iterations);

        /// <summary>
        /// 验证提供的口令是否与提供的哈希匹配
        /// </summary>
        /// <param name="pass">密码</param>
        /// <param name="encryData">加密数据</param>
        /// <param name="salt">盐值</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns></returns>
        public bool VerifyPassword(byte[] encryData,byte[] pass,  byte[] salt,int iterations);

    }
}
