using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
