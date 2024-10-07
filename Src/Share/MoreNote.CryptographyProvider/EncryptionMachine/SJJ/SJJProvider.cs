using github.hyfree.GM;

using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFile;
using MoreNote.Logic.Service;
using MoreNote.SecurityProvider.Core;
using MoreNote.SignatureService.NetSign;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.CryptographyProvider.EncryptionMachine.SJJ
{
    public class SJJProvider : ICryptographyProvider
    {
        WebSiteConfig webSiteConfig;
        byte[] masterKey;
        byte[] hmacKey;
        byte[] enckey;
        byte[] sm2Pubky;
        byte[] sm2PriKey;
        ISJJApi sjj;

        bool sm2PriKeyBaohu=false;
        public SJJProvider(ConfigFileService configFileService,ISJJApi sjj)
        {
            GMService gm = new GMService();
            this.webSiteConfig = configFileService.ReadConfig();
            //todo:实际上，我们必须理解由于Secret无法被保护，所以如果宿主机本身被攻击，一切加密手段都是毫无意义的，只能充当安慰剂。
            //这样做的唯一价值就是通过派生子密钥，减少Secret暴露的机会。
            masterKey = gm.PBKDF2_SM3(Convert.FromBase64String(webSiteConfig.SecurityConfig.Secret), Encoding.UTF8.GetBytes("masterKey"), 10000, 16);
            hmacKey = gm.PBKDF2_SM3(masterKey, Encoding.UTF8.GetBytes("hmacKey"), 10000, 16);
            enckey = gm.PBKDF2_SM3(masterKey, Encoding.UTF8.GetBytes("encKey"), 10000, 16);

            sm2Pubky = HexUtil.HexToByteArray(webSiteConfig.SecurityConfig.PublicKey);
            sm2PriKey = HexUtil.HexToByteArray(webSiteConfig.SecurityConfig.PrivateKey);

            this.sjj = sjj;
        }

        

        public async Task<byte[]> SM2Decrypt(byte[] data)
        {
            if (sm2PriKeyBaohu == false)
            {
                var miwen = await SM4Encrypt(this.sm2PriKey);
                var miwenHex = HexUtil.ByteArrayToHex(miwen);

                var mignwen = SM4Decrypt(miwen);

                Console.WriteLine(miwen);

            }
            GMService gm = new GMService();
            return gm.SM2Decrypt(data, sm2PriKey);
        }

        public async Task<byte[]> SM2Encrypt(byte[] data)
        {
            GMService gm = new GMService();
            return gm.SM2Encrypt(data,sm2PriKey);
        }

        public async Task<byte[]> SM3(byte[] data)
        {
            GMService gm = new GMService();
            return gm.SM3(data);

        }

        public async Task<byte[]> SM4Decrypt(byte[] data, byte[] iv)
        {
            var dataHex = HexUtil.ByteArrayToHex(data);
            var result = await sjj.decrypt(dataHex);
            var dec = HexUtil.HexToByteArray(result.Data);
            return dec;
        }

        public async Task<byte[]> SM4Decrypt(byte[] data)
        {
            var dataHex = HexUtil.ByteArrayToHex(data);
            var result = await sjj.decrypt(dataHex);
            var dec = HexUtil.HexToByteArray(result.Data);
            return dec;
        }

        public async Task<byte[]> SM4Encrypt(byte[] data, byte[] iv)
        {
           var dataHex=HexUtil.ByteArrayToHex(data);
            var result=await  sjj.encrypt(dataHex);
            var enc=HexUtil.HexToByteArray(result.Data);
            return enc;
        }

        public async Task<byte[]> SM4Encrypt(byte[] data)
        {
            var dataHex = HexUtil.ByteArrayToHex(data);
            var result = await sjj.encrypt(dataHex);
            var enc = HexUtil.HexToByteArray(result.Data);
            return enc;
        }

        public async Task<byte[]> TransEncrypted(byte[] cipher, byte[] salt)
        {

            GMService gm = new GMService();
            if (sm2PriKeyBaohu==false)
            {
               this.sm2PriKey= await SM4Decrypt(this.sm2PriKey);
               sm2PriKeyBaohu = true;
            }

            byte[] data = new byte[cipher.Length + 1];
            Array.Copy(cipher, 0, data, 1, cipher.Length);
            data[0] = 0x04;
            var Hexkey = HexUtil.ByteArrayToHex(sm2PriKey);
            var Hexsalt = HexUtil.ByteArrayToHex(salt);
            var HexPass = HexUtil.ByteArrayToHex(cipher);
            var plainText = gm.SM2Decrypt(data, sm2PriKey);
            var plainTextHex=HexUtil.ByteArrayToHex(plainText);

            byte[] iv = new byte[16];//固定值
            var enc =  await SM4Encrypt(plainText);
            return enc;

        }
        public async Task<byte[]> Hmac(byte[] data)
        {
           var hex=HexUtil.ByteArrayToHex(data);
           var result=await sjj.hmac(hex);
            var macHex= result.Data;
            return HexUtil.HexToByteArray(macHex);
        }
        public async Task<bool> VerifyHmac(byte[] data, byte[] mac)
        {
            var dataHex = HexUtil.ByteArrayToHex(data);
            var macHex = HexUtil.ByteArrayToHex(mac);
            var result = await sjj.verifyHmac(dataHex,macHex);
           
            return result.Ok;
        }
    }
}
