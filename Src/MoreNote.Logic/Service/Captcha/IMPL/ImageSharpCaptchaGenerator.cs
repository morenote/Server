using MoreNote.Common.Utils;
using MoreNote.Logic.Service.VerificationCode;

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Captcha.IMPL
{
    /// <summary>
    ///
    ///
    ///
    /// </summary>
    public class ImageSharpCaptchaGenerator : ICaptchaGenerator
    {
        public ImageSharpCaptchaGenerator()
        {
        }

        public byte[] GenerateImage(out string code, int codeLength = 4)
        {
            //验证码
            code = RandomTool.CreatRandom58String(codeLength);

            var random = new Random();

            //验证码颜色集合
            Color[] colorArray = {
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
            string[] fonts = {"fonts/Apalu-2.ttf",
                              
                              "fonts/Arvo-Italic.ttf",
                              "fonts/brelaregular.ttf",
                              "fonts/ColorTube-2.ttf",
                              "fonts/LeagueGothic-Italic.ttf",
                              "fonts/Quantum-2.ttf"
                              };
            var width = code.Length * 40;
            var height = 64;

            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            using (Image image = new Image<Rgba32>(width, height, Color.White))
            {
                 
                 //绘制干扰线条
                for (int i = 0; i < 2; i++)
                {
                    var cindex = random.Next(9);//随机颜色索引值
                    int x1 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);

                    int x2 = random.Next(image.Width);
                    int y2 = random.Next(image.Height);

                    int x3 = random.Next(image.Width);
                    int y3 = random.Next(image.Height);

                    int x4 = random.Next(image.Width);
                    int y4 = random.Next(image.Height);

                    int x5 = random.Next(image.Width);
                    int y5 = random.Next(image.Height);
                    var thickness=random.Next(2,4);

                    var linerSegemnt = new LinearLineSegment(
                        new Vector2(x1, y1),
                        new Vector2(x2, y2),
                        new Vector2(x3, y3),
                        new Vector2(x4, y4),
                        new Vector2(x5, y5)
                    );
                    var color=colorArray[cindex];
                    var p = new SixLabors.ImageSharp.Drawing.Path(linerSegemnt);
                    image.Mutate(x => x.Draw(color, thickness, p));
                }

                //绘制文字
                for (int i = 0; i < code.Length; i++)
                {
                    var cindex = random.Next(7);//随机颜色索引值
                    var findex = random.Next(6);//随机字体索引值

                    var fontSize = random.Next(35, 50);

                    FontCollection collection = new();
                    FontFamily family = collection.Add(fonts[findex]);
                    Font font = family.CreateFont(fontSize);


                    Color selectColor = colorArray[cindex];

                    //产生一个轻微的抖动
                    int shakeX = random.Next(-5, 15);
                    int shakeY = random.Next(-5, 25);

                    float x = 3 + (i * 30) + shakeX;//x坐标
                    float y = 0 + shakeY;//Y坐标
                    string character = code.Substring(i, 1);//绘制的字符

                    image.Mutate(opera => opera.DrawText(character, font, selectColor, new PointF(x, y)));
                }
                //在随机位置画背景点
                for (int i = 0; i < 5; i++)
                {
                    var cindex = random.Next(7);//随机颜色索引值
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                }
               
                //绘制干扰点
                for (int i = 0; i < 40; i++)
                {
                    var cindex = random.Next(7);//随机颜色索引值
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    var  rectangle = new  Rectangle(x, y, 1, 1);
                    var coloer=colorArray[cindex];
                    image.Mutate(x=>x.Draw(coloer,4f,rectangle));
                }

                image.SaveAsBmp(ms);

                buffer = ms.ToArray();
            }
            return buffer;
        }
    }
}