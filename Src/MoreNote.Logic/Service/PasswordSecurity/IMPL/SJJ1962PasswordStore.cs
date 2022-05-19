using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity.IMPL
{
    public class SJJ1962PasswordStore : IPasswordStore
    {
        public SJJ1962PasswordStore()
        {

        }
        /// <summary>
        /// 加密口令
        /// </summary>
        /// <param name="pass">sm2的数字信封</param>
        /// <param name="salt">盐（无效参数）</param>
        /// <param name="iterations">轮数（无效参数）</param>
        /// <returns></returns>
        public byte[] Encryption(byte[] pass, byte[] salt, int iterations)
        {
           
        }
        /// <summary>
        ///  验证口令
        /// </summary>
        /// <param name="encryData">加密数据</param>
        /// <param name="pass">数字信封数据</param>
        /// <param name="salt">盐（无效参数）</param>
        /// <param name="iterations">轮数（无效参数）</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
        {
            throw new NotImplementedException();
        }
    }
}
