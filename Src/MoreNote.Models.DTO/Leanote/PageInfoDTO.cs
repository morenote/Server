namespace MoreNote.Models.DTO.Leanote
{
	public class PageInfoDTO
	{
		/// <summary>
		/// 分页位置 从0开始 
		/// </summary>
		public int PageNumber { get; set; }
		/// <summary>
		/// 分页的大小,每页包含多少数据 
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// 一共有多少页
		/// </summary>
		public int PageSum { get; set; }

	}
}
