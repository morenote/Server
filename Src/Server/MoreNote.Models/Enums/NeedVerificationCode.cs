namespace MoreNote.Models.Enums
{
	/// <summary>
	/// 是否启用人机验证码 
	/// </summary>
	public enum NeedVerificationCode
	{
		/// <summary>
		/// 关闭
		/// </summary>
		OFF = 0x00,
		/// <summary>
		/// 打开
		/// </summary>
		ON = 0x01,

		/// <summary>
		/// 由程序自动判断
		/// </summary>
		AUTO = 0x02
	}
}
