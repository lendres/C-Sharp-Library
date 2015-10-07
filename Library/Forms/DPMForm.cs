using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using DigitalProduction.Registry;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// DPMForm Class.  this is the base class for all Digital Production Management forms.  It contains
	/// the common code for all applications (e.g. company name).
	/// 
	/// The event "Install" is provided to allow all class to chain installation call back functions to it
	/// so that each class can define it's installation requirements.  This is primarily used for debugging
	/// purposes, a setup routine should handle the installation of things normally so that they can be
	/// removed with an un-installation routine.
	/// 
	/// If this form is the child of another the name of the form is held in _appname;
	/// </summary>
	public class DPMForm : System.Windows.Forms.Form
	{
		#region Members

		private static string						_companyname		= "Digital Production Management";
		private readonly DPMForm					_owner;
		private readonly string						_appname;
		private readonly bool						_ischildform;

		private readonly WindowStateManager			_windowstatemanager;
		private readonly FormWinRegistryAccess		_winregaccess;

		#endregion

		#region Construction / Destruction / Disposing / Run install.

		/// <summary>
		/// Constructor required for form designer.  Do not use this constructor.
		/// </summary>
		protected DPMForm() {}

		/// <summary>
		/// Constructor applications should use.
		/// </summary>
		/// <param name="applicationname">Name of the application (used as registry name also).</param>
		public DPMForm(string applicationname)
		{
			_owner				= null;
			_appname			= applicationname;
			_ischildform		= false;

			_winregaccess		= new FormWinRegistryAccess(this);
			_windowstatemanager	= new WindowStateManager(_winregaccess);
		}

		/// <summary>
		/// Constructor top level applications should use if the application is for a different company
		/// other than the default.
		/// </summary>
		/// <param name="companyname">Name of the company (used as registry top level name).</param>
		/// <param name="applicationname">Name of the application (used as registry name also).</param>
		public DPMForm(string companyname, string applicationname)
		{
			_companyname		= companyname;
			_owner				= null;
			_appname			= applicationname;
			_ischildform		= false;

			_winregaccess		= new FormWinRegistryAccess(this);
			_windowstatemanager	= new WindowStateManager(_winregaccess);
		}

		/// <summary>
		/// Constructor dialog boxes put up by a parent dialog box (such as the application) should use.
		/// </summary>
		/// <param name="owner">Form that owns this form.</param>
		/// <param name="dialogname">Name of this form (used as registry name also).</param>
		public DPMForm(DPMForm owner, string dialogname)
		{
			_companyname		= owner.CompanyName;
			_owner				= owner;
			_appname			= owner.AppName;
			_ischildform		= true;

			_winregaccess		= new DialogWinRegistryAccess(this, dialogname);
			_windowstatemanager	= new WindowStateManager(_winregaccess);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion

		#region Properties.

		/// <summary>
		/// Get the name of the company.
		/// </summary>
		public string DPMCompanyName
		{
			get
			{
				return _companyname;
			}
		}

		/// <summary>
		/// Get the name of the application.
		/// </summary>
		public string AppName
		{
			get
			{
				return _appname;
			}
		}

		/// <summary>
		/// Returns true if the form is the child of another.
		/// </summary>
		public bool IsChildForm
		{
			get
			{
				return _ischildform;
			}
		}

		/// <summary>
		/// Returns the owner of this form if it is a child of another form.  Otherwise null is returned.
		/// </summary>
		public DPMForm OwnerDPM
		{
			get
			{
				return _owner;
			}
		}

		#endregion

	} // End class.
} // End namespace.