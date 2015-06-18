using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalProduction.Extensions
{
	/// <summary>
	/// Statistical extensions for a list.
	/// Based upon: http://www.martijnkooij.nl/2013/04/csharp-math-mean-variance-and-standard-deviation/
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		/// Get the mean (average) of a list.
		/// </summary>
		/// <param name="values">List to calculate the mean (average) from.</param>
		/// <returns>Mean (average) of data.</returns>
		public static double Mean(this List<double> values)
		{
			return values.Count == 0 ? 0 : values.Mean(0, values.Count);
		}

		/// <summary>
		/// Get the mean (average) of a subset of list entries.
		/// </summary>
		/// <param name="values">List to calculate the mean (average) from.</param>
		/// <param name="start">Index to start from.</param>
		/// <param name="count">Number of entries to take the mean (average) of.</param>
		/// <returns>Mean (average) of data.</returns>
		public static double Mean(this List<double> values, int start, int count)
		{
			double s = 0;
			for (int i = start; i < start+count; i++)
			{
				s += values[i];
			}
			return s / (count);
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static List<double> RemoveMean(this List<double> values)
		{
			return values.RemoveMean(0, values.Count);
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="start">Starting index.</param>
		/// <param name="end">Ending index (not included in calculation).</param>
		public static List<double> RemoveMean(this List<double> values, int start, int end)
		{
			double mean = values.Mean(start, end);
			List<double> newValues = new List<double>(end - start + 1);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] - mean);
			}
			return newValues;
		}
		
		/// <summary>
		/// Calculates the variance of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static double Variance(this List<double> values)
		{
			return values.Variance(values.Mean(), 0, values.Count);
		}
		
		/// <summary>
		/// Calculates the variance of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="mean">The mean (average) value of the entries.</param>
		public static double Variance(this List<double> values, double mean)
		{
			return values.Variance(mean, 0, values.Count);
		}
		
		/// <summary>
		/// Calculates the variance of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="mean">The mean (average) value of the entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="end">Ending index (not included in calculation).</param>
		public static double Variance(this List<double> values, double mean, int start, int end)
		{
			double variance = 0;
			for (int i = start; i < end; i++)
			{
				variance += System.Math.Pow((values[i] - mean), 2);
			}
			
			int n = end - start;

			if (start > 0)
			{
				n -= 1;
			}
			
			return variance / (n);
		}
		
		/// <summary>
		/// Calculates the standard deviation of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static double StandardDeviation(this List<double> values)
		{
			return values.Count == 0 ? 0 : values.StandardDeviation(0, values.Count);
		}
		
		/// <summary>
		/// Calculates the standard deviation of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="start">Starting index.</param>
		/// <param name="end">Ending index (not included in calculation).</param>
		public static double StandardDeviation(this List<double> values, int start, int end)
		{
			double mean = values.Mean(start, end);
			double variance = values.Variance(mean, start, end);
			return System.Math.Sqrt(variance);
		}

		/// <summary>
		/// Calculates a moving average.  This is a "backward" moving average.  A data point in the moving average is the
		/// average of the previous "windowLength" points from the input data.  This is different from a "central" or
		/// "forward" moving average.
		/// </summary>
		/// <param name="values">Values to average.</param>
		/// <param name="windowLength">Window size for the average.</param>
		public static List<double> MovingAverage(List<double> values, int windowLength)
		{
			double sum = 0;
			List<double> averages = new List<double>(values.Count);

			for (int i = 0; i < values.Count; i++)
			{
				if (i < windowLength)
				{
					sum += values[i];
					//averages[i] = (i == windowLength - 1) ? sum / (double)windowLength : 0;
					averages.Add(sum / (double)windowLength);
				}
				else
				{
					sum = sum - values[i - windowLength] + values[i];
					averages.Add(sum / (double)windowLength);
				}
			}
			return averages;
		}

		/// <summary>
		/// Calculate the average for each segment.
		/// </summary>
		/// <param name="values">Values to take average of.</param>
		/// <param name="segmentIndices">Indices of the segments.</param>
		/// <param name="forPlotting">If true, the additional values are added (values are added twice) so that a continuous line can be plotted.</param>
		public static List<double> SegmentAverage(List<double> values, List<int> segmentIndices, bool forPlotting)
		{
			int size				= segmentIndices.Count;
			List<double> results	= new List<double>(2*size);

			for (int i = 1; i < size; i++)
			{
				double average = 0;
				for (int j = segmentIndices[i-1]; j < segmentIndices[i]; j++)
				{
					average += values[j];
				}
				average /= segmentIndices[i] - segmentIndices[i-1] + 1;

				results.Add(average);

				if (forPlotting)
				{
					results.Add(average);
				}
			}

			return results;
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to multiply by.</param>
		public static void MultiplyInPlace(this List<double> values, double scalar)
		{
			for (int i = 0; i < values.Count; i++)
			{
				values[i] *= scalar;
			}
		}

	} // End class.
} // End namespace.