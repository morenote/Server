using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
   public  class InitServices
    {
        // onAppStart调用
        public static void InitService()
        {
            throw new Exception();
        }
        //----------------
        // service 公用方法

        // 将name=val的val进行encoding
        public static string decodeValue(string val)
        {
            throw new Exception();
        }
        public static string encodeValue(string val)
        {
            throw new Exception();
        }

        // 添加笔记时通过title得到urlTitle
        public static string fixUrlTitle(string urlTitle)
        {
            throw new Exception();
        }
        public static string getUniqueUrlTitle(long userId,string urlTitle,string types,int padding)
        {
            throw new Exception();
        }
        // 截取id 24位变成12位
        // 先md5, 再取12位
        public static string subIdHalf(long id)
        {
            throw new Exception();
        }
        // types == note,notebook,single
        // id noteId, notebookId, singleId 当title没的时候才有用, 用它来替换
        public static string GetUrTitle(long userId,string title,string type,long id)
        {
            throw new Exception();
        }
    }
}
