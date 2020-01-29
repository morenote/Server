using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using Snowflake.Core;

namespace MoreNote.Common.Utils
{
    public class SnowFlake_Net
    {
        private static IdWorker worker = null;
       
       // 定义一个标识确保线程同步
        private static readonly object locker = new object();
        // 定义私有构造函数，使外界不能创建该类实例
        private SnowFlake_Net()
        {
            if (worker == null)
            {
                worker = new IdWorker(1, 1);
            }
        }
        public static IdWorker GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (worker == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (worker == null)
                    {
                        worker = new IdWorker(1, 1);
                    }
                }
            }
            return worker;
        }
        /// <summary>
        /// 产生全局唯一的long类型ID
        /// </summary>
        /// <returns></returns>
        public static  long GenerateSnowFlakeID()
        {
         
            return   GetInstance().NextId();
        }
        /// <summary>
        /// 生成全局唯一的hex字符串
        /// </summary>
        /// <returns></returns>
        public static string GenerateSnowFlakeIDHex()
        {
            return GenerateSnowFlakeID().ToString("x");
        }
    }
}
