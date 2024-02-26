namespace MoreNote.Logic.Models.DTO.Vditor.Upload
{
	public class UploadData
	{
		public List<string> errFiles { get; set; } = new List<string>();
		public Dictionary<string, string> succMap { get; set; } = new Dictionary<string, string>();
	}
}
