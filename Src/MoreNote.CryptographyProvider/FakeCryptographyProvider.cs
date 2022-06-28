using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.CryptographyProvider
{
    public class FakeCryptographyProvider : ICryptographyProvider
    {
       /// <summary>
       /// 虚假的hamc
       /// </summary>
       /// <param name="data"></param>
       /// <returns></returns>
       /// <exception cref="NotImplementedException"></exception>
        public async Task<string> Hmac(string data)
        {
            var bytes=Convert.FromBase64String(data);
            var hash= SHA256.Create().ComputeHash(bytes);
            string base64=Convert.ToBase64String(hash);
            
            return base64;
        }

        public Task<string> SM4Decrypt(string base64Data)
        {
            throw new NotImplementedException();
        }

        public Task<string> SM4Encrypt(string DataStr)
        {
            throw new NotImplementedException();
        }

        public Task<string> TransEncrypted(string cipherBase64)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyHmac(string data, string mac)
        {
            throw new NotImplementedException();
        }
    }
}
