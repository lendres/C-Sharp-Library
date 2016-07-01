using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DigitalProduction.Mathmatics
{
	/// <summary>
	/// 
	/// </summary>
	public static class Conversion
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		static Conversion()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Convert degrees to radians.
		/// </summary>
		/// <param name="angle">Angle to convert.</param>
		/// <returns>Angle in radians.</returns>
		public static double DegreeToRadian(double angle)
		{
			return System.Math.PI * angle / 180.0;
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angle">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static double RadianToDegree(double angle)
		{
			return angle * (180.0 / System.Math.PI);
		}

		#endregion

	} // End class.
} // End namespace.