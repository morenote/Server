using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreNote.Logic.Service
{
    public class RandomImageService
    {
        public static async System.Threading.Tasks.Task InsertImageAsync(RandomImage randomImage)
        {
            using (DataContext db = DataContext.getDataContext())
            {
                db.RandomImage.Add(randomImage);
                await db.SaveChangesAsync();
            }
        }
        public static RandomImage GetRandomImage(string type)
        {
            using (DataContext db = DataContext.getDataContext())
            {
                int count = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.Delete == false && b.Block == false).Count();
                if (count < 1)
                {
                    return null;
                }
                Random random = new Random();
                int num = random.Next(0, count);
                RandomImage result = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.Delete == false && b.Block == false).Skip(num).FirstOrDefault();
                return result;
            }
        }
        public static List<RandomImage> GetRandomImages(string type ,int size)
        {
            using (DataContext db = DataContext.getDataContext())
            {
                int count = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.Delete == false && b.Block == false).Count();
                if (count < size)
                {
                    size = count;
                }
                List<RandomImage> result = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false && b.Delete == false && b.Block == false).Take(size).ToList<RandomImage>();
                return result;
            }

        }
        public static bool Exist(string type, string fileSHA1)
        {
            using (DataContext db = DataContext.getDataContext())
            {
                int count = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.FileSHA1.Equals(fileSHA1)).Count();
                return count > 0;
            }
        }
        public static bool Exist(string fileSHA1)
        {
            using (DataContext db = DataContext.getDataContext())
            {
                int count = db.RandomImage.Where(b => b.FileSHA1.Equals(fileSHA1)).Count();
                return count > 0;
            }
        }
    }
}
