using MoreNote.Models.Entity.Leanote.Management.Loggin;

using System;
using System.Collections.Generic;

namespace MoreNote.Logic.Service.Logging
{
	public interface ILoggingService
	{

		public void Error(string message, Exception exception);
		public void Info(string message);
		public void Debug(string message);
		public void Warn(string message);


		public void Save(LoggingLogin loggingLogin);

		public List<LoggingLogin> GetAllUserLoggingLogin();
	}
}
