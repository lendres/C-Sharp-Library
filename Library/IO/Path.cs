using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;

namespace DigitalProduction.IO
{
	/// <summary>
	/// Additional path utilities.
	/// </summary>
	public static class Path
	{

		#region Structures

		/// <summary>
		/// Options for controlling what determines if a file name is valid or not.
		/// </summary>
		public struct ValidFileNameOptions
		{
			/// <summary>Specifies if the path is required to exist for the file name to be valid.  The default is false.</summary>
			public bool		RequirePathToExist;
		}

		#endregion

		#region DLL Imports

		[DllImport("kernel32.dll")]
		private static extern int GetDriveType(string drive);

		/// <summary>
		/// Get information about volume.  Imported from kernel32.dll.
		/// </summary>
		/// <param name="PathName">String of the drive letter to get the volume label of.</param>
		/// <param name="VolumeName"></param>
		/// <param name="VolumeNameSize"></param>
		/// <param name="VolumeSerialNumber"></param>
		/// <param name="MaximumComponentLength"></param>
		/// <param name="FileSystemFlags"></param>
		/// <param name="FileSystemName"></param>
		/// <param name="FileSystemNameSize"></param>
		/// <returns>0 if failed, not zero if succeeded.</returns>
		/// <example>
		/// StringBuilder volumename = new StringBuilder(256);
		/// long serialnumber= new long();
		/// long maxcomponetlength = new long();
		/// long systemflags = new long();
		/// StringBuilder systemname = new StringBuilder(256);
		/// long returnvalue= new long();
		/// 
		/// returnvalue = GetVolumeInformation(@"D:\", volumename, 256, serialnumber, maxcomponetlength,	systemflags, systemname, 256);
		/// if (returnvalue != 0) // do something.
		/// else // do nothing.
		/// </example>
		[DllImport("kernel32.dll")]
		public static extern long GetVolumeInformation(string PathName, StringBuilder VolumeName,
					long VolumeNameSize, long VolumeSerialNumber, long MaximumComponentLength,
					long FileSystemFlags, StringBuilder FileSystemName, long FileSystemNameSize);

		#endregion

		#region Static functions

		#region Files

		/// <summary>
		/// Replace the file extension on a path with a new extension.
		/// </summary>
		/// <param name="path">Path (file name with extension, with or with out directory)</param>
		/// <param name="newExtension">New file extension.</param>
		public static string ChangeFileExtension(string path, string newExtension)
		{
			string newPath	= System.IO.Path.GetDirectoryName(path);
			newPath			= System.IO.Path.Combine(newPath, System.IO.Path.GetFileNameWithoutExtension(path));
			if (newExtension[0] != '.')
			{
				newPath			+= ".";
			}
			newPath			+= newExtension;
			return newPath;
		}

		/// <summary>
		/// Insert a subdirectory name between the directory part and the file part of the path.
		/// </summary>
		/// <param name="path">Path (directory and file name).</param>
		/// <param name="subdirectory">Name of the subdirectory to insert.</param>
		public static string InsertSubdirectory(string path, string subdirectory)
		{
			string newPath	= System.IO.Path.GetDirectoryName(path);
			newPath			= System.IO.Path.Combine(newPath, subdirectory);
			newPath			= System.IO.Path.Combine(newPath, System.IO.Path.GetFileName(path));
			return newPath;
		}

		/// <summary>
		/// Checks that a path is savable.  It has to be a valid file name, the directory must exist, and
		/// it cannot already be an existing, write only file.
		/// </summary>
		/// <param name="path"></param>
		public static bool PathIsWritable(string path)
		{
			// First thing to check is that we have a file name.
			bool saveable = IsValidFileName(path) == ValidFileNameResult.Valid;

			if (saveable)
			{
				// The next thing to check is that the directory exists.
				if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
				{
					saveable = false;
				}
				else
				{
					// If the file already exists, we must be able to overwrite it.
					if (System.IO.File.Exists(path))
					{
						// Now check to make sure the file is not locked.
						System.IO.FileAttributes fileAttributes = System.IO.File.GetAttributes(path);
						if ((fileAttributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
						{
							saveable = false;
						}
					}
				}
			}

			return saveable;
		}

		#endregion

		#region Directories

		/// <summary>
		/// Get directory from the path.  Checks that the directory exists also.
		/// </summary>
		/// <param name="path">Path to get the directory from.</param>
		/// <returns>Directory from the path if it exists, otherwise a blank string.</returns>
		public static string GetDirectory(string path)
		{
			path = path.Trim();

			if (path != "")
			{
				try
				{
					string initdir = System.IO.Path.GetDirectoryName(path);
					if (Directory.Exists(initdir))
					{
						return initdir;
					}
				}
				catch
				{
				}
			}

			return "";
		}

		/// <summary>
		/// Alters a directory name (given as a string) in a manner that is similar to what the DOS 
		/// command "CD.." does to the current directory at the DOS prompt (gives the parent directory).
		/// </summary>
		/// <param name="directory">The starting directory as a string.</param>
		/// <returns>The new directory name as a string.</returns>
		public static string ChangeDirectoryDotDot(string directory)
		{
			return ChangeDirectoryDotDot(directory, 1);
		}

		/// <summary>
		/// Alters a directory name (given as a string) in a manner that is similar to what the DOS 
		/// command "CD.." does to the current directory at the DOS prompt (gives the parent directory).
		/// </summary>
		/// <param name="directory">The starting directory as a string.</param>
		/// <param name="levels">Number of levels to move up.</param>
		/// <returns>The new directory name as a string.</returns>
		public static string ChangeDirectoryDotDot(string directory, int levels)
		{
			// Ensure that the path is in a format we expect, i.e. it should not end in "\".
			if (directory.EndsWith(@"\"))
			{
				directory = directory.Remove(directory.Length-1);
			}

			// Need to do some error handling here.
			// Check to make sure there are enough directories to back out of.
			// Check to make sure the path does not contain invalid characters.

			// Remove the directories.
			for (int i = 0; i < levels; i++)
			{
				// Remove all the text from the last back slash to the end.
				directory = directory.Remove(directory.LastIndexOf(@"\"));
			}

			// Return the result.
			return directory;
		}

		/// <summary>
		/// Copies a directory from one location to another.  Does not copy subdirectories.
		/// </summary>
		/// <param name="sourcedirname">Path to the source directory.</param>
		/// <param name="destdirname">Path to the destination directory.</param>
		public static void DirectoryCopy(string sourcedirname, string destdirname)
		{
			DirectoryCopy(sourcedirname, destdirname, false, false, new List<string>());
		}

		/// <summary>
		/// Copies a directory from one location to another.
		/// </summary>
		/// <param name="sourcedirname">Path to the source directory.</param>
		/// <param name="destdirname">Path to the destination directory.</param>
		/// <param name="copysubdirs">Specifies if the subdirectories should be copied.</param>
		public static void DirectoryCopy(string sourcedirname, string destdirname, bool copysubdirs)
		{
			DirectoryCopy(sourcedirname, destdirname, copysubdirs, false, new List<string>());
		}

		/// <summary>
		/// Copies a directory from one location to another.
		/// </summary>
		/// <param name="sourcedirname">Path to the source directory.</param>
		/// <param name="destdirname">Path to the destination directory.</param>
		/// <param name="copysubdirs">Specifies if the subdirectories should be copied.</param>
		/// <param name="overwrite">Overwrite existing files.</param>
		public static void DirectoryCopy(string sourcedirname, string destdirname, bool copysubdirs, bool overwrite)
		{
			DirectoryCopy(sourcedirname, destdirname, copysubdirs, overwrite, new List<string>());
		}

		/// <summary>
		/// Copies a directory from one location to another.
		/// </summary>
		/// <param name="sourcedirname">Path to the source directory.</param>
		/// <param name="destdirname">Path to the destination directory.</param>
		/// <param name="copysubdirs">Specifies if the subdirectories should be copied.</param>
		/// <param name="overwrite">Overwrite existing files.</param>
		/// <param name="excludedfiles">A list of files not to copy.</param>
		/// <remarks>
		///		Original code taken from the MSDN library.
		///		url: http://msdn.microsoft.com/en-us/library/bb762914.aspx
		///		Modified to provided additional functionality.
		/// </remarks>
		public static void DirectoryCopy(string sourcedirname, string destdirname, bool copysubdirs, bool overwrite, List<string> excludedfiles)
		{
			DirectoryInfo	dir		= new DirectoryInfo(sourcedirname);
			DirectoryInfo[] dirs	= dir.GetDirectories();

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourcedirname);
			}

			// If the destination directory does not exist, create it.
			if (!Directory.Exists(destdirname))
			{
				Directory.CreateDirectory(destdirname);
			}

			// Get the file contents of the directory to copy.
			FileInfo[] files = dir.GetFiles();

			foreach (FileInfo file in files)
			{
				// Create the path to the new copy of the file.
				string temppath = System.IO.Path.Combine(destdirname, file.Name);

				// Copy the file.
				if (!excludedfiles.Contains(file.Name))
				{
					file.CopyTo(temppath, overwrite);
				}
			}

			// If copysubdirs is true, copy the subdirectories.
			if (copysubdirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					// Create the subdirectory.
					string temppath = System.IO.Path.Combine(destdirname, subdir.Name);

					// Copy the subdirectories.
					DirectoryCopy(subdir.FullName, temppath, copysubdirs, overwrite, excludedfiles);
				}
			}
		}

		/// <summary>
		/// Creates a new directory in the user's temporary folder.  The subFolder is used to organize
		/// several temporary directories.  The final result will be similar in form to:
		/// C:\Users\user\AppData\Local\Temp\subFolder\temp.tmp\
		/// </summary>
		public static string GetTemporaryDirectory(string subFolder)
		{
			string tempDirectory = System.IO.Path.Combine(System.IO.Path.GetTempPath(), subFolder, System.IO.Path.GetRandomFileName());
			Directory.CreateDirectory(tempDirectory);
			return tempDirectory;
		}

		/// <summary>
		/// Creates a new directory in the user's temporary folder.
		/// </summary>
		public static string GetTemporaryDirectory()
		{
			// The Path.Combine will eat the blank string.
			return GetTemporaryDirectory("");
		}

		#endregion

		#region Drives

		/// <summary>
		/// Returns the type of drive represented by the string.
		/// </summary>
		/// <param name="drive">String of the drive letter to get the type of drive of.</param>
		public static DriveType DiskDriveType(string drive)
		{
			return (DriveType)GetDriveType(drive);
		}

		/// <summary>
		/// Returns the volume name of a disk drive.
		/// </summary>
		/// <param name="drive">String of the drive letter to get the volume label of.</param>
		public static string DiskDriveName(string drive)
		{
			StringBuilder volumename	= new StringBuilder(256);
			long serialnumber			= new long();
			long maxcomponetlength		= new long();
			long systemflags			= new long();
			StringBuilder systemname	= new StringBuilder(256);
			long returnvalue			= new long();

			returnvalue = GetVolumeInformation(drive, volumename, 256, serialnumber, maxcomponetlength, systemflags, systemname, 256);

			if (returnvalue != 0)
			{
				return volumename.ToString();
			}
			else
			{
				return "";
			}
		}

		#endregion

		#region Valid File Name

		/// <summary>
		/// Checks to insure that a file name passes the criteria to be valid.
		/// </summary>
		/// <param name="file">File name to check.</param>
		/// <returns>A ValidFileNameResult result that indicates if the file name is valid, or if not, what the error was.</returns>
		public static ValidFileNameResult IsValidFileName(string file)
		{
			return IsValidFileName(file, new ValidFileNameOptions());
		}

		/// <summary>
		/// Checks to insure that a file name passes the criteria to be valid.
		/// </summary>
		/// <param name="file">File name to check.</param>
		/// <param name="options">Options to controlling what determines if a file name is valid or not.</param>
		/// <returns>A ValidFileNameResult result that indicates if the file name is valid, or if not, what the error was.</returns>
		public static ValidFileNameResult IsValidFileName(string file, ValidFileNameOptions options)
		{
			file = file.Trim();
			string filename = System.IO.Path.GetFileName(file);

			// A file name was not provided.
			if (filename.Length == 0)
			{
				return ValidFileNameResult.ZeroLength;
			}

			// The file name is too long.
			if (file.Length > 255)
			{
				return ValidFileNameResult.TooLong;
			}

			// Check for invalid characters.
			// I would prefer to use the Path.GetInvalidPathChars() method, but it doesn't seem to handle all cases,
			// for example, the "*" is not included in the returned values.
			//
			// For reference the call is:
			// char[] invalidpathchars2 = Path.GetInvalidPathChars();
			char[] invalidpathchars = { '\"', '*', '/', ':', '<', '>', '?', '\\', '|' };
			if (filename.IndexOfAny(invalidpathchars) > 0)
			{
				return ValidFileNameResult.InvalidCharacters;
			}

			// If the path is required to exist, we will check that now.
			if (options.RequirePathToExist)
			{
				string path = System.IO.Path.GetDirectoryName(file);
				if (!Directory.Exists(path))
				{
					return ValidFileNameResult.PathDoesNotExist;
				}
			}

			// The file name cannot start with a "dot."
			if (filename.StartsWith("."))
			{
				return ValidFileNameResult.FileNameNotProvided;
			}

			// Check to see if the file name starts with any device names.
			string[] devicenames =
			{
				"CLOCK$",	"AUX",	"CON",	"NUL",	"PRN",	"COM1",	"COM2",	"COM3",	"COM4",	"COM5",	"COM6",	"COM7", 
				"COM8",		"COM9",	"LPT1",	"LPT2",	"LPT3",	"LPT4",	"LPT5",	"LPT6",	"LPT7",	"LPT8",	"LPT9"
			};

			string filenamenoextension = System.IO.Path.GetFileNameWithoutExtension(filename);
			foreach (string name in devicenames)
			{
				// If the file name is shorter or longer than the device name, it is ok.
				if (filenamenoextension.Length < name.Length || filenamenoextension.Length > name.Length)
				{
					continue;
				}

				if (name == filenamenoextension.ToUpper())
				{
					return ValidFileNameResult.DeviceName;
				}
			}

			return ValidFileNameResult.Valid;
		}

		#endregion

		#endregion

	} // End class.
} // End namespace.