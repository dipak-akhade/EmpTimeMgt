using System;
using Android.Telephony;
using Android.Content;
using Android.App;



[assembly:Xamarin.Forms.Dependency(typeof(nWorksLeaveApp.Droid.GetSimNumber))]
namespace nWorksLeaveApp.Droid
{
	public class GetSimNumber:IGetSimNumber
	{
		public string GetPlatformSimNumber ()
		{
			//Getting phone number
			TelephonyManager mgr = Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
			return mgr.DeviceId.ToString();

			//getting IMEI number/device id
//			return Android.Telephony.TelephonyManager.
		}
	}
}

