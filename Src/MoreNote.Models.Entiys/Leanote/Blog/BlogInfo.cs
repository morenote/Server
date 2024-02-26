namespace MoreNote.Models.Entity.Leanote.Blog
{
	public class BlogInfo
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public string UserLogo { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string Logo { get; set; }
		public bool OpenComment { get; set; }
		public string CommentType { get; set; } // leanote, or disqus
		public string DisqusId { get; set; }
		public string ThemeId { get; set; }
		public string SubDomain { get; set; }
		public string Domain { get; set; }
	}

}
