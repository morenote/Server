namespace MoreNote.Config.ConfigFile
{
	/// <summary>
	/// 图片爬虫配置
	/// </summary>
	public class ImageSpidersConfig
	{
		/// <summary>
		/// 是否启用爬虫服务
		/// </summary>
		public bool CanCrawlerWorker { get; set; }
		/// <summary>
		/// 爬虫抓取速度 每隔几秒
		/// </summary>
		public int Reptile_Delay_Second { get; set; }




	}
}
