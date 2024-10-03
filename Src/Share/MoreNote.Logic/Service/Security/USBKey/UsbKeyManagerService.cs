using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Leanote.User;

using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service.Security.USBKey
{
	public class UsbKeyManagerService
	{
		private DataContext dataContext;
		private IDistributedIdGenerator idGenerator;
		private ICryptographyProvider cryptographyProvider;
		private WebSiteConfig WebSiteConfig { get; set; }

		public UsbKeyManagerService(DataContext dataContext,
			IDistributedIdGenerator distributedIdGenerator,
			 ICryptographyProvider cryptographyProvider,
		ConfigFileService configFileService)
		{
			this.dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
			this.idGenerator = distributedIdGenerator;
			this.WebSiteConfig = configFileService.ReadConfig();
			this.cryptographyProvider = cryptographyProvider;
		}

		public List<UserSM2Binding> ListAllUsbKey(long? userId)
		{
			return this.dataContext.UserSM2Binding.Where(b => b.UserId == userId).ToList();
		}

		public async void DeleteUsbkey(long? id)
		{
			this.dataContext.UserSM2Binding.Where(b => b.Id == id).DeleteFromQuery();
			await this.dataContext.SaveChangesAsync();
		}

		public UserSM2Binding Find(long? id)
		{
			var usbkey = this.dataContext.UserSM2Binding.Where(b => b.Id == id).FirstOrDefault();
			return usbkey;
		}
		public bool IsExist(long? userId, string publicKey)
		{
			var any = this.dataContext.UserSM2Binding.Where(b => b.UserId == userId && b.PublicKey == publicKey).Any();
			return any;
		}
		public bool IsExist(long? userId)
		{
			var any = this.dataContext.UserSM2Binding.Where(b => b.UserId == userId).Any();
			return any;
		}


		public UserSM2Binding Add(UserSM2Binding usbkey)
		{
			if (usbkey.Id == null)
			{
				usbkey.Id = idGenerator.NextId();
			}

			this.dataContext.UserSM2Binding.Add(usbkey);

			this.dataContext.SaveChanges();
			return usbkey;
		}
	}
}