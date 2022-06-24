

using MoreNote.GM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MoreNote.GM
{
    public class GMService
    {
        public string SM2Encrypt(string data, string key)
        {
            var enc = SM2Utils.EncryptC1C3C2(HexUtil.HexToByteArray(key),HexUtil.HexToByteArray(data));
            return enc;
        }

        public string SM2Decrypt(string data, string key,bool outHex)
        {
          var dec= SM2Utils.DecryptC1C3C2(HexUtil.HexToByteArray(key), HexUtil.HexToByteArray(data));
            if (outHex)
            {
                var hex = HexUtil.ByteArrayToHex(dec);
                return hex;
            }
            else
            {
                return Encoding.UTF8.GetString(dec);
            }
        }

        public string SM3(string hex)
        {
            SM3Util sm3 = new SM3Util();
            var result = sm3.Hash(hex);
            return result;
        }
        public string SM4_Encrypt_CBC(string value, string key,string iv,bool outHex)
        {
            SM4Utils sm4 = new SM4Utils();
            sm4.secretKey = key;
            sm4.iv = iv;
            return sm4.Encrypt_CBC(value, outHex);
        }
        /// <summary>
        ///  SM4解密
        /// </summary>
        /// <param name="data">SM4加密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">初始向量</param>
        /// <param name="outHex">解密结果输出为utf8字符串还是Hex字符串</param>
        /// <returns></returns>
        public string SM4_Decrypt_CBC(string data, string key,string iv, bool outHex)
        {
            SM4Utils sm4 = new SM4Utils();
            sm4.secretKey = key;
            sm4.iv = iv;
            
            return sm4.Decrypt_CBC(data, outHex);
        }

    }
}
