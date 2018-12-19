using System;
using System.ComponentModel;
using System.Drawing;
using Editor_Template.Interfaces;
using Editor_Template.Singletons;
//using KellyControls.PlugInToolBtn;

namespace Editor_Template.Core
{
	[Serializable]
	public class UISettings : Base, INotifyPropertyChanged
	{

		#region [ Constants ]

		// Property name constants
		public const string Property_MouseDownPosition = "MouseDownPosition";
		public const string Property_MousePosition = "MousePosition";
		public const string Property_MouseSelectionSize = "MouseSelectionSize";
		public const string Property_ShowRuler = "ShowRuler";
		public const string Property_TraceLogFilename = "TraceLogFilename";

		#endregion [ Constants ]

		#region [ Private Variables ]

		#region [ NonSerialized ]

		// UI Menu settings
		private bool _showRuler = true;

		// Mouse position
		private Point _currentMouseCell = Point.Empty;
		private Point _mouseDownCell = Point.Empty;
		private Size _mouseSelectionSize = Size.Empty;

		//private List<PlugInTool> _tools = null;

		#endregion [ NonSerialized ]

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Shortcut to the Active Document
		/// </summary>
		public IDocument ActiveDocument
		{
			get { return DocumentController.Instance.Active; }
		}
		
		/// <summary>
		/// Point (in Cells) at which the Mouse button was clicked last
		/// </summary>
		public Point MouseDownPosition
		{
			get { return _mouseDownCell; }
			set
			{
				if (!_mouseDownCell.Equals(value))
				{
					_mouseDownCell = value;
					if (!_mouseDownCell.IsEmpty)
						MouseSelectionSize = new Size(Math.Abs(_currentMouseCell.X - _mouseDownCell.X), Math.Abs(_currentMouseCell.Y - _mouseDownCell.Y));
					else
						MouseSelectionSize = Size.Empty;
					OnPropertyChanged(Property_MouseDownPosition, false);
				}
			}
		}

		/// <summary>
		/// Location of the mouse cursor in Cells
		/// </summary>
		public Point MousePosition
		{
			get { return _currentMouseCell; }
			set
			{
				if (!_currentMouseCell.Equals(value))
				{
					_currentMouseCell = value;
					if (!_mouseDownCell.IsEmpty)
						MouseSelectionSize = new Size(Math.Abs(_currentMouseCell.X - _mouseDownCell.X), Math.Abs(_currentMouseCell.Y - _mouseDownCell.Y));
					else
						MouseSelectionSize = Size.Empty;
					OnPropertyChanged(Property_MousePosition, false);
				}
			}
		}

		/// <summary>
		/// Distance between the current mouse position and where the mouse button was down
		/// </summary>
		public Size MouseSelectionSize
		{
			get { return _mouseSelectionSize; }
			set
			{
				if (!_mouseSelectionSize.Equals(value))
				{
					_mouseSelectionSize = value;
					OnPropertyChanged(Property_MouseSelectionSize, false);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Rulers are display on the CanvasWindow.
		/// </summary>
		public bool ShowRuler
		{
			get { return _showRuler; }
			set
			{
				if (_showRuler != value)
				{
					_showRuler = value;
					OnPropertyChanged(Property_ShowRuler, true);
				}
			}
		}

		///// <summary>
		///// List of all the Tools
		///// </summary>
		//internal List<PlugInTool> Tools
		//{
		//	get { return _tools; }
		//	set { _tools = value; }
		//}

		#endregion [ Properties ]

		#region [ Event Handlers ]

		/// <summary>
		/// Occurs when the mouse button is pressed property changes.
		/// </summary>
		public EventHandler MouseDown;

		/// <summary>
		/// Occurs when the mouse position on the Canvas changes.
		/// </summary>
		public EventHandler MousePoint;

		#endregion [ Event Handlers ]

		#region [ Constructor ]

		public UISettings()
			: base()
		{
			//_tools = new List<PlugInTool>();
		}

		#endregion [ Constructor ]

		#region [ Methods ]

		/// <summary>
		/// Load the UI settings
		/// </summary>
		public void LoadSettings()
		{
			ShowRuler = Settings.Instance.GetValue(Property_ShowRuler, true);
		}

		public void SaveSettings()
		{
			Settings.Instance.SetValue(Property_ShowRuler, ShowRuler);
		}

		#endregion [ Methods ]

	}
}
