using System;
using System.Text;

namespace MoreNote.Common.Utils
{
	public class HexUtil
	{
        /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
        /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
        /// <returns> Returns an array of bytes. </returns>
        public static byte[] HexToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }

        /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
        /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
        /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
        public static string ByteArrayToHex(byte[] data, bool pad = false)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                if (pad)
                {
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
                }
                else
                {
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                }

            }

            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }

            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /**/

        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {
            string[] tmpary = hexstring.Trim().Split(' ');
            byte[] buff = new byte[tmpary.Length];
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] = Convert.ToByte(tmpary[i], 16);
            }
            return buff;
        }

        /// <summary>
        /// 将byte型转换为字符串
        /// </summary>
        /// <param name="arrInput">byte型数组</param>
        /// <returns>目标字符串</returns>
        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            //将此实例的值转换为System.String
            return sOutput.ToString();
        }

        /// <summary>
        /// 对接收到的数据进行解包（将接收到的byte型数组解包为Unicode字符串）
        /// </summary>
        /// <param name="recbytes">byte型数组</param>
        /// <returns>Unicode编码的字符串</returns>
        public static string disPackage(byte[] recbytes)
        {
            string temp = "";
            foreach (byte b in recbytes)
                temp += b.ToString("X2") + " ";//ToString("X2") 为C#中的字符串格式控制符
            return temp;
        }

        /**
        * int转byte[]
        * 该方法将一个int类型的数据转换为byte[]形式，因为int为32bit，而byte为8bit所以在进行类型转换时，知会获取低8位，
        * 丢弃高24位。通过位移的方式，将32bit的数据转换成4个8bit的数据。注意 &0xff，在这当中，&0xff简单理解为一把剪刀，
        * 将想要获取的8位数据截取出来。
        * @param i 一个int数字
        * @return byte[]
        */

        public static byte[] int2ByteArray(int i)
        {
            byte[] result = new byte[4];
            result[0] = (byte)((i >> 24) & 0xFF);
            result[1] = (byte)((i >> 16) & 0xFF);
            result[2] = (byte)((i >> 8) & 0xFF);
            result[3] = (byte)(i & 0xFF);
            return result;
        }

        /**
         * byte[]转int
         * 利用int2ByteArray方法，将一个int转为byte[]，但在解析时，需要将数据还原。同样使用移位的方式，将适当的位数进行还原，
         * 0xFF为16进制的数据，所以在其后每加上一位，就相当于二进制加上4位。同时，使用|=号拼接数据，将其还原成最终的int数据
         * @param bytes byte类型数组
         * @return int数字
         */

        public static int bytes2Int(byte[] bytes)
        {
            int num = bytes[3] & 0xFF;
            num |= ((bytes[2] << 8) & 0xFF00);
            num |= ((bytes[1] << 16) & 0xFF0000);
            num |= ((bytes[0] << 24) & 0xFF0000);
            return num;
        }

        public static string Int2String(int str)
        {
            string S = Convert.ToString(str);
            return S;
        }

        public static int String2Int(string str)
        {
            int a;
            int.TryParse(str, out a);
            int a1 = Convert.ToInt32(str);
            return a1;
        }

        /*将int转为低字节在后，高字节在前的byte数组
        b[0] = 11111111(0xff) & 01100001
        b[1] = 11111111(0xff) & 00000000
        b[2] = 11111111(0xff) & 00000000
        b[3] = 11111111(0xff) & 00000000
        */
        public static byte[] IntToByteArray2(int value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }

        //将高字节在前转为int，低字节在后的byte数组(与IntToByteArray2想对应)
        public int ByteArrayToInt2(byte[] bArr)
        {
            if (bArr.Length != 4)
            {
                return -1;
            }
            return (int)((((bArr[0] & 0xff) << 24)
                       | ((bArr[1] & 0xff) << 16)
                       | ((bArr[2] & 0xff) << 8)
           | ((bArr[3] & 0xff) << 0)));
        }

        public static string StringToHexArray(string input)
        {
            char[] values = input.ToCharArray();
            StringBuilder sb = new StringBuilder(input.Length * 3);
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", value);
                sb.Append(Convert.ToString(value, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return sb.ToString().ToUpper();
        }
    }
}
