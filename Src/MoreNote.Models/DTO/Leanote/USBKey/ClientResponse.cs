using MoreNote.Common.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.USBKey
{
    /// <summary>
    /// 客户端应答
    /// </summary>
    public class ClientResponse
    {
        /// <summary>
        /// 挑战ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 客户端证书
        /// </summary>
        public string Certificate { get; set; }

        

        /// <summary>
        /// 客户端签名
        /// </summary>
        public string Sign { get; set; }


       
        public static ClientResponse FromJSON(string json)
        {
            var clientResponse = JsonSerializer.Deserialize<ClientResponse>(json,MyJsonConvert.GetLeanoteOptions());
            return clientResponse;
        }

        public string ToJson()
        {
            var json = JsonSerializer.Serialize(this);
            return json;
        }
    }
}