using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace MoreNote.Logic.Service
{
    public class ConfigService
    {
        private static ConfigService _configService;
        private static object _lockObject = new object();
        private const string path = @"Config\config.json";
        public EmailConfig emailConfig;
        public class EmailConfig
        {
            public String Host { get; set; }
            public bool EnableSsl { get; set; }
            public int Port { get; set; }
            public bool UseDefaultCredentials { get; set; }
            public string userName { get; set; }
            public string password { get; set; }
        }
        public static ConfigService GetConfigService()
        {
            lock (_lockObject)
            {
                if (_configService != null)
                {
                    return _configService;
                }
                else
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                        {
                            string str = streamReader.ReadToEnd();
                            _configService = JsonSerializer.Deserialize<ConfigService>(str);
                        }
                        return _configService;
                    }
                    catch (Exception e)
                    {
                        return null;

                    }


                }

            }


        }
        public static void Save(ConfigService configService)
        {
            lock (_lockObject)
            {
                using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string a = JsonSerializer.Serialize(configService);
                    streamWriter.WriteLine(a);
                    streamWriter.Flush();
                }
                _configService = configService;

            }

        }




    }
}
