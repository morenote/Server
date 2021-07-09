using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.PasswordSecurity
{
    public class Sha256PasswordStore : IPasswordStore
    {
        public byte[] Encryption(byte[] pass, byte[] salt, int iterations)
        {
            var sha256 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] hash= sha256.ComputeHash(pass);

            for (int i = 0; i < iterations; i++)
            {
                if (i%2==0)
                {
                    hash = sha256.ComputeHash(Sum(hash, salt));
                }
                else
                {
                    hash = sha256.ComputeHash(Sum(salt, hash));
                }
            }
            return hash;
        }

        public bool VerifyPassword(byte[] encryData, byte[] pass, byte[] salt, int iterations)
        {
           var hash=Encryption(pass,salt,iterations);
           return SecurityUtil.ComparePassword(hash,encryData);
        }
        private byte[] Sum(byte[] s1,byte[] s2)
        {
            byte[] result=new byte[s1.Length+ s2.Length];
            Array.Copy(s1,0,result,0,s1.Length);
            Array.Copy(s2,0,result, s1.Length, s2.Length);
          
            return result;
        }
    }
}
