using System.Text;

namespace MoreNote.Models.DTO.Leanote.USBKey
{
	public class SignData
	{
		public long? Id { get; set; }// ID
		public string Data { get; set; }//待签名数据 可以为json或xml
		public string Hash { get; set; }//内容哈希
		public long? UserId { get; set; }//用户名
		public string? Tag { get; set; }//标签
		public int UinxTime { get; set; }//时间戳
		public string? Operate { get; set; }//操作


		public byte[] GetBytes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Id=" + this.Id);
			stringBuilder.Append("Data=" + this.Data);
			stringBuilder.Append("Hash=" + this.Hash);
			stringBuilder.Append("UserId=" + this.UserId);
			stringBuilder.Append("Tag=" + this.Tag);
			stringBuilder.Append("Operate=" + this.Operate);

			var data = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			return data;
		}
	}
}
