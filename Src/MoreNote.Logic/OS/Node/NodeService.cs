using Microsoft.Extensions.Logging;

namespace MoreNote.Logic.OS.Node
{
	public class NodeService
	{
		private readonly ILogger _logger;
		public NodeService(ILogger logger)
		{
			this._logger = logger;
		}

		public NodePackageManagement GetNPM(NPMType npmName)
		{
			switch (npmName)
			{
				case NPMType.NPM:
					return new NPMService();

				case NPMType.PNPM:
					return new PNPMService(this._logger);

				case NPMType.YARN:
					return new Yarn();
				default:
					return new NPMService();
			}


		}

	}
}
