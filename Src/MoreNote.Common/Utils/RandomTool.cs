﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace MoreNote.Common.Utils
{
	/// <summary>
	/// 随机数据发生工具类
	/// </summary>
	public class RandomTool
	{
		/// <summary>
		/// 使用加密型强随机数生成器生产安全的随机字符串0-9a-zA-Z
		/// 不恰当的使用会产生泄露的风险
		/// </summary>
		/// <param name="VcodeNum">随机字符串的长度</param>
		/// <returns>返回一个随机字符串</returns>
		public static string CreatRandomString(int VcodeNum)
		{
			//验证码可以显示的字符集合  
			var Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
				",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
				",R,S,T,U,V,W,X,Y,Z,!,@,#,$,%,^,&,*,(,),+,=,<,>,?";
			var vcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
			return CreatRandomString(VcodeNum, vcArray);
		}
		/// <summary>
		/// 使用加密型强随机数生成器生产安全的随机字符串0-9a-zA-Z
		/// </summary>
		/// <param name="VcodeNum"></param>
		/// <returns></returns>
		public static string CreatRandom09AZ(int VcodeNum)
		{
			//验证码可以显示的字符集合  
			var Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p" +
				",q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q" +
				",R,S,T,U,V,W,X,Y,Z";
			var vcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
			return CreatRandomString(VcodeNum, vcArray);
		}

		public static string CreatRandom58String(int VcodeNum)
		{
			//验证码可以显示的字符集合  
			var Vchar = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
			var vcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
			return CreatRandomString(VcodeNum, vcArray);
		}




		public static string CreatRandomString(int VcodeNum, string[] vcArray)
		{

			var code = new StringBuilder();//产生的随机数  
			var temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  


			//采用一个简单的算法以保证生成随机数的不同  
			for (int i = 1; i < VcodeNum + 1; i++)
			{

				int random = RandomNumberGenerator.GetInt32(0, vcArray.Length);//获取随机数  
				while (temp == random)
				{
					random = RandomNumberGenerator.GetInt32(0, vcArray.Length);//如果获取的随机数重复，则重新获取
				}
				temp = random;//把本次产生的随机数记录起来  
				code.Append(vcArray[random]);//随机数的位数加一  
			}
			return code.ToString();
		}




		/// <summary>
		/// 产生Base64形式的安全随机数
		/// </summary>
		/// <param name="ByteLength"></param>
		/// <returns></returns>
		public static string CreatSafeRandomBase64(int ByteLength = 32)
		{

			byte[] tokenData = RandomNumberGenerator.GetBytes(ByteLength);
			string token = Convert.ToBase64String(tokenData);
			return token;
		}
		/// <summary>
		/// 产生Hex形式的安全随机数
		/// </summary>
		/// <param name="ByteLength">字节长度</param>
		/// <returns></returns>
		public static string CreatSafeRandomHex(int ByteLength = 32)
		{
			using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
			{

				byte[] tokenData = new byte[ByteLength];
				rng.GetBytes(tokenData);
				string token = Convert.ToHexString(tokenData);
				return token;
			}
		}
		/// <summary>
		/// 产生不安全的随机整数
		/// </summary>
		/// <returns></returns>
		public static int CreatUnSafeNumber()
		{
			Random random = new Random();
			return random.Next();
		}
		/// <summary>
		/// 生成一个不可预测的盐,字节数组,默认256位
		/// </summary>
		/// <param name="saltLength">盐的长度x byte</param>
		/// <returns></returns>
		public static byte[] CreatSafeSaltByteArray(int saltLength = 32)
		{
			byte[] tokenData = RandomNumberGenerator.GetBytes(saltLength);
			return tokenData;
		}
		/// <summary>
		/// 生成一个不可预测的盐,默认256位
		/// </summary>
		/// <param name="saltLength">盐的长度x byte</param>
		/// <returns></returns>
		public static string CreatSafeSaltBase64(int saltLength = 32)
		{
			var tokenData = CreatSafeSaltByteArray(saltLength);
			string token = Convert.ToBase64String(tokenData);
			return token;
		}


	}
}
