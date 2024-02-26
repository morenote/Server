using MoreNote.Common.Utils;

using System.Text.Json;

namespace MoreNote.Models.DTO.Leanote.USBKey
{
	public class DataSignDTO
	{
		public SignData SignData { get; set; }
		public string Sign { get; set; }
		public string Certificate { get; set; }


		public static DataSignDTO FromJSON(string json)
		{
			var dataSignDTO = JsonSerializer.Deserialize<DataSignDTO>(json, MyJsonConvert.GetLeanoteOptions());
			return dataSignDTO;
		}

		public string ToJson()
		{
			var json = JsonSerializer.Serialize(this);
			return json;
		}


	}
}
