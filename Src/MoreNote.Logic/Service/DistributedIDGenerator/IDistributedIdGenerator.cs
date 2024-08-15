namespace MoreNote.Logic.Service.DistributedIDGenerator
{
	/// <summary>
	///  分布式id生成器
	/// </summary>
	public interface IDistributedIdGenerator
	{
		public long NextId();
		public string NextHexId();

	}
}
