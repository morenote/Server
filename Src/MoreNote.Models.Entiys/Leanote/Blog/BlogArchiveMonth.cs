namespace MoreNote.Models.Entity.Leanote.Blog
{
	// 归档 数据容器 不需要数据库储存
	public class BlogArchiveMonth
	{
		public int Month { get; set; }
		public BlogPost[] Posts { get; set; }
	}

}
