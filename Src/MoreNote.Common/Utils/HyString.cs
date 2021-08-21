using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MoreNote.Common.HySystem
{
   public class HyString
    {
        public static string FilterSpecial(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");
                return str;
            }
        }


        public static string IsSpecial(string str)
        {
            if (str == "")
            {
                return str;
            }
            else
            {
              
                str = str.Replace("'", "");
                str = str.Replace("<", "");
                str = str.Replace(">", "");
                str = str.Replace("%", "");
                str = str.Replace("'delete", "");
                str = str.Replace("''", "");
                str = str.Replace("\"\"", "");
                str = str.Replace(",", "");
                str = str.Replace(".", "");
                str = str.Replace(">=", "");
                str = str.Replace("=<", "");
                str = str.Replace("-", "");
                str = str.Replace("_", "");
                str = str.Replace(";", "");
                str = str.Replace("||", "");
                str = str.Replace("[", "");
                str = str.Replace("]", "");
                str = str.Replace("&", "");
                str = str.Replace("#", "");
                str = str.Replace("/", "");
                str = str.Replace("-", "");
                str = str.Replace("|", "");
                str = str.Replace("?", "");
                str = str.Replace(">?", "");
                str = str.Replace("?<", "");
                str = str.Replace(" ", "");
                return str;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool isSpecialChar(String str) {
            String regEx = "[ _`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]|\n|\r|\t";
            return true;
        }
        /// <summary>
        /// 判断是否是非法字符
        /// </summary>
        /// <param name="str">判断是字符</param>
        /// <returns></returns>
        public static Boolean isLegalNumber(string str)
        {
            char[] charStr = str.ToLower().ToCharArray();
            for (int i = 0; i < charStr.Length; i++)
            {
                int num = Convert.ToInt32(charStr[i]);
                if (!(IsChineseLetter(num)|| (num >= 48 && num <= 57) || (num >= 97 && num <= 123) || (num >= 65 && num <= 90) || num == 45))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 判断字符的Unicode值是否是汉字
        /// </summary>
        /// <param name="code">字符的Unicode</param>
        /// <returns></returns>
        protected static bool IsChineseLetter(int code)
        {
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);

            if (code >= chfrom && code <= chend)
            {
                return true;     //当code在中文范围内返回true

            }
            else
            {
                return false;    //当code不在中文范围内返回false
            }

          
        }
        /// <summary>
        /// 合法字符为：字母,数字,字符: a-z （97-123） A-Z（ 65-90 ） 0-9 （48-57 ） -（45） . （46） @（64） _（95）;括号内为字符对应的ASCII码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsHaveIllegalChar(string s) 
        { 
            for (int i = 0; i < s.Length; i++) 
            { 
                char c = s.ToLower().ToCharArray()[i]; 
                if ((!((c >= 97 && c <= 123) || (c >= 48 && c <= 57) || c == 45 || c == 46 || c == 64 || c == 95))) 
                    return false; 
                break; 
            } 
            return true; 
        }
        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }

    
    }
}
