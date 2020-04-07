using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class RandomImageService
    {
        public static async System.Threading.Tasks.Task InsertImageAsync(RandomImage randomImage)
        {
            using (var db = new DataContext())
            {
                db.RandomImage.Add(randomImage);
                await  db.SaveChangesAsync();
            }
        }
        public static RandomImage GetRandomImage(string type)
        {
            using (var db = new DataContext())
            {
                var count = db.RandomImage.Where(b=>b.TypeName.Equals(type)&&b.Sex==false).Count() ;
                if (count<1)
                {
                    return null;
                }
                Random random = new Random();
                var num = random.Next(0, count);
                var result = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.Sex == false).Skip(num).FirstOrDefault();
                return result;
            }
        }
        public static bool Exist(string type, string fileNameSHA1)
        {
            using (var db = new DataContext())
            {
                var count = db.RandomImage.Where(b => b.TypeName.Equals(type) && b.FileNameSHA1 .Equals(fileNameSHA1)).Count();
                return count>0;
            }
        }


    }
}
