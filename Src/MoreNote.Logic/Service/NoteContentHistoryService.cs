using MoreNote.Models.Entity.Leanote.Notes;

using System;

namespace MoreNote.Logic.Service
{
	public class NoteContentHistoryService
	{
		// 每个历史记录最大值
		int maxSize = 10;
		// 新建一个note, 不需要添加历史记录
		// 添加历史
		public void AddHistory(long? noteId, long? userId, EachHistory eachHistory)
		{
			throw new Exception();
		}
		// 新建历史
		public void newHistory(long? noteId, long? userId, EachHistory eachHistory)
		{
			throw new Exception();
		}
		// 列表展示
		public EachHistory[] ListHistories(long? noteId, long? userId)
		{
			throw new Exception();

		}

	}
}
