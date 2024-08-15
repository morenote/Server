namespace MoreNote.Config.ConfigFilePath.IMPL.Linux
{
	public class EnvConfig : IConfigFilePahProvider
	{

		public string GetConfigFilePah()
		{
			var env = Environment.GetEnvironmentVariable("MORENOTE_CONFIG");
			return env;
		}

	}
}
