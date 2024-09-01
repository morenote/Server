namespace MoreNote.Models.Enums
{
	/// <summary>
	/// 文件储存方式
	/// 这个值决定了执行文件操作
	/// </summary>
	public enum StorageTypeEnum
	{
		/// <summary>
		/// 本地磁盘（自己实现的低性能方法）
		/// </summary>
		LocalDisk = 0x00,
		/// <summary>
		/// 又拍云 对象储存
		/// </summary>
		UpYunOSS = 0x01,
		/// <summary>
		/// 阿里云对象储存
		/// </summary>
		ALiYunOSS = 0x02,
		/// <summary>
		/// 优刻得对象储存
		/// </summary>
		UcloudOSS = 0x03,
		/// <summary>
		/// 七牛云对象储存
		/// </summary>
		QiNiuOSS = 0x04,
		/// <summary>
		/// 华为云对象储存
		/// </summary>
		HuaWeiOSS = 0x05,
		/// <summary>
		/// 腾讯云对象储存
		/// </summary>
		TencentOSS = 0x06,
		/// <summary>
		/// FTP服务
		/// </summary>
		FTP = 0x10,
		/// <summary>
		/// WebDAV服务
		/// </summary>
		WebDAV = 0x11,
		/// <summary>
		/// Minio对象储存
		/// </summary>
		Minio = 0x12

	}
}