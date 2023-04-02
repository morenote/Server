using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Org.BouncyCastle.Math;
using MoreNote.Common.Utils;

namespace MoreNote.SignatureService.RSASign
{
    public static class RsaUtil
    {

        /** 算法 */
        private static String ALGORITHM = "RSA";
        /** RSA bits */
        private static int KEYSIZE = 1024;

        /** 由模和指数构造公钥对象，模和指数由16进制字符串表示 */
        public static RSAParameters GetRSAParametersByBase64(String modulusIn16Radix, String exponentIn16Radix)
        {
            RSAParameters result = new RSAParameters()
            {
                // If the following is working on your system:
                Modulus = Convert.FromBase64String(modulusIn16Radix),
     
                Exponent = HexUtil.HexToByteArray(exponentIn16Radix)
            };
            return result;

        }
       

        #region 加载私钥


        /// <summary>
        /// 转换私钥字符串为RSACryptoServiceProvider
        /// </summary>
        /// <param name="privateKeyStr">私钥字符串</param>
        /// <param name="keyFormat">PKCS8,PKCS1</param>
        /// <param name="signType">RSA 私钥长度1024 ,RSA2 私钥长度2048</param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKey(string privateKeyStr, string keyFormat)
        {
            string signType = "RSA";
            if (privateKeyStr.Length > 1024)
            {
                signType = "RSA2";
            }
            //PKCS8,PKCS1
            if (keyFormat == "PKCS1")
            {
                return LoadPrivateKeyPKCS1(privateKeyStr, signType);
            }
            else
            {
                return LoadPrivateKeyPKCS8(privateKeyStr);
            }
        }

        /// <summary>
        /// PKCS1 格式私钥转 RSACryptoServiceProvider 对象
        /// </summary>
        /// <param name="strKey">pcsk1 私钥的文本内容</param>
        /// <param name="signType">RSA 私钥长度1024 ,RSA2 私钥长度2048 </param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKeyPKCS1(string privateKeyPemPkcs1, string signType)
        {
            try
            {
                privateKeyPemPkcs1 = privateKeyPemPkcs1.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();
                privateKeyPemPkcs1 = privateKeyPemPkcs1.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

                byte[] data = null;
                //读取带

                data = Convert.FromBase64String(privateKeyPemPkcs1);


                RSACryptoServiceProvider rsa = DecodeRSAPrivateKey(data, signType);
                return rsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey, string signType)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);


                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                CspParameters CspParameters = new CspParameters();
                CspParameters.Flags = CspProviderFlags.UseMachineKeyStore;

                int bitLen = 1024;
                if ("RSA2".Equals(signType))
                {
                    bitLen = 2048;
                }

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(bitLen, CspParameters);
                //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception ex)
            {
                throw ex;
                // return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }

        /// <summary>
        /// PKCS8 文本转RSACryptoServiceProvider 对象
        /// </summary>
        /// <param name="privateKeyPemPkcs8"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPrivateKeyPKCS8(string privateKeyPemPkcs8)
        {

            try
            {
                //PKCS8是“BEGIN PRIVATE KEY”
                privateKeyPemPkcs8 = privateKeyPemPkcs8.Replace("-----BEGIN RSA PRIVATE KEY-----", "").Replace("-----END RSA PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();
                privateKeyPemPkcs8 = privateKeyPemPkcs8.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

                //pkcs8 文本先转为 .NET XML 私钥字符串
                string privateKeyXml = RSAPrivateKeyJava2DotNet(privateKeyPemPkcs8);

                RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
                publicRsa.FromXmlString(privateKeyXml);
                return publicRsa;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PKCS8 私钥文本 转 .NET XML 私钥文本
        /// </summary>
        /// <param name="privateKeyPemPkcs8"></param>
        /// <returns></returns>
        public static string RSAPrivateKeyJava2DotNet(string privateKeyPemPkcs8)
        {
            RsaPrivateCrtKeyParameters privateKeyParam = (RsaPrivateCrtKeyParameters)PrivateKeyFactory.CreateKey(Convert.FromBase64String(privateKeyPemPkcs8));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
            Convert.ToBase64String(privateKeyParam.Modulus.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.PublicExponent.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.P.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Q.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DP.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.DQ.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.QInv.ToByteArrayUnsigned()),
            Convert.ToBase64String(privateKeyParam.Exponent.ToByteArrayUnsigned()));
        }

        #endregion


        /// <summary>
        /// 加载公钥证书
        /// </summary>
        /// <param name="publicKeyCert">公钥证书文本内容</param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPublicCert(string publicKeyCert)
        {

            publicKeyCert = publicKeyCert.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "").Replace("\r", "").Replace("\n", "").Trim();

            byte[] bytesCerContent = Convert.FromBase64String(publicKeyCert);
            X509Certificate2 x509 = new X509Certificate2(bytesCerContent);
            RSACryptoServiceProvider rsaPub = (RSACryptoServiceProvider)x509.PublicKey.Key;
            return rsaPub;

        }


        /// <summary>
        /// pem 公钥文本 转  .NET RSACryptoServiceProvider。
        /// </summary>
        /// <param name="publicKeyPem"></param>
        /// <returns></returns>
        public static RSACryptoServiceProvider LoadPublicKey(string publicKeyPem)
        {

            publicKeyPem = publicKeyPem.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace("\r", "").Replace("\n", "").Trim();

            //pem 公钥文本 转  .NET XML 公钥文本。
            string publicKeyXml = RSAPublicKeyJava2DotNet(publicKeyPem);

            RSACryptoServiceProvider publicRsa = new RSACryptoServiceProvider();
            publicRsa.FromXmlString(publicKeyXml);
            return publicRsa;


        }

        /// <summary>
        /// pem 公钥文本 转  .NET XML 公钥文本。
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        private static string RSAPublicKeyJava2DotNet(string publicKey)
        {
            RsaKeyParameters publicKeyParam = (RsaKeyParameters)PublicKeyFactory.CreateKey(Convert.FromBase64String(publicKey));
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                Convert.ToBase64String(publicKeyParam.Modulus.ToByteArrayUnsigned()),
                Convert.ToBase64String(publicKeyParam.Exponent.ToByteArrayUnsigned()));
        }

    }
}
