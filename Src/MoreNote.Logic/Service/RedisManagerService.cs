using Newtonsoft.Json;

using StackExchange.Redis;

using System;

namespace MoreNote.Logic.Service
{
    public class RedisManagerService
    {
        
        private static IDatabase db=null;
        private string configContentStr=null;
        public RedisManagerService(ConfigFileService configFileService)
        {
           
            if (db == null)
            {
                configContentStr = configFileService.WebConfig.RedisConfig.Configuration;
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configContentStr);
                db = redis.GetDatabase();
            }
        }
     
        public  void SetString(string K,string V, TimeSpan? expiry = null)
        {
        
            db.StringSet(K, V,expiry);
        }

        public  string GetString(string K)
        {
            

           return db.StringGet(K);
        }
        public void SetObject(string K,Object o, TimeSpan? expiry = null)
        {
            string demojson = JsonConvert.SerializeObject(o);//序列化
            db.StringSet(K, demojson);
        }
        public Object GetObject(string K)
        {
            string model = db.StringGet(K); 
            return JsonConvert.DeserializeObject(model);
        }
        public void SetLocker(string K, RedisValue token, TimeSpan expiry)
        {
           db.LockTake(K,token,expiry);
        }
        public void LockRelease(string K, RedisValue token)
        {
            db.LockRelease("lock_key", token);//释放锁
        }


    }
}
