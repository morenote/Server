using MoreNote.Common.Utils;
using MoreNote.Config.ConfigFilePath.IMPL.Linux;
using MoreNote.Config.ConfigFilePath.IMPL.Windows;

namespace MoreNote.Config.ConfigFilePath.IMPL
{
	/// <summary>
	/// 寻找配置文件
	/// </summary>
	public class ServerConfigFilePahFinder: IConfigFilePahFinder
    {

		public  string GetConfigFilePah()
		{
			List<IConfigFilePahProvider> lists = new List<IConfigFilePahProvider>();

			if (RuntimeEnvironment.IsWindows)
			{
				lists.Add(new WindowsEnvConfig());
				lists.Add(new WindowsDefaultConfig());

			}
			else
			{
				lists.Add(new EnvConfig());
				lists.Add(new WSLDevConfig());
				lists.Add(new RootConfig());
				lists.Add(new EtcConfig());
			}
			foreach (var item in lists)
			{
				var path = item.GetConfigFilePah();
				if (!string.IsNullOrEmpty(path))
				{
					return path;

				}

			}
			throw new Exception("Configuration file cannot be found");
		}

	}
}
