namespace MoreNote.Models.Entity.ConfigFile
{
	/// <summary>
	/// 通用密钥设备配置
	/// </summary>
	public class SDFConfig
	{

		public string ConfigFilePath { get; set; } = @"E:\library\Windows_win32\htjncipher.ini";//SDF配置文件地址
		public string SDFDLLFilePath { get; set; } = @"E:\library\Windows_win32\HtjnCpSDF.dll";//SDF驱动文件地址

		public string PucKey { get; set; } = "71F1DB7305615E115B9B27FE86E55D69";//导出密钥，此密钥被DefaultSM4KeyIndex所指示的密钥加密

		public uint DefaultSM4KeyIndex { get; set; } = 1;//预定义使用的SM4密钥索引
		public uint DefaultSM2KeyIndex { get; set; } = 11;//预定义使用的SM2密钥索引

	}
}
