using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
