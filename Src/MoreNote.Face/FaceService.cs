using FaceRecognitionDotNet;

using MoreNote.Logic.Entity.ConfigFile;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreNote.Logic.Service.Security.Face
{
    public class FaceService
    {
        /// <summary>
        /// 人脸模型的文件夹路径
        /// </summary>
        string directory = "";
        /// <summary>
        /// 容差阈值。如果比较面部的差距小于容差阈值则相同,否则不一样
        /// </summary>
        double tolerance = 0.2d;
        public FaceService(WebSiteConfig webConfig)
        {
            var config = webConfig.SecurityConfig.FaceConfig;
            this.directory = config.ModelFilesDirectory;
            this.tolerance = config.Tolerance;
        }


        public bool CompareFace()
        {
           
            using (FaceRecognition fr = FaceRecognition.Create(directory))
            {

                using (Image imageA = FaceRecognition.LoadImageFile(@"E:\Share\01.jpg"))
                using (Image imageB = FaceRecognition.LoadImageFile(@"E:\Share\03.jpg"))
                {

                    IEnumerable<Location> locationsA = fr.FaceLocations(imageA);
                    IEnumerable<Location> locationsB = fr.FaceLocations(imageB);


                    IEnumerable<FaceEncoding> encodingA = fr.FaceEncodings(imageA, locationsA);
                    IEnumerable<FaceEncoding> encodingB = fr.FaceEncodings(imageB, locationsB);

                    
                    bool match = FaceRecognition.CompareFace(encodingA.First(), encodingB.First(), tolerance);

                    return match;
                }


            }
        }

    }
}
