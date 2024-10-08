﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Utils;

using System;
using System.Text;

namespace MoreNote.Logic.Service.PasswordSecurity.Tests
{
	[TestClass()]
	public class PasswordStoreTests
	{
		string hex = "000102030405060708090A0B0C0D0E0F";
		string password = "12345678";

		[TestMethod()]
		public void BCryptPasswordStoreTest()
		{
			var salt = HexUtil.HexToByteArray(hex);
			IPasswordStore passwordStore = new BCryptPasswordStore();
			var enc = passwordStore.Encryption(Encoding.UTF8.GetBytes(password), salt, 9);
			var text = HexUtil.ByteArrayToHex(enc);
			Console.WriteLine(text);
		}

		[TestMethod()]
		public void Argon2PasswordStoreTest()
		{
			var salt = HexUtil.HexToByteArray(hex);
			IPasswordStore passwordStore = new Argon2PasswordStore();
			var enc = passwordStore.Encryption(Encoding.UTF8.GetBytes(password), salt, 8);
			var text = HexUtil.ByteArrayToHex(enc);
			Console.WriteLine(text);
		}

		[TestMethod()]
		public void PDKDF2PasswordStoreTest()
		{
			var salt = HexUtil.HexToByteArray(hex);
			IPasswordStore passwordStore = new PDKDF2PasswordStore();
			var enc = passwordStore.Encryption(Encoding.UTF8.GetBytes(password), salt, 1000 * 80);
			var text = HexUtil.ByteArrayToHex(enc);
			Console.WriteLine(text);
		}

		[TestMethod()]
		public async void Sha256PasswordStoreTest()
		{
			var salt = HexUtil.HexToByteArray(hex);
			IPasswordStore passwordStore = new SHA256PasswordStore();
			var enc = passwordStore.Encryption(Encoding.UTF8.GetBytes(password), salt, 1000 * 160);
			var text = HexUtil.ByteArrayToHex(enc);
			Console.WriteLine(text);

		}
	}
}