using Microsoft.AspNetCore.Http;
using MoreNote.Common.Utils;
using MoreNote.Models.Entity.Leanote.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreNote.Logic.Service
{
    public  class SessionService
    {

        // Session存储到mongodb中
        // morenote的理想储存容器是redis
        public  bool Update(long?  sessionId,string value)
        {
            
            throw new Exception();
        }
        // 注销时清空session
        public  bool Clear(long? sessionId)
        {
            throw new Exception();
        }
        public  Session Get(long? sessionId)
        {
            throw new Exception();
        }
        //------------------
        // 错误次数处理

        // 登录错误时间是否已超过了
        public  bool LoginTimesIsOver(long? sessionId)
        {
            throw new Exception();
        }
        // 登录成功后清空错误次数
        public  bool ClearLoginTimes(long? sessionId)
        {
            throw new Exception();
        }
        // 增加错误次数
        public  bool IncrLoginTimes(long? sessionId)
        {
            throw new Exception();
        }
        //----------
        // 验证码
        public  string GetCaptcha(long? GetCaptcha)
        {
            throw new Exception();
        }
        public  bool SetCaptcha(long? sessionId,string captcha)
        {
            throw new Exception();
        }
        //-----------
        // API
        public  long? GetUserId(long? sessionId)
        {
            throw new Exception();
        }
        // 登录成功后设置userId
        public  bool SetUserId(long? sessionId,long? userId)
        {
            throw new Exception();
        }
     

    }
}
