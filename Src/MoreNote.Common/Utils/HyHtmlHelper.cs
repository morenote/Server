﻿using System.Text.RegularExpressions;

namespace MoreNote.Common.Utils
{
	public class HyHtmlHelper
	{
		/// <summary>
		/// 过滤HTML成普通字符串
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string Filter(string html)
		{
			html = html.Replace("<", "&lt;");
			html = Regex.Replace(html, "&lt;(?=(span|[/]span|p|[/]p|strong|[/]strong|em|[/]em|/br|br/))", "<");
			return html;
		}

		public static string InterceptHtml(string html)
		{
			string tmp = Filter(html);
			return tmp.Substring(0, 100);
		}


	}
}
