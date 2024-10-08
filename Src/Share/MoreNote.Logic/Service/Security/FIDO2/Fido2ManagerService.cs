﻿using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Database;
using MoreNote.Logic.Service.DistributedIDGenerator;
using MoreNote.Models.Entity.Security.FIDO2;
using MoreNote.SecurityProvider.Core;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Security.FIDO2
{
	public class Fido2ManagerService
	{
		private DataContext dataContext;
		private IDistributedIdGenerator idGenerator;
		private ICryptographyProvider cryptographyProvider;
		private WebSiteConfig WebSiteConfig { get; set; }

		public Fido2ManagerService(DataContext dataContext,
			IDistributedIdGenerator distributedIdGenerator,
			 ICryptographyProvider cryptographyProvider,
		ConfigFileService configFileService)
		{
			this.dataContext = dataContext;
			this.idGenerator = distributedIdGenerator;
			this.WebSiteConfig = configFileService.ReadConfig();
			this.cryptographyProvider = cryptographyProvider;

		}
		public List<FIDO2Item> ListAllFido2(long? userId)
		{
			return this.dataContext.Fido2Items.Where(b => b.UserId == userId).ToList();
		}
		public async Task SetFIDO2Name(long? id, long? userId, string keyName)
		{
			await this.dataContext.Fido2Items.Where(b => b.Id == id && b.UserId == userId).UpdateFromQueryAsync(b => new FIDO2Item() { FIDO2Name = keyName });
			await this.dataContext.SaveChangesAsync();
		}

		public async Task DeleteFido2(long? id)
		{
			this.dataContext.Fido2Items.Where(b => b.Id == id).DeleteFromQuery();
			await this.dataContext.SaveChangesAsync();
		}

		public FIDO2Item Find(long? id)
		{
			var fido2 = this.dataContext.Fido2Items.Where(b => b.Id == id).FirstOrDefault();
			return fido2;
		}
		public bool IsExist(long? userId, byte[] CredentialId)
		{
			var any = this.dataContext.Fido2Items.Where(b => b.UserId == userId && b.CredentialId == CredentialId).Any();
			return any;
		}
		public bool IsExist(long? userId)
		{
			var any = this.dataContext.Fido2Items.Where(b => b.UserId == userId).Any();
			return any;
		}


		public FIDO2Item Add(FIDO2Item fido2)
		{
			if (fido2.Id == null)
			{
				fido2.Id = idGenerator.NextId();
			}

			this.dataContext.Fido2Items.Add(fido2);

			this.dataContext.SaveChanges();
			return fido2;
		}
	}
}
