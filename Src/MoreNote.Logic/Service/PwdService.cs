using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoreNote.Common.entity;
using MoreNote.Logic.Database;
using MoreNote.Logic.Entity;
   using Microsoft.Extensions.DependencyInjection;

namespace MoreNote.Logic.Service
{
    
    public class PwdService
    {

        DataContext dataContext;
        public PwdService(DataContext dataContext)
        {
            this.dataContext = dataContext;
          
        }

        // 找回密码
        // 修改密码
       public int  overHours = 2; // 小时后过期
       public  int overSecond= 7200;//秒后过期 适用于unix时间戳
        // 1. 找回密码, 通过email找用户,
        // 用户存在, 生成code
        public  bool FindPwd(string email)
        {
        
		     var userid = dataContext.User
                    .Where(b => b.Email.Equals(email)).First();
                if (userid == null)
                {
                    return false;
                }
                return true;
	
        }
        // 重置密码时
        // 修改密码
        // 先验证
        public  bool UpdatePwd(string token,string pwd)
        {
           throw new Exception();
        }
    }
}
