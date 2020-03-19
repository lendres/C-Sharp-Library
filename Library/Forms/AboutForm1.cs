using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A simple "about" dialog box.
	///
	/// Extracts information from the assembly to population the labels.
	/// </summary>
	public partial class AboutForm1 : Form
	{
		#region Members

		private string			_reportErrorsTo			= "lendres@fifthrace.com";

		#endregion

		#region Construction

		/// <summary>
		/// Constructor with contact email.
		/// </summary>
		/// <param name="contactemail">Contact email address.</param>
		public AboutForm1(string contactemail)
		{
			Initialize(contactemail, " ");
		}

		/// <summary>
		/// Constructor for replacing the default image.
		/// </summary>
		/// <param name="contactemail">Contact email address.</param>
		/// <param name="imageresourcename">An image to replace default.  Image is located as a resource.</param>
		public AboutForm1(string contactemail, string imageresourcename)
		{
			Initialize(contactemail, imageresourcename);
		}

		/// <summary>
		/// Initialize the form.
		/// </summary>
		/// <param name="contactEmail">Email address of the contact person.</param>
		/// <param name="imageResourceName">Image resource name.</param>
		private void Initialize(string contactEmail, string imageResourceName)
		{
			InitializeComponent();
			_reportErrorsTo = contactEmail;

			System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();

			this.Text = string.Format("About {0}", DigitalProduction.Reflection.Assembly.Title(entryAssembly));

			this.labelProductName.Text		= DigitalProduction.Reflection.Assembly.Product(entryAssembly);
			this.labelVersion.Text			= string.Format("Version {0}", (object)DigitalProduction.Reflection.Assembly.Version(entryAssembly));
			this.labelCopyright.Text		= DigitalProduction.Reflection.Assembly.Copyright(entryAssembly);
			this.labelCompanyName.Text		= DigitalProduction.Reflection.Assembly.Company(entryAssembly);
			this.textBoxxDescription.Text	= DigitalProduction.Reflection.Assembly.Description(entryAssembly);
			Stream manifestResourceStream	= entryAssembly.GetManifestResourceStream(imageResourceName);

			if (manifestResourceStream != null)
			{
				this.pictureBoxLogo.Image = (Image)new Bitmap(manifestResourceStream);
			}
			this.linkReportErrors.Text = this._reportErrorsTo;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Linked clicked event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void linkReportErrors_Clicked(object sender, LinkLabelLinkClickedEventArgs eventArgs)
		{
			Process.Start("mailto:" + this._reportErrorsTo + "?subject=" + DigitalProduction.Reflection.Assembly.Product(System.Reflection.Assembly.GetCallingAssembly()) + "&body=Please describe your bugs and/or comments as accurately as possible.  For bug reporting, attach any required input files.");
		}

		/// <summary>
		/// Ok button event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void buttonOK_Click(object sender, EventArgs eventArgs)
		{
			this.Close();
		}

		#endregion

	} // End class.
} // End namespace.