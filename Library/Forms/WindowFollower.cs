using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Allows a non-modal (does not block input to form that called it) to minimize and maximize the
	/// same as the dialog that called it.
	/// </summary>
	public class WindowFollower
	{
		#region Members

		private Form						_master						= null;
		private Form						_slave						= null;
		private bool						_closeslave					= false;
		private FormWindowState				_masterlastwindowstate;
		private FormWindowState				_slavelastwindowstate;
		private FormWindowState				_masterrestorestate;
		private FormWindowState				_slaverestorestate;

		#endregion

		#region DLL Import Functions

		const uint SWP_NOSIZE			= 0x0001;
		const uint SWP_NOMOVE			= 0x0002;
		const uint SWP_NOZORDER			= 0x0004;
		const uint SWP_NOREDRAW			= 0x0008;
		const uint SWP_NOACTIVATE		= 0x0010;
		const uint SWP_FRAMECHANGED		= 0x0020;
		const uint SWP_SHOWWINDOW		= 0x0040;
		const uint SWP_HIDEWINDOW		= 0x0080;
		const uint SWP_NOCOPYBITS		= 0x0100;
		const uint SWP_NOOWNERZORDER	= 0x0200;
		const uint SWP_NOSENDCHANGING	= 0x0400;

		/// <summary>
		/// Set position of window.
		/// </summary>
		/// <param name="hWnd">Windows handle of window.</param>
		/// <param name="hWndInsertAfter">Handle of window to insert after.</param>
		/// <param name="x">X position.</param>
		/// <param name="y">Y position.</param>
		/// <param name="cx">cx.</param>
		/// <param name="cy">cy.</param>
		/// <param name="uFlags">Flags.</param>
		[DllImport("user32.dll")]
		protected static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

		#endregion

		#region Construction

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="master">The main window or application.</param>
		/// <param name="slave">The follower dialog or window.</param>
		public WindowFollower(System.Windows.Forms.Form master, System.Windows.Forms.Form slave)
		{
			_master					= master;
			_slave					= slave;
			_slavelastwindowstate	= _slave.WindowState;
			_masterlastwindowstate	= _master.WindowState;

			// Hook onto resizing events.
			_master.Resize			+= new EventHandler(MasterResize);
			_slave.Resize			+= new EventHandler(SlaveResize);

			// Hook onto closing events.
			_master.Closing			+= new System.ComponentModel.CancelEventHandler(MasterClosing);
			_slave.Closing			+= new System.ComponentModel.CancelEventHandler(SlaveClosing);

			_master.Activated		+= new EventHandler(MasterActivated);
			_slave.Activated		+= new EventHandler(SlaveActivated);
		}

		#endregion

		#region Properties

		/// <value>
		/// Close the slave when the master closes if true.
		/// </value>
		public bool CloseSlaveWithMaster
		{
			get
			{
				return _closeslave;
			}

			set
			{
				_closeslave = value;
			}
		}

		#endregion

		#region Resizing Functions

		/// <summary>
		/// Called when the master is resized.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void MasterResize(object sender, EventArgs eventArgs)
		{
			// Make sure both the master and slave are still open.
			if (_slave == null || _master == null)
			{
				return;
			}

			// Don't want to maximize the slave, just minimize and restore it with the master.
			if (_master.WindowState == FormWindowState.Minimized)
			{
				_masterrestorestate = _masterlastwindowstate;
				_slave.WindowState = FormWindowState.Minimized;
			}
			else
			{
				_masterrestorestate = _master.WindowState;
				_slave.WindowState = _slaverestorestate;
			}

			_masterlastwindowstate = _master.WindowState;
		}

		/// <summary>
		/// Called when the slave is resized.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void SlaveResize(object sender, EventArgs eventArgs)
		{
			// Make sure both the master and slave are still open.
			if (_slave == null || _master == null)
			{
				return;
			}

			if (_slave.WindowState == FormWindowState.Minimized)
			{
				_slaverestorestate = _slavelastwindowstate;
				_master.WindowState = FormWindowState.Minimized;
			}
			else
			{
				_slaverestorestate = _slave.WindowState;
				_master.WindowState = _masterrestorestate;
			}

			_slavelastwindowstate = _slave.WindowState;
		}

		#endregion

		#region Closing Functions

		/// <summary>
		/// Called when the master is closing.  Closes slave is the CloseSlaveWithMaster is true.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void MasterClosing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
		{
			if (_closeslave)
			{
				_slave.Close();
				_slave = null;
			}

			_master = null;
		}

		/// <summary>
		/// Called when the slave is closing.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void SlaveClosing(object sender, System.ComponentModel.CancelEventArgs eventArgs)
		{
			_slave = null;
		}

		#endregion

		#region Windows Activated Functions

		/// <summary>
		/// Called when the master is activated.  Sets the slave window as the second highest window.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void MasterActivated(object sender, EventArgs eventArgs)
		{
			if (_slave != null && _master != null)
			{
				SetWindowPos(_slave.Handle, _master.Handle, _slave.Location.X, _slave.Location.Y, _slave.Size.Width, _slave.Size.Height, SWP_NOACTIVATE);
			}
		}

		/// <summary>
		/// Called when the slave is activated.  Sets the master window as the second highest window.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguements.</param>
		private void SlaveActivated(object sender, EventArgs eventArgs)
		{
			if (_slave != null && _master != null)
			{
				SetWindowPos(_master.Handle, _slave.Handle, _master.Location.X, _master.Location.Y, _master.Size.Width, _master.Size.Height, SWP_NOACTIVATE);
			}
		}

		#endregion

	} // End class.
} // End namespace.