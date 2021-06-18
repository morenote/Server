using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace MoreNote.Logic.Service.PasswordSecurity
{
    /// <summary>
    /// 实现BCrypt算法验证
    /// </summary>
    public class BCryptPasswordStore : IPasswordStore
    {
        public byte[] Encryption(byte[] pass, byte[] salt, int iterations)
        {
           return  BCryptHlper.HashPassword(pass,salt,iterations);
        }

        public bool VerifyPassword(byte[] encryData,byte[] pass , byte[] salt, int iterations)
        {
            return BCryptHlper.Verify(encryData,pass,salt,iterations);
        }
    }
}
