using System;
using System.Collections.Generic;
using System.Collections;

namespace DigitalProduction.Generic
{
	/// <summary>
	/// Summary description for ObjectFactory.
	/// </summary>
	public class ObjectFactory<KeyType, GeneralType>
	{
		#region Delegates

		/// <summary>Create delegate.</summary>
		public delegate GeneralType CreateDelegate();

		#endregion

		#region Members

		private SortedList<KeyType, CreateDelegate>				_products;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ObjectFactory()
		{
			_products = new SortedList<KeyType, CreateDelegate>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Register a class with the object factory.
		/// </summary>
		/// <param name="key">Key used to retrieve the new object from.</param>
		public void Register<SpecificType>(KeyType key) where SpecificType : GeneralType, new()
		{
			CreateDelegate creator = new CreateDelegate(Creator<SpecificType>);
			_products.Add(key, creator);
		}

		/// <summary>
		/// Register a class with the object factory.
		/// </summary>
		/// <param name="key">Key used to retrieve the new object from.</param>
		/// <param name="creator">CreateDelegate used to create the SpecificProduct.</param>
		public void Register<SpecificType>(KeyType key, CreateDelegate creator) where SpecificType : GeneralType, new()
		{
			_products.Add(key, creator);
		}

		/// <summary>
		/// Create a class associated with a given key.
		/// </summary>
		/// <param name="key">Key used to retrieve the new object from.</param>
		/// <returns>A new object.</returns>
		public GeneralType Create(KeyType key)
		{
			CreateDelegate create = _products[key];
			return create();
		}

		/// <summary>
		/// Get an array of keys.
		/// </summary>
		/// <returns>The array of keys.</returns>
		public KeyType[] GetArrayOfKeys()
		{
			KeyType[] keys = new KeyType[_products.Count];

			_products.Keys.CopyTo(keys, 0);

			return keys;
		}

		/// <summary>
		/// Get a list of keys.
		/// </summary>
		/// <returns>The list of keys.</returns>
		public List<KeyType> GetListOfKeys()
		{
			List<KeyType> keys = new List<KeyType>();

			for (int i = 0; i < _products.Count; i++)
			{
				keys.Add(_products.Keys[i]);
			}

			return keys;
		}

		/// <summary>
		/// Does the actual creation of a new object.
		/// </summary>
		/// <typeparam name="SpecificType">Subclass type to return.</typeparam>
		/// <returns>A new object (subclassed from GeneralProduct) of in the form of the super class (base class) type.</returns>
		private GeneralType Creator<SpecificType>() where SpecificType : GeneralType, new()
		{
			return new SpecificType();
		}

		#endregion

	} // End class.
} // End namespace.