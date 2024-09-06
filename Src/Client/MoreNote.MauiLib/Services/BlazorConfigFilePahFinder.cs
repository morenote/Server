using MoreNote.Config.ConfigFilePath.IMPL;
using MoreNote.MauiLib.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.MauiLib.Services
{
    public class BlazorConfigFilePahFinder : IConfigFilePahFinder
    {
        public string GetConfigFilePah()
        {
            return MyPathUtil.ConfigFile;
        }
    }
}
