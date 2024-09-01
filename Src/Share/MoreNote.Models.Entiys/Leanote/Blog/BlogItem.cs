using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entity.Leanote.User;

namespace MoreNote.Models.Entity.Leanote.Blog
{
	// 只为blog, 不为note copy hahaha
	public class BlogItem
	{

		public Note Note { get; set; }
		public string Abstract { get; set; }
		public string Content { get; set; } //可能是content的一部分, 截取.点击more后就是整个信息了
		public bool HasMore { get; set; }//是否是否还有
		public UserInfo User { get; set; }//用户信息

	}
}
