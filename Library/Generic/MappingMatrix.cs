using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DigitalProduction;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DigitalProduction.Generic
{
	/// <summary>
	/// Stores a matrix (2 dimensional array) of data which can be accessed by an enumeration, but the data does not have
	/// to be stored in the matrix in the same order as the items in the enumeration are defined.
	/// </summary>
	/// <typeparam name="KeyType">Enumeration type used as a key to access data.</typeparam>
	/// <typeparam name="DataType">Type of data to store in the matrix.</typeparam>
	public class MappingMatrix<KeyType, DataType>
	{
		#region Members / Variables / Delegates.

		int										_numberofkeys;

		List<KeyType>							_activekeys;
		int										_numberofactivekeys;

		// Array which maps the enumerations to the position in "_data".  This maps between the enumeration values 
		// and the location in the array "_data" that the data is in.
		private int[]							_map;

		// Main data set.  Data is stored with the keys as rows.  So to access data the key is the first index,
		// the index to that key (which data point in the row) is the second index.  Example layout:
		//
		// Key						Entries
		//					1		2		3		4		5
		// KeyType.First	3.1		4.2		5.5		6.1		7.1
		// KeyType.Second	98.6	86.3	76.5	92.4	82.3
		// KeyType.Third	15351	16523	18352	14366	13546
		private List<List<DataType>>			_data					= new List<List<DataType>>();

		#endregion

		#region Construction.

		/// <summary>
		/// Parameterless constructor for serialization.
		/// </summary>
		private MappingMatrix() {}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="activekeys">List of active keys, in the order that they are contained in the data.</param>
		public MappingMatrix(List<KeyType> activekeys)
		{
			_numberofkeys		= Reflection.EnumUtils.NumberOfDefinedItems<KeyType>();
			_activekeys			= new List<KeyType>(activekeys);
			_numberofactivekeys	= _activekeys.Count;
			_map				= new int[_numberofkeys];

			// First we need to initialize the entry map to values which are not valid.  That way we can determine
			// if the data is present or not.
			for (int i = 0; i < _numberofkeys; i++)
			{
				// Set to an invalid value.
				_map[i] = -1;
			}

			// Now we can set the values in the entry map based on the provided active entries.
			for (int i = 0; i < _numberofactivekeys; i++)
			{
				// Set a map from the enumeration into the data container.
				_map[System.Convert.ToInt32(_activekeys[i])] = i;
				_data.Add(new List<DataType>());
			}
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="original"></param>
		public MappingMatrix(MappingMatrix<KeyType, DataType> original)
		{
			_numberofkeys		= original._numberofkeys;
			_activekeys			= new List<KeyType>(original._activekeys);
			_activekeys			= original._activekeys;
			_numberofactivekeys	= original._numberofactivekeys;
			_map				= new int[_numberofkeys];

			// Copy map.
			original._map.CopyTo(_map, 0);

			// Copy data.
			foreach (List<DataType> list in original._data)
			{
				_data.Add(new List<DataType>(list));
			}
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Total number of keys available in the enumeration used for KeyType.
		/// </summary>
		[XmlAttribute("numberofkeys")]
		public int NumberOfKeys
		{
			get
			{
				return _numberofkeys;
			}

			set
			{
				_numberofkeys = value;
			}
		}

		/// <summary>
		/// List of keys which we have data for.
		/// </summary>
		[XmlArray("activekeys"), XmlArrayItem("key")]
		public List<KeyType> ActiveKeys
		{
			get
			{
				return _activekeys;
			}

			set
			{
				_activekeys = value;
			}
		}

		/// <summary>
		/// Number of the keys that we have data for.
		/// </summary>
		[XmlAttribute("numberofactivekeys")]
		public int NumberOfActiveKeys
		{
			get
			{
				return _numberofactivekeys;
			}

			set
			{
				_numberofactivekeys = value;
			}
		}

		/// <summary>
		/// Map which specifies where data is in the data array.
		/// </summary>
		[XmlArray("map"), XmlArrayItem("index")]
		public int[] Map
		{
			get
			{
				return _map;
			}

			set
			{
				_map = value;
			}
		}

		/// <summary>
		/// Raw data.
		/// </summary>
		[XmlArray("data"), XmlArrayItem("dataset")]
		public List<List<DataType>> Data
		{
			get
			{
				return _data;
			}
		}

		/// <summary>
		/// Get the number of entries (length) for the data.
		/// </summary>
		public int LengthOfData
		{
			get
			{
				return _data[0].Count;
			}
		}

		/// <summary>
		/// Brackets operator.
		/// </summary>
		/// <param name="key">Which set of data to get.</param>
		/// <returns>The set of data associated with the key in a List.</returns>
		[XmlIgnore()]
		public List<DataType> this[KeyType key]
		{
			get
			{
				int index = _map[System.Convert.ToInt32(key)];

				if (index < 0)
				{
					throw new Exception("The requested data is not available.");
				}

				return _data[index];
			}

			set
			{
				int index = _map[System.Convert.ToInt32(key)];

				if (index < 0)
				{
					throw new Exception("The requested data is not available.");
				}

				_data[index] = value;
			}
		}

		/// <summary>
		/// Length of the data (number of elements for each KeyType).
		/// </summary>
		[XmlIgnore()]
		public int NumberOfEntries
		{
			get
			{
				return _data[0].Count;
			}
		}

		#endregion

		#region Other methods.

		/// <summary>
		/// Specifies if the KeyType is active (has data associated with it).
		/// </summary>
		/// <param name="key">KeyType to check.</param>
		/// <returns>True is data exists for the key time, false otherwise.</returns>
		public bool IsActiveKey(KeyType key)
		{
			if (_map[System.Convert.ToInt32(key)] < 0)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		#endregion

		#region Data access.

		/// <summary>
		/// Adds a set of data entries to the back of the data.
		/// </summary>
		/// <param name="entries">Set of data, one entry per each active key type, in the same order as the active key types.</param>
		public void Push(List<DataType> entries)
		{
			Debug.Assert(entries.Count == _numberofactivekeys, "The entries supplied to MappingMatrix.Push are not sized correctly.");

			for (int i = 0; i < _numberofactivekeys; i++)
			{
				_data[i].Add(entries[i]);
			}
		}

		/// <summary>
		/// Removes a range of elements.
		/// </summary>
		/// <param name="index">The zero-based starting index of the range of elements to remove.</param>
		/// <param name="count">The number of elements to remove.</param>
		public void RemoveRange(int index, int count)
		{
			for (int i = 0; i < _numberofactivekeys; i++)
			{
				_data[i].RemoveRange(index, count);
			}
		}

		/// <summary>
		/// Removes sections of the data.  Much more effecient that RemoveRange for removing multiple ranges.
		/// </summary>
		/// <param name="indexestoremove">Which sections to be removed.</param>
		public void RemoveRanges(List<int[]> indexestoremove)
		{
			// Make sure some indices were supplied before we start trying to access them.  If nothing was supplied,
			// we can just return.
			if (indexestoremove.Count == 0)
			{
				return;
			}

			// We are going to create a new data structure and copy the relavent data from the old data to the new, ignoring
			// those data points specified in indexestoremove.  We do this because calling "RemoveRange" multiple times in not
			// going to be very efficient.

			// We are going to need the length of the existing data and number of data types in a few places, so get it once.
			int lengthofdata		= this.NumberOfEntries;

			// Create a new data structure for our data.  Because all the data is now loaded, we can provide the List constructor
			// with size information, which should help keep things a little more efficient.
			List<List<DataType>> newdata = new List<List<DataType>>(_numberofactivekeys);
			for (int i = 0; i < _numberofactivekeys; i++)
			{
				newdata.Add(new List<DataType>());
			}

			int removingset = 0;
			int boundry		= indexestoremove[0][0];

			// Loop over all the data and look for boundaries.
			for (int i = 0; i < lengthofdata; i++)
			{
				if (i < boundry)
				{
					// Before we get to the next start of a range to be removed, we copy the existing data and upate the times.

					// Add data.
					for (int j = 0; j < _numberofactivekeys; j++)
					{
						newdata[j].Add(_data[j][i]);
					}
				}
				else
				{
					// Now we have reached a range that is to be removed from the existing data.

					// Ending index for this section.
					int endofremovalsection = indexestoremove[removingset][1];

					// We can fast forward the indexer to after the removed section.
					i = endofremovalsection + 1;

					// Go to next set of indexes to remove.
					removingset++;

					// Check to see if we are at the end of the indexesToRemove.
					if (removingset == indexestoremove.Count)
					{
						// We just finished removing the last set of indexes, now we can set the boundary to the end of the original data
						// and just finish the copying.
						boundry = lengthofdata;
					}
					else
					{
						// There is another set of indexes to remove, so set the boundary to the start of the next set.
						boundry = indexestoremove[removingset][0];
					}
				}
			}

			// Save the new values and allow the old to be deleted.
			_data	= newdata;
		}

		#endregion

	} // End class.
} // End namespace.