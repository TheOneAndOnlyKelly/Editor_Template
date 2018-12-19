using System;
using System.Drawing;
using System.Windows.Forms;
using Editor_Template.Core;
using Editor_Template.Singletons;
using Editor_Template.Utilities;
using KellyControls.PlugInToolBtn.Interfaces;
using KellyControls.PlugInToolBtn.Tools;
using CanvasPoint = System.Drawing.Point;
using ImageHandler = Editor_Template.Singletons.ImageController;

namespace Editor_Template.Tools
{
	[Tool("Zoom")]
	public class ZoomTool : BaseTool, IPlugInTool
	{
		#region [ Enums ]

		enum ZoomMode
		{
			NotSet = -1,
			ZoomIn,
			ZoomOut
		}

		#endregion [ Enums ]

		#region [ Constants ]

		private const string PERCENT_FORMAT = "0%";
		private const float ZOOM_CLICK_ADJ = 0.5f;

		#endregion [ Constants ]

		#region [ Private Variables ]

		// Controls from ToolStrip
		private ToolStripComboBox ZoomFactor = null;
		private ToolStripButton ZoomIn = null;
		private ToolStripButton ZoomOut = null;
		private ToolStripButton Zoom100 = null;

		private bool _isSetting = false;

		#endregion [ Private Variables ]

		#region [ Constructors ]

		public ZoomTool() 
			: base()
		{
			this.ID = (int)ToolID.Zoom;
			this.Name = "Zoom";
			this.ToolBoxImage = ImageHandler.Instance.GetBitmap(ImageType.Zoom);
			this.ToolBoxImageSelected = ImageHandler.Instance.GetBitmap(ImageType.Zoom_Selected);
			base.Cursor = CreateCursor(ImageHandler.Instance.GetBitmap(ImageType.Zoom), new Point(7, 7));
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
				ZoomFactor.SelectedIndexChanged += new EventHandler(ZoomFactor_SelectedIndexChanged);
				ZoomFactor.KeyPress += new KeyPressEventHandler(ZoomFactor_KeyPress);
				ZoomFactor.Leave += new EventHandler(ZoomFactor_Leave);
				ZoomIn.Click += new EventHandler(ZoomIn_Click);
				ZoomOut.Click += new EventHandler(ZoomOut_Click);
				Zoom100.Click += new EventHandler(Zoom100_Click);
				ZoomFactor.KeyPress += new KeyPressEventHandler(SignedFloatOnly_KeyPress);
			}
			else
			{
				ZoomFactor.SelectedIndexChanged -= ZoomFactor_SelectedIndexChanged;
				ZoomFactor.KeyPress -= ZoomFactor_KeyPress;
				ZoomFactor.Leave -= ZoomFactor_Leave;
				ZoomIn.Click -= ZoomIn_Click;
				ZoomOut.Click -= ZoomOut_Click;
				Zoom100.Click -= Zoom100_Click;
				ZoomFactor.KeyPress -= SignedFloatOnly_KeyPress;
			}

			base.AttachEvents(attach);
		}

		/// <summary>
		/// Gets the text out of the ZoomFactor drop down list box and converts it to a decimal value that is used within the Document.
		/// </summary>
		private float GetZoomAmountFromDropDown()
		{
			// Blanking out the textbox sets the value to 100%
			if (ZoomFactor.Text.Length == 0)
				ZoomFactor.Text = Scaling.ZOOM_100.ToString(PERCENT_FORMAT);

			string Value = ZoomFactor.Text.Replace("%", string.Empty).Trim();
			return (float)Convert.ToDecimal(Value) / 100.0f;
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

			if ((ToolStripLabel)GetItem<ToolStripLabel>(1) == null)
				return;

			// Get a pointer to the controls on the toolstrip that belongs to us.
			ZoomFactor = (ToolStripComboBox)GetItem<ToolStripComboBox>(1);
			ZoomIn = (ToolStripButton)GetItem<ToolStripButton>(1);
			ZoomOut = (ToolStripButton)GetItem<ToolStripButton>(2);
			Zoom100 = (ToolStripButton)GetItem<ToolStripButton>(3);
		}

		/// <summary>
		/// Canvas MouseDown event was fired
		/// </summary>
		/// <param name="buttons">From the MouseEventArgs, indicates which mouse button was clicked</param>
		/// <param name="latticePoint">Point on the picture box (in Cells) where the mouse event happened</param>
		/// <param name="actualCanvasPoint">Point on the picture box (in Pixel) where the mouse event happened</param>
		public override void MouseDown(MouseButtons buttons, Point latticePoint, CanvasPoint actualCanvasPoint)
		{
			return;
		}

		/// <summary>
		/// Canvas MouseMove event was fired
		/// </summary>
		/// <param name="buttons">From the MouseEventArgs, indicates which mouse button was clicked</param>
		/// <param name="latticePoint">Point on the picture box (in Cells) where the mouse event happened</param>
		/// <param name="actualCanvasPoint">Point on the picture box (in Pixel) where the mouse event happened</param>
		public override bool MouseMove(MouseButtons buttons, Point latticePoint, CanvasPoint actualCanvasPoint)
		{
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
			_isSetting = true;
			float Zoom = GlobalController.Instance.Scaling.Zoom.GetValueOrDefault(Scaling.ZOOM_100);
			float NewZoom = Zoom;

			if (buttons == MouseButtons.Left)
				NewZoom += ZOOM_CLICK_ADJ;
			else if (buttons == MouseButtons.Right)
				NewZoom -= ZOOM_CLICK_ADJ;

			if (NewZoom < Scaling.MIN_ZOOM)
				NewZoom = Scaling.MIN_ZOOM;
			else if (NewZoom > Scaling.MAX_ZOOM)
				NewZoom = Scaling.MAX_ZOOM;

			if (Zoom != NewZoom)
			{
				SetToolBarControlsToZoom(NewZoom);
				if (buttons == MouseButtons.Left)
					GlobalController.Instance.Document.SetClickZoom(actualCanvasPoint, NewZoom);
				else if (buttons == MouseButtons.Right)
					GlobalController.Instance.Scaling.Zoom = NewZoom;

				// Fire the event to indicate that this tool has finished working.
				SaveUndo();

				_isSetting = false;
			}
			return true;
		}

		/// <summary>
		/// Occurs when this Tool has been selected from the main Toolbar
		/// </summary>
		public override void OnSelected()
		{
			base.OnSelected();
			SetToolBarControlsToZoom();
		}

		/// <summary>
		/// Saves the undo information, using the new zoom amount in the text.
		/// </summary>
		private void SaveUndo()
		{
			Document.SaveUndo("Zoom to " + GlobalController.Instance.Scaling.Zoom.GetValueOrDefault(Scaling.ZOOM_100).ToString("0%"));
		}

		/// <summary>
		/// Adjust the toolbar controls to the specified zoom level.
		/// </summary>
		private void SetToolBarControlsToZoom(float zoomLevel)
		{
			ZoomFactor.SelectedIndex = (int)zoomLevel - 1;
			ZoomFactor.Text = zoomLevel.ToString(PERCENT_FORMAT);
			ZoomIn.Enabled = true;
			ZoomOut.Enabled = true;

			if (zoomLevel == Scaling.MIN_ZOOM)
				ZoomOut.Enabled = false;

			else if (zoomLevel == Scaling.MAX_ZOOM)
				ZoomIn.Enabled = false;

			Zoom100.Enabled = (zoomLevel != Scaling.ZOOM_100);
		}

		/// <summary>
		/// Adjust the toolbar controls to the zoom level on the current Document.
		/// </summary>
		private void SetToolBarControlsToZoom()
		{
			if (Document != null)
				SetToolBarControlsToZoom(GlobalController.Instance.Scaling.Zoom.GetValueOrDefault(Scaling.ZOOM_100));
		}

		private void SetZoomFromDropDown()
		{
			float Zoom = GetZoomAmountFromDropDown();
			if (GlobalController.Instance.Scaling.Zoom != Zoom)
			{
				GlobalController.Instance.Scaling.Zoom = Zoom;
				SaveUndo();
			}
			SetToolBarControlsToZoom();
		}

		public override void ShutDown()
		{
			base.ShutDown();
			ZoomFactor = null;
			ZoomIn = null;
			ZoomOut = null;
			Zoom100 = null;
		}

		/// <summary>
		/// Create the Toolstrip control for this Tool
		/// </summary>
		protected override void CreateToolStrip()
		{
			_toolStrip = new ToolStrip
			{
				Name = "Zoom_ToolStrip",
				BackColor = Color.White,
				GripStyle = ToolStripGripStyle.Hidden,
			};

			ZoomFactor = new ToolStripComboBox()
			{
				Name = "Zoom_ZoomPercent",
				AutoSize = false,
				Size = new System.Drawing.Size(69, 23),
				Text = "100%",
				ToolTipText = "Zoom Amount",
			};
			ZoomFactor.Items.AddRange(new object[] 
			{
				"100%",
				"200%",
				"300%",
				"400%",
				"500%"
			});

			ZoomIn = new ToolStripButton() // ZoomIn
			{
				Name = "ZoomIn",
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				Image = ImageHandler.Instance.GetBitmap(ImageType.Zoom_Plus),
				Size = new Size(23, 22),
				Text = "Zoom In (F2)"
			};

			ZoomOut = new ToolStripButton() // ZoomOut
			{
				Name = "ZoomOut",
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				Image = ImageHandler.Instance.GetBitmap(ImageType.Zoom_Minus),
				Size = new Size(23, 22),
				Text = "Zoom Out (F3)"
			};

			Zoom100 = new ToolStripButton() // Zoom100
			{
				Name = "Zoom100",
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				Image = ImageHandler.Instance.GetBitmap(ImageType.Zoom_Original_Size),
				Size = new Size(23, 22),
				Text = "Zoom to 100%"
			};

			_toolStrip.Items.AddRange(new ToolStripItem[] {
				new ToolStripLabel()
				{
					Name = "lblToolName",
					BackColor = SystemColors.ControlDark,
					Font = new Font("Segoe UI", 9F, FontStyle.Bold),
					Image = ImageHandler.Instance.GetBitmap(ImageType.Move),
					Size = new Size(102, 22),
					Text = "Zoom"
				},
				ZoomFactor,
				ZoomIn,
				ZoomOut,
				Zoom100
			});
		}

		#endregion [ Methods ]

		#region [ ToolStrip Event Delegates ]

		/// <summary>
		/// Validate that the value entered in the text box is a proper number. If so, set the value and format the text in the box with a percent sign
		/// </summary>
		private void ZoomFactor_Leave(object sender, EventArgs e)
		{
			SetZoomFromDropDown();
		}

		private void ZoomFactor_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)13)
			{
				SetZoomFromDropDown();
				ZoomFactor.SelectAll();
			}
		}

		private void ZoomFactor_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetZoomFromDropDown();
		}

		private void Zoom100_Click(object sender, EventArgs e)
		{
			_isSetting = true;
			GlobalController.Instance.Scaling.Zoom = 1;
			SetToolBarControlsToZoom();
			SaveUndo();
			_isSetting = false;
		}

		private void ZoomIn_Click(object sender, EventArgs e)
		{
			_isSetting = true;
			GlobalController.Instance.Scaling.Zoom += 1;
			SetToolBarControlsToZoom();
			SaveUndo();
			_isSetting = false;
		}

		private void ZoomOut_Click(object sender, EventArgs e)
		{
			_isSetting = true;
			GlobalController.Instance.Scaling.Zoom -= 1;
			SetToolBarControlsToZoom();
			SaveUndo();
			_isSetting = false;
		}

		#endregion [ ToolStrip Event Delegates ]

		#region [ Events ]

		private void SignedFloatOnly_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (char.IsDigit(e.KeyChar) ||
				char.IsControl(e.KeyChar) ||
				(e.KeyChar == '-') ||
				(e.KeyChar == '+') ||
				(e.KeyChar == '.') ||
				(e.KeyChar == '°'))
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
