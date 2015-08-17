using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Helper functions for selecting a directory using a dialog box.
	/// </summary>
	public static class DirectorySelect
	{
		#region Members

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		static DirectorySelect()
		{
		}

		#endregion

		#region Properties

		#endregion

		#region Methods

		/// <summary>
		/// Run a directory selection dialog.
		/// </summary>
		/// <param name="title">Title to display in the dialog box.</param>
		/// <param name="showNewFolder">Specifies if the "New Folder" button is shown.</param>
		/// <param name="startingDirectory">Directory to start browsing from.</param>
		public static string SelectDirectory(string title, bool showNewFolder, string startingDirectory)
		{
			// Save the current directory.
//string currentDirectory = System.IO.Directory.GetCurrentDirectory();
//string currentDirectory = Environment.CurrentDirectory;
//			Environment.GetFolderPath(Environment.SpecialFolder.d

			FolderBrowserDialog dialog	= new FolderBrowserDialog();
			dialog.Description			= title;
			dialog.ShowNewFolderButton	= showNewFolder;

			// Start in the previous directory (if it exists).
			if (System.IO.Directory.Exists(startingDirectory))
			{
				dialog.SelectedPath = startingDirectory;
			}

			string selectedPath	= "";
			DialogResult result	= dialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				 selectedPath = dialog.SelectedPath;
			}

			// Restore the current directory.
//System.IO.Directory.SetCurrentDirectory(currentDirectory);
//Environment.CurrentDirectory = currentDirectory;

			return selectedPath;
		}

		/// <summary>
		/// Run a directory selection dialog.
		/// </summary>
		/// <param name="title">Title to display in the dialog box.</param>
		/// <param name="showNewFolder">Specifies if the "New Folder" button is shown.</param>
		/// <param name="textBoxToPopulateWithPath">TextBox to retrieve the initial starting directory from and to populate with the result.</param>
		public static string SelectDirectory(string title, bool showNewFolder, TextBox textBoxToPopulateWithPath)
		{
			// Start in the directory in the TextBox, if it exists.
			string directory = "";
			if (System.IO.Directory.Exists(textBoxToPopulateWithPath.Text))
			{
				directory = textBoxToPopulateWithPath.Text;
			}

			// Run the directory selection.
			directory = SelectDirectory(title, showNewFolder, directory);

			// Populate the TextBox, if a directory was selected.
			if (directory != "")
			{
				textBoxToPopulateWithPath.Text = directory;
			}

			// Return the result.
			return directory;
		}

		#endregion

	} // End class.
} // End namespace.