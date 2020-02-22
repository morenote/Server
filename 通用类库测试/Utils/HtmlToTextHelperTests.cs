using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MoreNote.Common.Utils;

namespace MoreNote.Common.Utils.Tests
{
    [TestClass()]
    public class HtmlToTextHelperTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            HtmlToTextHelper convert = new HtmlToTextHelper();
            string md_text = File.ReadAllText(@"C:\Users\WangXianQiang\Desktop\content.md");
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
    }
}