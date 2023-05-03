using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFilePath.IMPL.Linux
{
    public class EtcConfig : IConfigFilePahProvider
    {
        public string? GetConfigFilePah()
        {
            var configFile = "/etc/morenote/config.json";
            if (File.Exists(configFile))
            {
                return configFile;
            }
            return null;
        }
    }
}
