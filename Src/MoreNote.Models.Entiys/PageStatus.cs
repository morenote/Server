using System.ComponentModel.DataAnnotations;


namespace NickelProject.Logic.Entity
{
	public class PageStatus
	{
		[Key]//主键 
			 //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //设置自增
		public string Location { get; set; }
		public UserEntity user { get; set; }

	}
}
