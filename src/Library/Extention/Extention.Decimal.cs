using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Extention
{
	public static partial class Extention
	{

		/// <summary>
		/// decimal转bool
		/// </summary>
		/// <param name="value">值</param>
		/// <returns></returns>
		public static bool ToBool(this decimal value)
		{
			if (value == 0)
				return false;
			if (value == 1)
				return true;
			return Convert.ToBoolean(value);
		}
	}
}
