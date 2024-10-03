using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Logic.Service.Logging;
using MoreNote.Logic.Service.PasswordSecurity;
using MoreNote.Models.DTO.Leanote.USBKey;
using MoreNote.Models.Entity.Leanote.Management.Loggin;
using MoreNote.SecurityProvider.Core;

namespace MoreNote.Logic.Service
{
	public class DataSignService
	{
		private DataContext dataContext;

		public WebSiteConfig Config { get; set; }
		private IDistributedIdGenerator idGenerator;
		private PasswordStoreFactory passwordStoreFactory;

		private ILoggingService logging { get; set; }

		protected ICryptographyProvider cryptographyProvider { get; set; }
		public DataSignService(DataContext dataContext, IDistributedIdGenerator idGenerator,
			ConfigFileService configFileService,
			PasswordStoreFactory passwordStoreFactory,
			ILoggingService logging,
			ICryptographyProvider cryptographyProvider)
		{
			this.idGenerator = idGenerator;
			this.dataContext = dataContext;
			this.Config = configFileService.ReadConfig();
			this.passwordStoreFactory = passwordStoreFactory;
			this.cryptographyProvider = cryptographyProvider;
			this.logging = logging;
		}
		public void AddDataSign(DataSignDTO dataSignDTO, string tag)
		{
			var dataSignLogging = new DataSignLogging()
			{
				Id = idGenerator.NextId(),
				Tag = tag,
				DataSignJson = dataSignDTO.ToJson()
			};
			
			this.dataContext.SaveChanges();
		}
	}
}
