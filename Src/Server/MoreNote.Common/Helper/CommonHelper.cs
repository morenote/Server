using AngleSharp;
using AngleSharp.Dom;

using IP2Region;

using Masuit.Tools;

using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Model;
using MaxMind.GeoIP2.Responses;

using Polly;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TimeZoneConverter;

namespace Masuit.MyBlogs.Core.Common
{
	/// <summary>
	/// 公共类库
	/// </summary>
	public static class CommonHelper
	{
		private static readonly FileSystemWatcher FileSystemWatcher = new(AppContext.BaseDirectory + "App_Data", "*.txt")
		{
			IncludeSubdirectories = true,
			EnableRaisingEvents = true,
			NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size
		};

		static CommonHelper()
		{
			Init();
			FileSystemWatcher.Changed += (_, _) => Init();
		}

		private static void Init()
		{
			string ReadFile(string s)
			{
				return Policy<string>.Handle<IOException>().WaitAndRetry(5, i => TimeSpan.FromSeconds(i)).Execute(() =>
				{
					using var fs = File.Open(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					using var sr = new StreamReader(fs, Encoding.UTF8);
					return sr.ReadToEnd();
				});
			}

			BanRegex = ReadFile(Path.Combine(AppContext.BaseDirectory + "App_Data", "ban.txt"));
			ModRegex = ReadFile(Path.Combine(AppContext.BaseDirectory + "App_Data", "mod.txt"));
			DenyIP = ReadFile(Path.Combine(AppContext.BaseDirectory + "App_Data", "denyip.txt"));
			var lines = File.Open(Path.Combine(AppContext.BaseDirectory + "App_Data", "DenyIPRange.txt"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite).ReadAllLines(Encoding.UTF8);
			DenyIPRange = new Dictionary<string, string>();
			foreach (var line in lines)
			{
				try
				{
					var strs = line.Split(' ');
					DenyIPRange[strs[0]] = strs[1];
				}
				catch (IndexOutOfRangeException)
				{
				}
			}

			IPWhiteList = ReadFile(Path.Combine(AppContext.BaseDirectory + "App_Data", "whitelist.txt")).Split(',', '，').ToList();
		}

		/// <summary>
		/// 敏感词
		/// </summary>
		public static string BanRegex { get; set; }

		/// <summary>
		/// 审核词
		/// </summary>
		public static string ModRegex { get; set; }

		/// <summary>
		/// 全局禁止IP
		/// </summary>
		public static string DenyIP { get; set; }

		/// <summary>
		/// ip白名单
		/// </summary>
		public static List<string> IPWhiteList { get; set; }

		/// <summary>
		/// 每IP错误的次数统计
		/// </summary>
		public static ConcurrentDictionary<string, int> IPErrorTimes { get; set; } = new();

		/// <summary>
		/// 系统设定
		/// </summary>
		//public static ConcurrentDictionary<string, string> SystemSettings { get; set; } = new ConcurrentDictionary<string, string>();

		/// <summary>
		/// IP黑名单地址段
		/// </summary>
		public static Dictionary<string, string> DenyIPRange { get; set; }

		/// <summary>
		/// 判断IP地址是否被黑名单
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public static bool IsDenyIpAddress(this string ip)
		{
			if (IPWhiteList.Contains(ip))
			{
				return false;
			}

			return DenyIP.Contains(ip) || DenyIPRange.AsParallel().Any(kv => kv.Key.StartsWith(ip.Split('.')[0]) && ip.IpAddressInRange(kv.Key, kv.Value));
		}


		private static readonly DbSearcher IPSearcher = new(Path.Combine(AppContext.BaseDirectory + "App_Data", "ip2region.db"));
		public static readonly DatabaseReader MaxmindReader = new(Path.Combine(AppContext.BaseDirectory + "App_Data", "GeoLite2-City.mmdb"));
		private static readonly DatabaseReader MaxmindAsnReader = new(Path.Combine(AppContext.BaseDirectory + "App_Data", "GeoLite2-ASN.mmdb"));

		/// <summary>
		/// 是否是代理ip
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static async Task<bool> IsProxy(this IPAddress ip, HttpClient httpClient, CancellationToken cancellationToken = default)
		{
			// var httpClient = Startup.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
			httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.107 Safari/537.36 Edg/92.0.902.62");
			return await httpClient.GetStringAsync("https://ipinfo.io/" + ip, cancellationToken).ContinueWith(t =>
			{
				if (t.IsCompletedSuccessfully)
				{
					var ctx = BrowsingContext.New(Configuration.Default);
					var doc = ctx.OpenAsync(res => res.Content(t.Result)).Result;
					var isAnycast = doc.DocumentElement.QuerySelectorAll(".title").Where(e => e.TextContent.Contains("Anycast")).Select(e => e.Parent).Any(n => n.TextContent.Contains("True"));
					var isproxy = doc.DocumentElement.QuerySelectorAll("#block-privacy img").Any(e => e.OuterHtml.Contains("right"));
					return isAnycast || isproxy;
				}
				return false;
			});
		}

		public static AsnResponse GetIPAsn(this IPAddress ip)
		{
			if (ip.IsPrivateIP())
			{
				return new AsnResponse();
			}

			return Policy<AsnResponse>.Handle<AddressNotFoundException>().Fallback(new AsnResponse()).Execute(() => MaxmindAsnReader.Asn(ip));
		}

		private static CityResponse GetCityResp(IPAddress ip)
		{
			return Policy<CityResponse>.Handle<AddressNotFoundException>().Fallback(new CityResponse()).Execute(() => MaxmindReader.City(ip));
		}

		public static string GetIPLocation(this string ips)
		{
			return GetIPLocation(IPAddress.Parse(ips));
		}

		public static IPLocation GetIPLocation(this IPAddress ip)
		{
			if (ip.IsPrivateIP())
			{
				return new IPLocation("内网", null, null, "内网IP", null);
			}

			var city = GetCityResp(ip);
			var asn = GetIPAsn(ip);
			var countryName = city.Country.Names.GetValueOrDefault("zh-CN") ?? city.Country.Name;
			var cityName = city.City.Names.GetValueOrDefault("zh-CN") ?? city.City.Name;
			switch (ip.AddressFamily)
			{
				case AddressFamily.InterNetworkV6 when ip.IsIPv4MappedToIPv6:
					ip = ip.MapToIPv4();
					goto case AddressFamily.InterNetwork;
				case AddressFamily.InterNetwork:
					var parts = IPSearcher.MemorySearch(ip.ToString())?.Region.Split('|');
					if (parts != null)
					{
						var network = parts[^1] == "0" ? asn.AutonomousSystemOrganization : parts[^1] + "/" + asn.AutonomousSystemOrganization;
						parts[0] = parts[0] != "0" ? parts[0] : countryName;
						parts[3] = parts[3] != "0" ? parts[3] : cityName;
						return new IPLocation(parts[0], parts[2], parts[3], network?.Trim('/'), asn.AutonomousSystemNumber)
						{
							Address2 = countryName + cityName,
							Coodinate = city.Location
						};
					}

					goto default;
				default:
					return new IPLocation(countryName, null, cityName, asn.AutonomousSystemOrganization, asn.AutonomousSystemNumber)
					{
						Coodinate = city.Location
					};
			}
		}

		/// <summary>
		/// 获取ip所在时区
		/// </summary>
		/// <param name="ip"></param>
		/// <returns></returns>
		public static string GetClientTimeZone(this IPAddress ip)
		{
			if (ip.IsPrivateIP())
			{
				return "Asia/Shanghai";
			}

			return GetCityResp(ip).Location.TimeZone ?? "Asia/Shanghai";
		}




		/// <summary>
		/// 清理html的img标签的除src之外的其他属性
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static async Task<string> ClearImgAttributes(this string html)
		{
			var context = BrowsingContext.New(Configuration.Default);
			var doc = await context.OpenAsync(req => req.Content(html));
			var nodes = doc.DocumentElement.GetElementsByTagName("img");
			var allows = new[] { "src", "data-original", "width", "style", "class" };
			foreach (var node in nodes)
			{
				for (var i = 0; i < node.Attributes.Length; i++)
				{
					if (allows.Contains(node.Attributes[i].Name))
					{
						continue;
					}

					node.RemoveAttribute(node.Attributes[i].Name);
				}
			}

			return doc.Body.InnerHtml;
		}



		/// <summary>
		/// 获取文章摘要
		/// </summary>
		/// <param name="html"></param>
		/// <param name="length">截取长度</param>
		/// <param name="min">摘要最少字数</param>
		/// <returns></returns>
		public static Task<string> GetSummary(this string html, int length = 150, int min = 10)
		{
			var context = BrowsingContext.New(Configuration.Default);
			return context.OpenAsync(req => req.Content(html)).ContinueWith(t =>
			{
				var summary = t.Result.DocumentElement.GetElementsByTagName("p").FirstOrDefault(n => n.TextContent.Length > min)?.TextContent ?? "没有摘要";
				return summary.Length > length ? summary[..length] + "..." : summary;
			});
		}

		public static string TrimQuery(this string path)
		{
			return path.Split('&').Where(s => s.Split('=', StringSplitOptions.RemoveEmptyEntries).Length == 2).Join("&");
		}


		/// <summary>
		/// 转换时区
		/// </summary>
		/// <param name="time">UTC时间</param>
		/// <param name="zone">时区id</param>
		/// <returns></returns>
		public static DateTime ToTimeZone(this in DateTime time, string zone)
		{
			return TimeZoneInfo.ConvertTime(time, TZConvert.GetTimeZoneInfo(zone));
		}

		/// <summary>
		/// 转换时区
		/// </summary>
		/// <param name="time">UTC时间</param>
		/// <param name="zone">时区id</param>
		/// <param name="format">时间格式字符串</param>
		/// <returns></returns>
		public static string ToTimeZoneF(this in DateTime time, string zone, string format = "yyyy-MM-dd HH:mm:ss")
		{
			return ToTimeZone(time, zone).ToString(format);
		}


	}

	public class IPLocation
	{
		public IPLocation(string country, string province, string city, string isp, long? asn)
		{
			Country = country?.Trim('0');
			Province = province?.Trim('0');
			City = city?.Trim('0');
			ISP = isp;
			ASN = asn;
		}

		public string Country { get; set; }
		public string Province { get; set; }
		public string City { get; set; }
		public string ISP { get; set; }
		public long? ASN { get; set; }
		public string Address => new[] { Country, Province, City }.Where(s => !string.IsNullOrEmpty(s)).Distinct().Join("");
		public string Address2 { get; set; }
		public string Network => ASN.HasValue ? ISP + "(AS" + ASN + ")" : ISP;
		public Location Coodinate { get; set; }

		public override string ToString()
		{
			string address = Address;
			string network = Network;
			if (string.IsNullOrWhiteSpace(address))
			{
				address = "未知地区";
			}

			if (string.IsNullOrWhiteSpace(network))
			{
				network = "未知网络";
			}

			return new[] { address, Address2, network }.Where(s => !string.IsNullOrEmpty(s)).Distinct().Join("|");
		}

		public static implicit operator string(IPLocation entry)
		{
			return entry.ToString();
		}

		public void Deconstruct(out string location, out string network, out string info)
		{
			location = new[] { Address, Address2 }.Where(s => !string.IsNullOrEmpty(s)).Distinct().Join("|");
			network = Network;
			info = ToString();
		}
	}
}