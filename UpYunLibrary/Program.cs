using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System.Reflection;

namespace UpYunLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            UpYun upyun = new UpYun("bucket", "username", "password");

            /*
            /// 切换 API 接口的域名
            /// {默认 v0.api.upyun.com 自动识别, v1.api.upyun.com 电信, v2.api.upyun.com 联通, v3.api.upyun.com 移动}
            // upyun.setApiDomain('v0.api.upyun.com');

            /// 获取空间占用大小
            Console.WriteLine("获取空间占用大小");
            Console.WriteLine(upyun.getBucketUsage());

            /// 创建目录
            // Console.WriteLine(upyun.mkDir("/a/"));
            // 创建目录时可使用 upyun.mkDir("/a/b/c", true) //进行父级目录的自动创建（最深10级目录）
            */
            /// 上传文件
            Hashtable headers = new Hashtable();
            //uy.delete("tes\ttd.jpg", headers);
            FileStream fs = new FileStream("..\\..\\test.jpeg", FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            byte[] postArray = r.ReadBytes((int)fs.Length);

            /// 设置待上传文件的 Content-MD5 值（如又拍云服务端收到的文件MD5值与用户设置的不一致，将回报 406 Not Acceptable 错误）
            // upyun.setContentMD5(UpYun.md5_file("..\\..\\test.jpeg"));

            /// 设置待上传文件的 访问密钥（注意：仅支持图片空！，设置密钥后，无法根据原文件URL直接访问，需带 URL 后面加上 （缩略图间隔标志符+密钥） 进行访问）
            /// 如缩略图间隔标志符为 ! ，密钥为 bac，上传文件路径为 /folder/test.jpg ，那么该图片的对外访问地址为： http://空间域名/folder/test.jpg!bac
            // upyun.setFileSecret("bac");
            Console.WriteLine("上传文件");
            bool b = upyun.writeFile("/a/test.jpg", postArray, true);
            // 上传文件时可使用 upyun.writeFile("/a/test.jpg",postArray, true); //进行父级目录的自动创建（最深10级目录）
            Console.WriteLine(b);

            /// 获取上传后的图片信息（仅图片空间有返回数据）
            Console.WriteLine("获取上传后的图片信息");
            Console.WriteLine(upyun.getWritedFileInfo("x-upyun-width"));
            Console.WriteLine(upyun.getWritedFileInfo("x-upyun-height"));
            Console.WriteLine(upyun.getWritedFileInfo("x-upyun-frames"));
            Console.WriteLine(upyun.getWritedFileInfo("x-upyun-file-type"));

            /// 读取目录
            Console.WriteLine("读取目录");
            ArrayList str = upyun.readDir("/a/");
            foreach (var item in str)
            {
                FolderItem a = (FolderItem)item;
                Console.WriteLine(a.filename);
            }

            /// 获取某个目录的空间占用大小
            Console.WriteLine("获取某个目录的空间占用大小");
            Console.WriteLine(upyun.getFolderUsage("/a/"));

            /// 读取文件
            /// 请查阅代码中的 readFile 函数代码，又拍云存储最大文件限制 100Mb，对于普通用户可以改写该值，以减少内存消耗
            /// 或在此基础上改写成自己需要的形式

            /// 另外推荐通过web访问文件接口下载文件而非api接口
            Console.WriteLine("读取文件");
            byte[] contents = upyun.readFile("/a/test.jpg");
            Console.WriteLine(contents.Length);

            /// 获取文件信息 return Hashtable('type'=> file | folder, 'size'=> file size, 'date'=> unix time) 或 null
            Console.WriteLine("获取文件信息");
            Hashtable ht = upyun.getFileInfo("/a/test.jpg");
            Console.WriteLine(ht["type"]);
            Console.WriteLine(ht["size"]);
            Console.WriteLine(ht["date"]);
            /*
            /// 删除文件
            Console.WriteLine("删除文件");
            b = upyun.deleteFile("/a/test.jpg");
            Console.WriteLine(b);

            /// 删除目录（目录必须为空）
            Console.WriteLine("删除目录");
            b = upyun.rmDir("/a/");
            Console.WriteLine(b);*/

            Console.Read();
        }
    }


 

    
}
