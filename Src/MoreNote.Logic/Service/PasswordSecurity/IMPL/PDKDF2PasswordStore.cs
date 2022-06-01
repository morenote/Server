using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MoreNote.Common.Utils;

namespace MoreNote.Logic.Service.PasswordSecurity
{
    /// <summary>
    /// 实现PDKDF2(使用SHA512哈希算法)
    /// </summary>
    public class PDKDF2PasswordStore : IPasswordStore
    {
        public const int HASH_SIZE = 64; // 字节大小

        public async Task<byte[]> Encryption(byte[] pass, byte[] salt, int iterations)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, salt, iterations, HashAlgorithmName.SHA512);
            return pbkdf2.GetBytes(HASH_SIZE);
        }

        public async Task<bool> VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pass, salt, iterations, HashAlgorithmName.SHA512);
            byte[] hash = pbkdf2.GetBytes(HASH_SIZE);
            return SecurityUtil.SafeCompareByteArray(hash, encryData);
        }
    }
}