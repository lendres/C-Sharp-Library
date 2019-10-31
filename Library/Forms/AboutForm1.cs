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
		#region Members.

		private string _reporterrorsto = "lendres@fifthrace.com";

		#endregion

		#region Construction.

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

		private void Initialize(string contactemail, string imageresourcename)
		{
			InitializeComponent();
			_reporterrorsto = contactemail;

			System.Reflection.Assembly entryassembly = System.Reflection.Assembly.GetEntryAssembly();

			this.Text = string.Format("About {0}", DigitalProduction.Reflection.Assembly.Title(entryassembly));

			this.lblProductName.Text		= DigitalProduction.Reflection.Assembly.Product(entryassembly);
			this.lblVersion.Text			= string.Format("Version {0}", (object)DigitalProduction.Reflection.Assembly.Version(entryassembly));
			this.lblCopyright.Text			= DigitalProduction.Reflection.Assembly.Copyright(entryassembly);
			this.lblCompanyName.Text		= DigitalProduction.Reflection.Assembly.Company(entryassembly);
			this.txtbxDescription.Text		= DigitalProduction.Reflection.Assembly.Description(entryassembly);
			Stream manifestResourceStream	= entryassembly.GetManifestResourceStream(imageresourcename);

			if (manifestResourceStream != null)
			{
				this.picboxLogo.Image = (Image)new Bitmap(manifestResourceStream);
			}
			this.lnkReportErrors.Text = this._reporterrorsto;
		}

		#endregion

		#region Event handlers.

		private void lnkReportErrors_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("mailto:" + this._reporterrorsto + "?subject=" + DigitalProduction.Reflection.Assembly.Product(System.Reflection.Assembly.GetCallingAssembly()) + "&body=Please describe your bugs and/or comments as accurately as possible.  For bug reporting, attach any required input files.");
		}

		private void btnOK_Click(object sender, EventArgs eventArgs)
		{
			this.Close();
		}

		#endregion

	} // End class.
} // End namespace.