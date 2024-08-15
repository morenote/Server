namespace MoreNote.Config.ConfigFilePath.IMPL.Linux
{
	public class WSLDevConfig : IConfigFilePahProvider
	{
		public string? GetConfigFilePah()
		{
			var configFile = "/mnt/c/morenote/config.json";
			if (File.Exists(configFile))
			{
				return configFile;
			}
			return null;
		}
	}
}
