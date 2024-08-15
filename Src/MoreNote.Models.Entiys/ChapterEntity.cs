using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickelProject.Logic.Entity
{
	public class ChapterEntity
	{
		[Key]//主键 
			 //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
		public string ChapterId { get; set; }
		public string ArticleId { get; set; }
		public string Content { get; set; }
		public string Summary { get; set; }
		public int SerialNumber { get; set; }
		public string Title { get; set; }
		[Column(TypeName = "datetime")]
		public DateTime CreatTime { get; set; }

	}
}
