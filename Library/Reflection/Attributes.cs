using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using DigitalProduction.ComponentModel;

namespace DigitalProduction.Reflection
{
	/// <summary>
	/// Get attributes.  Provide convenient access for common attribute properties.
	/// 
	/// 
	/// Get description of an enumeration:
	///		Converts from a [Description("")] to a enum value.
	///		Grabs the [Description("")] from a enum value.
	/// 
	///		Based on code written by skot:
	///		http://www.codeproject.com/useritems/EnumDescriptionAttribute.asp
	/// </summary>
	public static class Attributes
	{
		#region Methods

		/// <summary>
		/// Gets display name of an object.
		/// </summary>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <returns>Display name string if it is found.</returns>
		public static string GetDisplayName(Type type)
		{
			return GetDisplayName(type, string.Empty);
		}

		/// <summary>
		/// Gets display name of an object.
		/// </summary>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <param name="defaultValue">Default value to use if the attribute is not found.</param>
		/// <returns>Display name string if it is found.</returns>
		public static string GetDisplayName(Type type, string defaultValue)
		{
			string name = defaultValue;

			DisplayNameAttribute attribute = GetAttribute<DisplayNameAttribute>(type);
			if (attribute != null)
			{
				name = attribute.DisplayName;
			}
			return name;
		}

		/// <summary>
		/// Gets display name of an object.
		/// </summary>
		/// <param name="instance">Instance of the object type to retrieve the Attribute from.</param>
		/// <returns>Display name string if it is found.</returns>
		public static string GetDisplayName(object instance)
		{
			return GetDisplayName(instance, string.Empty);
		}

		/// <summary>
		/// Gets display name of an object.
		/// </summary>
		/// <param name="instance">Instance of the object type to retrieve the Attribute from.</param>
		/// <param name="defaultValue">Default value to use if the attribute is not found.</param>
		/// <returns>Display name string if it is found.</returns>
		public static string GetDisplayName(object instance, string defaultValue)
		{
			string name = defaultValue;

			DisplayNameAttribute attribute = GetAttribute<DisplayNameAttribute>(instance);
			if (attribute != null)
			{
				name = attribute.DisplayName;
			}
			return name;
		}

		/// <summary>
		/// Gets the description attribute of an enumeration.
		/// </summary>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <returns>Description string for the enumeration if found, default value if not.</returns>
		public static string GetDescription(Type type)
		{
			return GetDescription(type, string.Empty);
		}

		/// <summary>
		/// Gets the description attribute of an enumeration.
		/// </summary>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <param name="defaultValue">Default value to return if description is not found.</param>
		/// <returns>Description string for the enumeration if found, default value if not.</returns>
		public static string GetDescription(Type type, string defaultValue)
		{    
			string description = defaultValue;

			DescriptionAttribute attribute = GetAttribute<DescriptionAttribute>(type);
			if (attribute != null)
			{
				description = attribute.Description;
			}
			return description;
		}

		/// <summary>
		/// Gets the description attribute of an enumeration.
		/// </summary>
		/// <param name="instance">Value of the enumeration.</param>
		/// <returns>Description string for the enumeration if found, default value if not.</returns>
		public static string GetDescription(object instance)
		{
			return GetDescription(instance, string.Empty);
		}

		/// <summary>
		/// Gets the description attribute of an enumeration.
		/// </summary>
		/// <param name="instance">Value of the enumeration.</param>
		/// <param name="defaultValue">Default value to return if description is not found.</param>
		/// <returns>Description string for the enumeration if found, default value if not.</returns>
		public static string GetDescription(object instance, string defaultValue)
		{    
			string description = defaultValue;

			DescriptionAttribute attribute = GetAttribute<DescriptionAttribute>(instance);
			if (attribute != null)
			{
				description = attribute.Description;
			}
			return description;
		}

		/// <summary>
		/// Gets a list of names provided by the Alias attribute.
		/// </summary>
		/// <param name="instance">Instance of the object to retrieve the aliases from.</param>
		/// <returns>A List of strings containing the aliases of the class/structure.</returns>
		public static List<string> GetAliases(object instance)
		{
			return GetAliases(instance.GetType());
		}

		/// <summary>
		/// Gets a list of names provided by the Alias attribute.
		/// </summary>
		/// <param name="type">Type of object to retrieve the aliases from.</param>
		/// <returns>A List of strings containing the aliases of the class/structure.</returns>
		public static List<string> GetAliases(Type type)
		{
			List<string> aliases = new List<string>();

			List<AliasAttribute> attributes = GetAllAttributes<AliasAttribute>(type);

			foreach (AliasAttribute attribute in attributes)
			{
				aliases.Add(attribute.Alias);
			}

			return aliases;
		}

		/// <summary>
		/// Get the first Attribute of type "T" for the Type that the provided object is.
		/// </summary>
		/// <typeparam name="T">Type of attribute to get (not type of the object).</typeparam>
		/// <param name="instance">Instance of the object type to retrieve the Attribute from.</param>
		/// <returns>The first Attribute found of the type, or the default value of type T otherwise.</returns>
		public static T GetAttribute<T>(object instance) where T : Attribute
		{
			Type type		= instance.GetType();
			T attribute		= default(T);

			if (type.IsEnum)
			{
				// An "instance" of an enum is one of the members of the enumerator list.  They have to be
				// handled differently.
				FieldInfo fieldinfo = instance.GetType().GetField(instance.ToString());

				if (null != fieldinfo)
				{
					object[] attributes = fieldinfo.GetCustomAttributes(typeof(T), true);
					if (attributes != null && attributes.Length > 0)
					{
						attribute = ((T)attributes[0]);
					}
				}
			}
			else
			{
				attribute = GetAttribute<T>(instance.GetType());
			}

			return attribute;
		}

		/// <summary>
		/// Get the first Attribute of type "T" for the Type that the provided object is.
		/// </summary>
		/// <typeparam name="T">Type of attribute to get (not type of the object).</typeparam>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <returns>The first Attribute found of the type, or the default value of type T otherwise.</returns>
		public static T GetAttribute<T>(System.Type type) where T : Attribute
		{
			T attribute = default(T);

			Attribute[] attributes = Attribute.GetCustomAttributes(type);

			foreach (Attribute attr in attributes)
			{
				if (attr is T)
				{
					attribute = (T)attr;
				}
			}
        
			return attribute;
		}

		/// <summary>
		/// Gets a list of Attributes of type "T" for the Type that the provided object is.
		/// </summary>
		/// <typeparam name="T">Type of attribute to get (not type of the object).</typeparam>
		/// <param name="instance">Instance of the object type to retrieve the Attribute from.</param>
		/// <returns>A List of all Attributes found of the specified type.</returns>
		public static List<T> GetAllAttributes<T>(object instance) where T : Attribute
		{
			return GetAllAttributes<T>(instance.GetType());
		}

		/// <summary>
		/// Get a list of Attributes of the specified Type.
		/// </summary>
		/// <typeparam name="T">Type of attribute to get (not type of the object).</typeparam>
		/// <param name="type">Type of the object to retrieve the Attribute from.</param>
		/// <returns>A List of all Attributes found of the specified type.</returns>
		public static List<T> GetAllAttributes<T>(Type type) where T : Attribute
		{
			List<T> attribute = new List<T>();

			Attribute[] attributes = Attribute.GetCustomAttributes(type);

			foreach (System.Attribute attr in attributes)
			{
				if (attr is T)
				{
					attribute.Add((T)attr);
				}
			}
        
			return attribute;
		}

		#endregion

	} // End class.
} // End namespace.