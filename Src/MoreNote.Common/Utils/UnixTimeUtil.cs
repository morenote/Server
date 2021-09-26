using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Common.Utils
{
   public class UnixTimeUtil
    {
     
        public static long? GetTimeStampInLong()
        {
            return  DateTimeOffset.Now.ToUnixTimeSeconds();
        }
           public static long  GetUnixTimeMillisecondsInLong()
        {
            return  DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        public static int GetTimeStampInInt32()
        {  
            
            return (int)DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        /// <summary>
        /// 检查是否有效
        /// </summary>
        /// <param name="oldTime"></param>
        /// <param name="validtime"></param>
        /// <returns></returns>
        public static bool IsValid(long? oldTime,long? validtime)
        {
            long? unixTimestamp = GetTimeStampInLong();
            bool valid = unixTimestamp < (oldTime + validtime);
            return valid;
        }
        public static long? GetTimeStampInLong(DateTime dateTime)
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }


        /// <summary>
        /// 将时间戳转换为日期类型，并格式化
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        private static string LongDateTimeToDateTimeString(string longDateTime)
        {
            //用来格式化long类型时间的,声明的变量
            double unixDate;
            DateTime start;
            DateTime date;
            //ENd

            unixDate = long.Parse(longDateTime);
            start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date.ToString("yyyy-MM-dd HH:mm:ss");

        }
        /// <summary> 
        /// 获取时间戳 10位
        /// </summary> 
        /// <returns></returns> 
        private static long? GetTimeStampTen3()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
