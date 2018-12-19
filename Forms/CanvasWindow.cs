using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Editor_Template.Core;
using Editor_Template.Interfaces;
using Editor_Template.Singletons;
using Editor_Template.Utilities;
using KellyControls.CommonClasses;
using WeifenLuo.WinFormsUI.Docking;

namespace Editor_Template.Forms
{
	public partial class CanvasWindow : DockContent
	{
		#region [ Private Variables ]

		/// <summary>
		/// Workshop is a Singleton type of class. Simply getting the Instance variable from the Static object will get the object already loaded with our data
		/// </summary>
		private GlobalController _global = GlobalController.Instance;
		private Settings _settings = Settings.Instance;

		private StringFormat _rulerTextFormatter = null;
		private float _hScrollPct = 0.5f;
		private float _vScrollPct = 0.5f;
		//private bool _movingMask = false;
		private bool _mouseOverCanvasPane = false;
		private bool _isFormShown = false;
		private bool _settingScroll = false;
		private bool _settingZoom = false;
		private IDocument _Document = null;
		private MouseButtons _mouseButtons;

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Text that displays for the tab for this form.
		/// </summary>
		public string DisplayText
		{
			set
			{
				this.Text = value;
				this.TabText = value;
			}
			get { return this.Text; }
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		public CanvasWindow()
		{
			InitializeComponent();
			ChangeShowRuler();

			// Hide the data label when not working with scrolling and positioning
			lblData.Visible = false;

			// Testing to make these controls visible from each other.
			//this.pnlWorkspace.BackColor = Color.Red;
			//this.pnlEasel.BackColor = Color.Blue;

			// Handle general UI properties such as MousePosition and ShowRuler
			_global.UI.PropertyChanged += new PropertyChangedEventHandler(this.UI_PropertyChanged);

			_rulerTextFormatter = new StringFormat();
			_rulerTextFormatter.LineAlignment = StringAlignment.Center;
			_rulerTextFormatter.Alignment = StringAlignment.Center;

			pnlEasel.HorizontalScroll.LargeChange = 1000;
			pnlEasel.VerticalScroll.LargeChange = 1000;
		}

		public CanvasWindow(IDocument Document)
			: this()
		{
			_Document = Document;

			if (_Document == null)
				return;

			_Document.ClickZoom += new EventHandlers.ZoomEventHandler(this.Document_ClickZoom);
			_Document.Loaded += new EventHandler(this.Document_Loaded);
			_Document.PropertyChanged += new PropertyChangedEventHandler(this.Document_PropertyChanged);
			_Document.ScalingChanged += new EventHandler(this.Document_ScalingChanged);
			_Document.DirtyChanged += new EventHandlers.DirtyEventHandler(this.Document_DirtyChanged);

			this.Text = _Document.Name;
			this.TabText = _Document.Name;
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Detach all events that are related to outside objects.
		/// </summary>
		public void DetachEvents()
		{
			_Document.Undo_Completed -= this.Undo_Completed;
			_global.UI.PropertyChanged -= this.UI_PropertyChanged;
			_Document.ClickZoom -= this.Document_ClickZoom;
			_Document.Loaded -= this.Document_Loaded;
			_Document.PropertyChanged -= this.Document_PropertyChanged;
			_Document.ScalingChanged -= this.Document_ScalingChanged;
			_Document.DirtyChanged -= this.Document_DirtyChanged;
			_Document = null;
			_global = null;
		}

		/// <summary>
		/// Creates a string describing the size and position of Easel and Canvas in regards to pnlWorkspace.
		/// </summary>
		private void DisplayPositionData()
		{
			string Report = string.Empty;

			Report += string.Format("WORKSPACE: Size ({0},{1}); ", pnlWorkspace.Size.Width, pnlWorkspace.Size.Height);
			Report += string.Format("Position ({0},{1}); ", pnlWorkspace.Location.X, pnlWorkspace.Location.Y);
			Report += "DockStyle." + pnlWorkspace.Dock.ToString() + "; ";

			Report += string.Format("EASEL: Size ({0},{1}); ", pnlEasel.Size.Width, pnlEasel.Size.Height);
			Report += string.Format("Position ({0},{1}); ", pnlEasel.Location.X, pnlEasel.Location.Y);
			Report += "DockStyle." + pnlEasel.Dock.ToString() + "; ";

			Report += string.Format("CANVAS: Size ({0},{1}); ", CanvasPane.Size.Width, CanvasPane.Size.Height);
			Report += string.Format("Position ({0},{1}); ", CanvasPane.Location.X, CanvasPane.Location.Y);

			lblData.Text = Report;
		}

		/// <summary>
		///  Sets the Undo.Complete event delegate. Called by the owning Document's InitializeUndo method.
		/// </summary>
		public void InitializeUndo()
		{
			_Document.Undo_Completed += new EventHandler(this.Undo_Completed);
		}

		/// <summary>
		/// Forces the refresh of the Ruler controls, if they are visible. This should be called everytime there is a 
		/// chance the tick marks could be moved, or if the mouse cursor
		/// is within the CanvasPane and it's ticks need to update.
		/// </summary>
		private void RefreshRulers()
		{
			if (RulerHorz.Visible)
				RulerHorz.Refresh();
			if (RulerVert.Visible)
				RulerVert.Refresh();
		}

		/// <summary>
		/// Moves the position of the control CanvasPane on the form.
		/// </summary>
		private void SetCanvasPosition()
		{
			if (!_isFormShown || (_Document == null))
				return;

			int ClientRectangleWidth = pnlWorkspace.Width;
			int ClientRectangleHeight = pnlWorkspace.Height;
			Size CanvasSize = _Document.Scaling.CanvasSize;

			if ((CanvasSize.Width < ClientRectangleWidth) &&
				(CanvasSize.Height < ClientRectangleHeight))
			{
				pnlEasel.Dock = DockStyle.Fill;
				_hScrollPct = 0.5f;
				_vScrollPct = 0.5f;
			}
			else
			{
				pnlEasel.Dock = DockStyle.None;
				// CanvasSize from the UISettings object is already scaled to zoom
				pnlEasel.Size = new Size(CanvasSize.Width + (ClientRectangleWidth / 2),
										 CanvasSize.Height + (ClientRectangleHeight / 2));

			}

			CanvasPane.Location = new Point((pnlEasel.Width - CanvasPane.Width) / 2,
											(pnlEasel.Height - CanvasPane.Height) / 2);

			if (!_settingZoom)
				pnlWorkspace.PerformLayout();
		}

		/// <summary>
		/// Updates the size of the CanvasPane control to match what UISettings has calculated for it. Updates the Easel control to best fit and determine if the scrollbars should display.
		/// This method is called whenever CellSize, GridLineWidth, Zoom, or the LatticeSize has been changed, as well as initially when the form first loads.
		/// </summary>
		private void SetCanvasSize()
		{
			SetCanvasSize(_Document.Scaling.CanvasSize);
		}

		/// <summary>
		/// Updates the size of the CanvasPane control to match what UISettings has calculated for it. Updates the Easel control to best fit and determine if the scrollbars should display.
		/// This method is called whenever CellSize, GridLineWidth, Zoom, or the LatticeSize has been changed, as well as initially when the form first loads.
		/// </summary>
		/// <param name="canvasSize">Size of the CanvasPane pictureBox</param>
		private void SetCanvasSize(Size canvasSize)
		{
			if (canvasSize.IsEmpty)
				return;

			CanvasPane.Size = new Size(canvasSize.Width, canvasSize.Height);
		}

		/// <summary>
		/// Determines what the percentage a given scroll bar is at
		/// </summary>
		/// <param name="scroll">ScrollProperties attribute from a scroll bar control</param>
		/// <returns>The calculated percentage a scroll bar is currently at</returns>
		private float GetScrollPercent(ScrollProperties scroll)
		{
			int MaxScroll = scroll.Maximum - scroll.LargeChange - 1;
			return (float)scroll.Value / (float)MaxScroll;
		}

		/// <summary>
		/// Set the scrolling amount based on pre-calculated percentages.
		/// </summary>
		private void SetScrollPercent()
		{
			SetScrollPercent(pnlWorkspace.HorizontalScroll, _hScrollPct);
			SetScrollPercent(pnlWorkspace.VerticalScroll, _vScrollPct);
			if (!_settingZoom)
				pnlWorkspace.PerformLayout();
		}

		/// <summary>
		/// Sets the scroll value of a scroll bar control based on the percentage passed in
		/// </summary>
		/// <param name="scroll">ScrollProperties attribute from a scroll bar control</param>
		/// <param name="scrollpercent">Percent of scrolling to set. 0 indicated all the way left.</param>
		private void SetScrollPercent(ScrollProperties scroll, float scrollpercent)
		{
			scrollpercent = (scrollpercent > 1) ? 1 : (scrollpercent < 0) ? 0 : scrollpercent;

			int MaxScroll = scroll.Maximum - scroll.LargeChange - 1;
			if (scrollpercent < 0f)
				scrollpercent = 0f;
			else if (scrollpercent > 100f)
				scrollpercent = 100f;
			scroll.Value = (int)(scrollpercent * MaxScroll);
		}

		/// <summary>
		/// Changes the size of the table layout control based on the visibility of the rulers.
		/// </summary>
		private void ChangeShowRuler()
		{
			bool Visible = true;// _global.UI.ShowRuler;
			RulerHorz.Visible = Visible;
			RulerVert.Visible = Visible;

			if (Visible)
			{
				tblLayout.RowStyles[0].Height = 25;
				tblLayout.ColumnStyles[0].Width = 35;
			}
			else
			{
				tblLayout.RowStyles[0].Height = 0;
				tblLayout.ColumnStyles[0].Width = 0;
			}

			SetCanvasPosition();
		}

		#endregion [ Methods ]

		#region [ Events ]

		#region [ CanvasPane Events ]

		/// <summary>
		/// Occurs when the cursor enters the CanvasPane
		/// </summary>
		private void CanvasPane_MouseEnter(object sender, EventArgs e)
		{
			_mouseOverCanvasPane = true;
			CanvasPane.Refresh();
		}

		/// <summary>
		/// Occurs when the cursor leaves the CanvasPane
		/// </summary>
		private void CanvasPane_MouseLeave(object sender, EventArgs e)
		{
			_mouseOverCanvasPane = false;
			CanvasPane.Refresh();
		}

		/// <summary>
		/// Occurs when the mouse moves over the CanvasPane.
		/// Update the CurrentMouseCell in UISettings with the mouse position, and calls the Rulers to update.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CanvasPane_MouseMove(object sender, MouseEventArgs e)
		{
			_global.UI.MousePosition = _global.CalcLatticePoint(new Point(e.X, e.Y));
			if (_global.CurrentTool != null)
				_global.CurrentTool.Tool.MouseMove(e.Button, _global.UI.MousePosition, e.Location);
			RefreshRulers();
		}

		/// <summary>
		/// Create the display of the Canvas for the CanvasPane
		/// </summary>
		private void CanvasPane_Paint(object sender, PaintEventArgs e)
		{
			DisplayPositionData();

			try
			{
				// ...

				RefreshRulers();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
			finally
			{ }
		}

		/// <summary>
		/// Occurs when the user clicks the mouse button on the CanvasPane
		/// </summary>
		private void CanvasPane_Click(object sender, EventArgs e)
		{
			_global.MouseClick(_mouseButtons, _global.UI.MousePosition, _global.UI.MousePosition);
		}

		/// <summary>
		/// Occurs when the user double-clicks the mouse button on the CanvasPane
		/// </summary>
		private void CanvasPane_DoubleClick(object sender, EventArgs e)
		{
			_global.MouseDoubleClick(_mouseButtons, _global.UI.MousePosition, _global.UI.MousePosition);
		}

		/// <summary>
		/// Occurs when the user holds the mouse button down on the CanvasPane
		/// </summary>
		private void CanvasPane_MouseDown(object sender, MouseEventArgs e)
		{
			// Capture the button state, to be used with Click and DoubleClick
			_mouseButtons = e.Button;
			_global.MouseDown(_mouseButtons, _global.UI.MousePosition, e.Location);
		}

		/// <summary>
		/// Occurs when the user releases the mouse button over the CanvasPane
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CanvasPane_MouseUp(object sender, MouseEventArgs e)
		{
			// Capture the button state, to be used with Click and DoubleClick
			_mouseButtons = e.Button;
			_global.MouseUp(_mouseButtons, _global.UI.MousePosition, e.Location);
		}

		#endregion [ CanvasPane Events ]

		#region [ Form Events ]

		/// <summary>
		/// Fires whenever this form become the active form. Indicate to the Document Controller that the Document who owns this form is now the Active Document.
		/// </summary>
		private void Form_Activated(object sender, EventArgs e)
		{
			//_global.DocumentController.Active = _Document;
		}

		/// <summary>
		/// The form has closed, detach all events and destroy all objects.
		/// </summary>
		private void Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			DetachEvents();
		}

		/// <summary>
		/// Event that fires when the window is first displayed
		/// </summary>
		private void Form_Shown(object sender, EventArgs e)
		{
			_isFormShown = true;
			SetCanvasPosition();
			_hScrollPct = 0.5f;
			_vScrollPct = 0.5f;
		}

		/// <summary>
		/// Event that fires whenever the size of the form changes
		/// </summary>
		private void Form_SizeChanged(object sender, EventArgs e)
		{
			SetCanvasPosition();
		}

		#endregion [ Form Events ]

		#region [ UISettings Events ]

		/// <summary>
		/// Occurs when a Property in UISettings changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UI_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case Editor_Template.Core.UISettings.Property_MousePosition:
					string ToolTip = string.Empty;
					ToolTip = string.Format("({0}, {1})", _global.UI.MousePosition.X, _global.UI.MousePosition.Y);

					//...

					toolTip1.SetToolTip(CanvasPane, ToolTip);
					break;

				case Editor_Template.Core.UISettings.Property_ShowRuler:
					ChangeShowRuler();
					break;
			}
		}

		private void UI_Refresh(object sender, EventArgs e)
		{
			CanvasPane.Refresh();
		}

		#endregion [ UISettings Events ]

		#region [ Mask Events ]

		/// <summary>
		/// Occurs when the Mask is Defined
		/// </summary>
		private void Mask_Defined(object sender, EventArgs e)
		{
			MarqueeAnimationTimer.Enabled = true;
		}

		/// <summary>
		/// Occurs when the Mask is Cleared
		/// </summary>
		private void Mask_Cleared(object sender, EventArgs e)
		{
			MarqueeAnimationTimer.Enabled = false;
		}

		#endregion [ Mask Events ]

		#region [ Document Events ]

		/// <summary>
		/// This event fires when the user, using the Zoom Tool, has clicked on the Canvas. The normal Changed event is suppressed,
		/// and this one fires instead. The CanvasPane should be scrolled such that the click point is centered on the screen, and the new Zoom Level
		/// set.
		/// </summary>
		private void Document_ClickZoom(object sender, ZoomEventArgs e)
		{
			_settingZoom = true;
			_settingScroll = true;

			DrawingControl.SuspendDrawing(pnlWorkspace);

			float OldZoom = _Document.Scaling.Zoom.GetValueOrDefault();

			// Perform the changes to zoom
			_Document.Scaling.Zoom = e.ZoomLevel;

			SetCanvasSize();
			SetCanvasPosition();

			// Regenerate the composite background image with the correct zoom scale
			//_Document.Background.Set();

			PointF ZoomPoint = e.ZoomPoint;
			SizeF Size = _Document.Scaling.CanvasSizeF;

			// Convert the zoom point from old zoom, to the new value.
			// We do this because the Zoom event has already fired by the time this event fires, and we need to scale the 
			// point accordingly
			ZoomPoint = new PointF((ZoomPoint.X / OldZoom) * e.ZoomLevel, (ZoomPoint.Y / OldZoom) * e.ZoomLevel);
			OldZoom = e.ZoomLevel;
			_hScrollPct = ZoomPoint.X / Size.Width;
			_vScrollPct = ZoomPoint.Y / Size.Height;

			_settingScroll = false;
			_settingZoom = false;

			SetScrollPercent();
			DrawingControl.ResumeDrawing(pnlWorkspace);
		}

		/// <summary>
		/// Fires when the dirty bit for the Document changed.
		/// </summary>
		private void Document_DirtyChanged(object sender, DirtyEventArgs e)
		{
			string Dirty = _Document.Dirty ? "*" : string.Empty;
			this.Text = _Document.Name + Dirty;
			this.TabText = _Document.Name + Dirty;
		}

		/// <summary>
		/// Occurs when the Document has finished loading. Reformat the display with the proper settings.
		/// </summary>
		private void Document_Loaded(object sender, EventArgs e)
		{
			DrawingControl.SuspendDrawing(pnlWorkspace);
			SetCanvasSize();
			SetCanvasPosition();

			// Regenerate the composite background image with the correct zoom scale
			//CanvasPane.BackColor = _Document.Background.Color;
			//_Document.Background.Set();

			if (_Document.Scaling.Zoom > Scaling.MIN_ZOOM)
				SetScrollPercent();
			DrawingControl.ResumeDrawing(pnlWorkspace);
			CanvasPane.Refresh();
		}

		/// <summary>
		/// Occurs when one of the Document's properties changed.
		/// </summary>
		private void Document_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (string.Compare(e.PropertyName, "Zoom", true) != 0)
				return;

			if (!_settingZoom)
			{
				DrawingControl.SuspendDrawing(pnlWorkspace);
				SetCanvasSize();
				SetCanvasPosition();

				if (_Document.Scaling.Zoom > Scaling.MIN_ZOOM)
					SetScrollPercent();
				DrawingControl.ResumeDrawing(pnlWorkspace);
			}

			CanvasPane.Refresh();
		}

		/// <summary>
		/// Occurs when one of the Document's scaling variables has changed.
		/// </summary>
		private void Document_ScalingChanged(object sender, EventArgs e)
		{
			if (_settingZoom)
				return;

			DrawingControl.SuspendDrawing(pnlWorkspace);
			SetCanvasSize();
			SetCanvasPosition();

			// Regenerate the composite background image with the correct zoom scale
			//_Document.Background.Set();

			if (_Document.Scaling.Zoom > Scaling.MIN_ZOOM)
				SetScrollPercent();
			DrawingControl.ResumeDrawing(pnlWorkspace);

			CanvasPane.Refresh();
		}

		/// <summary>
		/// Fires when an Undo or Redo operation completes.
		/// </summary>
		private void Undo_Completed(object sender, EventArgs e)
		{
			DrawingControl.SuspendDrawing(pnlWorkspace);
			SetCanvasSize();
			SetCanvasPosition();

			// Regenerate the composite background image with the correct zoom scale
			//CanvasPane.BackColor = _Document.Background.Color;
			//_Document.Background.Set();

			if (_Document.Scaling.Zoom > Scaling.MIN_ZOOM)
				SetScrollPercent();
			DrawingControl.ResumeDrawing(pnlWorkspace);
			CanvasPane.Refresh();

			//MarqueeAnimationTimer.Enabled = _Document.HasMask;
		}

		#endregion [ Document Events ]

		/// <summary>
		/// Timer that animates the marquee around a masked area
		/// </summary>
		private void MarqueeAnimationTimer_Tick(object sender, EventArgs e)
		{
			//if (_Document.HasMask)
			//{
			//	GraphicsPath TempPath = (GraphicsPath)_Document.GetMaskOutline(UnitScale.Canvas).Clone();
			//	TempPath.Widen(new Pen(Color.White, 1));
			//	CanvasPane.Invalidate(new Region(TempPath));
			//	TempPath.Dispose();
			//}
		}

		/// <summary>
		/// This event fires when one or more of the pnlWorkspace scroll bars are scrolled.
		/// </summary>
		private void Workspace_Scroll(object sender, ScrollEventArgs e)
		{
			RefreshRulers();
			if (!_settingScroll)
			{
				_hScrollPct = GetScrollPercent(pnlWorkspace.HorizontalScroll);
				_vScrollPct = GetScrollPercent(pnlWorkspace.VerticalScroll);
			}
		}

		/// <summary>
		/// Paint on the ruler ticks
		/// </summary>
		private void RulerHorz_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(new SolidBrush(RulerHorz.BackColor), e.ClipRectangle);

			Pen DrawPen = new Pen(Color.Black);
			SolidBrush TextBrush = new SolidBrush(RulerHorz.ForeColor);

			RectangleF Rect;
			string Text = string.Empty;

			// Determine the offset for the Ruler
			// First account for the difference in where the ruler's origin is vs the Workspace's origin.
			int StartTick = pnlWorkspace.Location.X - RulerHorz.Location.X;

			// Next, determine where the origin of the Canvas is relative to this point
			StartTick += pnlEasel.Location.X;
			StartTick += CanvasPane.Location.X;

			//Debug.WriteLine(CanvasPane.Location);

			int Tick = 0;
			int Bump = 0;
			int TextPos = 0;

			for (int x = 0; x <= _Document.Scaling.LatticeSize.Width; x++)
			{
				Tick = StartTick + (x * _Document.Scaling.CellScale);
				Bump = (x % 10 == 0) ? 12 : ((x % 10 == 5) ? 8 : 6);
				e.Graphics.DrawLine(DrawPen, new Point(Tick, RulerHorz.Height - Bump), new Point(Tick, RulerHorz.Height));

				if (x % 10 == 0)
				{
					Text = x.ToString();
					TextPos = RulerHorz.Height - Bump;
					Rect = new RectangleF(new Point(Tick, TextPos), e.Graphics.MeasureString(Text, RulerHorz.Font));
					TextPos -= (int)Rect.Height;
					Rect.Location = new PointF(Tick - (Rect.Width / 2), TextPos);
					e.Graphics.DrawString(Text, RulerHorz.Font, TextBrush, Rect, _rulerTextFormatter);
				}
			}

			// If the mouse is over the CanvasPane, draw the position tick mark
			if (_mouseOverCanvasPane)
			{
				Tick = StartTick + _global.CalcCanvasPoint(_global.UI.MousePosition).X;
				e.Graphics.DrawLine(DrawPen, new Point(Tick, RulerHorz.Top), new Point(Tick, RulerHorz.Height));
			}

			DrawPen?.Dispose();
			TextBrush?.Dispose();
		}

		/// <summary>
		/// Paint on the ruler ticks
		/// </summary>
		private void RulerVert_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.FillRectangle(SystemBrushes.Control, e.ClipRectangle);
			Pen DrawPen = new Pen(Color.Black);
			RectangleF Rect;
			SolidBrush TextBrush = new SolidBrush(RulerVert.ForeColor);
			string Text = string.Empty;

			// Determine the offset for the Ruler
			// First account for the difference in where the ruler's origin is vs the Workspace's origin.
			int StartTick = pnlWorkspace.Location.Y - RulerVert.Location.Y;

			// Next, determine where the origin of the Canvas is relative to this point
			StartTick += pnlEasel.Location.Y;
			StartTick += CanvasPane.Location.Y;
			int Tick = 0;
			int Bump = 0;
			int TextPos = 0;

			for (int y = 0; y <= _Document.Scaling.LatticeSize.Height; y++)
			{
				Tick = StartTick + (y * _Document.Scaling.CellScale);
				Bump = (y % 10 == 0) ? 12 : ((y % 10 == 5) ? 8 : 6);
				e.Graphics.DrawLine(DrawPen, new Point(RulerVert.Width - Bump, Tick), new Point(RulerVert.Width, Tick));

				if (y % 10 == 0)
				{
					Text = y.ToString();
					TextPos = RulerVert.Width - Bump;
					Rect = new RectangleF(new Point(TextPos, Tick), e.Graphics.MeasureString(Text, RulerVert.Font));
					TextPos -= (int)Rect.Width;
					Rect.Location = new PointF(TextPos, Tick - (Rect.Height / 2));
					e.Graphics.DrawString(Text, RulerVert.Font, TextBrush, Rect, _rulerTextFormatter);
				}
			}

			// If the mouse is over the CanvasPane, draw the position tick mark
			if (_mouseOverCanvasPane)
			{
				Tick = StartTick + _global.CalcCanvasPoint(_global.UI.MousePosition).Y;
				e.Graphics.DrawLine(DrawPen, new Point(RulerVert.Left, Tick), new Point(RulerVert.Width, Tick));
			}

			DrawPen?.Dispose();
			TextBrush?.Dispose();
		}

		#endregion [ Events ]
	}
}

