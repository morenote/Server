using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fido2NetLib.Development;
using Fido2NetLib.Objects;

using Masuit.Tools.Models;
using Microsoft.EntityFrameworkCore;

using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Entity;


using StackExchange.Redis;

namespace MoreNote.Models.Entity.Security.FIDO2
{
    
    [Table("fido2_item"), Index(nameof(Id), IsUnique =true)]
    public class FIDO2Item
    {

        [Key]
        [Column("id")]
        public long? Id{get; set;}

         [Column("user_id")]
        public long? UserId { get; set; }

        /// <summary>
        /// FIDO2名称
        /// </summary>
        [Column("fido2_name")]
        public string? FIDO2Name { get;set;}

         /// <summary>
        /// 安全密钥凭证唯一ID
        /// </summary>
        [Column("fido2_credential_id")]
        
        public byte[]? CredentialId { get; set; }
        /// <summary>
        /// FIDO2用户公钥
        /// </summary>
        [Column("fido2_public_key")]
        public byte[]? PublicKey { get; set; }
        /// <summary>
        /// FIDO2用户唯一标识
        /// </summary>
        [Column("fido2_user_handle")]
        public byte[]? UserHandle { get; set; }
        /// <summary>
        /// FIDO2签名次数
        /// </summary>
        [Column("fido2_signature_counter")]
        public uint SignatureCounter { get; set; }
        /// <summary>
        /// FIDO2凭证类型
        /// </summary>
        [Column("fido2_cred_type")]
        public string? CredType { get; set; }
        /// <summary>
        /// FIDO2注册时间
        /// </summary>
        [Column("fido2_reg_date")]
        public DateTime? RegDate { get; set; }
        /// <summary>
        /// FIDO2唯一序列号
        /// </summary>
        [Column("fido2_guid")]
        public Guid? AaGuid { get; set; }


        public StoredCredential GetStoredCredentialByUser()
        {
            var storedCredential = new StoredCredential()
            {
                Descriptor = new PublicKeyCredentialDescriptor(this.CredentialId),
                PublicKey = this.PublicKey,
                UserHandle = this.UserHandle,
                SignatureCounter = (uint)this.SignatureCounter
            };
            return storedCredential;
        }
        public PublicKeyCredentialDescriptor GetDescriptor()
        {
            return new PublicKeyCredentialDescriptor(this.CredentialId);
        }


    }
}
