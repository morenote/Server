using MoreNote.Common.ExtensionMethods;
using MoreNote.Logic.Service.DistributedIDGenerator;

using System;

namespace MoreNote.Logic.Service
{
	public class InitServices
	{
		private IDistributedIdGenerator idGenerator;
		// onAppStart调用
		public void InitService(IDistributedIdGenerator idGenerator)
		{
			this.idGenerator = idGenerator;
		}
		//----------------
		// service 公用方法

		// 将name=val的val进行encoding
		public string decodeValue(string val)
		{
			throw new Exception();
		}
		public string encodeValue(string val)
		{
			throw new Exception();
		}

		// 添加笔记时通过title得到urlTitle
		public string fixUrlTitle(string urlTitle)
		{
			return idGenerator.NextHexId();
		}
		public string getUniqueUrlTitle(long? userId, string urlTitle, string types, int padding)
		{
			// 判断urlTitle是不是过长, 过长则截断, 300
			// 不然生成index有问题
			// it will not index a single field with more than 1024 bytes.
			// If you're indexing a field that is 2.5MB, it's not really indexing it, it's being skipped.

			return urlTitle;
		}
		// 截取id 24位变成12位
		// 先md5, 再取12位
		public string subIdHalf(long? id)
		{
			return id.ToHex();
		}
		// types == note,notebook,single
		// id noteId, notebookId, singleId 当title没的时候才有用, 用它来替换

		public string GetUrTitle(long? userId, string title, string type, long? id)
		{
			string urlTitle = title.Trim();
			if (string.IsNullOrEmpty(urlTitle))
			{
				urlTitle = "Untitled-" + userId.ToHex();

			}
			else
			{
				urlTitle = subIdHalf(id);
			}
			urlTitle = fixUrlTitle(urlTitle);

			return getUniqueUrlTitle(userId, urlTitle, type, 1);
		}
	}
}
