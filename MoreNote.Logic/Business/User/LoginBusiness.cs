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
        public static int GoLogin(string email, string passwd)
        {
            return 0;
        }

        public static bool CanLogin(string email, string passwd)
        {


            return true;
        }
        public static bool CanLogin(IRequestCookieCollection cookies)
        {
            return true;
        }
    }
}
