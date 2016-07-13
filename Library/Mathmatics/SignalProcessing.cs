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
	public static class SignalProcessing
	{
		#region Enumerations

		#endregion

		#region Delegates

		#endregion

		#region Events

		#endregion

		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		static SignalProcessing()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		//k=0;				% initialize k to 0
		//i=1;				% initialize the counter to 1
		//alpha=pi;			% set alpha to the desired Tolerance. In this case, pi

		//for i = 1:(size(u)-1)
		//	yout(i,:)=u(i)+(2*pi*k); % add 2*pi*k to ui
		//	if((abs(u(i+1)-u(i)))>(abs(alpha)))  %if diff is greater than alpha, increment or decrement k

		//		if u(i+1)<u(i)   % if the phase jump is negative, increment k
		//			k=k+1;
		//		else             % if the phase jump is positive, decrement k
		//			k=k-1;
		//		end
		//	end
		//end
		//yout((i+1),:)=u(i+1)+(2*pi*k); % add 2*pi*k to the last element of the input

		/// <summary>
		/// Unwrap the phase angle of an input array.
		/// </summary>
		/// <param name="angles">Input angles (in radians).</param>
		public static List<double> Unwrap(List<double> angles)
		{
			int		revolutionCount	= 0;
			double	tolerance		= System.Math.PI;
			int		count			= angles.Count;

			List<double> output = new List<double>(count);

			for (int i = 0; i < count-1; i++)
			{
				output.Add(angles[i] + 2*System.Math.PI*revolutionCount);

				if (System.Math.Abs(angles[i+1] - angles[i]) > System.Math.Abs(tolerance))
				{
					if (angles[i+1] < angles[i])
					{
						revolutionCount = revolutionCount + 1;
					}
					else
					{
						revolutionCount = revolutionCount - 1;
					}
				}
			}

			// Add last entry.
			output.Add(angles[count-1] + 2*System.Math.PI*revolutionCount);

			return output;
		}

		#endregion

	} // End class.
} // End namespace.