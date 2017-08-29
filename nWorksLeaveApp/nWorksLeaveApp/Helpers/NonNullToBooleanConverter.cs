using System;
using Xamarin.Forms;
using System.Globalization;

namespace nWorksLeaveApp
{
	public class NonNullToBooleanConverter:IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string) {
				return !string.IsNullOrEmpty ((string)value);
			}

			return value != null;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}

