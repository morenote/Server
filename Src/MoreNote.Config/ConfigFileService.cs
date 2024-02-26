using MoreNote.Config.ConfigFile;
using MoreNote.Config.ConfigFilePath.IMPL;

using System.Text.Json;

namespace MoreNote.Logic.Service
{
	/// <summary>
	/// 读取配置文件的类
	/// </summary>
	public class ConfigFileService
	{
		private string path = null;

		private WebSiteConfig _config { get; set; }

		// 定义一个标识确保线程同步
		private static readonly object locker = new object();

		public ConfigFileService()
		{
			if (_config == null)
			{
				this._config = WebConfig;
			}
		}

		/// <summary>
		/// 从配置文件中重新加载配置文件，但是某些功能仍然需要重启程序后生效
		/// </summary>
		public void Reload()
		{
			lock (locker)
			{
				path = GetConfigPath();
				Console.WriteLine($"重新加载配置文件:{path}");
				if (!File.Exists(path))
				{
					throw new IOException("Configuration file cannot be found!!");
				}
				string json = File.ReadAllText(path);
				this._config = System.Text.Json.JsonSerializer.Deserialize<WebSiteConfig>(json);
			}
		}

		/// <summary>
		/// 保存所作的修改到配置文件
		/// </summary>
		public void Save()
		{
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,//优质打印 无压缩
			};
			string json = System.Text.Json.JsonSerializer.Serialize(this._config, options);

			File.WriteAllText(GetConfigPath(), json);
		}

		/// <summary>
		/// 获取配置文件路径
		/// linux=/morenote/config.json
		/// window=C:\morenote\config.json
		/// </summary>
		/// <returns></returns>
		public static string GetConfigPath()
		{

			return ConfigFilePahFinder.GetConfigFilePah();
		}

		/// <summary>
		/// 初始化模板
		/// </summary>
		private void InitTemplateConfig()
		{

			WebSiteConfig webSiteConfig = new WebSiteConfig();
			var options = new JsonSerializerOptions
			{
				WriteIndented = true,//优质打印 无压缩
			};
			string json = System.Text.Json.JsonSerializer.Serialize(webSiteConfig, options);
			File.Create(path).Close();
			File.WriteAllText(path, json);
		}

		public WebSiteConfig WebConfig
		{
			get
			{
				if (_config == null)
				{
					lock (locker)
					{
						if (_config == null)
						{
							path = GetConfigPath();
							Console.WriteLine($"已经加载配置文件:{path}");
							if (!File.Exists(path))
							{
								InitTemplateConfig();
							}
							string json = File.ReadAllText(path);
							_config = JsonSerializer.Deserialize<WebSiteConfig>(json);
						}
					}
				}

				return _config;
			}
		}

		public void Save(WebSiteConfig tempConfig, string onePath)
		{
			lock (locker)
			{
				if (tempConfig == null)
				{
					return;
				}
				string json = System.Text.Json.JsonSerializer.Serialize(tempConfig);
				File.WriteAllText(onePath, json);
				_config = tempConfig;
			}
		}
	}
}