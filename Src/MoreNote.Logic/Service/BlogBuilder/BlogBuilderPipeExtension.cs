namespace MoreNote.Logic.Service.BlogBuilder
{
	public static class BlogBuilderPipeExtension
	{
		/// <summary>
		/// 调用管道代码处理数据，并吐出数据
		/// </summary>
		/// <param name="data"></param>
		/// <param name="blogBuilderPipe"></param>
		/// <returns></returns>
		public static string BlogBuilderPipe(this string data, BlogBuilderPipeInterface blogBuilderPipe)
		{
			return blogBuilderPipe.BlogBuilderDataProcessing(data);
		}
	}
}

