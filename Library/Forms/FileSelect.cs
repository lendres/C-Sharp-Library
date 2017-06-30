using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Helper functions for selecting a file using a dialog box.
	/// </summary>
	public static class FileSelect
	{
		#region Members

		private static string							_lastDirectory		= "";

		#endregion

		#region Properties

		/// <summary>
		/// Standard string used for all files.
		/// </summary>
		public static string AllFilesFilterString
		{
			get
			{
				return "All files (*.*)|*.*";
			}
		}

		/// <summary>
		/// Standard string used for text files.
		/// </summary>
		public static string CSVFilesFilterString
		{
			get
			{
				return "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
			}
		}

		/// <summary>
		/// Standard string used for text files.
		/// </summary>
		public static string TextFilesFilterString
		{
			get
			{
				return "Text files (*.txt)|*.txt|All files (*.*)|*.*";
			}
		}

		#endregion

		#region Browse for an XML File

		/// <summary>
		/// Use an OpenFileDialog box to get the location of an XML file.  Starting directory is the current
		/// working directory and the title of the dialog box is "Open."
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAnXMLFile(IWin32Window owner)
		{
			return BrowseForAnXMLFile(owner, "Open", "");
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of an XML file.  Starting directory is the current
		/// working directory.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAnXMLFile(IWin32Window owner, string title)
		{
			return BrowseForAnXMLFile(owner, title, "");
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of an XML file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <param name="initialDirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAnXMLFile(IWin32Window owner, string title, string initialDirectory)
		{
			return BrowseForAFile(owner, "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*", title, initialDirectory, false);
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of an XML file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <param name="initialDirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAnXMLFile(IWin32Window owner, string title, string initialDirectory, bool keepCurrentDirectory)
		{
			return BrowseForAFile(owner, "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*", title, initialDirectory, keepCurrentDirectory);
		}

		#endregion

		#region Browse for a File

		/// <summary>
		/// Use an OpenFileDialog box to get the location of a file.  Starting directory is the current
		/// working directory and the title of the dialog box is "Open."
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAFile(IWin32Window owner, string filter)
		{
			return BrowseForAFile(owner, filter, "Open", "", false);
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of a file.  Starting directory is the current
		/// working directory.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAFile(IWin32Window owner, string filter, string title)
		{
			return BrowseForAFile(owner, filter, title, "", false);
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of a file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <param name="initialDirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAFile(IWin32Window owner, string filter, string title, string initialDirectory)
		{
			return BrowseForAFile(owner, filter, title, initialDirectory, false);
		}

		/// <summary>
		/// Use an OpenFileDialog box to get the location of a file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <param name="initialDirectory">Directory to start in.</param>
		/// <param name="restoreDirectory">
		/// If true, the directory from the selected file will be used when the dialog is next opened.  Otherwise, it will be ignored and defaulted to what
		/// folder was current before the dialog was opened.
		/// </param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForAFile(IWin32Window owner, string filter, string title, string initialDirectory, bool restoreDirectory)
		{
			// Create the dialog box.
			OpenFileDialog dialog	= new OpenFileDialog();
			dialog.Title			= title;
			dialog.CheckFileExists	= true;
			dialog.ValidateNames	= true;
			dialog.Multiselect		= false;
			dialog.RestoreDirectory = restoreDirectory;
			dialog.Filter			= filter;
			dialog.FilterIndex		= 1;
			dialog.InitialDirectory	= _lastDirectory;

			// Start in the same directory that the previous file was in (if the file and directory exist).
			if (initialDirectory != "" && System.IO.Directory.Exists(initialDirectory))
			{
				dialog.InitialDirectory = initialDirectory;
			}

			// Get the file.
			DialogResult result = dialog.ShowDialog(owner);


			// If the dialog is canceled, then just get out of here.
			if (result == DialogResult.Cancel)
			{
				return "";
			}
			else
			{
				if (!restoreDirectory)
				{
					_lastDirectory = System.IO.Path.GetDirectoryName(dialog.FileName);
				}
				return dialog.FileName;
			}
		}

		/// <summary>
		/// Use an OpenFileDialog box to select several files.  Starting directory is the current directory.
		/// working directory.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string[] BrowseForMultipleFiles(IWin32Window owner, string filter, string title)
		{
			return BrowseForMultipleFiles(owner, filter, title, "");
		}

		/// <summary>
		/// Use an OpenFileDialog box to select several files.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Files of type"  box in the dialog box.</param>
		/// <param name="title">Title of the OpenFileDialog box.</param>
		/// <param name="initialdirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string[] BrowseForMultipleFiles(IWin32Window owner, string filter, string title, string initialdirectory)
		{
			OpenFileDialog dialog	= new OpenFileDialog();
			dialog.Title			= title;
			dialog.CheckFileExists	= true;
			dialog.ValidateNames	= true;
			dialog.Multiselect		= true;
			dialog.RestoreDirectory = true;
			dialog.Filter			= filter;
			dialog.FilterIndex		= 1;

			// Start in the same directory that the previous file was in (if the file and directory exist).
			if (initialdirectory != "" && System.IO.Directory.Exists(initialdirectory))
			{
				dialog.InitialDirectory = initialdirectory;
			}

			// Get the file.
			DialogResult result = dialog.ShowDialog(owner);

			// If the dialog is canceled, then just get out of here.
			if (result == DialogResult.Cancel)
			{
				return null;
			}
			else
			{
				return dialog.FileNames;
			}
		}

		#endregion

		#region Browse for a New XML File Location

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new XML file.  Starting directory is the current
		/// working directory and the title of the dialog box is "Save As."
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewXMLFileLocation(IWin32Window owner)
		{
			return BrowseForANewXMLFileLocation(owner, "Save As", "");
		}

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new XML file.  Starting directory is the current
		/// working directory.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="title">Title of the SaveFileDialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewXMLFileLocation(IWin32Window owner, string title)
		{
			return BrowseForANewXMLFileLocation(owner, title, "");
		}

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new XML file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="title">Title of the SaveFileDialog box.</param>
		/// <param name="initialdirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewXMLFileLocation(IWin32Window owner, string title, string initialdirectory)
		{
			return BrowseForANewFileLocation(owner, "XML files (*.xml)|*.xml|Text files (*.txt)|*.txt|All files (*.*)|*.*", title, initialdirectory);
		}

		#endregion

		#region Browse for a New File Location

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new file.  Starting directory is the current
		/// working directory and the title of the dialog box is "Save As."
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Saves as file type"  box in the dialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewFileLocation(IWin32Window owner, string filter)
		{
			return BrowseForANewFileLocation(owner, filter, "Save As", "");
		}

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new file.  Starting directory is the current
		/// working directory.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Saves as file type"  box in the dialog box.</param>
		/// <param name="title">Title of the SaveFileDialog box.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewFileLocation(IWin32Window owner, string filter, string title)
		{
			return BrowseForANewFileLocation(owner, filter, title, "");
		}

		/// <summary>
		/// Use a SaveFileDialog box to get a location for a new file.
		/// </summary>
		/// <param name="owner">Owner window.</param>
		/// <param name="filter">The file name filter string, which determines the choices that appear in the "Saves as file type"  box in the dialog box.</param>
		/// <param name="title">Title of the SaveFileDialog box.</param>
		/// <param name="initialDirectory">Directory to start in.</param>
		/// <returns>The new file selected, or "" if a valid file is not selected.</returns>
		public static string BrowseForANewFileLocation(IWin32Window owner, string filter, string title, string initialDirectory)
		{
			SaveFileDialog dialog	= new SaveFileDialog();
			dialog.Title			= title;
			dialog.CheckFileExists	= false;
			dialog.ValidateNames	= true;
			dialog.RestoreDirectory = false;
			dialog.Filter			= filter;
			dialog.FilterIndex		= 1;
			dialog.AddExtension		= true;

			//// Set the default extension as the first extension in the filter string.
			//Regex regex = new Regex(@"(?<Name>[^|]*)\|(?<Extension>[^|]*)\|?");
			//MatchCollection matches = regex.Matches(filter);
			//dialog.DefaultExt		= matches[0].Groups["Extension"].Value;

			// Start in the same directory that the previous file was in (if the file and directory exist).
			if (initialDirectory != "" && System.IO.Directory.Exists(initialDirectory))
			{
				dialog.InitialDirectory = initialDirectory;
			}

			// Get the file.
			DialogResult result = dialog.ShowDialog(owner);

			// If the dialog is canceled, then just get out of here.
			if (result == DialogResult.Cancel)
			{
				return "";
			}
			else
			{
				return dialog.FileName;
			}
		}

		#endregion

	} // End class.
} // End namespace.