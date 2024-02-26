using MoreNote.Common.Utils;
using MoreNote.Logic.Service.VerificationCode;

using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace MoreNote.Logic.Service.Captcha.IMPL
{
	/// <summary>
	/// 使用ZKWeb.System.Drawing https://github.com/zkweb-framework/ZKWeb.System.Drawing
	/// Ubuntu 16.04 及更高版本：
	/// apt-get install libgdiplus
	/// cd /usr/lib
	/// ln -s libgdiplus.so gdiplus.dll
	/// </summary>
	public class ZKWebCaptchaGenerator : ICaptchaGenerator
	{
		public byte[] GenerateImage(out string code, int codeLength = 4)
		{
			//验证码
			code = RandomTool.CreatRandom58String(codeLength);
			//Bitmap img = null;
			//Graphics g = null;

			var random = new Random();
			//验证码颜色集合
			Color[] c = {
				Color.Black,
				Color.Red,
				Color.DarkBlue,
				Color.Green,
				Color.Orange,
				Color.Brown,
				Color.DarkCyan,
				Color.Purple,
				Color.Yellow,
				Color.Cyan};

			//验证码字体集合
			string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
			byte[] imageBytes;
			using (var ms = new MemoryStream())
			using (var img = new Bitmap(code.Length * 40, 64))
			{
				using (var g = Graphics.FromImage(img))
				{
					g.Clear(Color.White);//背景设为白色
										 //验证码绘制在g中
					for (int i = 0; i < code.Length; i++)
					{
						var cindex = random.Next(7);//随机颜色索引值
						var findex = random.Next(5);//随机字体索引值
						Font font = new Font(fonts[findex], 15, FontStyle.Bold);//字体
						Brush brush = new SolidBrush(c[cindex]);//颜色
																//产生一个轻微的抖动
						int shakeX = random.Next(0, 5);
						int shakeY = random.Next(0, 10);
						float x = 3 + (i * 30) + shakeX;//x坐标
						float y = 0 + shakeY;//Y坐标
						string character = code.Substring(i, 1);//绘制的字符
						g.DrawString(character, font, brush, x, y);//绘制一个验证字符
					}
					//在随机位置画背景点
					for (int i = 0; i < 5; i++)
					{
						var cindex = random.Next(7);//随机颜色索引值
						int x = random.Next(img.Width);
						int y = random.Next(img.Height);
						using (var pen = new Pen(c[cindex], 2))
						{
							g.DrawRectangle(pen: pen, x: x, y: y, width: 1, height: 1);
						}
					}
					//随机线条
					for (int i = 0; i < 2; i++)
					{
						var cindex = random.Next(9);//随机颜色索引值
						int x1 = random.Next(img.Width);
						int y1 = random.Next(img.Height);
						int x2 = random.Next(img.Width);
						int y2 = random.Next(img.Height);
						using (var pen = new Pen(c[cindex], 2))
						{
							g.DrawLine(pen: pen, x1: x1, y1: y1, x2: x2, y2: y2);
						}
					}

					img.Save(ms, ImageFormat.Jpeg);//将此图像以Png图像文件的格式保存到流中
					imageBytes = ms.ToArray();
				}
			}

			return imageBytes;
		}
	}
}