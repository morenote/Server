using MoreNote.Common.Utils;
using MoreNote.GM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoreNote.Models.DTO.Leanote.ApiRequest
{
    public class DigitalEnvelope
    {
        public string PayLoad { get; set; }
        public string Key { get; set; }


        public static DigitalEnvelope FromJSON(string json)
        {
            var dataPack = JsonSerializer.Deserialize<DigitalEnvelope>(json, MyJsonConvert.GetLeanoteOptions());
            return dataPack;
        }

        public string GetPayLoadValue(GMService gMService,string priKey)
        {
            var rawKey = gMService.SM2Decrypt(this.Key, priKey,false);
            var rawPayLod=gMService.SM4_Decrypt_CBC(this.PayLoad, rawKey, "00000000000000000000000000000000",false);
            var rawPayLodObj= PayLoadDTO.FromJSON(rawPayLod);
            var myHash = gMService.SM3(rawPayLodObj.Data);
            if (!myHash.ToUpper().Equals(rawPayLodObj.Hash))
            {
                return null;
            }
            return rawPayLodObj.Data;
        }

        public string ToJson()
        {
            var json = JsonSerializer.Serialize(this);
            return json;
        }
    }
}
