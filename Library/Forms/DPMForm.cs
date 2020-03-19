using DigitalProduction.Registry;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// DPMForm Class.  this is the base class for all Digital Production Management forms.  It contains
	/// the common code for all applications (e.g. company name).
	///
	/// The event "Install" is provided to allow all class to chain installation call back functions to it
	/// so that each class can define it's installation requirements.  This is primarily used for debugging
	/// purposes, a setup routine should handle the installation of things normally so that they can be
	/// removed with an uninstaller.
	///
	/// If this form is the child of another the name of the form is held in _appname;
	/// </summary>
	public class DPMForm : Form
	{
		#region Members

		private static string						_companyName		= "Digital Production Management";
		private readonly DPMForm					_owner;
		private readonly string						_appName;
		private readonly bool						_isChildForm;

		private readonly WindowStateManager			_windowStateManager;
		private readonly FormWinRegistryAccess		_winRegistryAccess;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor required for form designer.  Do not use this constructor.
		/// </summary>
		protected DPMForm()
		{
		}

		/// <summary>
		/// Constructor applications should use.
		/// </summary>
		/// <param name="applicationName">Name of the application (used as registry name also).</param>
		public DPMForm(string applicationName)
		{
			_owner				= null;
			_appName			= applicationName;
			_isChildForm		= false;

			_winRegistryAccess	= new FormWinRegistryAccess(this);
			_windowStateManager	= new WindowStateManager(_winRegistryAccess);
		}

		/// <summary>
		/// Constructor top level applications should use if the application is for a different company
		/// other than the default.
		/// </summary>
		/// <param name="companyName">Name of the company (used as registry top level name).</param>
		/// <param name="applicationName">Name of the application (used as registry name also).</param>
		public DPMForm(string companyName, string applicationName)
		{
			_companyName		= companyName;
			_owner				= null;
			_appName			= applicationName;
			_isChildForm		= false;

			_winRegistryAccess		= new FormWinRegistryAccess(this);
			_windowStateManager	= new WindowStateManager(_winRegistryAccess);
		}

		/// <summary>
		/// Constructor dialog boxes put up by a parent dialog box (such as the application) should use.
		/// </summary>
		/// <param name="owner">Form that owns this form.</param>
		/// <param name="dialogName">Name of this form (used as registry name also).</param>
		public DPMForm(DPMForm owner, string dialogName)
		{
			_companyName		= owner.CompanyName;
			_owner				= owner;
			_appName			= owner.AppName;
			_isChildForm		= true;

			_winRegistryAccess		= new DialogWinRegistryAccess(this, dialogName);
			_windowStateManager	= new WindowStateManager(_winRegistryAccess);
		}

		#endregion

		#region Disposing

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Get the name of the company.
		/// </summary>
		public string DPMCompanyName
		{
			get
			{
				return _companyName;
			}
		}

		/// <summary>
		/// Get the name of the application.
		/// </summary>
		public string AppName
		{
			get
			{
				return _appName;
			}
		}

		/// <summary>
		/// Returns true if the form is the child of another.
		/// </summary>
		public bool IsChildForm
		{
			get
			{
				return _isChildForm;
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