using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
    public class ShareService
    {

        //-----------------------------------
        // 返回shareNotebooks, sharedUserInfos
        // info.ShareNotebooksByUser, []info.User

        // 总体来说, 这个方法比较麻烦, 速度未知. 以后按以下方案来缓存用户基础数据

        // 以后建个用户的基本数据表, 放所有notebook, sharedNotebook的缓存!!
        // 每更新一次则启动一个goroutine异步更新
        // 共享只能共享本notebook下的, 如果其子也要共享, 必须设置其子!!!
        // 那么, 父, 子在shareNotebooks表中都会有记录

        // 得到用户的所有*被*共享的Notebook
        // 1 得到别人共享给我的所有notebooks
        // 2 按parent进行层次化
        // 3 每个层按seq进行排序
        // 4 按用户分组
        // [ok]

        // 谁共享给了我的Query
        public static long getOrQ(long userId)
        {
            throw new Exception();
        }
        // 得到共享给我的笔记本和用户(谁共享给了我)
        public static User[] GetShareNotebooks(long userId)
        {
            throw new Exception();
        }
        // 排序
        public static void GetShareNotebooks(long userId,out object ShareNotebooksByUser,out User user)
        {
            throw new Exception();
        }
        // 排序
        public static SubNotebooks sortSubShareNotebooks(SubNotebooks subNotebooks)
        {
            throw new Exception();
        }
        // 将普通的notebooks添加perm及shareNotebook信息
     //todo 未完成



    }
}
