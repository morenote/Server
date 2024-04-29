using Microsoft.AspNetCore.Http;

using MoreNote.Framework.Controllers;
using MoreNote.Logic.Service;

using System.Collections.Generic;

namespace MoreNote.Controllers.Admin
{
	/// <summary>
	/// 管理员页面基类
	/// </summary>
	public class AdminBaseController : BaseController
	{

		public AdminBaseController(AttachService attachService
			  , TokenSerivce tokenSerivce
			  , NoteFileService noteFileService
			  , UserService userService
			  , ConfigFileService configFileService
			  , IHttpContextAccessor accessor


			  ) : base(attachService, tokenSerivce, noteFileService, userService, configFileService, accessor)
		{
		}
		
		public string GetSorterSQL(string sorterField)
		{
			HashSet<string> hashSet = new HashSet<string> { "email", "username", "verified", "createdTime", "accountType" };
			string sql = "";
			if (string.IsNullOrEmpty(sorterField))
			{
				return sql;
			}
			var sorterFields = sorterField.Split("-");
			if (sorterFields.Length != 2)
			{
				return sql;
			}
			if (!hashSet.Contains(sorterFields[0]))
			{
				return sql;
			}
			var sqlSorterField = "user_id";
			switch (sorterFields[0])
			{
				case "email":
					sqlSorterField = "email";
					break;

				case "username":
					sqlSorterField = "username";
					break;

				case "verified":
					sqlSorterField = "verified";
					break;

				case "createdTime":
					sqlSorterField = "created_time";
					break;
				case "accountType":
					sqlSorterField = "account_type";
					break;
			}

			sql = $"order by {sqlSorterField}";
			if (sorterFields[1].Equals("down"))
			{
				sql = sql + " desc";
			}
			return sql;
		}
	}
}