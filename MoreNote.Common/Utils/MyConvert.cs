using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MoreNote.Common.Utils
{
    public class MyConvert
    {
        public static long ConvertStrToLong(string numberstr)
        {
           
            long number = 0;
            try
            {
                //return Convert.ToInt64(numberstr, 16);
                return long.Parse(numberstr, NumberStyles.HexNumber);
            }
            catch(Exception e)
            {
                return 0;
            }
           
          
        }
    }
}
