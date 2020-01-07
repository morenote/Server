using MySql.Data.MySqlClient;
using NickelProject.Logic.DB;
using NickelProject.Logic.Entity;
using System.Collections.Generic;

namespace NickelProject.Logic.Service
{
    public class PayManager
    {
        public static int BuyChapter(PayEntity payEntity)
        {
            int a = 0;
            using (var db = new DataContext())
            {
                db.Pay.Add(payEntity);
                a=db.SaveChanges();
            }
            return a;
        }

        public static bool CheckPurchase(string chapterId, string userid)
        {

            return false;
        }
        public static List<PayEntity> SelectPayByUserId(string userid)
        {

            return null;
        }
        public static List<PayEntity> SelectPayByUserId(string userid, string chapterid)
        {

            return null;

        }
        public static List<PayEntity> SelectAllChapter(int minLimit, int maxLimit)
        {

            return null;
        }
        private static PayEntity MySqlRead2Pay(MySqlDataReader reader)
        {
            return null;
        }
    }
}
