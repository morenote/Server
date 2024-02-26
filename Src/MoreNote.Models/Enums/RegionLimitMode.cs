using System.ComponentModel;

namespace MoreNote.Logic.Models.Enum
{
	/// <summary>
	/// 地区限制
	/// </summary>
	public enum RegionLimitMode
	{
		[Description("不限")]
		All,
		[Description("指定地区可见：{0}")]
		AllowRegion,
		[Description("指定地区不可见：{0}")]
		ForbidRegion,
		[Description("可见地区：{0}，排除地区：{1}")]
		AllowRegionExceptForbidRegion,
		[Description("不可见地区：{0}，排除地区：{1}")]
		ForbidRegionExceptAllowRegion
	}
}
