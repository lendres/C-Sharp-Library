﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DigitalProduction.Mathmatics;

namespace DigitalProduction.Extensions
{
	/// <summary>
	/// Statistical extensions for a list.
	/// Based upon: http://www.martijnkooij.nl/2013/04/csharp-math-mean-variance-and-standard-deviation/
	/// </summary>
	public static class ListExtensions
	{
		#region Statistics

		/// <summary>
		/// Get the mean (average) of a subset of list entries.
		/// </summary>
		/// <param name="values">List to calculate the mean (average) from.</param>
		/// <param name="start">Index to start from.</param>
		/// <param name="count">Number of entries to take the mean (average) of.</param>
		public static double Average(this List<double> values, int start, int count)
		{
			double	average	= 0;
			int		end		= start + count;
			for (int i = start; i < end; i++)
			{
				average += values[i];
			}
			return average / count;
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static List<double> RemoveAverage(this List<double> values)
		{
			return values.RemoveAverage(0, values.Count);
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to remove the mean (average) of.</param>
		public static List<double> RemoveAverage(this List<double> values, int start, int count)
		{
			int		end		= start + count;
			double	average	= values.Average(start, count);

			List<double> newValues = new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] - average);
			}
			return newValues;
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static void RemoveAverageInPlace(this List<double> values)
		{
			values.RemoveAverageInPlace(0, values.Count);
		}

		/// <summary>
		/// Removes the mean (subtracts) from a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to remove the mean (average) of.</param>
		public static void RemoveAverageInPlace(this List<double> values, int start, int count)
		{
			int		end		= start + count;
			double	average	= values.Average(start, count);

			for (int i = start; i < end; i++)
			{
				values[i] = values[i] - average;
			}
		}

		/// <summary>
		/// Calculates the variance of a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		public static double Variance(this List<double> values)
		{
			return values.Variance(values.Average(), 0, values.Count);
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
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static double Variance(this List<double> values, double mean, int start, int count)
		{
			int		end			= start + count;
			double	variance	= 0;

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
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static double StandardDeviation(this List<double> values, int start, int count)
		{
			double mean		= values.Average(start, count);
			double variance	= values.Variance(mean, start, count);
			return System.Math.Sqrt(variance);
		}

		/// <summary>
		/// Calculates a moving average.  This is a "backward" moving average.  A data point in the moving average is the
		/// average of the previous "windowLength" points from the input data.  This is different from a "central" or
		/// "forward" moving average.
		/// </summary>
		/// <param name="values">Values to average.</param>
		/// <param name="windowLength">Window size for the average.</param>
		public static List<double> MovingAverage(this List<double> values, int windowLength)
		{
			double sum = 0;
			List<double> averages = new List<double>(values.Count);

			for (int i = 0; i < values.Count; i++)
			{
				if (i < windowLength)
				{
					sum += values[i];
					//averages[i] = (i == windowLength - 1) ? sum / (double)windowLength : 0;
					averages.Add(sum / (double)(i+1));
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
		public static List<double> SegmentAverage(this List<double> values, List<int> segmentIndices, bool forPlotting)
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

		#endregion

		#region Add

		/// <summary>
		/// Addition of a scalar with a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to add to the list entries.</param>
		public static List<double> Add(this List<double> values, double scalar)
		{
			return values.Add(scalar, 0, values.Count);
		}

		/// <summary>
		/// Addition of a scalar with a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to add to the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Add(this List<double> values, double scalar, int start, int count)
		{
			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] + scalar);
			}
			return newValues;
		}

		/// <summary>
		/// Addition of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		public static List<double> Add(this List<double> leftValues, List<double> rightValues)
		{
			return leftValues.Add(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Addition of a subset of two list of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Add(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			if (leftValues.Count != rightValues.Count)
			{
				throw new Exception("Left and right arrays must be the same length.");
			}

			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(leftValues[i] + rightValues[i]);
			}
			return newValues;
		}

		/// <summary>
		/// Fast version of addition by a scalar.  Does addition of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to add to the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to add.</param>
		public static void AddInPlace(this List<double> values, double scalar)
		{
			values.AddInPlace(scalar, 0, values.Count);
		}

		/// <summary>
		/// Fast version of addition by a scalar.  Does addition of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to add to the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to add.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void AddInPlace(this List<double> values, double scalar, int start, int count)
		{
			int end = start + count;

			for (int i = start; i < end; i++)
			{
				values[i] += scalar;
			}
		}

		/// <summary>
		/// Addition of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		public static void AddInPlace(this List<double> leftValues, List<double> rightValues)
		{
			leftValues.AddInPlace(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Addition of a subset of two list of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void AddInPlace(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			if (leftValues.Count != rightValues.Count)
			{
				throw new Exception("Left and right arrays must be the same length.");
			}

			int				end				= start + count;

			for (int i = start; i < end; i++)
			{
				leftValues[i] += rightValues[i];
			}
		}

		#endregion

		#region Substract

		/// <summary>
		/// Subtraction of a scalar with a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to subtract from the list entries.</param>
		public static List<double> Subtract(this List<double> values, double scalar)
		{
			return values.Subtract(scalar, 0, values.Count);
		}

		/// <summary>
		/// Subtraction of a scalar with a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to subtract from the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Subtract(this List<double> values, double scalar, int start, int count)
		{
			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] - scalar);
			}
			return newValues;
		}

		/// <summary>
		/// Addition of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		public static List<double> Subtract(this List<double> leftValues, List<double> rightValues)
		{
			return leftValues.Subtract(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Addition of a subset of two list of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Subtract(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			if (leftValues.Count != rightValues.Count)
			{
				throw new Exception("Left and right arrays must be the same length.");
			}

			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(leftValues[i] - rightValues[i]);
			}
			return newValues;
		}

		/// <summary>
		/// Fast version of subtraction by a scalar.  Does subtract from values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to subtract from the values.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to subtract.</param>
		public static void SubtractInPlace(this List<double> values, double scalar)
		{
			values.SubtractInPlace(scalar, 0, values.Count);
		}

		/// <summary>
		/// Fast version of subtract by a scalar.  Does subtract from values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to subtract from the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to subtract.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void SubtractInPlace(this List<double> values, double scalar, int start, int count)
		{
			int end = start + count;

			for (int i = start; i < end; i++)
			{
				values[i] -= scalar;
			}
		}

		/// <summary>
		/// Addition of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		public static void SubtractInPlace(this List<double> leftValues, List<double> rightValues)
		{
			leftValues.SubtractInPlace(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Addition of a subset of two list of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to add to the list entries.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void SubtractInPlace(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			if (leftValues.Count != rightValues.Count)
			{
				throw new Exception("Left and right arrays must be the same length.");
			}

			int				end				= start + count;

			for (int i = start; i < end; i++)
			{
				leftValues[i] -= rightValues[i];
			}
		}

		#endregion

		#region Multiply

		/// <summary>
		/// Multiplication of a scalar with a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to multiply the list entries by.</param>
		public static List<double> Multiply(this List<double> values, double scalar)
		{
			return values.Multiply(scalar, 0, values.Count);
		}

		/// <summary>
		/// Multiplication of a scalar with a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Value to multiply the list entries by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Multiply(this List<double> values, double scalar, int start, int count)
		{
			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] * scalar);
			}
			return newValues;
		}

		/// <summary>
		/// Entry wise multiplication of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to multiply the list entries by.</param>
		public static List<double> Multiply(this List<double> leftValues, List<double> rightValues)
		{
			return leftValues.Multiply(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Entry wise multiplication of two lists of doubles.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to multiply the list entries by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Multiply(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			int				end				= start + count;
			List<double>	newValues		= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(leftValues[i] * rightValues[i]);
			}
			return newValues;
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to multiply by.</param>
		public static void MultiplyInPlace(this List<double> values, double scalar)
		{
			values.MultiplyInPlace(scalar, 0, values.Count);
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to multiply by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void MultiplyInPlace(this List<double> values, double scalar, int start, int count)
		{
			int end = start + count;

			for (int i = start; i < end; i++)
			{
				values[i] *= scalar;
			}
		}

		/// <summary>
		/// Entry wise multiplication of two lists of doubles.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to multiply the list entries by.</param>
		public static void MultiplyInPlace(this List<double> leftValues, List<double> rightValues)
		{
			leftValues.Multiply(rightValues, 0, leftValues.Count);
		}

		/// <summary>
		/// Entry wise multiplication of two lists of doubles.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="leftValues">Values for calculation (this list).</param>
		/// <param name="rightValues">Values to multiply the list entries by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void MultiplyInPlace(this List<double> leftValues, List<double> rightValues, int start, int count)
		{
			int				end				= start + count;

			for (int i = start; i < end; i++)
			{
				leftValues[i] *= rightValues[i];
			}
		}

		#endregion

		#region Divide

		/// <summary>
		/// Division by a scalar.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Scalar to divide by.</param>
		public static List<double> Divide(this List<double> values, double scalar)
		{
			return values.Divide(scalar, 0, values.Count);
		}

		/// <summary>
		/// Multiplication of a scalar with a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="scalar">Scalar to divide by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Divide(this List<double> values, double scalar, int start, int count)
		{
			int				end			= start + count;
			List<double>	newValues	= new List<double>(count);

			for (int i = start; i < end; i++)
			{
				newValues.Add(values[i] / scalar);
			}
			return newValues;
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to divide by.</param>
		public static void DivideInPlace(this List<double> values, double scalar)
		{
			values.DivideInPlace(scalar, 0, values.Count);
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="scalar">Scalar to divide by.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void DivideInPlace(this List<double> values, double scalar, int start, int count)
		{
			int end = start + count;

			for (int i = start; i < end; i++)
			{
				values[i] /= scalar;
			}
		}

		#endregion

		#region Normalization

		/// <summary>
		/// Multiplication of a scalar with a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="normalizationType">Method used to normalize the values.</param>
		public static List<double> Normalize(this List<double> values, NormalizationType normalizationType)
		{
			return values.Normalize(normalizationType, 0, values.Count);
		}

		/// <summary>
		/// Multiplication of a scalar with a subset of a list of doubles.
		/// </summary>
		/// <param name="values">Values for calculation (this list).</param>
		/// <param name="normalizationType">Method used to normalize the values.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static List<double> Normalize(this List<double> values, NormalizationType normalizationType, int start, int count)
		{
			double max = values.Max();
			return values.Divide(max, start, count);
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="normalizationType">Method used to normalize the values.</param>
		public static void NormalizeInPlace(this List<double> values, NormalizationType normalizationType)
		{
			values.NormalizeInPlace(normalizationType, 0, values.Count);
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		/// <param name="normalizationType">Method used to normalize the values.</param>
		/// <param name="start">Starting index.</param>
		/// <param name="count">Number of entries to use in the calculation.</param>
		public static void NormalizeInPlace(this List<double> values, NormalizationType normalizationType, int start, int count)
		{
			double max = NormalizationValue(values, normalizationType);
			values.MultiplyInPlace(max, start, count);
		}

		/// <summary>
		/// Get the normalization value of a list based on the specified NormalizationType.
		/// </summary>
		/// <param name="values">Input list.</param>
		/// <param name="normalizationType">NormalizationType.</param>
		private static double NormalizationValue(List<double> values, NormalizationType normalizationType)
		{
			switch (normalizationType)
			{
				case NormalizationType.Euclidean:
				{
					return 1.0 / VectorLength(values);
				}

				case NormalizationType.MaxValueIsHalfPi:
				{
					return Math.PI / 2.0 / values.Max();
				}

				case NormalizationType.MaxAbsoluteValueIsOne:
				{
					return 1.0 / values.MaxAbsoluteValue();
				}

				case NormalizationType.MaxValueIsOne:
				{
					return 1.0 / values.Max();
				}

				default:
				{
					throw new Exception("NormalizationType not valid.");
				}
			}
		}

		#endregion

		#region Other

		/// <summary>
		/// Vector length (Euclidian norm) of a list of doubles.
		/// </summary>
		/// <param name="values">Input list.</param>
		public static double VectorLength(this List<double> values)
		{
			int		count	= values.Count;
			double	sum		= 0;

			for (int i = 0; i < count; i++)
			{
				sum += values[i] * values[i];
			}

			return Math.Sqrt(sum);
		}

		/// <summary>
		/// Find the maximum of the absolute values.
		/// </summary>
		/// <param name="values">Input list.</param>
		public static double MaxAbsoluteValue(this List<double> values)
		{
			int		count				= values.Count;
			double	maxAbsoluteValue	= 0;

			for (int i = 0; i < count; i++)
			{
				double absValue = Math.Abs(values[i]);
				if (absValue > maxAbsoluteValue)
				{
					maxAbsoluteValue = absValue;
				}
			}

			return maxAbsoluteValue;
		}

		/// <summary>
		/// Square root entry by entry.
		/// </summary>
		/// <param name="values">List to take the square root of.</param>
		public static List<double> SquareRoot(this List<double> values)
		{
			int				count			= values.Count;
			List<double>	newValues		= new List<double>(count);

			for (int i = 0; i < count; i++)
			{
				newValues.Add(Math.Sqrt(values[i]));
			}
			return newValues;
		}

		/// <summary>
		/// Fast version of multiply by a scalar.  Does multiplication of values in list "in place," meaning
		/// the values in the list are overwritten.
		/// </summary>
		/// <param name="values">List to multiple the values of.  Values in list are overwritten.</param>
		public static void SquareRootInPlace(this List<double> values)
		{
			int	count = values.Count;

			for (int i = 0; i < count; i++)
			{
				values[i] = Math.Sqrt(values[i]);
			}
		}

		/// <summary>
		/// Entry wise SRSS (the hypotenuse) of the lists.
		/// </summary>
		/// <param name="leftValues">Values for calculation.</param>
		/// <param name="rightValues">Values for calculation.</param>
		public static List<double> Hypotenuse(this List<double> leftValues, List<double> rightValues)
		{
			int				count			= leftValues.Count;
			List<double>	newValues		= new List<double>(count);

			if (count != rightValues.Count)
			{
				throw new Exception("Left and right lists are not the same length.");
			}

			for (int i = 0; i < count; i++)
			{
				newValues.Add(Math.Sqrt(leftValues[i]*leftValues[i] + rightValues[i]*rightValues[i]));
			}
			return newValues;
		}

		/// <summary>
		/// Takes the derivative of a list of doubles.  Duplicates the last entry to create an output list
		/// that is the same length of the input list.
		/// 
		/// Assumes constant time step size.
		/// </summary>
		/// <param name="values">List to take the derivative of.</param>
		/// <param name="timestep">Time step size to take derivative over.</param>
		public static List<double> Derivative(this List<double> values, double timestep)
		{
			int				count				= values.Count;
			List<double>	derivatives			= new List<double>(count);

			for (int i = 0; i < count-1; i++)
			{
				derivatives.Add((values[i+1] - values[i]) / timestep);
			}

			// Last entry.  Used to make sure length of lists agrees.
			derivatives.Add(derivatives[count-2]);

			return derivatives;
		}

		#endregion

	} // End class.
} // End namespace.