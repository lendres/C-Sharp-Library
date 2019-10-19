using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DigitalProduction.Mathmatics;

namespace DigitalProduction.Forms
{
	/// <summary>
	/// A label that can rotate text.
	/// 
	/// This class is partially based on the concepts posted by users Javed Akram and Buddy at:
	/// https://stackoverflow.com/questions/1371943/c-sharp-vertical-label-in-a-windows-forms
	/// </summary>
	public partial class RotatedLabel : UserControl
	{
		#region Members

		private int						_angle			= 0;
		private double					_radians;

		private string					_text			= "rotatedLabel";
		private ContentAlignment		_alignment		= ContentAlignment.TopLeft;

		private int						_quadrant		= 1;

		#endregion

		#region Construction

		/// <summary>
		/// Default constructor.
		/// </summary>
		public RotatedLabel()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Text angle.
		/// </summary>
		[Category("Appearance")]
		public int Angle
		{
			get
			{
				return _angle;
			}

			set
			{
				// Make sure angle is in the range of 0-360 degrees.
				_angle		= ((value % 360) + 360) % 360;

				// Store radians so it is at the ready.
				_radians	= Conversion.DegreesToRadians(_angle);

				// Calculate the quadrant we are in.
				CalculateQuadrant();

				// Need to update the control.
				Refresh();
			}
		}

		/// <summary>
		/// Text.
		/// </summary>
		[Category("Appearance")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return _text;
			}

			set
			{
				_text		= value;
				base.Text	= value;
				Refresh();
			}
		}

		/// <summary>
		/// Alignment.
		/// </summary>
		[Category("Appearance")]
		public ContentAlignment TextAlign
		{
			get
			{
				return _alignment;
			}

			set
			{
				_alignment = value;
				Refresh();
			}
		}

		/// <summary>
		/// The font used to display the text in the control.
		/// </summary>
		/// <remarks>
		/// Capture change so we can update control.
		/// </remarks>
		public override Font Font
		{
			get
			{
				return base.Font;
			}

			set
			{
				base.Font = value;
				Refresh();
			}
		}

		/// <summary>
		/// Enables automatic resizing based on font size.
		/// </summary>
		/// <remarks>
		/// Capture change so we can update control.
		/// </remarks>
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}

			set
			{
				base.AutoSize = value;
				Refresh();
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Painting event that does the work.
		/// </summary>
		/// <param name="paintEventArgs">Event arguments.</param>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs paintEventArgs)
		{
			// Get the text size.
			SizeF textSize			= paintEventArgs.Graphics.MeasureString(_text, this.Font, this.Parent.Width);

			// When you rotate the text, the width and height will now have an X and a Y component because they no longer fall along the X or Y axis.  These are the components of
			// the width and height after rotation.
			Point rotatedHeight		= new Point(Math.Abs((int)Math.Ceiling(textSize.Height * Math.Sin(_radians))), Math.Abs((int)Math.Ceiling(textSize.Height * Math.Cos(_radians))));
			Point rotatedWidth		= new Point(Math.Abs((int)Math.Ceiling(textSize.Width * Math.Cos(_radians))), Math.Abs((int)Math.Ceiling(textSize.Width * Math.Sin(_radians))));

			// Size of the rotated text.  This is the size of the bounding box around the text.  SetControlSize will update the control size or leave it alone
			// based on how auto sizing is set.
			Size textBoundingBox	= new Size(rotatedWidth.X + rotatedHeight.X, rotatedWidth.Y + rotatedHeight.Y);
			SetControlSize(textBoundingBox);

			// Two offsets (translations) are used.  The first is to accounts for the effects of the rotation.  The second accounts
			// for the effects of the alignment.  It's a lot easier to calculate them separately and add together than to try to account
			// for them both at the same time.
			Point rotationOffset	= CalculateOffsetForRotation(ref rotatedHeight, ref rotatedWidth, ref textBoundingBox);
			Point alignmentOffset	= CalculateOffsetForAlignment(ref textBoundingBox);

			// Apply the transformation and rotation to the graphics.
			paintEventArgs.Graphics.TranslateTransform(rotationOffset.X + alignmentOffset.X, rotationOffset.Y + alignmentOffset.Y);
			paintEventArgs.Graphics.RotateTransform(_angle);

			// Draw the text and let the base class do its painting.
			paintEventArgs.Graphics.DrawString(_text, this.Font, new SolidBrush(this.ForeColor), 0f, 0f);
			base.OnPaint(paintEventArgs);
		}

		/// <summary>
		/// When we resize, we need to update the text.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="eventArgs">Event arguments.</param>
		private void RotatedLabel_Resize(object sender, EventArgs eventArgs)
		{
			Refresh();
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Finds the starting offset based on the rotated text position.
		/// </summary>
		/// <param name="rotatedHeight">Height rotated by the text angle.</param>
		/// <param name="rotatedWidth">Width rotated by the text angle.</param>
		/// <param name="textBoundingBox">Size of bounding box for the rotated text (height and width of the rotated text).</param>
		private Point CalculateOffsetForRotation(ref Point rotatedHeight, ref Point rotatedWidth, ref Size textBoundingBox)
		{
			Point offset = new Point(0, 0);

			switch (_quadrant)
			{
				case 1:
					offset.X = rotatedHeight.X;
					break;

				case 2:
					offset.X = textBoundingBox.Width;
					offset.Y = rotatedHeight.Y;
					break;

				case 3:
					offset.X = rotatedWidth.X;
					offset.Y = textBoundingBox.Height;
					break;

				case 4:
					offset.Y = rotatedWidth.Y;
					break;
			}

			return offset;
		}

		/// <summary>
		/// Finds the offset required to create the text alignment.
		/// </summary>
		/// <param name="textBoundingBox">Size of bounding box for the rotated text (height and width of the rotated text).</param>
		private Point CalculateOffsetForAlignment(ref Size textBoundingBox)
		{
			Point offset = new Point(0, 0);

			switch (_alignment)
			{
				case ContentAlignment.TopLeft:
					break;

				case ContentAlignment.TopCenter:
					offset.X = (int)(0.5*this.Width - 0.5*textBoundingBox.Width);
					break;

				case ContentAlignment.TopRight:
					offset.X = (int)(this.Width - textBoundingBox.Width);
					break;

				case ContentAlignment.MiddleLeft:
					offset.Y = (int)(0.5*this.Height - 0.5*textBoundingBox.Height);
					break;

				case ContentAlignment.MiddleCenter:
					offset.X = (int)(0.5*this.Width - 0.5*textBoundingBox.Width);
					offset.Y = (int)(0.5*this.Height - 0.5*textBoundingBox.Height);
					break;

				case ContentAlignment.MiddleRight:
					offset.X = (int)(this.Width - textBoundingBox.Width);
					offset.Y = (int)(0.5*this.Height - 0.5*textBoundingBox.Height);
					break;

				case ContentAlignment.BottomLeft:
					offset.Y = (int)(this.Height - textBoundingBox.Height);
					break;

				case ContentAlignment.BottomCenter:
					offset.X = (int)(0.5*this.Width - 0.5*textBoundingBox.Width);
					offset.Y = (int)(this.Height - textBoundingBox.Height);
					break;

				case ContentAlignment.BottomRight:
					offset.X = (int)(this.Width - textBoundingBox.Width);
					offset.Y = (int)(this.Height - textBoundingBox.Height);
					break;
			}

			return offset;
		}

		/// <summary>
		/// Sizes the control based on the AutoSize setting.
		/// </summary>
		/// <param name="textBoundingBox">Size of bounding box for the rotated text (height and width of the rotated text).</param>
		private void SetControlSize(Size textBoundingBox)
		{
			if (this.AutoSize)
			{
				this.Width			= textBoundingBox.Width;
				this.Height			= textBoundingBox.Height;
			}
		}

		/// <summary>
		/// Determines the quadrant we are in.
		/// </summary>
		private void CalculateQuadrant()
		{
			_quadrant = (_angle >= 0 && _angle < 90) ? 1 :
						(_angle >= 90 && _angle < 180) ? 2 :
						(_angle >= 180 && _angle < 270) ? 3 :
						(_angle >= 270 && _angle < 360) ? 4 : 0;

			System.Diagnostics.Debug.Assert(_quadrant != 0, "Invalid angle in rotated label.");
		}

		#endregion

	} // End class.
} // End name space.