using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Imaging;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A DataGridView with copy and pasting of cells enabled.
	/// </summary>
	public class ExtendedDataGridView : DataGridView
	{
		#region Events

		/// <summary>
		/// Template for function which performs insertion of new row of data.
		/// </summary>
		public delegate							void InsertDelegate(int rowindex);

		/// <summary>
		/// Insert event.
		/// </summary>
		public event InsertDelegate				Insertion								= null;

		#endregion

		#region Members

		private ContextMenuStrip				contextmnuCutCopyPaste;
		private ToolStripMenuItem				mnuCut;
		private ToolStripMenuItem				mnuCopy;
		private ToolStripMenuItem				mnuPaste;
		private ToolStripMenuItem				mnuDelete;
		private ToolStripSeparator				mnuContextSeparator1;
		private ToolStripMenuItem				mnuInsert;

		private int								_row;
		private int								_column;
		private	bool							_pastetoselectedCells					= false;
		private bool							_contextMenuEnabled						= true;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ExtendedDataGridView()
		{
			InitializeComponent();

			// For some reason, the first right mouse button click does not display the context menu.  This hack seems to fix it, but it isn't known why
			// the menu doesn't get displayed.  The events seem to fire, even the "ContextMenuStripOpened" event.  But no menu.
			this.contextmnuCutCopyPaste.Visible = true;
			this.contextmnuCutCopyPaste.Visible = false;

			// More context menu setup.
			this.ContextMenuStrip	= this.contextmnuCutCopyPaste;
			this.ClipboardCopyMode	= DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

			// We have to hood into the opening event so we can know if we are allowing the "Insert" menu item and whether or not the "Paste"
			// item is enabled.
			this.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			
			// Handling data entry errors (bad formats, et cetera).
			this.DataError += ExtendedDataGridView_DataError;
		}

		/// <summary>
		/// Initialize controls.
		/// </summary>
		private void InitializeComponent()
		{
			this.contextmnuCutCopyPaste = new System.Windows.Forms.ContextMenuStrip();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.contextmnuCutCopyPaste.SuspendLayout();

			this.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridViewWithCopyPaste_KeyDown);
			//
			// mnuCut
			//
			this.mnuCut				= new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCut.Name		= "mnuCut";
			this.mnuCut.Size		= new System.Drawing.Size(102, 22);
			this.mnuCut.Text		= "Cut";
			this.mnuCut.Click		+= new System.EventHandler(this.mnuCut_Click);
			//
			// mnuCopy
			//
			this.mnuCopy			= new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCopy.Name		= "mnuCopy";
			this.mnuCopy.Size		= new System.Drawing.Size(102, 22);
			this.mnuCopy.Text		= "&Copy";
			this.mnuCopy.Click		+= new System.EventHandler(this.mnuCopy_Click);
			//
			// mnuPaste
			//
			this.mnuPaste			= new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPaste.Name		= "mnuPaste";
			this.mnuPaste.Size		= new System.Drawing.Size(102, 22);
			this.mnuPaste.Text		= "&Paste";
			this.mnuPaste.Click		+= new System.EventHandler(this.mnuPaste_Click);
			//
			// mnuDelete
			//
			this.mnuDelete			= new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDelete.Name		= "mnuDelete";
			this.mnuDelete.Size		= new System.Drawing.Size(102, 22);
			this.mnuDelete.Text		= "&Delete";
			this.mnuDelete.Click	+= new System.EventHandler(this.mnuDelete_Click);
			// 
			// mnuContextSeparator1
			// 
			this.mnuContextSeparator1		= new System.Windows.Forms.ToolStripSeparator();
			this.mnuContextSeparator1.Name	= "mnuContextSeparator1";
			this.mnuContextSeparator1.Size	= new System.Drawing.Size(149, 6);
			//
			// mnuInsert
			//
			this.mnuInsert			= new System.Windows.Forms.ToolStripMenuItem();
			this.mnuInsert.Name		= "mnuInsert";
			this.mnuInsert.Size		= new System.Drawing.Size(102, 22);
			this.mnuInsert.Text		= "&Insert";
			this.mnuInsert.Click	+= new System.EventHandler(this.mnuInsert_Click);

			this.contextmnuCutCopyPaste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {this.mnuCut, this.mnuCopy, this.mnuPaste, this.mnuDelete, this.mnuContextSeparator1, this.mnuInsert});
			this.contextmnuCutCopyPaste.Name = "contextmnuCutCopyPaste";
			this.contextmnuCutCopyPaste.Size = new System.Drawing.Size(82, 48);

			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.contextmnuCutCopyPaste.ResumeLayout(false);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Specifies if the context menu for copy/cut/paste is enabled.
		/// </summary>
		public bool ContextMenuEnabled
		{
			get
			{
				return _contextMenuEnabled;
			}

			set
			{
				if (value == true)
				{
					this.ContextMenuStrip = this.contextmnuCutCopyPaste;
				}
				else
				{
					this.ContextMenuStrip = null;
				}
				_contextMenuEnabled = value;
			}
		}

		#endregion

		#region DataGridView Event Handlers

		/// <summary>
		/// Context menu opening.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event handlers.</param>
		void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SetContextMenuState();
		}

		/// <summary>
		/// Mouse click in a cell handlers.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event handlers.</param>
		private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				_row	= e.RowIndex;
				_column	= e.ColumnIndex;
			}
		}

		/// <summary>
		/// Handle data entry error.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event handlers.</param>
		void ExtendedDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBox.Show("The data entered is invalid.  Ensure that the data entered into the cells is valid and formatted correctly.", "Data Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

			// We need to tell the event arguments to still throw an error after we leave this function.  Otherwise, it is not know that an error occurred.  For example,
			// when we are pasting data into the DataGridView, if the data is in an invalid format, then a DataError is triggered and this functioned is entered.  After
			// leaving this function, by setting ThrowException to true, the error is thrown.  This is then caught in the Paste function and the Paste function can
			// exit.  We don't want the Paste function to continue trying to paste bad data and throwing error after error.
			e.ThrowException = true;
		}

		/// <summary>
		/// Cut event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event handlers.</param>
		private void mnuCut_Click(object sender, EventArgs e)
		{
			Cut();
		}

		/// <summary>
		/// Copy event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event handlers.</param>
		private void mnuCopy_Click(object sender, EventArgs e)
		{
			Copy();
		}

		/// <summary>
		/// Paste event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void mnuPaste_Click(object sender, EventArgs e)
		{
			// Perform paste Operation.
			Paste();
		}

		/// <summary>
		/// Delete event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void mnuDelete_Click(object sender, EventArgs e)
		{
			Delete();
		}

		/// <summary>
		/// Insert event handler.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void mnuInsert_Click(object sender, EventArgs e)
		{
			Insert();
		}

		/// <summary>
		/// Keyboard events.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		private void DataGridViewWithCopyPaste_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				// Control + KEY combinations.
				if (e.Modifiers == Keys.Control)
				{
					switch (e.KeyCode)
					{
						case Keys.X:
						{
							Cut();
							break;
						}

						case Keys.C:
						{
							Copy();
							break;
						}

						case Keys.V:
						{
							Paste();
							break;
						}
					}
				}

				// Single key events.
				switch (e.KeyCode)
				{
					case Keys.Delete:
					{
						Delete();
						e.Handled = true;
						break;
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("Cut, copy, or paste operation failed. " + exception.Message, "Cut, Copy, or Paste Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

		#region Copy/Paste Methods

		/// <summary>
		/// Setup the context menu.
		/// </summary>
		public void SetContextMenuState()
		{
			// If there is nothing on the clipboard, we have to disable the "Paste" menu item.
			if (Clipboard.GetText() == "")
			{
				this.mnuPaste.Enabled = false;
			}
			else
			{
				this.mnuPaste.Enabled = true;
			}

			// If a row is not selected, we will not enable the "Insert" menu item.
			if (this.Insertion == null)
			{
				this.mnuContextSeparator1.Visible	= false;
				this.mnuInsert.Visible				= false;
			}
			else
			{
				this.mnuContextSeparator1.Visible	= true;
				this.mnuInsert.Visible				= true;

				if (this.SelectedRows.Count > 0)
				{
					this.mnuContextSeparator1.Enabled	= true;
					this.mnuInsert.Enabled				= true;
				}
				else
				{
					this.mnuContextSeparator1.Enabled	= false;
					this.mnuInsert.Enabled				= false;
				}
			}
		}

		/// <summary>
		/// Copies the contents to the clipboard, then erases the data.
		/// </summary>
		private void Cut()
		{
			// Copy to clipboard.
			Copy();

			Delete();
		}

		/// <summary>
		/// Copies the data to the clipboard.  Performs a check to make sure the data exists before copying.
		/// </summary>
		private void Copy()
		{
			// Copy to clipboard.
			DataObject dataobject = GetClipboardContent();
			if (dataobject != null)
			{
				Clipboard.SetDataObject(dataobject);
			}
		}

		/// <summary>
		/// Paste data to the cells.
		/// </summary>
		private void Paste()
		{
			// Show error if no cell is selected.
			if (this.SelectedCells.Count == 0)
			{
				MessageBox.Show("Please select a cell.", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			// Get the starting Cell.
			DataGridViewCell startcell = GetStartCell();

			// Get the clipboard value in a dictionary.
			Dictionary<int, Dictionary<int, string>> clipboardvalues = ClipBoardValues(Clipboard.GetText());

			SuspendLayout();

			int rowindex = startcell.RowIndex;
			foreach (int rowkey in clipboardvalues.Keys)
			{
				int columnindex = startcell.ColumnIndex;
				foreach (int cellKey in clipboardvalues[rowkey].Keys)
				{
					// Check if the index is within the limit.
					if (columnindex <= this.Columns.Count-1)
					{
						DataGridViewCell cell = this[columnindex, rowindex];

						// Set the current cell and "dirty" it so the control knows it have been modified and can behavior accordingly.
						this.CurrentCell = cell;

						// Have to notify that we are beginning to edit the cell.
						BeginEdit(false);
						NotifyCurrentCellDirty(true);

						// Copy to selected cells if 'chkPasteToSelectedCells' is checked.
						if ((_pastetoselectedCells && cell.Selected) || (!_pastetoselectedCells))
						{
							try
							{
								// Have to parse the value before pasting to make sure we have a properly formated value.
								cell.Value = cell.ParseFormattedValue(clipboardvalues[rowkey][cellKey], cell.Style, null, null);
							}
							catch
							{
								// Error is handled by the DataError event handler so we just need to get out of here.  We don't want to continue trying to paste
								// data as it is possible there is more bad data and we will get message box after message box being displayed.
								return;
							}
						}

						// End the cell edit.
						EndEdit();
					}
					columnindex++;
				}
				rowindex++;
			}

			ResumeLayout(false);
		}

		/// <summary>
		/// Removes the contents of the cells and sets them to the default values.
		/// </summary>
		private void Delete()
		{
			SuspendLayout();

			// Clear selected cells.  Do this first so that we don't try to clear any rows
			// once they have been deleted.
			DataGridViewSelectedCellCollection cells = this.SelectedCells;
			foreach (DataGridViewCell cell in cells)
			{
				cell.Value = cell.DefaultNewRowValue;
			}

			// Deleted selected rows.
			DataGridViewSelectedRowCollection rows = this.SelectedRows;
			foreach (DataGridViewRow row in rows)
			{
				this.Rows.Remove(row);
			}

			ResumeLayout(false);
		}

		/// <summary>
		/// Inserts a new row.
		/// </summary>
		private void Insert()
		{
			DataGridViewSelectedRowCollection rows = this.SelectedRows;

			if (rows.Count > 0)
			{
				int rowindex = rows[0].Cells[0].RowIndex;
				int rowcount = rows.Count;

				// Depending if the rows were selected top to bottom or bottom to top, the first row in the selection
				// could be the top or bottom row.  Find the top row so that we always insert at the top of the selection.
				for (int i = 1; i < rowcount; i++)
				{
					if (rows[i].Cells[0].RowIndex < rowindex)
					{
						rowindex = rows[i].Cells[0].RowIndex;
					}
				}

				SuspendLayout();
				Insertion(rowindex);
				ResumeLayout(false);
			}
		}

		/// <summary>
		/// Find the starting cell (lowest row and column) of the selected cells.
		/// </summary>
		/// <returns>The DataGridViewCell with the lowerst row and column number of the selected cells.</returns>
		private DataGridViewCell GetStartCell()
		{
			// Get the smallest row,column index.
			if (this.SelectedCells.Count == 0)
			{
				return null;
			}

			int rowindex = this.Rows.Count - 1;
			int colindex = this.Columns.Count - 1;

			foreach (DataGridViewCell cell in this.SelectedCells)
			{
				if (cell.RowIndex < rowindex)
				{
					rowindex = cell.RowIndex;
				}

				if (cell.ColumnIndex < colindex)
				{
					colindex = cell.ColumnIndex;
				}
			}

			return this[colindex, rowindex];
		}

		/// <summary>
		/// Parses a clipboard string and returns the values in the string in a Dictionary.
		/// </summary>
		/// <param name="clipboardstring">Clipboard string to parse.</param>
		/// <returns>Values in the clipboard string in a Dictionary.</returns>
		private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardstring)
		{
			Dictionary<int, Dictionary<int, string>> values = new Dictionary<int, Dictionary<int, string>>();

			string[] lines = clipboardstring.Split('\n');

			int numberoflines = lines.Length;

			// Sometimes copying from Excel (and maybe others) sometimes leaves an extra blank line
			// at the end.  Ignore that.
			if (numberoflines > 1 && lines[numberoflines-1] == "")
			{
				numberoflines--;
			}

			for (int i = 0; i < numberoflines; i++)
			{
				values[i] = new Dictionary<int, string>();
				string[] linevalues = lines[i].Split('\t');

				// If an empty cell value is copied, then set the dictionary with an empty string, otherwise
				// set the value in dictionary to the value found.
				if (linevalues.Length == 0)
				{
					values[i][0] = string.Empty;
				}
				else
				{
					for (int j = 0; j < linevalues.Length; j++)
					{
						values[i][j] = linevalues[j];
					}
				}
			}
			return values;
		}

		#endregion

		#region Image Creation

		/// <summary>
		/// Write the control to the disk as an image.
		/// </summary>
		/// <param name="path">Path to write the image to.</param>
		/// <param name="format">Format to write the image in.</param>
		public void WriteAsImage(string path, ImageFormat format)
		{
			// Add the appropriate extension if it is not there.
			string extension = "." + format.ToString();
			if (!path.EndsWith(extension))
			{
				path += extension;
			}

			// Determine the area to create the image from.  Only select the part of the control that contains the headers
			// and the row information.  We don't need to write the bottom of the control if there are no rows there.
			Size size = this.Size;

			// We want the headers, plus the rows.  We also need to add in an additional factor (2) to account for the controls
			// interior elements.
			size.Height = this.Rows.GetRowsHeight(DataGridViewElementStates.None) + this.ColumnHeadersHeight + 2;
			Rectangle rectangle = new Rectangle(new Point(), size);

			// Create a Bitmap of the size we want and have the DataGridView draw into it.
			Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
			this.DrawToBitmap(bitmap, rectangle);

			bitmap.Save(path, format);
		}

		#endregion

	} // End class.
} // End namespace.
