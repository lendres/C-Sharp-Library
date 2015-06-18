using System;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Win32;
using DigitalProduction.WinRegistry;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// Stores the state of the window.  I.e. position, size, minimized, et cetera.
	/// </summary>
	public class WindowStateManager
	{
		#region Members / Variables.

		private WinRegistryAccess		_registryaccess;	// Access to the registry for the app I am monitoring.

		// Windows defines not maximized and not minimized as "normal".  It is these settings we need
		// to remember.
		private int[]					_formposition;		// Left, Top, Width, and Height of form.
		FormWindowState					_windowstate;		// Window state - minimized, normal, or maximized.

		#endregion

		#region Construction / Destruction / Install.

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="regaccess">Registry access for this application.</param>
		public WindowStateManager(WinRegistryAccess regaccess)
		{
			// Store the registry writer for this application.
			_registryaccess = regaccess;

			// Link my installation function on to the applications chain.
			_registryaccess.Owner.Install += new DPMForm.InstallDelegate(Install);

			// Latch on to the owner's events so that we can grab the data we need and store and restore
			// the data when necessary.
			_registryaccess.Owner.Closing	+= new System.ComponentModel.CancelEventHandler(OnClosing);
			_registryaccess.Owner.Resize	+= new System.EventHandler(OnResize);
			_registryaccess.Owner.Move		+= new System.EventHandler(OnMove);
			_registryaccess.Owner.Load		+= new System.EventHandler(OnLoad);

			// Initialize our data to the parents data.
			_formposition							= new int[4];
			_formposition[(int)FormPosition.Left]	= _registryaccess.Owner.Left;
			_formposition[(int)FormPosition.Top]	= _registryaccess.Owner.Top;
			_formposition[(int)FormPosition.Width]	= _registryaccess.Owner.Width;
			_formposition[(int)FormPosition.Height]	= _registryaccess.Owner.Height;

			_windowstate = _registryaccess.Owner.WindowState;
		}

		/// <summary>
		/// Install function used by the delegate to do installation work.  Primarily used for debugging a setup
		/// routine should handle normal installation.
		/// </summary>
		// Note that install cannot be static because the application key is dependent on the specific
		// application that is using an instance of this class.
		private void Install()
		{
			// Write our data to the registry so that when there is an attempt to read it
			// it will be available.
			OnClosing(null, null);

			// Move the window over just a little so it isn't in the corner.
			int[] position	= _registryaccess.WindowPosition;
			position[0]		= 20;
			position[1]		= 20;

			_registryaccess.WindowPosition = position;
		}

		#endregion

		#region Event capture functions for tracking window.

		/// <summary>
		/// Handles a window resize in case the window is going to be maximized or minimized.  Stores what
		/// the "normal" values are.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguements.</param>
		private void OnResize(object sender, System.EventArgs e)
		{
			if(_registryaccess.Owner.WindowState == FormWindowState.Normal)
			{
				_formposition[(int)FormPosition.Width]	= _registryaccess.Owner.Width;
				_formposition[(int)FormPosition.Height]	= _registryaccess.Owner.Height;
			}

			// Save the Window State.
			_windowstate = _registryaccess.Owner.WindowState;
		}

		/// <summary>
		/// Handles a window movement so that the new position is saved.
		/// </summary>
		/// <param name="sender">Object that sent the message.</param>
		/// <param name="e">Parameters</param>
		private void OnMove(object sender, System.EventArgs e)
		{
			if(_registryaccess.Owner.WindowState == FormWindowState.Normal)
			{
				_formposition[(int)FormPosition.Left]	= _registryaccess.Owner.Left;
				_formposition[(int)FormPosition.Top]	= _registryaccess.Owner.Top;
			}
		}

		/// <summary>
		/// Handles saving all required information to the registry on the closing of the window.
		/// </summary>
		/// <param name="sender">Object that sent the message.</param>
		/// <param name="e">Cancel event arguements.</param>
		private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Save the Window State.
			_registryaccess.WindowPosition = _formposition;
	
			// We don't want to restore the window minimized, so if the window is
			// minimized, store normal instead.
			if (_windowstate == FormWindowState.Minimized)
			{
				_registryaccess.WindowState = FormWindowState.Normal;
			}
			else
			{
				_registryaccess.WindowState = _registryaccess.Owner.WindowState;
			}

		}

		/// <summary>
		/// Restores values from the registry when the form loads.
		/// </summary>
		/// <param name="sender">Object that sent the message.</param>
		/// <param name="e">Parameters</param>
		private void OnLoad(object sender, System.EventArgs e)
		{
			int[] pos;
			pos = _registryaccess.WindowPosition;

			_registryaccess.Owner.Location		= new Point(pos[(int)FormPosition.Left], pos[(int)FormPosition.Top]);
			_registryaccess.Owner.Size			= new Size(pos[(int)FormPosition.Width], pos[(int)FormPosition.Height]);
			_registryaccess.Owner.WindowState	= _registryaccess.WindowState;

		} // OnLoad.

		#endregion

	} // End class.
} // End namespace.