using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
    public class Signature
    {
        /// <summary>
        /// Header+Payload的哈希值
        /// </summary>
        public  string Hash { get; set; }
        /// <summary>
        /// X509证书
        /// </summary>
        public string Certificate { get; set; }
        /// <summary>
        /// 公钥
        /// </summary>
        public  string PublicKey { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public  string UserId { get; set; }
        /// <summary>
        /// 签名值
        /// </summary>
        public  string Sign { get; set; }

    }
}
