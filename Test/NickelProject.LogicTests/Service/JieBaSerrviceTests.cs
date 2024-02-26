using JiebaNet.Segmenter;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace MoreNote.LogicTests.Service
{
	[TestClass()]
	public class JieBaSerrviceTests
	{
		[TestMethod()]
		public void JiebaSegmenterTest()
		{
			var segmenter = new JiebaSegmenter();
			string message = "多线程";
			var segments = segmenter.Cut(message, cutAll: true);
			Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

			segments = segmenter.Cut(message);  // 默认为精确模式
			Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

			segments = segmenter.Cut(message, hmm: true);  // 默认为精确模式，同时也使用HMM模型
			Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

			segments = segmenter.CutForSearch(message); // 搜索引擎模式
			Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));

			segments = segmenter.Cut(message);
			Console.WriteLine("【歧义消除】：{0}", string.Join("/ ", segments));


		}
	}
}
