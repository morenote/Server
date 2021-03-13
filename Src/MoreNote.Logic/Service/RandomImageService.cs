using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class RandomImageService
    {
        private DataContext dataContext;

        public RandomImageService(DataContext dataContext)

        {
            this.dataContext = dataContext;
        }

        private static Dictionary<string, List<RandomImage>> randomImageList = null;
        private static List<string> _imageTypeList = null;

        public List<string> GetImageTypeList()
        {
            if (_imageTypeList == null)
            {
                _imageTypeList = new List<string>();
                InitTypeList();
            }
            return _imageTypeList;
        }

        private static void InitTypeList()
        {
            _imageTypeList.Add("动漫综合1");
            _imageTypeList.Add("动漫综合2");
            _imageTypeList.Add("动漫综合3");
            _imageTypeList.Add("动漫综合4");
            _imageTypeList.Add("动漫综合5");
            _imageTypeList.Add("动漫综合6");
            _imageTypeList.Add("动漫综合7");
            _imageTypeList.Add("动漫综合8");
            _imageTypeList.Add("动漫综合9");
            _imageTypeList.Add("动漫综合10");
            _imageTypeList.Add("动漫综合11");
            _imageTypeList.Add("动漫综合12");
            _imageTypeList.Add("动漫综合13");
            _imageTypeList.Add("动漫综合14");
            _imageTypeList.Add("动漫综合15");
            _imageTypeList.Add("动漫综合16");
            _imageTypeList.Add("动漫综合17");
            _imageTypeList.Add("动漫综合18");

            _imageTypeList.Add("火影忍者1");

            _imageTypeList.Add("海贼王1");
            _imageTypeList.Add("从零开始的异世界生活1");
            _imageTypeList.Add("SAO1");

            _imageTypeList.Add("缘之空1");

            _imageTypeList.Add("东方project1");

            _imageTypeList.Add("猫娘1");

            _imageTypeList.Add("少女前线1");

            _imageTypeList.Add("风景系列1");
            _imageTypeList.Add("风景系列2");
            _imageTypeList.Add("风景系列3");
            _imageTypeList.Add("风景系列4");
            _imageTypeList.Add("风景系列5");
            _imageTypeList.Add("风景系列6");
            _imageTypeList.Add("风景系列7");
            _imageTypeList.Add("风景系列8");
            _imageTypeList.Add("风景系列9");
            _imageTypeList.Add("风景系列10");

            _imageTypeList.Add("物语系列1");
            _imageTypeList.Add("物语系列2");

            _imageTypeList.Add("明日方舟1");
            _imageTypeList.Add("明日方舟2");

            _imageTypeList.Add("重装战姬1");

            _imageTypeList.Add("P站系列1");
            _imageTypeList.Add("P站系列2");
            _imageTypeList.Add("P站系列3");
            _imageTypeList.Add("P站系列4");

            _imageTypeList.Add("CG系列1");
            _imageTypeList.Add("CG系列2");
            _imageTypeList.Add("CG系列3");
            _imageTypeList.Add("CG系列4");
            _imageTypeList.Add("CG系列5");

            _imageTypeList.Add("守望先锋");

            _imageTypeList.Add("王者荣耀");

            _imageTypeList.Add("少女写真1");
            _imageTypeList.Add("少女写真2");
            _imageTypeList.Add("少女写真3");
            _imageTypeList.Add("少女写真4");
            _imageTypeList.Add("少女写真5");
            _imageTypeList.Add("少女写真6");

            _imageTypeList.Add("死库水萝莉");
            _imageTypeList.Add("萝莉");
            _imageTypeList.Add("极品美女图片");
            _imageTypeList.Add("日本COS中国COS");

            _imageTypeList.Add("少女映画");

            _imageTypeList.Add("二次元视频");
            _imageTypeList.Add("舞蹈视频");
            _imageTypeList.Add("唱歌视频");
            _imageTypeList.Add("鬼畜视频");
            _imageTypeList.Add("鹿鸣系列视频");
        }

        public Dictionary<string, List<RandomImage>> GetRandomImageList()
        {
            if (randomImageList == null)
            {
                randomImageList = new Dictionary<string, List<RandomImage>>();
            }
            return randomImageList;
        }

        public async System.Threading.Tasks.Task InsertImageAsync(RandomImage randomImage)
        {
            dataContext.RandomImage.Add(randomImage);
            await dataContext.SaveChangesAsync();
        }

        public RandomImage GetRandomImage(string type)
        {
            int count = dataContext.RandomImage
                .Where(b => b.TypeName.Equals(type) &&
                            b.Sex == false &&
                            b.IsDelete == false &&
                            b.Block == false)
                .Count();
            if (count < 1)
            {
                return null;
            }
            Random random = new Random();
            int num = random.Next(0, count);
            RandomImage result = dataContext.RandomImage
                .Where(b => b.TypeName.Equals(type) &&
                            b.Sex == false &&
                            b.IsDelete == false &&
                            b.Block == false)
                .Skip(num)
                .FirstOrDefault();
            return result;
        }

        public List<RandomImage> GetRandomImages(string type, int size)
        {
            int count = dataContext.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.IsDelete == false && b.Block == false).Count();
            if (count < size)
            {
                size = count;
            }
            List<RandomImage> result = dataContext.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.IsDelete == false && b.Block == false).Take(size).ToList<RandomImage>();
            return result;
        }

        public bool Exist(string type, string fileSHA1)
        {
            int count = dataContext.RandomImage.Where(b => b.TypeName.Equals(type) && b.FileSHA1.Equals(fileSHA1)).Count();
            return count > 0;
        }

        public bool Exist(string fileSHA1)
        {
            int count = dataContext.RandomImage.Where(b => b.FileSHA1.Equals(fileSHA1)).Count();
            return count > 0;
        }
    }
}