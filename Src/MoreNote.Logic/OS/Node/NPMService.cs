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
    public class NPMService : NodePackageManagement
    {

        public NodePackageManagement SetWorkingDirectory(string workingDirectory)
        {
            throw new NotImplementedException();
        }

        public async Task<NodePackageManagement> SetRegistry(string registryURL)
        {
            var result = await Cli.Wrap($"npm config set registry {registryURL}")
                     .ExecuteAsync();
            return this;
        }

    
        public async Task<NodePackageManagement> Init()
        {
            throw new NotImplementedException();
        }

        public async Task<NodePackageManagement> Install(string packageName)
        {
            throw new NotImplementedException();
        }

        public Task<NodePackageManagement> InstallDev(string packageName)
        {
            throw new NotImplementedException();
        }

        public  async Task<NodePackageManagement> InstallGlobal(string packageName)
        {
            var result = await Cli.Wrap($"npm install -g {packageName}")
                        .ExecuteAsync();

            return this;

        }

        public async Task<NodePackageManagement> Run(string Command)
        {
            throw new NotImplementedException();
        }

     
    }
}
