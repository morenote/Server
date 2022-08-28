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
    public interface NodePackageManagement
    {

        public NodePackageManagement SetWorkingDirectory(string workingDirectory);
        public Task<NodePackageManagement> Init();

        public Task<NodePackageManagement> Install(string packageName);

        public Task<NodePackageManagement> InstallGlobal(string packageName);

        public Task<NodePackageManagement> InstallDev(string packageName);
        public Task<NodePackageManagement> Run(string Command);


        public Task<NodePackageManagement> SetRegistry(string registryURL);
    }
}