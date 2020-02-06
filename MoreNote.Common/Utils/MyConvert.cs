using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MoreNote.Common.Utils
{
    public class MyConvert
    {
        /// <summary>
        /// hex字符串转long类型
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns> 转换后的数字 失败返回 0 </returns>
        public static long HexToLong(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return 0;

            }
            long number = 0;
            try
            {
                //return Convert.ToInt64(numberstr, 16);
                return long.Parse(hex, NumberStyles.HexNumber);
            }
            catch(Exception e)
            {
                return 0;
            }
        }
        public static long? HexToLongObject(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return null;

            }
            long number = 0;
            try
            {
                //return Convert.ToInt64(numberstr, 16);
                return long.Parse(hex, NumberStyles.HexNumber);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
