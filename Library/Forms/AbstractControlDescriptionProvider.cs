using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Provides a way for the designer to work with abstract base classes.
	/// 
	/// To use, preface the class declaration with:
	/// [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider&lt;TAbstract, TBase&gt;))]
	/// 
	/// http://stackoverflow.com/questions/1620847/how-can-i-get-visual-studio-2008-windows-forms-designer-to-render-a-form-that-im/2406058#2406058
	/// </summary>
	/// <typeparam name="TAbstract">Abstract class.</typeparam>
	/// <typeparam name="TBase">Base class.</typeparam>
	public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
	{
		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public AbstractControlDescriptionProvider() :
			base(TypeDescriptor.GetProvider(typeof(TAbstract)))
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Tell anyone who reflects on us that the concrete form is the form to reflect against, not the abstract form. This way, the
		/// designer does not see an abstract class. 
		/// </summary>
		/// <param name="objectType">Object type.</param>
		/// <param name="instance">Instance.</param>
		public override Type GetReflectionType(Type objectType, object instance)
		{
			if (objectType == typeof(TAbstract))
			{
				return typeof(TBase);
			}

			return base.GetReflectionType(objectType, instance);
		}

		/// <summary>
		/// If the designer tries to create an instance of AbstractForm, we override it here to create a concrete form instead.
		/// </summary>
		/// <param name="provider">Service provider.</param>
		/// <param name="objectType">Object type.</param>
		/// <param name="argTypes">Argument Types.</param>
		/// <param name="args">Arguments.</param>
		public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
		{
			if (objectType == typeof(TAbstract))
			{
				objectType = typeof(TBase);
			}

			return base.CreateInstance(provider, objectType, argTypes, args);
		}

		#endregion

	} // End class.
} // End namespace.