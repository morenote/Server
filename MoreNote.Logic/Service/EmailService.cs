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


    }
}
