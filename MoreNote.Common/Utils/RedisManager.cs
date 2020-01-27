using StackExchange.Redis;

namespace MoreNote.Common.Utils
{
    public class RedisManager
    {
        private static IDatabase db=null;
        public RedisManager()
        {
           
            if (db == null)
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                db = redis.GetDatabase();
            }
            
        }

        public static void SetString(string K,string V)
        {
            if (db == null)
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                 db = redis.GetDatabase();

            }
            db.StringSet(K, V);
        }

        public static string GetString(string K)
        {
            if (db == null)
            {
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
                IDatabase db = redis.GetDatabase();

            }

           return db.StringGet(K);
        }

    }
}
