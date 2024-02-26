namespace MoreNote.Models.Entity.Leanote.Blog
{
	public struct BlogUrls
	{

		public long? BlogUrlsId { get; set; }
		public string IndexUrl { get; set; }
		public string CateUrl { get; set; }
		public string SearchUrl { get; set; }
		public string SingleUrl { get; set; }
		public string PostUrl { get; set; }
		public string ArchiveUrl { get; set; }
		public string TagsUrl { get; set; }
		public string TagPostsUrl { get; set; }
	}
}
