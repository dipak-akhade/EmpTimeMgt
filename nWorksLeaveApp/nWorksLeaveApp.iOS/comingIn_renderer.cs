using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using nWorksLeaveApp.iOS;
[assembly:Xamarin.Forms.Dependency(typeof(nWorksLeaveApp.iOS.comingIn_renderer))]

namespace nWorksLeaveApp.iOS
{
	public class comingIn_renderer:IScanner
	{
		#region IScanner implementation
		async public Task<string> Scan ()
		{
			//NOTE: On Android you MUST pass a Context into the Constructor!
			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			var result = await scanner.Scan();

			//if (result != null)
			//Console.WriteLine("Scanned Barcode: " + result.Text);

			return result.Text;
		}

		#endregion
	}
}
	