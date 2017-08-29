using System;
using Xamarin.Forms;
using System.Threading.Tasks;
[assembly: Xamarin.Forms.Dependency(typeof(nWorksLeaveApp.Droid.comingIn_renderer))]

namespace nWorksLeaveApp.Droid
{
	public class comingIn_renderer:IScanner
	{
		#region IScanner implementation
		async public Task<string> Scan ()
		{
			//NOTE: On Android you MUST pass a Context into the Constructor!

			var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			var result = await scanner.Scan();
			return result.Text;

			//if (result != null)
			//Console.WriteLine("Scanned Barcode: " + result.Text);

		}
		#endregion
	}
}
