using System.IO;

namespace MoreNote.Common.Utils
{
	public class Loger
	{
		public static void WriteLog(string log)
		{
			var fileName = "test.txt";

			//            using (StreamWriter writer = new StreamWriter(fileName))
			//            {
			//                //1,写入文本
			//                writer.Write(log);
			//            }
			//2,追加文本
			var sw = File.AppendText(fileName);
			// sw.Write(textToAdd);//不换行
			sw.WriteLine(log);//自动换行
			sw.Close();

		}
		public static string GotLog()
		{
			var fileName = "test.txt";

			//            using (StreamWriter writer = new StreamWriter(fileName))
			//            {
			//                //1,写入文本
			//                writer.Write(log);
			//            }
			//2,追加文本
			var sw = File.ReadAllText(fileName);
			// sw.Write(textToAdd);//不换行
			return sw;

		}
	}
}
