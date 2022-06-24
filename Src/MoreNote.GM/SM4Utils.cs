using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.GM
{
   public  class SM4Utils
    {
        public String secretKey = "";
        public String iv = "";
        public bool hexString = true;//默认使用Hex

        public String Encrypt_ECB(String plainText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = HexUtil.HexToByteArray(secretKey);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_ecb(ctx, Encoding.Default.GetBytes(plainText));

            String cipherText = Encoding.Default.GetString(encrypted);
            return cipherText;
        }

        public String Decrypt_ECB(String cipherText)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            if (hexString)
            {
                keyBytes = HexUtil.HexToByteArray(secretKey);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_ecb(ctx, HexUtil.HexToByteArray(cipherText));
            return Encoding.Default.GetString(decrypted);
        }
        public String Encrypt_CBC(String HexString,bool outHex)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_ENCRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = HexUtil.HexToByteArray(secretKey);
                ivBytes = HexUtil.HexToByteArray(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_enc(ctx, keyBytes);
            byte[] encrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, HexUtil.HexToByteArray(HexString));
            if (outHex)
            {
                return HexUtil.ByteArrayToHex(encrypted);
            }
            else
            {
                String cipherText = Encoding.Default.GetString((encrypted));
                return cipherText;
            }
            
        }

        public String Decrypt_CBC(String cipherText,bool outHex=false)
        {
            SM4_Context ctx = new SM4_Context();
            ctx.isPadding = true;
            ctx.mode = SM4.SM4_DECRYPT;

            byte[] keyBytes;
            byte[] ivBytes;
            if (hexString)
            {
                keyBytes = HexUtil.HexToByteArray(secretKey);
                ivBytes = HexUtil.HexToByteArray(iv);
            }
            else
            {
                keyBytes = Encoding.Default.GetBytes(secretKey);
                ivBytes = Encoding.Default.GetBytes(iv);
            }

            SM4 sm4 = new SM4();
            sm4.sm4_setkey_dec(ctx, keyBytes);
            byte[] decrypted = sm4.sm4_crypt_cbc(ctx, ivBytes, HexUtil.HexToByteArray(cipherText));
            if (outHex)
            {
                return HexUtil.ByteArrayToHex(decrypted);

            }
            else
            {
                return Encoding.Default.GetString(decrypted);

            }
           
        }

        //[STAThread]
        //public static void Main()
        //{
        //    String plainText = "ererfeiisgod";  

        //    SM4Utils sm4 = new SM4Utils();  
        //    sm4.secretKey = "JeF8U9wHFOMfs2Y8";  
        //    sm4.hexString = false;  

        //    System.Console.Out.WriteLine("ECB模式");  
        //    String cipherText = sm4.Encrypt_ECB(plainText);  
        //    System.Console.Out.WriteLine("密文: " + cipherText);  
        //    System.Console.Out.WriteLine("");  

        //    plainText = sm4.Decrypt_ECB(cipherText);  
        //    System.Console.Out.WriteLine("明文: " + plainText);  
        //    System.Console.Out.WriteLine("");  

        //    System.Console.Out.WriteLine("CBC模式");  
        //    sm4.iv = "UISwD9fW6cFh9SNS";  
        //    cipherText = sm4.Encrypt_CBC(plainText);  
        //    System.Console.Out.WriteLine("密文: " + cipherText);  
        //    System.Console.Out.WriteLine("");  

        //    plainText = sm4.Decrypt_CBC(cipherText);  
        //    System.Console.Out.WriteLine("明文: " + plainText);

        //    Console.ReadLine();
        //}
    }
}
