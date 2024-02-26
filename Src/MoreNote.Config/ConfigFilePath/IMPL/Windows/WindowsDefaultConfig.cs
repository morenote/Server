namespace MoreNote.Config.ConfigFilePath.IMPL.Windows
{
	public class WindowsDefaultConfig : IConfigFilePahProvider
	{
		public string? GetConfigFilePah()
		{
			return "C:\\morenote\\config.json";
		}
	}
}
