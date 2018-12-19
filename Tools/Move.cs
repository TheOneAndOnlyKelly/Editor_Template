using System;
using System.Drawing;
using System.Windows.Forms;
using Editor_Template.Utilities;
using KellyControls.CommonClasses;
using KellyControls.PlugInToolBtn.Interfaces;
using KellyControls.PlugInToolBtn.Tools;
using CanvasPoint = System.Drawing.Point;
using ImageHandler = Editor_Template.Singletons.ImageController;

namespace Editor_Template.Tools
{
	[Tool("Move")]
	public class MoveTool : BaseTool, IPlugInTool
	{
		#region [ Private Variables ]

		// Settings from the ToolStrip
		private Point _moveOffset = Point.Empty;

		// Controls from ToolStrip
		private ToolStripButton MoveAll = null;
		private ToolStripButton ExecuteMove = null;
		private ToolStripTextBox OffsetX = null;
		private ToolStripTextBox OffsetY = null;

		// Used for moving
		private Point _moveEndCanvasPoint = Point.Empty;
		private CanvasPoint _moveStartCanvasPoint = new Point();
		private Cursor _grabbingCursor = null;
		private IntPtr _grabCursorHandle = IntPtr.Zero;

		#endregion [ Private Variables ]

		#region [ Constants ]

		private const string OFFSET_X = "Offset_X";
		private const string OFFSET_Y = "Offset_Y";

		#endregion [ Constants ]

		#region [ Properties ]

		public override string UndoText
		{
			get
			{
				if (MoveAll.Checked)
					return "Move All";

				else
					return "Move";
			}
			set
			{
				base.UndoText = value;
			}
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		public MoveTool() 
			: base()
		{
			this.ID = (int)ToolID.Move;
			this.Name = "Move";
			this.ToolBoxImage = ImageHandler.Instance.GetBitmap(ImageType.Move);
			this.ToolBoxImageSelected = ImageHandler.Instance.GetBitmap(ImageType.Move_Selected);
			this.Cursor = base.CreateCursor(ImageHandler.Instance.GetBitmap(ImageType.Pan), new Point(7, 7));
			_grabbingCursor = base.CreateCursor(ImageHandler.Instance.GetBitmap(ImageType.Grabbed), new Point(7, 7));
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Attaches or detaches events to objects, such as Click events to buttons.
		/// </summary>
		/// <param name="attach">Indicates that the events should be attached. If false, then detaches the events</param>
		protected override void AttachEvents(bool attach)
		{
			// If we've already either attached or detached, exit out.
			if (attach && _eventsAttached)
				return;

			if (attach)
			{
				ExecuteMove.Click += new EventHandler(ExecuteMove_Click);
				OffsetX.Leave += new EventHandler(OffsetX_Leave);
				OffsetX.KeyPress += new KeyPressEventHandler(SignedNumberOnly_KeyPress);
				OffsetY.Leave += new EventHandler(OffsetY_Leave);
				OffsetY.KeyPress += new KeyPressEventHandler(SignedNumberOnly_KeyPress);
			}
			else
			{
				ExecuteMove.Click -= ExecuteMove_Click;
				OffsetX.Leave -= OffsetX_Leave;
				OffsetX.KeyPress -= SignedNumberOnly_KeyPress;
				OffsetY.Leave -= OffsetY_Leave;
				OffsetY.KeyPress -= SignedNumberOnly_KeyPress;
			}
			base.AttachEvents(attach);
		}

		/// <summary>
		/// Load in the saved values from the Settings Xml file. The path to be used should be 
		/// ToolSettings|[Name of this tool].
		/// We use the pipe character to delimit the names, because we don't want to be necessarily tied down to only one
		/// format for saving. If it gets changed at some later date, doing it this way prevents code from being recompiled
		/// for these PlugIns, as the AdjustablePreview code converts the pipe to the proper syntax.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();

			// Load the Settings values
			_moveOffset = new Point(LoadValue(OFFSET_X, 0), LoadValue(OFFSET_Y, 0));

			if ((ToolStripLabel)GetItem<ToolStripLabel>(1) == null)
				return;

			// Get a pointer to the controls on the toolstrip that belongs to us.
			MoveAll = (ToolStripButton)GetItem<ToolStripButton>(1);
			ExecuteMove = (ToolStripButton)GetItem<ToolStripButton>(2);
			OffsetX = (ToolStripTextBox)GetItem<ToolStripTextBox>(1);
			OffsetY = (ToolStripTextBox)GetItem<ToolStripTextBox>(2);

			// Set the initial value for the contol from what we had retrieve from Settings
			OffsetX.Text = _moveOffset.X.ToString();
			OffsetY.Text = _moveOffset.Y.ToString();
		}



		/// <summary>
		/// Canvas MouseDown event was fired
		/// </summary>
		/// <param name="buttons">From the MouseEventArgs, indicates which mouse button was clicked</param>
		/// <param name="latticePoint">Point on the picture box (in Cells) where the mouse event happened</param>
		/// <param name="actualCanvasPoint">Point on the picture box (in Pixel) where the mouse event happened</param>
		public override void MouseDown(MouseButtons buttons, Point latticePoint, CanvasPoint actualCanvasPoint)
		{
			_isMouseDown = true;
			Document.Cursor = _grabbingCursor;
			_moveStartCanvasPoint = _global.CalcCanvasPoint(latticePoint);

			// ...
		}

		/// <summary>
		/// Handles keystrokes for the tool. Returns true if the keystroke was handled within the tool
		/// </summary>
		/// <param name="e"></param>
		public override bool KeyDown(KeyEventArgs e)
		{
			_moveStartCanvasPoint = new CanvasPoint(0, 0);
			_moveEndCanvasPoint = Point.Empty;

			int Amount = _global.Scaling.CellScale;

			if (Control.ModifierKeys == Keys.Shift)
				Amount *= 5;

			switch (e.KeyCode)
			{
				case Keys.Up:
					_moveEndCanvasPoint = new CanvasPoint(0, -Amount);
					break;

				case Keys.Down:
					_moveEndCanvasPoint = new CanvasPoint(0, Amount);
					break;

				case Keys.Left:
					_moveEndCanvasPoint = new CanvasPoint(-Amount, 0);
					break;

				case Keys.Right:
					_moveEndCanvasPoint = new CanvasPoint(Amount, 0);
					break;
			}

			if (_moveEndCanvasPoint.IsEmpty)
				return false;

			MoveTheseItems();

			return true;
		}

		/// <summary>
		/// Canvas MouseMove event was fired
		/// </summary>
		/// <param name="buttons">From the MouseEventArgs, indicates which mouse button was clicked</param>
		/// <param name="latticePoint">Point on the picture box (in Cells) where the mouse event happened</param>
		/// <param name="actualCanvasPoint">Point on the picture box (in Pixel) where the mouse event happened</param>
		public override bool MouseMove(MouseButtons buttons, Point latticePoint, CanvasPoint actualCanvasPoint)
		{
			//if (Document == null)
			//	return false;

			if (!_isMouseDown)
				return false;

			_moveEndCanvasPoint = _global.CalcCanvasPoint(latticePoint);
			MoveTheseItems();
			_moveStartCanvasPoint = _global.CalcCanvasPoint(latticePoint);
			return true;
		}

		/// <summary>
		/// Canvas MouseUp event was fired
		/// </summary>
		/// <param name="buttons">From the MouseEventArgs, indicates which mouse button was clicked</param>
		/// <param name="latticePoint">Point on the picture box (in Cells) where the mouse event happened</param>
		/// <param name="actualCanvasPoint">Point on the picture box (in Pixel) where the mouse event happened</param>
		public override bool MouseUp(MouseButtons buttons, Point latticePoint, CanvasPoint actualCanvasPoint)
		{
			_isMouseDown = false;

			_moveEndCanvasPoint = _global.CalcCanvasPoint(latticePoint);
			MoveTheseItems();
			//Document.Cursor = this.Cursor;

			// Fire the event to indicate that this tool has finished working.
			EndOperation();

			return true;
		}

		/// <summary>
		/// Move all the selected items
		/// </summary>
		private void MoveTheseItems()
		{
			Point Offset = _global.CalcLatticePoint(new Point(_moveEndCanvasPoint.X - _moveStartCanvasPoint.X, _moveEndCanvasPoint.Y - _moveStartCanvasPoint.Y));

			//...

			Document.Refresh();
		}

		/// <summary>
		/// Save this toolstrip settings back to the Settings object.
		/// </summary>
		public override void SaveSettings()
		{
			SaveValue(OFFSET_X, _moveOffset.X);
			SaveValue(OFFSET_Y, _moveOffset.Y);
		}

		/// <summary>
		/// Method fires when we are closing out of the editor, want to clean up all our objects.
		/// </summary>
		public override void ShutDown()
		{
			base.ShutDown();

			MoveAll = null;
			ExecuteMove = null;
			OffsetX = null;
			OffsetY = null;
			if (_grabbingCursor != null)
			{
				CustomCursors.DestroyCreatedCursor(_grabCursorHandle);
				_grabbingCursor.Dispose();
				_grabbingCursor = null;
			}
		}

		/// <summary>
		/// Create the Toolstrip control for this Tool
		/// </summary>
		protected override void CreateToolStrip()
		{
			OffsetX = new ToolStripTextBox() // MC_OffsetX
			{
				Name = "OffsetX",
				AutoSize = false,
				BorderStyle = BorderStyle.FixedSingle,
				Font = new Font("Segoe UI", 7F),
				Size = new Size(23, 20),
				ToolTipText = "Amount to move horizontally, negative numbers move left"
			};

			OffsetY = new ToolStripTextBox() // MC_OffsetY
			{
				Name = "OffsetY",
				AutoSize = false,
				BorderStyle = BorderStyle.FixedSingle,
				Font = new Font("Segoe UI", 7F),
				Size = new Size(23, 20),
				ToolTipText = "Amount to move vertically, negative numbers move upwards"
			};

			ExecuteMove = new ToolStripButton()
			{
				Name = "ExecuteMove",
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				Image = ImageHandler.Instance.GetBitmap(ImageType.Move),
				Size = new Size(23, 22),
				Text = "Move By Offset Values"
			};

			_toolStrip = new ToolStrip
			{
				BackColor = Color.White,
				GripStyle = ToolStripGripStyle.Hidden,
				Name = "Move_ToolStrip"
			};

			_toolStrip.Items.AddRange(new ToolStripItem[] {
				new ToolStripLabel()
				{
					Name = "lblToolName",
					BackColor = SystemColors.ControlDark,
					Font = new Font("Segoe UI", 9F, FontStyle.Bold),
					Image = ImageHandler.Instance.GetBitmap(ImageType.Move),
					Size = new Size(102, 22),
					Text = "Move"
				},
				new ToolStripSeparator()
				{
				},
				new ToolStripLabel()
				{
					Name = "_txtOffsetX",
					Text = "Move by offset X:"
				},
				OffsetX,
				new ToolStripLabel()
				{
					Name = "_txtOffsetY",
					Text = "   Y:"
				},
				OffsetY,
				ExecuteMove

			});
		}

		#endregion [ Methods ]

		#region [ ToolStrip Event Delegates ]

		/// <summary>
		/// Execute the move using the offsets in the tool strip
		/// </summary>
		private void ExecuteMove_Click(object sender, EventArgs e)
		{
			_moveStartCanvasPoint = new Point(0, 0);
			_moveEndCanvasPoint = new Point(_moveOffset.X * _global.Scaling.CellScale, _moveOffset.Y * _global.Scaling.CellScale);

			MoveTheseItems();

			Document.SaveUndo(this.UndoText);
			Document.Refresh();
		}
		
		/// <summary>
		/// Validate that the text entered in the textbox is a proper number. If so, set the value into our variable.
		/// If not, reset the text in the text box with the original value of our variable
		/// </summary>
		private void OffsetX_Leave(object sender, EventArgs e)
		{
			if (OffsetX.TextLength == 0)
				OffsetX.Text = "0";

			int X = ValidateInteger(OffsetX, _moveOffset.X);
			_moveOffset = new Point(X, _moveOffset.Y);
		}

		/// <summary>
		/// Validate that the text entered in the textbox is a proper number. If so, set the value into our variable.
		/// If not, reset the text in the text box with the original value of our variable
		/// </summary>
		private void OffsetY_Leave(object sender, EventArgs e)
		{
			if (OffsetY.TextLength == 0)
				OffsetY.Text = "0";

			int Y = ValidateInteger(OffsetY, _moveOffset.Y);
			_moveOffset = new Point(_moveOffset.X, Y);
		}

		#endregion [ ToolStrip Event Delegates ]

		#region [ Events ]

		private void SignedNumberOnly_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (char.IsDigit(e.KeyChar) ||
				char.IsControl(e.KeyChar) ||
				(e.KeyChar == '-') ||
				(e.KeyChar == '+'))
			{
				// We like these, do nothing
				e.Handled = false;
			}
			else
				e.Handled = true;
		}

		#endregion [ Events ]

	}
}
