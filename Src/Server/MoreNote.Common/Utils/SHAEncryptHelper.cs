﻿using System;
using System.Text;

namespace MoreNote.Common.Utils
{
	public class SHAEncryptHelper
	{
		/// <summary>
		/// 计算文件的 MD5 值
		/// </summary>
		/// <param name="fileName">要计算 MD5 值的文件名和路径</param>
		/// <returns>MD5 值16进制字符串</returns>
		public string MD5File(string fileName)
		{
			return HashFile(fileName, AlgNameEnum.MD5);
		}

		/// <summary>
		/// 计算文件的 sha1 值
		/// </summary>
		/// <param name="fileName">要计算 sha1 值的文件名和路径</param>
		/// <returns>sha1 值16进制字符串</returns>
		public string SHA1File(string fileName)
		{
			return HashFile(fileName, AlgNameEnum.SHA1);
		}
		/// <summary>
		/// 计算文件的哈希值
		/// </summary>
		/// <param name="fileName">要计算哈希值的文件名和路径</param>
		/// <param name="algName">算法:sha1,md5</param>
		/// <returns>哈希值16进制字符串</returns>
		private string HashFile(string fileName, AlgNameEnum algNameEnum)
		{
			if (!System.IO.File.Exists(fileName))
				return string.Empty;

			System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			byte[] hashBytes = HashData(fs, algNameEnum);
			fs.Close();
			return ByteArrayToHexString(hashBytes);
		}
		/// <summary>
		/// 计算哈希值(默认SHA1)
		/// </summary>
		/// <param name="stream">要计算哈希值的 Stream</param>
		/// <param name="algName">算法:sha1,md5</param>
		/// <returns>哈希值字节数组</returns>
		private byte[] HashData(System.IO.Stream stream, AlgNameEnum algNameEnum)
		{
			System.Security.Cryptography.HashAlgorithm algorithm;
			switch (algNameEnum)
			{
				case AlgNameEnum.MD5:
					algorithm = System.Security.Cryptography.MD5.Create();
					break;
				case AlgNameEnum.SHA1:
					algorithm = System.Security.Cryptography.SHA1.Create();
					break;
				case AlgNameEnum.SHA256:
					algorithm = System.Security.Cryptography.SHA256.Create();
					break;
				case AlgNameEnum.SHA384:
					algorithm = System.Security.Cryptography.SHA384.Create();
					break;
				case AlgNameEnum.SHA512:
					algorithm = System.Security.Cryptography.SHA512.Create();
					break;
				default:
					algorithm = System.Security.Cryptography.SHA1.Create();
					break;
			}
			return algorithm.ComputeHash(stream);
		}
		enum AlgNameEnum
		{
			MD5, SHA1, SHA256, SHA384, SHA512
		}

		/// <summary>
		/// 字节数组转换为16进制表示的字符串
		/// </summary>
		private string ByteArrayToHexString(byte[] buf)
		{
			return BitConverter.ToString(buf).Replace("-", "");
		}
		
	
		/// <summary>
		/// Sha256加密
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public static string Hash256Encrypt(string password)
		{
			var sha256 = new System.Security.Cryptography.SHA256CryptoServiceProvider();
			byte[] hashedDataBytes;
			hashedDataBytes = sha256.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(password));
			StringBuilder tmp = new StringBuilder();
			foreach (byte i in hashedDataBytes)
			{
				tmp.Append(i.ToString("x2"));
			}
			return tmp.ToString();
		}


		public static string Hash1Encrypt(string password)
		{
			var sha256 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			byte[] hashedDataBytes;
			hashedDataBytes = sha256.ComputeHash(Encoding.GetEncoding("UTF-8").GetBytes(password));
			StringBuilder tmp = new StringBuilder();
			foreach (byte i in hashedDataBytes)
			{
				tmp.Append(i.ToString("x2"));
			}
			return tmp.ToString();
		}
		public static string Hash1Encrypt(byte[] message)
		{
			var sha256 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			byte[] hashedDataBytes;
			hashedDataBytes = sha256.ComputeHash(message);
			StringBuilder tmp = new StringBuilder();
			foreach (byte i in hashedDataBytes)
			{
				tmp.Append(i.ToString("x2"));
			}
			return tmp.ToString();
		}

	}
}

