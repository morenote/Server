using System.IO;

namespace MoreNote.Common.Utils
{
	public class MyHtmlHelper
	{

		public static string SubHTMLToRaw(string html, int length)
		{
			if (html == null)
			{
				return "空白文章，没有笔记内容：SubHTMLToRaw=>null Exception";
			}
			if (string.IsNullOrEmpty(html))
			{
				return "空白文章，没有笔记内容：SubHTMLToRaw=>empty Exception";
			}
			//todo:需要完成函数MyHtmlHelper.SubStringHTMLToRaw
			HtmlToTextHelper convert = new HtmlToTextHelper();
			string text = convert.Convert(html);
			if (length < 0)
			{
				length = 200;
			}
			if (length > text.Length)
			{
				length = text.Length;
			}
			string result = text.Substring(0, length);
			return result;
		}
		public static string SubMarkDownToRaw(string markdown, int length)
		{
			if (string.IsNullOrEmpty(markdown))
			{
				return "SubMarkDownToRaw=>null Exception";
			}
			//todo:需要完成函数MyHtmlHelper.SubStringHTMLToRaw
			string html = "";
			HtmlToTextHelper convert = new HtmlToTextHelper();
			using (var reader = new StringReader(markdown))
			{
				using (var writer = new StringWriter())
				{
					CommonMark.CommonMarkConverter.Convert(reader, writer);
					//writer.ToString()即为转换好的html
					html = writer.ToString();
				}
			}
			string text = convert.Convert(html);
			if (length < 0)
			{
				length = 200;
			}
			if (length > text.Length)
			{
				length = text.Length;
			}
			string result = text.Substring(0, length);
			return result;
		}


	}
}
