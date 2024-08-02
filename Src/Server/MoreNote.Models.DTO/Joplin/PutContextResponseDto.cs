namespace MoreNote.Logic.Models.DTO.Joplin
{
	public class PutContextResponseDto
	{
		public string name { get; set; }
		public string id { get; set; }
		/// <summary>
		/// 创建时间 Unix timestamp 毫秒
		/// </summary>
		public long created_time { get; set; }
		/// <summary>
		/// 更新时间  Unix timestamp 毫秒
		/// </summary>
		public long updated_time { get; set; }
	}
}
