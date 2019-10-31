/*
 * FolderSelect.cs: A folder browser example.
 * Version 1.0
 * Copyright (C) 2001 Chris Warner
 *
 * You are free to use this in any personal or commercial application
 * so long as none of these comments are changed or removed from this file.
 *
 * E-mail: jabrwoky@pacbell.net
 *
 *
 * Modifications by Lance A. Endres May 23, 2005.
 * +Removed controls from panels (annoying behavior in design view and didn't accomplish anything).
 * +Changed the Select and Cancel buttons position to stay near the center of the form, slightly
 *	offset, by adding a resize event handler.
 * +Changed the text box so that it cannot be manually changed (that didn't seem like a good behavior for
 *	the form).
 * +Changed the catches in the try-catch block so that it didn't write to the console.
 * +A few other minor changes.
 * +Some code stylization changes.  This is not intended to anyway disguise the fact that this code was written by
 *	above author.  For credit is given where it is deserved.  However, it does not appear that this code was written
 *	as a finished product so much as an example, so some changes where made (like renaming controls) for clarification.
 * */
using System;
using System.IO;
using System.Windows.Forms;

namespace DigitalProduction.Forms
{
	/// <summary> Class FolderSelect
	/// <para>An example on how to build a folder browser dialog window using C# and the .Net framework.</para>
	/// </summary>
	public class FolderSelectForm : System.Windows.Forms.Form
	{
		#region Members

		private static string						driveLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private DirectoryInfo						_folder;
		private System.Windows.Forms.TreeView		tvwDirectories;
		private System.Windows.Forms.Button			btnSelect;
		private System.Windows.Forms.Button			btnCancel;
		private System.Windows.Forms.Label			lblPath;
		private System.Windows.Forms.TextBox		txtbxPath;
		private System.Windows.Forms.ImageList		imageList;
		private System.ComponentModel.IContainer	components;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public FolderSelectForm()
		{
			InitializeComponent();

			// Initialize the treeview.
			FillTree();

			// Initial position of select and cancel buttons.
			PositionButtons();
		}

		#endregion

		#region Disposing

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Directory Searching Functions

		/// <summary>This method is used to initially fill the treeView control with a list
		/// of available drives from which you can search for the desired folder.
		/// </summary>
		private void FillTree()
		{
			DirectoryInfo directory;
			string sCurPath = "";

			// Clear out the old values.
			tvwDirectories.Nodes.Clear();

			// Loop through the drive letters and find the available drives.
			foreach (char c in driveLetters)
			{
				sCurPath = c + ":\\";
				try
				{
					// Get the directory information for this path.
					directory = new DirectoryInfo(sCurPath);

					// If the retrieved directory information points to a valid
					// directory or drive in this case, add it to the root of the 
					// treeView.
					if (directory.Exists == true)
					{
						TreeNode newNode = new TreeNode(directory.FullName);

						// Add the new node to the root level.
						tvwDirectories.Nodes.Add(newNode);

						// Scan for any sub folders on this drive.
						GetSubDirectories(newNode);
					}
				}
				catch (Exception ex)
				{
					DisplayError(ex);
				}
			}
		}

		/// <summary>This function will scan the specified parent for any subfolders 
		/// if they exist.  To minimize the memory usage, we only scan a single 
		/// folder level down from the existing, and only if it is needed.
		/// </summary>
		/// <param name="parent">The parent folder in which to search for sub-folders.</param>
		private void GetSubDirectories(TreeNode parent)
		{
			DirectoryInfo directory;
			try
			{
				// if we have not scanned this folder before
				if (parent.Nodes.Count == 0)
				{
					directory = new DirectoryInfo(parent.FullPath);
					foreach (DirectoryInfo dir in directory.GetDirectories())
					{
						TreeNode newNode = new TreeNode(dir.Name);
						parent.Nodes.Add(newNode);
					}
				}

				// Now that we have the children of the parent, see if they
				// have any child members that need to be scanned.  Scanning 
				// the first level of sub folders insures that you properly 
				// see the '+' or '-' expanding controls on each node that represents
				// a sub folder with it's own children.
				foreach (TreeNode node in parent.Nodes)
				{
					// If we have not scanned this node before.
					if (node.Nodes.Count == 0)
					{
						// get the folder information for the specified path.
						directory = new DirectoryInfo(node.FullPath);

						// check this folder for any possible sub-folders
						foreach (DirectoryInfo dir in directory.GetDirectories())
						{
							// Make a new TreeNode and add it to the treeView.
							TreeNode newNode = new TreeNode(dir.Name);
							node.Nodes.Add(newNode);
						}
					}
				}
			}
			catch (Exception ex)
			{
				DisplayError(ex);
			}
		}

		/// <summary> Fixes the path for display.
		/// 
		/// For some reason, the treeView will only work with paths constructed like the following example.
		/// "c:\\Program Files\Microsoft\...".  What this method does is strip the leading "\\" next to the drive
		/// letter.
		/// </summary>
		/// <param name="node">The folder that needs it's path fixed for display.</param>
		private string FixPath(TreeNode node)
		{
			string sRet = "";
			try
			{
				sRet = node.FullPath;
				int index = sRet.IndexOf("\\\\");
				if (index > 1)
				{
					sRet = node.FullPath.Remove(index, 1);
				}
			}
			catch (Exception ex)
			{
				DisplayError(ex);
			}
			return sRet;
		}

		/// <summary>
		/// Displays a message box with the error message.
		/// </summary>
		/// <param name="ex">Exception that occurred.</param>
		private void DisplayError(Exception ex)
		{
			// Cannot show a message box because if the plus button is clicked to extend the tree the error
			// keeps occurring and continues to display the message box over and over.
			//MessageBox.Show(this, ex.Message, this.Text + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion

		#region Windows Form Designer Generated Code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FolderSelectForm));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.tvwDirectories = new System.Windows.Forms.TreeView();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblPath = new System.Windows.Forms.Label();
			this.txtbxPath = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tvwDirectories
			// 
			this.tvwDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tvwDirectories.ImageList = this.imageList;
			this.tvwDirectories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.tvwDirectories.Location = new System.Drawing.Point(8, 49);
			this.tvwDirectories.Name = "tvwDirectories";
			this.tvwDirectories.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
																					   new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
																																										  new System.Windows.Forms.TreeNode("Node1", new System.Windows.Forms.TreeNode[] {
																																																															 new System.Windows.Forms.TreeNode("Node2")})})});
			this.tvwDirectories.SelectedImageIndex = 1;
			this.tvwDirectories.Size = new System.Drawing.Size(280, 239);
			this.tvwDirectories.TabIndex = 2;
			this.tvwDirectories.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
			this.tvwDirectories.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			// 
			// btnSelect
			// 
			this.btnSelect.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSelect.Location = new System.Drawing.Point(63, 297);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.Size = new System.Drawing.Size(75, 28);
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "&Select";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(159, 297);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 28);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "&Cancel";
			// 
			// lblPath
			// 
			this.lblPath.Location = new System.Drawing.Point(8, 8);
			this.lblPath.Name = "lblPath";
			this.lblPath.Size = new System.Drawing.Size(260, 23);
			this.lblPath.TabIndex = 0;
			this.lblPath.Text = "Full Path:";
			// 
			// txtbxPath
			// 
			this.txtbxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtbxPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtbxPath.Location = new System.Drawing.Point(8, 24);
			this.txtbxPath.Name = "txtbxPath";
			this.txtbxPath.ReadOnly = true;
			this.txtbxPath.Size = new System.Drawing.Size(280, 20);
			this.txtbxPath.TabIndex = 1;
			this.txtbxPath.TabStop = false;
			this.txtbxPath.Text = "";
			// 
			// FolderSelect
			// 
			this.AcceptButton = this.btnSelect;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(296, 334);
			this.ControlBox = false;
			this.Controls.Add(this.txtbxPath);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.tvwDirectories);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblPath);
			this.DockPadding.All = 6;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(230, 300);
			this.Name = "FolderSelect";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select a Folder";
			this.Resize += new System.EventHandler(this.FolderSelect_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		#region Treeview Event Handlers

		/// <summary>
		/// <para>Before we select a tree node we want to make sure that we scan the soon to be selected
		/// tree node for any sub-folders.  this insures proper tree construction on the fly.</para>
		/// </summary>
		/// <param name="sender">The object that invoked this event</param>
		/// <param name="eventArgs">The TreeViewCancelEventArgs event arguments.</param>
		/// <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/>
		/// <see cref="System.Windows.Forms.TreeView"/>
		private void treeView1_BeforeSelect(object sender, System.Windows.Forms.TreeViewCancelEventArgs eventArgs)
		{
			// Get the sub-folders for the selected node.
			GetSubDirectories(eventArgs.Node);

			// Update the user selection text box.
			txtbxPath.Text = FixPath(eventArgs.Node);

			// Get it's Directory info.
			_folder = new DirectoryInfo(eventArgs.Node.FullPath);
		}

		/// <summary>
		/// <para>Before we expand a tree node we want to make sure that we scan the soon to be expanded
		/// tree node for any sub-folders.  This insures proper tree construction on the fly.</para>
		/// </summary>
		/// <param name="sender">The object that invoked this event.</param>
		/// <param name="eventArgs">The TreeViewCancelEventArgs event arguments.</param>
		/// <see cref="System.Windows.Forms.TreeViewCancelEventArgs"/>
		/// <see cref="System.Windows.Forms.TreeView"/>
		private void treeView1_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs eventArgs)
		{
			// Get the sub-folders for the selected node.
			GetSubDirectories(eventArgs.Node);

			// Update the user selection text box.
			txtbxPath.Text = FixPath(eventArgs.Node);

			// Get it's Directory info.
			_folder = new DirectoryInfo(eventArgs.Node.FullPath);
		}

		/// <summary>
		/// Handles the resizing of the form.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguements.</param>
		private void FolderSelect_Resize(object sender, System.EventArgs eventArgs)
		{
			PositionButtons();
		}

		/// <summary>
		/// Adjusts select and cancel buttons so that they stay in the middle.  Adjusts
		/// the X position only.
		/// </summary>
		private void PositionButtons()
		{
			// Move the select button.
			System.Drawing.Point loc = this.btnSelect.Location;
			loc.X = this.Width / 2 - this.btnSelect.Width - 8;
			this.btnSelect.Location = loc;

			// Move the cancel button.
			loc = this.btnCancel.Location;
			loc.X = this.Width / 2 + 8;
			this.btnCancel.Location = loc;
		}

		#endregion

		#region Button Click Handlers

		/// <summary>
		/// <para>This method cancels the folder browsing.</para>
		/// </summary>
		private void cancelBtn_Click(object sender, System.EventArgs eventArgs)
		{
			_folder = null;
			this.Close();
		}

		/// <summary>
		/// This method accepts which ever folder is selected and closes this application 
		/// with a DialogResult.OK result if you invoke this form though Form.ShowDialog().  
		/// In this example this method simply looks up the selected folder, and presents the 
		/// user with a message box displaying the name and path of the selected folder.
		/// </summary>
		private void selectBtn_Click(object sender, System.EventArgs eventArgs)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		#endregion

		#region Access Functions

		/// <summary>
		/// A method to retrieve the short name for the selected folder.
		/// </summary>
		public string FolderName
		{
			get
			{
				return ((_folder != null && _folder.Exists))? _folder.Name : null;
			}
		}

		/// <summary>
		/// Retrieve the full path for the selected folder.
		/// <seealso cref="FolderSelectForm.FixPath"/>
		/// </summary>
		public string FullPath
		{
			get
			{
				return ((_folder != null && _folder.Exists && tvwDirectories.SelectedNode != null))? FixPath(tvwDirectories.SelectedNode) : null;
			}
		}

		/// <summary>
		/// Retrieve the full DirectoryInfo object associated with the selected folder.  Note
		/// that this will not have the corrected full path string.
		/// <see cref="System.IO.DirectoryInfo"/>
		/// </summary>
		public DirectoryInfo Info
		{
			get
			{
				return ((_folder != null && _folder.Exists))? _folder : null;
			}
		}

		#endregion

	} // End class.
} // End namespace.