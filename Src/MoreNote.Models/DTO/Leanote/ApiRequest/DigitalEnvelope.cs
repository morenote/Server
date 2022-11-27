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
    public class DigitalEnvelope
    {
        public string PayLoad { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
        public static DigitalEnvelope FromJSON(string json)
        {
            var dataPack = JsonSerializer.Deserialize<DigitalEnvelope>(json, MyJsonConvert.GetLeanoteOptions());
            return dataPack;
        }
        public string getSM4Key(GMService gMService, string priKey)
        {
            var rawKey = gMService.SM2Decrypt("04" + this.Key, priKey, false);
            return rawKey;
        }
        public PayLoadDTO GetPayLoadDTO(GMService gMService, string priKey)
        {
            var rawKey = gMService.SM2Decrypt("04" + this.Key, priKey, false);
            var rawPayLod = gMService.SM4_Decrypt_CBC(this.PayLoad, rawKey, this.IV, false);
            var rawPayLodObj = PayLoadDTO.FromJSON(rawPayLod);

            return rawPayLodObj;

        }

        public string GetPayLoadSM3(GMService gMService, string priKey)
        {
            var payload = GetPayLoadDTO(gMService, priKey);
            var dataHex = Common.Utils.HexUtil.ByteArrayToSHex(Encoding.UTF8.GetBytes(payload.Data));
            var myHash = gMService.SM3(dataHex);

            return myHash;
        }
        public string GetPayLoadValue(GMService gMService,string priKey)
        {
            var rawKey = gMService.SM2Decrypt("04"+this.Key, priKey,false);
            var rawPayLod=gMService.SM4_Decrypt_CBC(this.PayLoad, rawKey, this.IV,false);
            var rawPayLodObj= PayLoadDTO.FromJSON(rawPayLod);
            var dataHex= Common.Utils.HexUtil.ByteArrayToSHex(Encoding.UTF8.GetBytes(rawPayLodObj.Data));
            var myHash = gMService.SM3(dataHex);
            if (!myHash.ToUpper().Equals(rawPayLodObj.Hash.ToUpper()))
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
