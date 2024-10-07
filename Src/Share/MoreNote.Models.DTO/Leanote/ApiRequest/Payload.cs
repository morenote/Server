using github.hyfree.GM;

using MoreNote.Common.Utils;

using System.Text;
using System.Text.Json;

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
		public void SetData(string data)
		{
			var gm = new GMService();
			this.Data = data;
			var hex = Common.Utils.HexUtil.ByteArrayToHex(Encoding.UTF8.GetBytes(data));
			this.Hash = gm.SM3(hex);
		}

		public string ToJson()
		{
			var json = JsonSerializer.Serialize(this);
			return json;
		}
	}
}
