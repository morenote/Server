using MoreNote.Common.ExtensionMethods;

using System;
using System.ComponentModel;
using System.Globalization;

namespace MoreNote.Common.MyTypeConverter
{
	public class HexToLongTypeConverter : TypeConverter
	{
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				string hex = value as string;
				var number = hex.ToLongByHex();
				if (number != null)
				{
					return number;
				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}