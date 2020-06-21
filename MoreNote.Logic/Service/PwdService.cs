using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Common.entity;
using MoreNote.Logic.DB;
using MoreNote.Logic.Entity;

namespace MoreNote.Logic.Service
{
    public class PwdService
    {
        // 找回密码
        // 修改密码
        int  overHours = 2; // 小时后过期
        int overSecond= 7200;//秒后过期 适用于unix时间戳
        // 1. 找回密码, 通过email找用户,
        // 用户存在, 生成code
        public static bool FindPwd(string email)
        {

            using (var db = DataContext.getDataContext())
            {
                var userid = db.User
                    .Where(b => b.Email.Equals(email)).First();
                if (userid == null)
                {
                    return false;
                }
                return true;
            }
        }
        // 重置密码时
        // 修改密码
        // 先验证
        public static bool UpdatePwd(string token,string pwd)
        {
           throw new Exception();
        }
    }
}
