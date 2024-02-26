namespace MoreNote.Logic.Models.DTO.Vditor.Upload
{
	public class UploadFileResponse
	{

		public string msg { get; set; } = string.Empty;
		public int code { get; set; } = 0;
		public UploadData data { get; set; }
	}
}
