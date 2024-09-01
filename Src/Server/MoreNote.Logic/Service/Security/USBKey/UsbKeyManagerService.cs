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
			this.WebSiteConfig = configFileService.WebConfig;
			this.cryptographyProvider = cryptographyProvider;
		}

		public List<USBKeyBinding> ListAllUsbKey(long? userId)
		{
			return this.dataContext.USBKeyBindings.Where(b => b.UserId == userId).ToList();
		}

		public async void DeleteUsbkey(long? id)
		{
			this.dataContext.USBKeyBindings.Where(b => b.Id == id).DeleteFromQuery();
			await this.dataContext.SaveChangesAsync();
		}

		public USBKeyBinding Find(long? id)
		{
			var usbkey = this.dataContext.USBKeyBindings.Where(b => b.Id == id).FirstOrDefault();
			return usbkey;
		}
		public bool IsExist(long? userId, string Modulus)
		{
			var any = this.dataContext.USBKeyBindings.Where(b => b.UserId == userId && b.Modulus == Modulus).Any();
			return any;
		}
		public bool IsExist(long? userId)
		{
			var any = this.dataContext.USBKeyBindings.Where(b => b.UserId == userId).Any();
			return any;
		}


		public USBKeyBinding Add(USBKeyBinding usbkey)
		{
			if (usbkey.Id == null)
			{
				usbkey.Id = idGenerator.NextId();
			}

			this.dataContext.USBKeyBindings.Add(usbkey);

			this.dataContext.SaveChanges();
			return usbkey;
		}
	}
}