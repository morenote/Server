using MoreNote.Common.Utils;

using System.Text;

namespace MoreNote.Models.DTO.Leanote.USBKey
{
	/// <summary>
	/// 服务器挑战
	/// </summary>
	public class ServerChallenge
	{
		/// <summary>
		/// 挑战ID UUID 流水号
		/// </summary>
		public long? Id { get; set; }
		/// <summary>
		/// 用户id
		/// </summary>
		public long? UserId { get; set; }
		/// <summary>
		/// 标签，标识其用途
		/// </summary>
		public string Tag { get; set; }

		/// <summary>
		/// 客户端请求序列号
		/// </summary>
		public string RequestNumber { get; set; }
		/// <summary>
		/// 随机数  16字节
		/// </summary>
		public string Random { get; set; }
		/// <summary>
		/// 挑战时间 unix时间戳
		/// </summary>
		public int UinxTime { get; set; }

		/// <summary>
		/// 检查挑战是否过期
		/// </summary>
		/// <param name="expirationSecond"></param>
		/// <returns></returns>
		public bool VerifyTime(int expirationSecond)
		{
			long now = UnixTimeUtil.GetTimeStampInInt32();
			var span = now - this.UinxTime;
			return span <= expirationSecond;
		}
		public byte[] GetBytes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Id=" + this.Id);
			stringBuilder.Append("UserId=" + this.UserId);
			stringBuilder.Append("Tag=" + this.Tag);
			stringBuilder.Append("RequestNumber=" + this.RequestNumber);
			stringBuilder.Append("Random=" + this.Random);
			stringBuilder.Append("UinxTime=" + this.UinxTime);
			var data = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			return data;
		}

	}
}
