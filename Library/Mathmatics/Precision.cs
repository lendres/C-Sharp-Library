using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DigitalProduction.Mathmatics
{
	/// <summary>
	/// Math utilities.
	/// </summary>
	public static class Precision
	{
		#region Members

		/// <summary>The epsilon, threshold using for determining whether two numbers are equal or not.</summary>
		private static double _epsilon = 1e-10;

		#endregion

		#region Properties

		/// <summary>
		/// The allowed threshold that are numbers are allowed to be different, but still considered equal.
		/// </summary>
		public static double Epsilon
		{
			get
			{
				return Precision._epsilon;
			}

			set
			{
				Precision._epsilon = value;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines if the input is zero within the allotted precision.
		/// </summary>
		/// <param name="val">The value to check.</param>
		/// <returns>True if the input is less than the default precision.</returns>
		public static bool IsZero(double val)
		{
			return IsZero(val, _epsilon);
		}

		/// <summary>
		/// Determines if the input is zero within the allotted precision.
		/// </summary>
		/// <param name="val">The value to check.</param>
		/// <param name="epsilon">The specified precision.</param>
		/// <returns>True if the input is less than the default precision.</returns>
		public static bool IsZero(double val, double epsilon)
		{
			if (System.Math.Abs(val) < epsilon)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Determines two numbers are equal within the default precision.
		/// </summary>
		/// <param name="val1">The first value.</param>
		/// <param name="val2">The second value.</param>
		/// <returns>True if val1 equals val2 within the allowed precision, false otherwise.</returns>
		public static bool Equals(double val1, double val2)
		{
			return Equals(val1, val2, _epsilon);
		}

		/// <summary>
		/// Determines two numbers are equal within the specified precision.
		/// </summary>
		/// <param name="val1">The first value.</param>
		/// <param name="val2">The second value.</param>
		/// <param name="epsilon">Precision required to consider the two value equal.</param>
		/// <returns>True if val1 equals val2 within the allowed precision, false otherwise.</returns>
		public static bool Equals(double val1, double val2, double epsilon)
		{
			double diff = System.Math.Abs(val1 - val2);
			return diff <= epsilon;
		}

		public static double CeilingWithPrecision(double val, int roundTo)
		{
			if (val % roundTo != 0)
			{
				return (val - val % roundTo) + roundTo;
			}
			else
			{
				return val;
			}
		}

		public static double FloorWithPrecision(double val, int roundTo)
		{
			if (val % roundTo != 0)
			{
				return val - val % roundTo;
			}
			else
			{
				return val;
			}
		}

		#endregion

	} // End class.
} // End namespace.