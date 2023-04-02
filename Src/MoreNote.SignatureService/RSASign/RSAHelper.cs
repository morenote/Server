using Masuit.Tools.Security;

using MoreNote.Common.Utils;

using Org.BouncyCastle.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.SignatureService.RSASign
{
    public class RSAHelper
    {
        public string Sign(string fnstr, string txtPrivateKey)
        {
            try
            {
                

                //SHA256withRSA
              
                //1。转换私钥字符串为RSACryptoServiceProvider对象
                RSACryptoServiceProvider rsaP = RsaUtil.LoadPrivateKey(txtPrivateKey, "PKCS8");
                byte[] data = Encoding.UTF8.GetBytes(fnstr);//待签名字符串转成byte数组，UTF8
                byte[] byteSign = rsaP.SignData(data, "SHA1");//对应JAVA的RSAwithSHA256
                string sign = Convert.ToBase64String(byteSign);//签名byte数组转为BASE64字符串

                return sign;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtSign">签名数据</param>
        /// <param name="fnstr">原始数据</param>
        /// <param name="txtPubKey">公钥</param>
        /// <returns></returns>
        public bool Verify(string txtSign,string fnstr,string txtPubKey)
        {
            byte[] signature = Convert.FromBase64String(txtSign);//签名值转为byte数组
                                                                 //SHA256withRSA
                                                                 //1。转换私钥字符串为RSACryptoServiceProvider对象
            RSACryptoServiceProvider rsaP = RsaUtil.LoadPublicKey(txtPubKey);
            byte[] data = Encoding.UTF8.GetBytes(fnstr);//待签名字符串转成byte数组，UTF8
            bool validSign = rsaP.VerifyData(data, "SHA1", signature);//对应JAVA的RSAwithSHA1

            return validSign;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="data">原始数据</param>
        /// <param name="parameters">公钥参数</param>
        /// <returns></returns>
        public bool Verify(byte[] signature, byte[]  data, RSAParameters  parameters)
        {
          

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(parameters);
                bool validSign = RSA.VerifyData(data, "SHA1", signature);//对应JAVA的RSAwithSHA1
                return validSign;
            }

        }
        public bool Verify(byte[] signature, byte[] data, byte[] modulusIn16Radix, byte[] exponentIn16Radix)
        {
            RSAParameters parameters = new RSAParameters()
            {
                // If the following is working on your system:
                Modulus = modulusIn16Radix,

                Exponent = exponentIn16Radix
            };

            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(parameters);
                bool validSign = RSA.VerifyData(data, "SHA1", signature);//对应JAVA的RSAwithSHA1
                return validSign;
            }

        }

    }
}
