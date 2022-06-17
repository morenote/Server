using MoreNote.Common.Utils;
using MoreNote.CryptographyProvider;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity.IMPL
{
    /// <summary>
    /// 使用加密芯片保护口令
    /// </summary>
    public class SJJ1962PasswordStore : IPasswordStore
    {
        private ICryptographyProvider cryptographyProvider;
        public SJJ1962PasswordStore(ICryptographyProvider  cryptographyProvider)
        {
            this.cryptographyProvider = cryptographyProvider;
        }
        /// <summary>
        /// 加密口令
        /// </summary>
        /// <param name="pass">sm2的数字信封</param>
        /// <param name="salt">盐（无效参数）</param>
        /// <param name="iterations">轮数（无效参数）</param>
        /// <returns></returns>
        public async Task<byte[]> Encryption(byte[] pass, byte[] salt, int iterations)
        {
            var base64Pass = Convert.ToBase64String(pass);
            var result=await cryptographyProvider.TransEncrypted(base64Pass);
            return Convert.FromBase64String(result);
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
        public async Task<bool> VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
        {
            var base64Pass = Convert.ToBase64String(pass);
            var result = await cryptographyProvider.TransEncrypted(base64Pass);
            var zjm = Convert.FromBase64String(result);
            //将数据库中存储的加密口令与 用户输入的口令的加密的加密结果比较
            var verify= SecurityUtil.SafeCompareByteArray(encryData, zjm);
            return verify;
        }
    }
}
