
using MoreNote.Common.Utils;
using MoreNote.Logic.Service.VerificationCode;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            code = RandomTool.CreatRandomString(codeLength);


            var width = code.Length * 20;
            var height = 32;

            byte[] buffer=null;
            using(MemoryStream ms = new MemoryStream())
            using (Image image =new Image<Rgba32>(width,height,Color.White))
            {
                FontCollection collection = new();
                FontFamily family = collection.Add("fonts/leanote-font3/leanote.ttf");
                Font font = family.CreateFont(12, FontStyle.Italic);
                image.Mutate(x => x.DrawText("hello", font,Color.Black,new PointF(10,10)));

                image.SaveAsBmp(ms);

               buffer= ms.ToArray();
            }
            return buffer;
        }
    }
}
