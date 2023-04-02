using Masuit.Tools.Models;
using Microsoft.EntityFrameworkCore;

using Morenote.Models.Models.Entity;
using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;

using Org.BouncyCastle.Crypto;

using StackExchange.Redis;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote.User
{
    [Table("usbkey_binding"), Index(nameof(UserId), nameof(Modulus))]
    public class USBKeyBinding: BaseEntity
    {

        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("modulus")]
        public string Modulus { get;set; }
        [Column("exponent")]
        public string Exponent { get; set; }
        [Column("hash")]
        public string Hash { get; set; }="SHA1";
        [Column("hmac")]
        public string Hmac { get; set; }

        public byte[] GetBytes()
        {
           StringBuilder stringBuilder= new StringBuilder();
           stringBuilder.Append("Id="+Id);
           stringBuilder.Append("Modulus=" + Modulus);
           stringBuilder.Append("Exponent" + Exponent);
           stringBuilder.Append("hash"+ Hash);
           return Encoding.UTF8.GetBytes(stringBuilder.ToString());
        }
        public USBKeyBinding AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = GetBytes();
            Hmac = cryptographyProvider.Hmac(bytes).ByteArrayToBase64();
            return this;
        }

        public bool VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
        
            var bytes = GetBytes();
            var result = cryptographyProvider.VerifyHmac(bytes, Hmac.Base64ToByteArray());
            return result;
        }

    }
}
