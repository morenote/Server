using CliWrap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.OS.Node
{
    /// <summary>
    /// Node环境依赖包管理
    /// </summary>
    public class PNPMService : NodePackageManagement
    {
        string workingDirectory;

        public NodePackageManagement SetWorkingDirectory(string workingDirectory)
        {
           this.workingDirectory=workingDirectory;
            return this;
        }

        public Task<NodePackageManagement> Init()
        {
            throw new NotImplementedException();
        }

        public Task<NodePackageManagement> Install(string packageName)
        {
            throw new NotImplementedException();
        }

        public Task<NodePackageManagement> InstallDev(string packageName)
        {
            throw new NotImplementedException();
        }

        public Task<NodePackageManagement> InstallGlobal(string packageName)
        {
            throw new NotImplementedException();
        }

        public Task<NodePackageManagement> Run(string Command)
        {
            throw new NotImplementedException();
        }

        public async Task<NodePackageManagement> SetRegistry(string registryURL)
        {
            var result = await Cli.Wrap($"pnpm config set registry {registryURL}")
                      .ExecuteAsync();
            return this;
        }

       
    }
}
