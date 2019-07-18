using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Cadi.UI.Converters
{
	public class BoolToValueConverter : IValueConverter
	{
		public object TrueValue { get; set; }
		public object FalseValue { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool boolVal)
			{
				return boolVal ? TrueValue : FalseValue;
			}
			return FalseValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
