using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreNote.Logic.DB;
using System.IO;
using System.Linq;
using System.Net;

namespace UpyunTest
{
    [TestClass]
    public class DownloadFormUpuyn
    {
        [TestMethod]
        public void DownFile()
        {
            //using (var db = new DataContext())
            //{
            //    var files = db.NoteFile.ToArray();
            //    int count=0;
            //    foreach (var file in files)
            //    {
            //        count++;
            //        System.Console.WriteLine($"正在下载：{count}");

            //        var url = $"https://upyun.morenote.top//{file.Path}";
            //        url = url.Replace("\\", "/");
            //        string temp = file.Path;
            //        temp = temp.Replace("/", "\\");

            //        var path = $@"C:\下载\file\{temp}";
            //        var dir = Path.GetDirectoryName(path);
            //        if (!Directory.Exists(dir))
            //        {
            //            Directory.CreateDirectory(dir);
            //        }
            //        if (!File.Exists(path))
            //        {
            //            HttpDownloadFile(url, path);
            //        }
                   
            //    }
            //}
        }
         

        public static string HttpDownloadFile(string url, string path)

        {
            try
            {
                // 设置参数

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                //直到request.GetResponse()程序才开始向目标网页发送Post请求

                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流

                Stream stream = new FileStream(path, FileMode.Create);
                byte[] bArr = new byte[1024];

                int size = responseStream.Read(bArr, 0, (int)bArr.Length);

                while (size > 0)
                {
                    stream.Write(bArr, 0, size);

                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }

                stream.Close();

                responseStream.Close();

                return path;
            }
            catch (System.Exception ex)
            {

                return null;
            }
            
        }
    }
}