using Microsoft.EntityFrameworkCore;
using MoreNote.Common.ExtensionMethods;
using MoreNote.CryptographyProvider;
using Morenote.Models.Models.Entity;
using NpgsqlTypes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MoreNote.Models.Entity.Leanote.Notes
{
    /// <summary>
    /// <para>笔记内容和可以被允许修改的属性</para>
    /// <para>
    ///  一个note可以拥有多个NoteContent,但是只允许有一个处于活动状态
    ///  剩余的NoteContent被识别为历史记录
    /// </para>
    /// </summary>
    [Table("note_content"), Index(nameof(NoteId), nameof(UserId), nameof(IsHistory))]
    public class NoteContent : BaseEntity
    {

        [Column("note_id")]
        public long? NoteId { get; set; }
        [Column("user_id")]
        public long? UserId { get; set; }
        [Column("is_blog")]
        public bool IsBlog { get; set; } // 为了搜索博客 
        [Column("content")]
        public string? Content { get; set; }//内容
        [Column("content_vector"), JsonIgnore]
        public NpgsqlTsVector? ContentVector { get; set; }


        //public string WebContent{ get;set;}//为web页面优化的内容
        [Column("abstract")]
        public string? Abstract { get; set; } // 摘要, 有html标签, 比content短, 在博客展示需要, 不放在notes表中
        [Column("created_time")]
        public DateTime CreatedTime { get; set; }
        [Column("updated_time")]
        public DateTime UpdatedTime { get; set; }
        [Column("updated_user_id")]
        public long? UpdatedUserId { get; set; } // 如果共享了,  并可写, 那么可能是其它他修改了
        [Column("is_history")]
        public bool IsHistory { get; set; }//是否是历史纪录
        [Column("is_encryption")]
        public bool IsEncryption { get; set; }//指示当前笔记内容是否被加密
        [Column("enc_key")]
        public string? EncryptionKey { get; set; }//加密密钥

        [Column("enc_algorithms")]
        public string? EncryptionAlgorithms { get;set;}="GM";//加密算法

        [Column("pbkdf2_salt")]
        public string? PBKDF2Salt { get; set; } //PBKDF2盐值
        [Column("hmac")]
        public string? Hmac { get; set; }
        [NotMapped]
        public bool HmacVerify { get; set; }
        public string ToStringNoMac()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Id=" + Id);
            stringBuilder.Append("NoteId=" + NoteId);
            stringBuilder.Append("Content=" + Content);
            return stringBuilder.ToString();
        }

        public NoteContent AddMac(ICryptographyProvider cryptographyProvider)
        {
            var bytes = Encoding.UTF8.GetBytes(ToStringNoMac());
            Hmac = cryptographyProvider.Hmac(bytes).ByteArrayToBase64();
            return this;
        }

        public NoteContent VerifyHmac(ICryptographyProvider cryptographyProvider)
        {
            if (string.IsNullOrEmpty(Hmac))
            {
                return this;
            }
            var bytes = Encoding.UTF8.GetBytes(ToStringNoMac());


            var result = cryptographyProvider.VerifyHmac(bytes, Hmac.Base64ToByteArray());
            HmacVerify = result;

            return this;
        }

    }
}
