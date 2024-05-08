using MoreNote.Models.Enums;

using System.Text.Json.Serialization;

namespace MoreNote.Config.ConfigFile
{
	/// <summary>
	/// Postgresql 连接配置
	/// </summary>
	public class DataBaseConfig
	{
		[JsonConverter(typeof(JsonStringEnumConverter))]
		public SqlEngine SqlEngine { get; set; }=SqlEngine.PostgreSQL;

        public string PostgreSQL { get; set; }
        public string MySQL { get; set; }
		public string SQLite3 { get; set; }


		public static DataBaseConfig GenerateTemplate()
		{
			DataBaseConfig postgreSqlConfig = new DataBaseConfig()
			{
                PostgreSQL = "Host=127.0.0.1;Port=5432;Database=postgres; User ID=postgres;Password=postgres;"
			};
			return postgreSqlConfig;
		}
	}
}
