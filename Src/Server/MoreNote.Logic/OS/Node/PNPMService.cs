using CliWrap;

using Microsoft.Extensions.Logging;

using System;
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
		ILogger logger;
		public PNPMService(ILogger logger)
		{
			this.logger = logger;
		}
		public NodePackageManagement SetWorkingDirectory(string workingDirectory)
		{
			this.workingDirectory = workingDirectory;
			return this;
		}

		public async Task<NodePackageManagement> Init()
		{
			var stdOutBuffer = new StringBuilder();
			var stdErrBuffer = new StringBuilder();

			var result = await Cli.Wrap("pnpm")
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

		public async Task<NodePackageManagement> InstallDev(string packageName)
		{
			var stdOutBuffer = new StringBuilder();
			var stdErrBuffer = new StringBuilder();

			var result = await Cli.Wrap("pnpm")
				.WithArguments($"install -D {packageName}")
				   .WithWorkingDirectory(this.workingDirectory)
				   .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
			   .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
				   .ExecuteAsync();
			return this;
		}

		public Task<NodePackageManagement> InstallGlobal(string packageName)
		{
			throw new NotImplementedException();
		}

		public async Task<NodePackageManagement> Run(string cmd)
		{
			var stdOutBuffer = new StringBuilder();
			var stdErrBuffer = new StringBuilder();
			try
			{
				var result = await Cli.Wrap("pnpm")
			   .WithArguments(cmd)
			   .WithWorkingDirectory(this.workingDirectory)
			  .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
			  .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
			   .ExecuteAsync();

			}
			catch (Exception ex)
			{

				this.logger.LogError(ex.ToString());
			}



			var stdOut = stdOutBuffer.ToString();
			var stdErr = stdErrBuffer.ToString();

			this.logger.LogDebug("PNPM=" + stdOut);
			this.logger.LogDebug("PNPM=" + stdErr);

			return this;
		}

		public async Task<NodePackageManagement> SetRegistry(string registryURL)
		{
			var result = await Cli.Wrap("pnpm")
				.WithArguments($"config set registry {registryURL}")
						  .WithWorkingDirectory(this.workingDirectory)
					  .ExecuteAsync();
			return this;
		}


	}
}
