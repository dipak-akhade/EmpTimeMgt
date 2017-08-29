using System;
using Xamarin.Forms;

namespace nWorksLeaveApp.Common
{
	public class ColorResources
	{
		public static readonly Color ListTextColor = Device.OnPlatform(Color.Black, Color.Black, Color.Black);
		public static readonly Color PageBackgroundColor = Color.FromHex("#000000");
		public static readonly Color commonButtonBackgroundColor = Color.FromHex("#ff0000");
		public static readonly Color btnTextColor = Color.White;
		public static readonly Color TextColor = Color.White;
		public static readonly Color tileTextColor = Color.Black;
		public static readonly Color btnBackgroundColor = Color.FromHex("#ed1f29");
		public static readonly Color listTextColor = Color.FromHex("#000000");
		public static readonly Color listBackgroundColor = Color.FromHex("#ffffff");
		public static readonly Color entryBackgroundColor = Color.White;
		public static readonly Color entryTextColor = Color.Black;

		public static readonly int headerFontSize=24;
		public static readonly int subHeaderFontSize=20;
		public static readonly int textFontSize=18;

		public static string LIVE_USER_ID{ get; set;}
		public static string LIVE_USER_TYPE{ get; set;}

		public static bool Already_In{ get; set;}
		public static bool Already_Out{ get; set;}

		public static bool _isBusy{ get; set;}

		public static bool isLeaveConfirm{ get; set;}

		public static string baseUrl= "http://localhost/MyTestWebService/api/";
		//public static string baseUrl= "http://192.168.0.106/MyTestWebService/api/";
        //public static string baseUrl="http://app-leavesapp-test.nworks.co/ws/api/";
        //public static string baseUrl = "http://192.168.1.101/webservice/api/";
        //public static string baseUrl="http://app-leavesapp-dev.nworks.co/webservice/api/";
    }
}