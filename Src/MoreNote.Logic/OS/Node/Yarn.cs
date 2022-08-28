using CliWrap;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.OS.Node
{
    public class Yarn : NodePackageManagement
    {
        private string workingDirectory;

        public NodePackageManagement SetWorkingDirectory(string workingDirectory)
        {
            this.workingDirectory = workingDirectory;
            return this;
        }

        public async Task<NodePackageManagement> Init()
        {
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            var result = await Cli.Wrap("yarn")
                .WithArguments("init --yes")
             .WithWorkingDirectory(this.workingDirectory)
               .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
               .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
             .ExecuteAsync();
            return this;
        }

        public Task<NodePackageManagement> Install(string packageName)
        {
            throw new NotImplementedException();
        }

        public async Task<NodePackageManagement> InstallGlobal(string packageName)
        {
            throw new NotImplementedException();
        }

        public async Task<NodePackageManagement> InstallDev(string packageName)
        {
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            var result = await Cli.Wrap("yarn")
                .WithArguments($"add -D {packageName}")
                   .WithWorkingDirectory(this.workingDirectory)
                   .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
               .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                   .ExecuteAsync();
            return this;
        }

        public async Task<NodePackageManagement> Run(string cmd)
        {
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();

            var result = await Cli.Wrap("yarn")
                 .WithArguments(cmd)
                 .WithWorkingDirectory(this.workingDirectory)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                 .ExecuteAsync();
            return this;
        }

        public async Task<NodePackageManagement> SetRegistry(string registryURL)
        {
            var result = await Cli.Wrap($"yarn")
                 .WithArguments($"config set registry {registryURL}")
                 .WithWorkingDirectory(this.workingDirectory)
                 .ExecuteAsync();
            return this;
        }
    }
}