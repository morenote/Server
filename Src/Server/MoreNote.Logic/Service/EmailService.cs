using MoreNote.Models.Entity.Leanote.Blog;
using MoreNote.Models.Entity.Leanote.Notes;
using MoreNote.Models.Entity.Leanote.User;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MoreNote.Logic.Service
{
	//todo:邮件发送服务
	public class EmailService : MailMessage
	{
		// 发送邮件
		string host = "";
		string emailPort = "";
		string username = "";
		string password = "";
		public EmailService()
		{

		}
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
				//todo:添加保密措施，防止邮箱密码泄露
				client.Credentials = new NetworkCredential(username, password);
				//发送
				client.Send(this);
			}
		}
		public void ToAddressAdd(string emailAddress)
		{
			this.To.Add(new MailAddress(emailAddress));
		}

		public EmailService NewEmailService()
		{
			throw
				new Exception();
		}

		bool ssl = false;
		public void InitEmailFromDb()
		{
			throw new Exception();
		}

		//return a smtp client
		public object dial(string addr)
		{
			throw new Exception();
		}
		public bool SendEmailWithSSL()
		{
			throw new Exception();
		}
		public bool SendEmail(string to, string subject, string body)
		{
			throw new Exception();
		}
		public bool RegisterSendActiveEmail(UserInfo userInfo, string email)
		{
			//todo:
			return true;
		}
		public bool UpdateEmailSendActiveEmail(UserInfo
			 userInfo, string email)
		{
			throw
				new Exception();
		}
		public bool FindPwdSendEmail(string token, string email)
		{
			throw new Exception();
		}
		public bool SendInviteEmail(UserInfo
			 userInfo, string email, string content)
		{
			throw
				new Exception();
		}
		public bool SendCommentEmail(Note note, BlogComment comment, long? userId, string content)
		{
			throw new Exception();
		}
		public bool ValidTpl(string str)
		{
			throw
				new Exception();
		}
		public bool getTpl(string str)
		{
			throw new Exception();
		}
		public bool renderEmail(string subject, string body)
		{
			throw new Exception();
		}
		public bool SendEmailToUsers(UserInfo user, string subject, string body)
		{
			throw new Exception();
		}
		public bool SendEmailToEmails(string[] email, string subject, string body)
		{
			throw new Exception();
		}
		public bool AddEmailLog(string email, string subject, string body, bool ok, string msg)
		{
			throw new Exception();
		}
		public bool DeleteEmails(long[] ids)
		{
			throw new Exception();
		}
		public List<object> ListEmailLogs(int pageNUmber, int pageSize, string sortField, bool
			 isAsc, string email)
		{
			throw new Exception();
		}



	}
}
