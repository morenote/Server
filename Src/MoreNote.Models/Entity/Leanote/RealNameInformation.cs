using Morenote.Models.Models.Entity;

using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.Entity.Leanote
{
    [Table("real_name_information")]
    public class RealNameInformation: BaseEntity
    {
        
        [Column("user_id")]
        public long? UserId { get;set;}
        [Column("id_card_no")]
   
        public string? IdCardNo { get;set;}
        [Column("is_encryption")]
        public bool IsEncryption { get;set;}


        [Column("hmac")]
        public string? Hmac { get;set;}
        [NotMapped]
        public bool Verify { get; set; }
        public string ToStringNoMac()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Id=" + this.Id);
            stringBuilder.Append("UserId=" + this.UserId);
            stringBuilder.Append("RealName=" + this.IdCardNo);
            return stringBuilder.ToString();
        }

        public RealNameInformation AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
            this.Hmac =  cryptographyProvider.Hmac(bytes).ByteArrayToBase64();
            return this;
        }

        public RealNameInformation VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            if (string.IsNullOrEmpty(this.Hmac))
            {
                return this;
            }
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
           

            var result=  cryptographyProvider.VerifyHmac(bytes, this.Hmac.Base64ToByteArray());
            this.Verify=result;

            return this;
        }

    }
}
