using System.IO;
using System.Runtime.InteropServices;

namespace MoreNote.Common.Utils
{
	public class RuntimeEnvironment
	{
		public static char DirectorySeparatorChar
		{
			get { return Path.DirectorySeparatorChar; }
		}

		public static bool Islinux
		{
			get { return RuntimeInformation.IsOSPlatform(OSPlatform.Linux); }
		}
		public static bool IsOSX
		{
			get { return RuntimeInformation.IsOSPlatform(OSPlatform.OSX); }
		}
		public static bool IsWindows
		{
			get { return RuntimeInformation.IsOSPlatform(OSPlatform.Windows); }
		}
	}
}
