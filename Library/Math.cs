using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalProduction
{
	/// <summary>
	/// Math functions, equations, and calculations.
	/// </summary>
	public static class Math
	{
		#region Enumerations

		#endregion

		#region Delegates

		#endregion

		#region Members

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

		/// <summary>
		/// Area of a ring (circle with inner concentric circle subtracted).
		/// </summary>
		/// <param name="outerdiameter">Diameter of outer circle.</param>
		/// <param name="innerdiameter">Diameter of inner circle.</param>
		/// <returns>Area.</returns>
		public static double AreaOfCircleByDiameter(double outerdiameter, double innerdiameter)
		{
			return System.Math.PI / 4 * (System.Math.Pow(outerdiameter, 2) - System.Math.Pow(innerdiameter, 2));
		}

		#endregion

		#region XML

		#endregion

	} // End class.
} // End namespace.