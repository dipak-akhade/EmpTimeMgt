using System;
using Foundation;
using System.Runtime.InteropServices;

[assembly:Xamarin.Forms.Dependency(typeof(nWorksLeaveApp.iOS.GetSimNumber))]

namespace nWorksLeaveApp.iOS
{
	public class GetSimNumber:IGetSimNumber
	{
		[DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
		private static extern uint IOServiceGetMatchingService(uint masterPort, IntPtr matching);

		[DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
		private static extern IntPtr IOServiceMatching(string s);

		[DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
		private static extern IntPtr IORegistryEntryCreateCFProperty(uint entry, IntPtr key, IntPtr allocator, uint options);

		[DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
		private static extern int IOObjectRelease(uint o);
		public string GetPlatformSimNumber ()
		{
			string serial = string.Empty;
			uint platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
			if (platformExpert != 0)
			{
				NSString key = (NSString) "IOPlatformSerialNumber";
				IntPtr serialNumber = IORegistryEntryCreateCFProperty(platformExpert, key.Handle, IntPtr.Zero, 0);
				if (serialNumber != IntPtr.Zero)
				{
					serial = NSString.FromHandle(serialNumber);
				}

				IOObjectRelease(platformExpert);
			}

			return serial;
		}
	}
}

