using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A control for selecting multiple files and listing them in a ListBox.
	/// </summary>
	public partial class MultipleFileSelectControl : UserControl
	{
		#region Members

		private string				_filterString;
		private string				_browseForFilesTitle;
		private string				_initialDirectory;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public MultipleFileSelectControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultipleFileSelectControl(string filterString)
		{
			InitializeComponent();
			_filterString				= filterString;
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public MultipleFileSelectControl(string filterString, string browsingTitle)
		{
			InitializeComponent();
			_filterString				= filterString;
			_browseForFilesTitle		= browsingTitle;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Filter string for file browsing.
		/// </summary>
		public string FilterString
		{
			get
			{
				return _filterString;
			}

			set
			{
				_filterString = value;
			}
		}

		/// <summary>
		/// Title for browsing for files.
		/// </summary>
		public string BrowseForFilesTitle
		{
			get
			{
				return _browseForFilesTitle;
			}

			set
			{
				_browseForFilesTitle = value;
			}
		}

		/// <summary>
		/// Initial directory for file browsing.
		/// </summary>
		public string InitialDirectory
		{
			get
			{
				return _initialDirectory;
			}

			set
			{
				_initialDirectory = value;
			}
		}

		/// <summary>
		/// Change the name of the GroupBox around the controls.
		/// </summary>
		public string GroupBoxText
		{
			get
			{
				return this.filesGroupBox.Text;
			}

			set
			{
				this.filesGroupBox.Text = value;
			}
		}

		/// <summary>
		/// Items selected in the form.
		/// </summary>
		public string[] SelectedFiles
		{
			get
			{
				ListBox.SelectedObjectCollection selectedItems = this.filesListBox.SelectedItems;

				int size		= selectedItems.Count;
				string[] files	= new string[size];

				for (int i = 0; i < size; i++)
				{
					files[i] = selectedItems[i].ToString();
				}

				return files;
			}
		}

		/// <summary>
		/// Specifies if files have been selected.
		/// </summary>
		public bool FilesHaveBeenSelected
		{
			get
			{
				return this.filesListBox.Items.Count > 0;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Browse for files event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void selectFilesButton_Click(object sender, EventArgs e)
		{
			string[] files = FileSelect.BrowseForMultipleFiles(this, _filterString, _browseForFilesTitle, _initialDirectory);

			if (files != null)
			{
				this.filesListBox.Items.AddRange(files);
			}
		}

		/// <summary>
		/// Remove selected items from ListBox.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void removeFilesButton_Click(object sender, EventArgs e)
		{
			ListBox.SelectedIndexCollection selectedIndices = this.filesListBox.SelectedIndices;
			for (int i = this.filesListBox.SelectedIndices.Count-1; i >= 0; i--)
			{
				this.filesListBox.Items.RemoveAt(selectedIndices[i]);
			}
		}

		#endregion

		#region Methods

		private void SetControls()
		{
			bool enabled					= this.filesListBox.Items.Count > 0;
			this.removeFilesButton.Enabled	= enabled;
		}

		#endregion

	} // End class.
} // End namespace.