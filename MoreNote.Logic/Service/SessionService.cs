using MoreNote.Common.Utils;
using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
    public  class SessionService
    {

        // Session存储到mongodb中
        // morenote的理想储存容器是redis
        public static bool Update(long  sessionId,string value)
        {
            
            throw new Exception();
        }
        // 注销时清空session
        public static bool Clear(long sessionId)
        {
            throw new Exception();
        }
        public static Session Get(long sessionId)
        {
            throw new Exception();
        }
        //------------------
        // 错误次数处理

        // 登录错误时间是否已超过了
        public static bool LoginTimesIsOver(long sessionId)
        {
            throw new Exception();
        }
        // 登录成功后清空错误次数
        public static bool ClearLoginTimes(long sessionId)
        {
            throw new Exception();
        }
        // 增加错误次数
        public static bool IncrLoginTimes(long sessionId)
        {
            throw new Exception();
        }
        //----------
        // 验证码
        public static string GetCaptcha(long GetCaptcha)
        {
            throw new Exception();
        }
        public static bool SetCaptcha(long sessionId,string captcha)
        {
            throw new Exception();
        }
        //-----------
        // API
        public static long GetUserId(long sessionId)
        {
            throw new Exception();
        }
        // 登录成功后设置userId

        public static bool SetUserId(long sessionId,long userId)
        {
            throw new Exception();
        }
    }
}
