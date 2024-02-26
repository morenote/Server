using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.IO;

namespace MoreNote.Common.Utils.Tests
{
	[TestClass()]
	public class HtmlToTextHelperTests
	{
		[TestMethod()]
		public void ConvertTest()
		{
			HtmlToTextHelper convert = new HtmlToTextHelper();
			string md_text = File.ReadAllText(@"html.txt");
			string html = "";
			using (var reader = new StringReader(md_text))
			{
				using (var writer = new StringWriter())
				{
					CommonMark.CommonMarkConverter.Convert(reader, writer);
					//writer.ToString()即为转换好的html
					html = writer.ToString();
				}
			}
			string text = convert.Convert(html);
			Console.WriteLine(text);


		}

		[TestMethod()]
		public void ConvertTest1()
		{
			Assert.Fail();
		}
	}
}