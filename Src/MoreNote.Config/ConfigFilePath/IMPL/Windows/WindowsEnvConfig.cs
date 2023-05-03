using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config.ConfigFilePath.IMPL.Windows
{
    public class WindowsEnvConfig : IConfigFilePahProvider
    {
        public string? GetConfigFilePah()
        {
            var env = System.Environment.GetEnvironmentVariable("MORENOTE_CONFIG");
            return env;
        }
    }
}
