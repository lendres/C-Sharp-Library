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
			return angle * System.Math.PI / 180.0;
		}

		/// <summary>
		/// Convert degrees to radians.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in radians.</returns>
		public static List<double> DegreeToRadian(List<double> angles)
		{
			int				count		= angles.Count;
			List<double>	output		= new List<double>(count);

			for (int i = 0; i < count; i++)
			{
				output.Add(angles[i] * System.Math.PI / 180.0);
			}
			return output;
		}

		/// <summary>
		/// Convert degrees to radians.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in radians.</returns>
		public static void DegreeToRadianInPlace(List<double> angles)
		{
			int				count		= angles.Count;

			for (int i = 0; i < count; i++)
			{
				angles[i] *= System.Math.PI / 180.0;
			}
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angle">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static double RadianToDegree(double angle)
		{
			return angle * 180.0 / System.Math.PI;
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static List<double> RadianToDegree(List<double> angles)
		{
			int				count		= angles.Count;
			List<double>	output		= new List<double>(count);

			for (int i = 0; i < count; i++)
			{
				output.Add(angles[i] * 180.0 / System.Math.PI);
			}
			return output;
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static void RadianToDegreeInPlace(List<double> angles)
		{
			int				count		= angles.Count;

			for (int i = 0; i < count; i++)
			{
				angles[i] *= 180.0 / System.Math.PI;
			}
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static List<double> RadianToRevolution(List<double> angles)
		{
			int				count		= angles.Count;
			List<double>	output		= new List<double>(count);

			for (int i = 0; i < count; i++)
			{
				output.Add(angles[i] / 2.0 / System.Math.PI);
			}
			return output;
		}

		/// <summary>
		/// Convert radians to degrees.
		/// </summary>
		/// <param name="angles">Angle to convert.</param>
		/// <returns>Angle in degrees.</returns>
		public static void RadianToRevolutionInPlace(List<double> angles)
		{
			int				count		= angles.Count;

			for (int i = 0; i < count; i++)
			{
				angles[i] /= 2.0 * System.Math.PI;
			}
		}


		#endregion

	} // End class.
} // End namespace.