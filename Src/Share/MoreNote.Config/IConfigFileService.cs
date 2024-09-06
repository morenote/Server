using MoreNote.Config.ConfigFile;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Config
{
    public interface IConfigFileService
    {
        void Save();

        void Reload();

        WebSiteConfig ReadConfig();

        void InitTemplateConfig();

    }
}
