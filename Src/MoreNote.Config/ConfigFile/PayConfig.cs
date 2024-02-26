namespace MoreNote.Config.ConfigFile
{
	public class PayConfig
	{
		/// <summary>
		/// 是否开启支付能力
		/// </summary>
		public bool Can { get; set; }
		/// <summary>
		/// Pay服务商
		/// </summary>
		public int PayBy { get; set; }
		/// <summary>
		/// PayJSConfig =1
		/// </summary>
		public PayJSConfig PayJSConfig { get; set; }

		public static PayConfig GenerateTemplate()
		{
			var pay = new PayConfig()
			{
				Can = false,
				PayBy = 1,
				PayJSConfig = PayJSConfig.GenerateTemplate()
			};
			return pay;

		}
	}
}
