using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using Editor_Template.Forms;
using Editor_Template.Interfaces;
using Editor_Template.Singletons;
using Editor_Template.Utilities;

namespace Editor_Template.Core
{
	public class Document : Base, IDocument
	{
		#region [ Private Variables ]

		protected CanvasWindow _canvasWindow = null;
		private string _name;
		private Scaling _scaling;
		protected PictureBox _substituteCanvas = null;
		protected UndoController _undoController;
		protected object objectLock = new Object();

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Gets and Sets the cursor for the CanvasPane control.
		/// </summary>
		public Cursor Cursor
		{
			get
			{
				if (_canvasWindow?.CanvasPane != null)
					return _canvasWindow.CanvasPane.Cursor;
				else
					return Cursors.Default;
			}
			set
			{
				if (_canvasWindow?.CanvasPane != null)
					_canvasWindow.CanvasPane.Cursor = value;
			}
		}

		/// <summary>
		/// Path where files are saved by default.
		/// </summary>
		public string DefaultSavePath { get; set; }

		/// <summary>
		/// Indicate whether this type of Document should be currently displayed as an option in the Editor.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Extension for the filename
		/// </summary>
		public string FileExtension { get; set; }

		public string Filename { get; set; }

		public CanvasWindow Form
		{
			get => _canvasWindow;
			set => _canvasWindow = value;
		}

		/// <summary>
		/// Name of the type of Document.
		/// </summary>
		public string FormatName { get; }

		/// <summary>
		/// Indicates whether there are elements in the Redo stack.
		/// </summary>
		public bool HasRedo
		{
			get => UndoController?.HasRedo ?? false;
		}

		/// <summary>
		/// Indicates whether there are elements in the Redo stack.
		/// </summary>
		public bool HasUndo
		{
			get => UndoController?.HasUndo ?? false;
		}

		/// <summary>
		/// Image file that represents the Icon of the program used by this type of Document.
		/// </summary>
		public Bitmap IconImage { get; }

		/// <summary>
		/// Unique ID assigned to this Document type.
		/// </summary>
		public int ID { get; set; }

		public List<object> Items { get; set; }

		public virtual string Name
		{
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					if (_canvasWindow != null)
						_canvasWindow.DisplayText = value;
					OnPropertyChanged(Constants.PROPERTY_NAME, true);
				}
			}
		}

		public virtual Scaling Scaling
		{
			get { return _scaling; }
			set
			{
				_scaling.SuppressEvents = true;
				_scaling.LatticeSize = value.LatticeSize;
				_scaling.CellSize = value.CellSize;
				_scaling.ShowGridLines = value.ShowGridLines;
				_scaling.Zoom = value.Zoom;
				_scaling.SuppressEvents = false;
			}
		}

		[XmlIgnore(), Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public virtual PictureBox SubstituteCanvas
		{
			get => _substituteCanvas;
			set => _substituteCanvas = value;
		}

		public UndoController UndoController
		{
			get => _undoController;
			set => _undoController = value;
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		/// <summary>        
		/// The default Constructor.
		/// </summary>        
		public Document()
			: base()
		{
			this.Scaling = new Scaling(true);
			_scaling.DirtyChanged += new EventHandlers.DirtyEventHandler(ChildObject_Dirty);
			_scaling.PropertyChanged += new PropertyChangedEventHandler(Scaling_PropertyChanged);

			_canvasWindow = new Forms.CanvasWindow(this);
			_canvasWindow.FormClosing += new FormClosingEventHandler(this.Form_FormClosing);
			_canvasWindow.CanvasPane.MouseEnter += new EventHandler(CanvasWindow_MouseEnter);
			_canvasWindow.CanvasPane.MouseLeave += new EventHandler(CanvasWindow_MouseLeave);

			this.Filename = string.Empty;
			_name = string.Empty;
			_scaling.Clear(true);
		}

		/// <summary>
		/// Constructor with a Document's filename to open
		/// </summary>
		/// <param name="filename">Name of the Document file.</param>
		public Document(string filename)
			: this()
		{
			Load(filename);
		}
		
		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		///  Clears out all the value for the properies and protected virtual variables. Used to initialize the object initially, and when loading new data
		/// </summary>
		protected virtual void Clear()
		{
			_scaling.Clear(true);
		}

		/// <summary>
		/// Only currently used in Preview
		/// DELETE
		/// </summary>
		public void Clear_UndoStacks()
		{
			_undoController?.Clear();
		}

		/// <summary>
		/// Informs the Document to close and to fire its Closing event.
		/// </summary>
		public void Close()
		{
			Closing?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Creates and returns a text representative of the data in the Redo stack.
		/// </summary>
		public string Debug_RedoStack()
		{
			return string.Empty;
		}

		/// <summary>
		/// Creates and returns a text representative of the data in the Undo stack.
		/// </summary>
		public string Debug_UndoStack()
		{
			return string.Empty;
		}

		/// <summary>
		/// Creates and returns a text representative of the current undo snapshot.
		/// </summary>
		public string Debug_UndoSnapshot()
		{
			return string.Empty;
		}

		/// <summary>
		/// Clean up all child objects here, unlink all events and dispose
		/// </summary>
		protected override void DisposeChildObjects()
		{
			base.DisposeChildObjects();
			if (_canvasWindow != null)
			{
				_canvasWindow.FormClosing -= Form_FormClosing;
				_canvasWindow.CanvasPane.MouseEnter -= CanvasWindow_MouseEnter;
				_canvasWindow.CanvasPane.MouseLeave -= CanvasWindow_MouseLeave;
				_canvasWindow = null;
			}
			if (_scaling != null)
			{
				_scaling.DirtyChanged -= ChildObject_Dirty;
				_scaling.PropertyChanged -= Scaling_PropertyChanged;
				_scaling.Dispose();
				_scaling = null;
			}			
			if (_undoController != null)
			{
				_undoController.UndoChanged -= UndoController_UndoChanged;
				_undoController.RedoChanged -= UndoController_RedoChanged;
				_undoController.Completed -= UndoController_Completed;
				_undoController = null;
			}
		}

		/// <summary>
		/// System.Windows.PictureBox control from the CanvasWindow form.
		/// </summary>
		public PictureBox GetCanvas()
		{
			if (_substituteCanvas != null)
				return _substituteCanvas;

			if ((_canvasWindow != null) && (_canvasWindow.CanvasPane != null))
				return _canvasWindow.CanvasPane;
			else
				return null;
		}

		/// <summary>
		/// System.Drawing.Graphics object created from the CanvasPane object.
		/// </summary>
		public Graphics GetCanvasGraphics()
		{
			return _canvasWindow?.CanvasPane?.CreateGraphics();
		}

		/// <summary>
		/// Takes a peek at the topmost item on the undo (or redo) stack and reports back the text of the item.
		/// If there are no items on the stack, returns an empty string.
		/// </summary>
		public string GetUndoText(bool undo)
		{
			return undo ? _undoController.UndoText : _undoController.RedoText;
		}

		/// <summary>
		/// Set up the Undo/Redo stacks
		/// </summary>
		public void InitializeUndo()
		{
			if (_undoController == null)
			{
				_undoController = new UndoController(this);
				_undoController.UndoChanged += new EventHandlers.UndoEventHandler(UndoController_UndoChanged);
				_undoController.RedoChanged += new EventHandlers.UndoEventHandler(UndoController_RedoChanged);
				_undoController.Completed += new EventHandler(UndoController_Completed);
			}
			else
			{
				_undoController.Clear(true);
			}

			_undoController.GetInitialSnapshot();
			_canvasWindow.InitializeUndo();
		}

		/// <summary>
		/// Loads in the Document data using the filename stored within the object
		/// </summary>
		/// <returns>Returns true if the load is successful, false otherwise</returns>
		public bool Load()
		{
			return true;
		}

		/// <summary>
		/// Loads in the Document data from the file passed in.
		/// </summary>
		/// <param name="filename">Name of the file to load</param>
		/// <returns>Returns true if the load is successful, false otherwise</returns>
		public bool Load(string fileName)
		{
			return false;
		}

		/// <summary>
		/// Rewinds the last Undo performed, reapplying the changes
		/// </summary>
		public void Redo()
		{ }

		/// <summary>
		/// Release the capture of the mouse cursor
		/// </summary>
		public void ReleaseMouse()
		{
			if (_canvasWindow?.CanvasPane != null)
			{
				_canvasWindow.CanvasPane.Capture = false;
				Cursor.Clip = Rectangle.Empty;
			}
		}

		/// <summary>
		/// Refresh the redraw on the the CanvasPane.
		/// </summary>
		public void Refresh()
		{
			_canvasWindow?.CanvasPane?.Refresh();
		}

		/// <summary>
		/// Saves the Document.
		/// </summary>
		/// <returns>Returns true if the save is successful, false otherwise</returns>
		public bool Save()
		{
			return false;
		}

		/// <summary>
		/// Saves the Document to file using the filename passed in
		/// </summary>
		/// <param name="filename">Filename to use to save the file to</param>
		/// <returns>Returns true if the save is successful, false otherwise</returns>
		public bool Save(string filename)
		{
			return true;
		}

		/// <summary>
		/// The program has just performed an operation that can be undone. Grab a snapshot of the data
		/// and save the differences between this and the last as a Changeset
		/// </summary>
		/// <param name="action">Text of the operation complete, this will appear in the Undo menu in the Editor</param>
		public void SaveUndo(string action)
		{
			_undoController?.SaveUndo(action);
		}

		/// <summary>
		/// Sets the dirty flag to be false for this object all its child objects
		/// </summary>
		public override void SetClean()
		{
			base.SetClean();
			_scaling.SetClean();

			OnDirty();
		}

		/// <summary>
		/// Triggers an event to fire, due to the user clicking with the Zoom tool on a point on the canvas.
		/// </summary>
		/// <param name="zoomPoint">Point on the canvas the user has clicked</param>
		/// <param name="zoomLevel">New zoom amount to use.</param>
		public void SetClickZoom(Point zoomPoint, float zoomLevel)
		{
			if (ClickZoom == null)
			{
				_scaling.Zoom = zoomLevel;
				return;
			}
			ClickZoom(this, new ZoomEventArgs(zoomPoint, zoomLevel));
		}

		/// <summary>
		/// Returns the Name of this Document
		/// </summary>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Trap the mouse to only live inside of the canvas, so we don't get weird effects, like drawings starting outside, or ending outside the pictureBox.
		/// Call ReleaseMouse() on the MouseUp event to allow the cursor to act normal.
		/// </summary>
		public void TrapMouse()
		{
			// Trap the mouse into the Canvas while we are working
			if (_canvasWindow?.CanvasPane != null)
			{
				var rc = _canvasWindow.CanvasPane.RectangleToScreen(new Rectangle(Point.Empty, _canvasWindow.CanvasPane.ClientSize));
				Cursor.Clip = rc;
				_canvasWindow.CanvasPane.Capture = true;
			}
		}

		/// <summary>
		/// Detach the CanvasWindow object from the events and destroy it.
		/// </summary>
		public void UnclipCanvasWindow()
		{
			if (_canvasWindow == null)
				return;

			_canvasWindow.FormClosing -= Form_FormClosing;
			_canvasWindow.CanvasPane.MouseEnter -= CanvasWindow_MouseEnter;
			_canvasWindow.CanvasPane.MouseEnter -= CanvasWindow_MouseLeave;
			_canvasWindow.DetachEvents();
			_canvasWindow.Dispose();
		}

		/// <summary>
		/// Looks at the last set of changes and applies the old values.
		/// </summary>
		public virtual void Undo()
		{
			_undoController.Undo();
		}
		
		#endregion [ Methods ]

		#region [ Events ]

		#region [ Event Triggers ]

		/// <summary>
		/// Called once the Load method has finished to inform client objects of this fact.
		/// </summary>
		protected virtual void OnLoaded()
		{
			if (Loaded != null)
				this.Loaded(this, new EventArgs());
		}

		/// <summary>
		/// Throw the ScalingChanged event.
		/// </summary>
		protected virtual void OnScalingChanged()
		{
			if (SuppressEvents)
				return;

			ScalingChanged?.Invoke(this, new EventArgs());
		}

		#endregion [ Event Triggers ]

		#region [ Event Handlers ]

		/// <summary>
		/// Occurs when the cursor enters the rectangle defined by the Document's Canvas control.
		/// </summary>
		public event EventHandler Canvas_MouseLeave;

		/// <summary>
		/// Occurs when the cursor leaves the rectangle defined by the Document's Canvas control.
		/// </summary>
		public event EventHandler Canvas_MouseEnter;
		
		/// <summary>
		/// Occurs when the Zoom tool is selected and the user clicks on the Canvas.
		/// </summary>
		public EventHandlers.ZoomEventHandler ClickZoom;

		/// <summary>
		/// Fires when the Document is being closed, given objects that have event delegates for this Document a chance to remove them.
		/// </summary>
		public EventHandler Closing;

		/// <summary>
		/// Fires once the Load method has finished.
		/// </summary>
		public event EventHandler Loaded;

		/// <summary>
		/// Occurs when the Mask is cleared.
		/// </summary>
		//public event EventHandler Mask_Cleared;

		/// <summary>
		/// Occurs when the Mask is defined.
		/// </summary>
		//public event EventHandler Mask_Defined;

		/// <summary>
		/// Occurs when a new item is the top item on the Redo stack. This is a bubbled event from the UndoController.
		/// </summary>
		public EventHandlers.UndoEventHandler Redo_Changed;

		/// <summary>
		/// Occurs when one of the scaling properties have been changed.
		/// </summary>
		public event EventHandler ScalingChanged;
		
		/// <summary>
		/// Occurs when a new item is the top item on the Undo stack. This is a bubbled event from the UndoController.
		/// </summary>
		public EventHandlers.UndoEventHandler Undo_Changed;

		/// <summary>
		/// Occurs when an Undo or Redo operation has completed.
		/// </summary>
		public EventHandler Undo_Completed;

		// Explicit interface implementation required. 
		// Associate IDocument's event with ClickZoom
		event EventHandlers.ZoomEventHandler IDocument.ClickZoom
		{
			add
			{
				lock (objectLock)
				{
					ClickZoom += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					ClickZoom -= value;
				}
			}
		}

		event EventHandler IDocument.Closing
		{
			add
			{
				lock (objectLock)
				{
					Closing += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					Closing -= value;
				}
			}
		}

		event EventHandlers.DirtyEventHandler IBase.DirtyChanged
		{
			add
			{
				lock (objectLock)
				{
					DirtyChanged += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					DirtyChanged -= value;
				}
			}
		}

		event EventHandlers.UndoEventHandler IDocument.Redo_Changed
		{
			add
			{
				lock (objectLock)
				{
					Redo_Changed += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					Redo_Changed -= value;
				}
			}
		}

		event EventHandlers.UndoEventHandler IDocument.Undo_Changed
		{
			add
			{
				lock (objectLock)
				{
					Undo_Changed += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					Undo_Changed -= value;
				}
			}
		}

		event EventHandler IDocument.Undo_Completed
		{
			add
			{
				lock (objectLock)
				{
					Undo_Completed += value;
				}
			}
			remove
			{
				lock (objectLock)
				{
					Undo_Completed -= value;
				}
			}
		}

		#endregion [ Event Handlers ]

		#region [ Event Delegates ]

		/// <summary>
		/// Occurs when a property in the Background object is changed.
		/// </summary>
		protected virtual void Background_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(sender, e.PropertyName);
		}

		/// <summary>
		/// Occurs when the mouse has entered the Canvas control's rectangle.
		/// </summary>
		protected virtual void CanvasWindow_MouseEnter(object sender, EventArgs e)
		{
			if (SuppressEvents)
				return;
			Canvas_MouseEnter?.Invoke(sender, e);
		}

		/// <summary>
		/// Occurs when the mouse leaves the Canvas control's rectangle.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void CanvasWindow_MouseLeave(object sender, EventArgs e)
		{
			if (SuppressEvents)
				return;
			Canvas_MouseLeave?.Invoke(sender, e);
		}

		/// <summary>
		/// Occurs when the Dirty flag on a given object is changed
		/// </summary>
		protected virtual void ChildObject_Dirty(object sender, DirtyEventArgs e)
		{
			if (e.IsDirty)
				this.Dirty = true;
		}
		
		/// <summary>
		/// Occurs when the CanvasWindow form tries to close, either throw program shut down, using selecting a menu item that closes the form, or the user clicks the
		/// "X" on the corner of the form. If the Document is dirty, prompt the user to save changes, with a Yes|No|Cancel messagebox. If Cancel is clicked, then we need to cancel
		/// the closing.
		/// </summary>
		protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			DialogResult Result = DialogResult.None;
			string Message = "Save changes to this Document before closing?";
			if (this.Name.Length > 0)
				Message = string.Format("Save changes to \"{0}\" before closing?", this.Name);

			if (this.Dirty)
				Result = MessageBox.Show(Message, "Close Document", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if (Result == DialogResult.Cancel)
			{
				e.Cancel = true;
				return;
			}
			if (Result == DialogResult.Yes)
			{
				if (!Save())
				{
					e.Cancel = true;
					return;
				}
			}
			else
			{
				this.SuppressEvents = true;
				this.Dirty = false;
			}
		}

		/// <summary>
		/// Occurs when a property in the Scaling object is changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void Scaling_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnPropertyChanged(sender, e.PropertyName);
			OnScalingChanged();
		}

		/// <summary>
		/// Occurs when an undo/redo operation has been completed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void UndoController_Completed(object sender, EventArgs e)
		{
			if (SuppressEvents)
				return;
			Undo_Completed?.Invoke(sender, e);
		}

		/// <summary>
		/// Occurs when a new item is the topmost item in the Redo stack, or the Redo stack has been emptied
		/// </summary>
		protected virtual void UndoController_RedoChanged(object sender, UndoEventArgs e)
		{
			if (SuppressEvents)
				return;
			Redo_Changed?.Invoke(sender, e);
		}

		/// <summary>
		/// Occurs when a new item is the topmost item in the Undo stack, or the Undo stack has been emptied
		/// </summary>
		protected virtual void UndoController_UndoChanged(object sender, UndoEventArgs e)
		{
			if (SuppressEvents)
				return;
			Undo_Changed?.Invoke(sender, e);
		}

		#endregion [ Events Delegates ]

		#endregion [ Events ]
	}

}
