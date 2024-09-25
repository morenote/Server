using MoreNote.Config.ConfigFile;
using MoreNote.CryptographyProvider;
using MoreNote.Logic.Service.PasswordSecurity.IMPL;
using MoreNote.Models.Entity.Leanote.User;
using MoreNote.SecurityProvider.Core;

using System;

namespace MoreNote.Logic.Service.PasswordSecurity
{
	public class PasswordStoreFactory
	{
		private ICryptographyProvider cryptography;
		public PasswordStoreFactory(ICryptographyProvider cryptography)
		{
			this.cryptography = cryptography;
		}
		public IPasswordStore Instance(string hash_algorithm, int PasswordStoreDegreeOfParallelism = 8, int PasswordStoreMemorySize = 1024)
		{
			switch (hash_algorithm.ToLower())
			{
				case "sha256":
					return new SHA256PasswordStore();

				case "argon2":
					return new Argon2PasswordStore()
					{
						DegreeOfParallelism = PasswordStoreDegreeOfParallelism,
						MemorySize = PasswordStoreMemorySize
					};
				case "bcrypt":
					return new BCryptPasswordStore();
				case "pbkdf2":
					return new PDKDF2PasswordStore();
				case "sjj1962":
					return new SJJ1962PasswordStore(cryptography);
				default:
					throw new ArgumentException("hash_algorithm is error", "hash_algorithm");
			}
		}
		public IPasswordStore Instance(SecurityConfig securityConfig)
		{
			return Instance(securityConfig.PasswordHashAlgorithm, securityConfig.PasswordStoreDegreeOfParallelism, securityConfig.PasswordStoreMemorySize);
		}
		public IPasswordStore Instance(UserInfo user)
		{
			return Instance(user.PasswordHashAlgorithm, user.PasswordDegreeOfParallelism, user.PasswordMemorySize);
		}
	}
}