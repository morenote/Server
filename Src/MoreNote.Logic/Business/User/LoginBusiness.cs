using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



namespace MoreNote.Logic.Business.User
{
    public class LoginBusiness
    {
        public  int GoLogin(string email, string passwd)
        {
            return 0;
        }

        public  bool CanLogin(string email, string passwd)
        {


            return true;
        }
        public  bool CanLogin(IRequestCookieCollection cookies)
        {
            return true;
        }
    }
}
