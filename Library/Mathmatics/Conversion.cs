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
	/// Class for converting units.
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

		#region Angles

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

		#region Angular Velocity

		/// <summary>
		/// Convert RPM to Hertz (where 1 revolution in 1 second is 1 Hertz).  Hertz would be
		/// equivalent to 1 revolution per 1 second.
		/// </summary>
		/// <param name="rpm">Revolutions per minute.</param>
		/// <returns>Revolutions per minute as Hertz (cycles per second).</returns>
		public static double RpmToHertz(double rpm)
		{
			return rpm / 60.0;
		}

		#endregion

		#region Length

		/// <summary>
		/// Convert feet to inches.
		/// </summary>
		/// <param name="feet">Feet.</param>
		/// <returns>Inches.</returns>
		public static double FeetToInches(double feet)
		{
			return feet * 12.0;
		}

		/// <summary>
		/// Convert inches to feet.
		/// </summary>
		/// <param name="inches">Inches.</param>
		/// <returns>Feet.</returns>
		public static double InchesToFeet(double inches)
		{
			return inches / 12.0;
		}

		#endregion

		#region Vibration

		/// <summary>
		/// Convert frequency (Hertz) into period (seconds).
		/// </summary>
		/// <param name="frequency">Frequency in Hertz.</param>
		/// <returns>Period in seconds.</returns>
		public static double FrequencyToPeriod(double frequency)
		{
			return 1.0 / frequency;
		}

		/// <summary>
		/// Convert frequency Hertz into period (seconds).
		/// </summary>
		/// <param name="period">Period in seconds.</param>
		/// <returns>Frequency in Hertz.</returns>
		public static double PeriodToFrequency(double period)
		{
			return 1.0 / period;
		}

		#endregion

		#endregion

	} // End class.
} // End namespace.