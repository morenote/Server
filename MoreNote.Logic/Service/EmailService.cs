using MoreNote.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MoreNote.Logic.Service
{
    class EmailService: MailMessage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="FormAddress">发件人邮箱地址</param>
        /// <param name="ToAddress">收件人邮箱地址</param>
        /// <param name="Subject">邮件标题</param>
        /// <param name="Body">邮件内容</param>
        public EmailService(string FormAddress, string ToAddress, string Subject, string Body)
        {
            //发件人邮箱地址，方法重载不同，可以根据需求自行选择。
            this.From = new MailAddress(FormAddress);
            //收件人邮箱地址。
            this.To.Add(new MailAddress(ToAddress));
            //邮件标题。
            this.Subject = Subject;
            //邮件内容。
            this.Body = Body;
            //            //设置为html
            //            this.IsBodyHtml = true;
        }
        /// <summary>
        /// 发送邮件已经创建好的邮箱
        /// </summary>
        public void SendEmail()
        {

            //实例化一个SmtpClient类。
            using (SmtpClient client = new SmtpClient())
            {
                //在这里我使用的是qq邮箱，所以是smtp.qq.com，如果你使用的是126邮箱，那么就是smtp.126.com。
                client.Host = "smtp.qq.com";
                //使用安全加密连接。
                client.EnableSsl = true;
                //使用的端口
                client.Port = 465;
                //不和请求一块发送。
                client.UseDefaultCredentials = false;
                //验证发件人身份(发件人的邮箱，邮箱里的生成授权码);
                client.Credentials = new NetworkCredential("huanyinglike@qq.com", "ygJIJnve4fnu9gom");
                //发送
                client.Send(this);
            }
        }
        public void ToAddressAdd(string emailAddress)
        {
            this.To.Add(new MailAddress(emailAddress));
        }

        public static EmailService NewEmailService()
        {
            throw 
                new Exception();
        }
        // 发送邮件
        string host = "";
        string emailPort = "";
        string username = "";
        string password = "";
        bool ssl = false;
        public void InitEmailFromDb()
        {
            throw  new Exception();
        }

        //return a smtp client
        public static object dial(string addr)
        {
            throw new Exception();
        }
        public  bool SendEmailWithSSL()
        {
            throw  new Exception();
        }
        public bool  SendEmail(string to,string subject,string body)
        {
            throw new Exception();
        }
        public static bool RegisterSendActiveEmail(User userInfo,string email)
        {
            throw 
                new Exception();
        }
        public static bool UpdateEmailSendActiveEmail(User
             userInfo,string email)
        {
            throw 
                new Exception();
        }
        public bool FindPwdSendEmail(string token,string email)
        {
            throw new Exception();
        }
        public  bool SendInviteEmail(User
             userInfo,string email,string content)
        {
            throw 
                new Exception();
        }
        public bool SendCommentEmail(Note note,BlogComment comment ,long userId,string content)
        {
            throw new Exception();
        }
        public static  bool ValidTpl(string str)
        {
            throw 
                new Exception();
        }
        public bool getTpl(string str)
        {
            throw new Exception();
        }
        public static bool  renderEmail (string subject ,string body)
        {
            throw new Exception();
        }
        public static bool SendEmailToUsers(User user,string subject,string body)
        {
            throw new Exception();
        }
        public static bool SendEmailToEmails(string[] email,string subject,string body)
        {
            throw new Exception();
        }
        public static bool AddEmailLog(string email,string subject,string body,bool ok,string msg)
        {
            throw new Exception();
        }
        public static bool DeleteEmails(long[] ids)
        {
            throw new Exception();
        }
        public List<object> ListEmailLogs(int pageNUmber,int pageSize,string sortField,bool
             isAsc,string email)
        {
            throw new Exception();
        }



    }
}
