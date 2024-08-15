namespace NickelProject.Logic.Entity
{
	public class EditorImageUploadResult
	{
		/* "errno": 0,

    // data 是一个数组，返回若干图片的线上地址
    "data": [
        "图片1地址",
        "图片2地址",
        "……"
    ]*/
		public int errno { get; set; }
		public List<string> data { get; set; }

		public EditorImageUploadResult()
		{
			data = new List<string>();
		}
	}
}
