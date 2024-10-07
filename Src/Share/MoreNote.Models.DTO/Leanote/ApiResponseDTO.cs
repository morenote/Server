using MoreNote.Common.Utils;

using Newtonsoft.Json;

namespace MoreNote.Models.DTO.Leanote
{
	// 一般返回
	public class ApiResponseDTO
	{
		/// <summary>
		/// 唯一标识（可选）
		/// </summary>
		public long? Id { get; set; }
		/// <summary>
		/// 消息创建时间（可选）
		/// </summary>
		public long Timestamp { get; set; } = DateTime.Now.Ticks / 1000;
		/// <summary>
		/// 消息状态  成功或失败
		/// </summary>
		public bool Ok { get; set; }
		/// <summary>
		/// 提示信息或者错误信息，或者其他描述性辅助信息（可选）
		/// </summary>
		public string Msg { get; set; }
		/// <summary>
		/// 消息代码
		/// </summary>
		public int Code { get; set; } = 200;
		/// <summary>
		/// 返回的数据
		/// </summary>
		public dynamic Data { get; set; }
		/// <summary>
		/// 指示Data是否被加密；如果Data被加密，那么Data就是一个字符串
		/// </summary>
		public bool Encryption { get; set; }
		/// <summary>
		/// 分页信息
		/// </summary>
		public PageInfoDTO PageInfo { get; set; }


		public static ApiResponseDTO FormJson(string json)
		{
			return System.Text.Json.JsonSerializer.Deserialize<ApiResponseDTO>(json, MyJsonConvert.GetLeanoteOptions());
		}
        public  string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }

    }
}
