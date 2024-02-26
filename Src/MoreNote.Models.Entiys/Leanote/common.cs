using System.ComponentModel.DataAnnotations.Schema;

namespace MoreNote.Logic.Entity
{

	//分页数据
	public class Page
	{

		[Column("cur_page")]
		public int CurPage { get; set; } // 当前页码 
		[Column("total_page")]
		public int TotalPage { get; set; } // 总页 
		[Column("per_page_size")]
		public int PerPageSize { get; set; }
		[Column("count")]
		public int Count { get; set; } // 总记录数 

		public List<dynamic> List { get; set; }

		public static Page Instance(int page, int perPageSize, int count, List<dynamic> list)
		{


			var totalPage = 0;

			if (count > 0)
			{
				totalPage = count / perPageSize;

			}
			return new Page()
			{
				CurPage = page,
				TotalPage = totalPage,
				PerPageSize = perPageSize,
				Count = count,
				List = list


			};

		}
	}
}
