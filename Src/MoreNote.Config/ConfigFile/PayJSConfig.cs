namespace MoreNote.Config.ConfigFile
{

	public class PayJSConfig
	{
		/// <summary>
		/// 支付渠道代号
		/// </summary>
		public int PayCode { get; } = 1;
		//商户ID
		public String PayJS_MCHID { get; set; }
		//密钥
		public String PayJS_Key { get; set; }
		//回调ID
		public string Notify_Url { get; set; }


		public static PayJSConfig GenerateTemplate()
		{
			PayJSConfig payJSConfig = new PayJSConfig()
			{
				PayJS_MCHID = "商户ID",
				PayJS_Key = "密钥"
			};
			return payJSConfig;
		}
	}
}
