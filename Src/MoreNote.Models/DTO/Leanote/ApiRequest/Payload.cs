using github.hyfree.GM;

using MoreNote.Common.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
    public class PayLoadDTO
    {
        public string Data { get; set; }
        public string Hash { get; set; }
        public static PayLoadDTO FromJSON(string json)
        {
            var dataPack = JsonSerializer.Deserialize<PayLoadDTO>(json, MyJsonConvert.GetLeanoteOptions());
            return dataPack;
        }
        public void SetData(string data)  {
            var gm = new GMService();
            this.Data = data;
            var hex = Common.Utils.HexUtil.ByteArrayToSHex(Encoding.UTF8.GetBytes(data));
            this.Hash = gm.SM3(hex);
          }

    public string ToJson()
        {
            var json = JsonSerializer.Serialize(this);
            return json;
        }
    }
}
