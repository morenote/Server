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
    public class RealNameInformation
    {
        [Key]
        [Column("id")]
        public long? Id { get;set;}
        [Column("user_id")]
        public long? UserId { get;set;}
        [Column("real_name")]
        public string? RealName { get;set;}
        [Column("hmac")]
        public string? Hmac { get;set;}
        [NotMapped]
        public bool Verify { get; set; }
        public string ToStringNoMac()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Id=" + this.Id);
            stringBuilder.Append("UserId=" + this.UserId);
            stringBuilder.Append("RealName=" + this.RealName);
            return stringBuilder.ToString();
        }

        public async Task<RealNameInformation> AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
            var base64 = Convert.ToBase64String(bytes);
            this.Hmac = await cryptographyProvider.hmac(base64);
            return this;
        }

        public async Task<RealNameInformation> VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            if (string.IsNullOrEmpty(this.Hmac))
            {
                return this;
            }
            var bytes = Encoding.UTF8.GetBytes(this.ToStringNoMac());
            var base64 = Convert.ToBase64String(bytes);

            var result= await cryptographyProvider.verifyHmac(base64, this.Hmac);
            this.Verify=result;

            return this;
        }

    }
}
