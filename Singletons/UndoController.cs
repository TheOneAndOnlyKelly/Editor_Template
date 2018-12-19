using System;
using System.Collections.Generic;
using System.Text;
using Editor_Template.Core;
using Editor_Template.Core.UndoData;
using Editor_Template.Utilities;

namespace Editor_Template.Singletons
{
	/// <summary>
	/// Handles all the Undo and Redo operations by taking a snapshot of the data before and after an operation and saves the differences.
	/// </summary>
	public sealed class UndoController : Core.Base
	{
		#region [ Enum ]

		enum Activity : int
		{
			Undo,
			Redo
		}

		#endregion [ Enum ]

		#region [ Constants ]

		public const string UNCHANGED = "UNCHANGED";

		#endregion [ Constants ]

		#region [ Private Variables ]

		private Snapshot _lastSnapshot = null;
		private Stack<UndoRedo> _undoStack = null;
		private Stack<UndoRedo> _redoStack = null;
		private Document _Document = null;
		private bool _applyingChangeSet = false;
		private static readonly Lazy<UndoController> lazy = new Lazy<UndoController>(() => new UndoController());
		
		#endregion [ Private Variables ]

		#region [ Properties ]

		public static UndoController Instance { get { return lazy.Value; } }

		/// <summary>
		/// Indicates whether there are any Redos waiting on the stack
		/// </summary>
		public bool HasRedo
		{
			get { return (_redoStack.Count > 0); }
		}

		/// <summary>
		/// Indicates whether there are any Undos waiting on the stack
		/// </summary>
		public bool HasUndo
		{
			get { return (_undoStack.Count > 0); }
		}

		/// <summary>
		/// Returns the action text of the topmost Redo. If there are none present in the stack, returns an empty string.
		/// </summary>
		public string RedoText
		{
			get
			{
				if (_redoStack.Count > 0)
					return _redoStack.Peek().Action;
				else
					return string.Empty;
			}
		}

		/// <summary>
		/// Returns the action text of the top most Undo. If there are none present in the stack, returns an empty string.
		/// </summary>
		public string UndoText
		{
			get
			{
				if (_undoStack.Count > 0)
					return _undoStack.Peek().Action;
				else
					return string.Empty;
			}
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		public UndoController()
			: base()
		{
			_undoStack = new Stack<UndoRedo>();
			_redoStack = new Stack<UndoRedo>();
		}

		public UndoController(Document Document)
			: this()
		{
			_Document = Document;
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Applies changes to the data
		/// </summary>
		/// <param name="changeset">Set of data changes, either from an Undo or a Redo event</param>
		/// <param name="useUndoChangeSet">Indicates whether the Undo or the Redo Changeset should be used</param>
		private void ApplyChangeset(UndoRedo changeset, Activity activity)
		{
			_applyingChangeSet = true;
			bool Undoing = (activity == Activity.Undo);
			string Serialized = string.Empty;
			string ListString = string.Empty;

			_Document.SuppressEvents = true;

			#region [ Scaling ]

			Scaling Scaling = changeset.Scaling(Undoing);
			if (Scaling != null)
			{
				if (!Scaling.LatticeSize.IsEmpty)
				{
					_Document.Scaling.LatticeSize = Scaling.LatticeSize;
					//ChangedScaling = true;
				}

				if (Scaling.CellSize != null)
				{
					_Document.Scaling.CellSize = Scaling.CellSize.GetValueOrDefault();
					//ChangedScaling = true;
				}

				if (Scaling.ShowGridLines != null)
				{
					_Document.Scaling.ShowGridLines = Scaling.ShowGridLines.GetValueOrDefault(true);
					//ChangedScaling = true;
				}

				if (Scaling.Zoom != null)
				{
					_Document.Scaling.Zoom = Scaling.Zoom.GetValueOrDefault();
					//ChangedScaling = true;
				}
			}

			#endregion [ Scaling ]

			#region [ Clipboard ]

			bool ClipboardChanged = false;

			if (ClipboardChanged)
				ClipboardController.Instance.DisplayDiagnosticData();

			#endregion [ Clipboard ]

			_Document.SuppressEvents = false;
			_applyingChangeSet = false;
			
		}

		/// <summary>
		/// Clears out the Undo stacks
		/// </summary>
		/// <param name="suppressEvents">Indiciates whether the Changed events should supressed from firing when performing this task</param>
		public void Clear(bool suppressEvents)
		{
			if (_undoStack.Count > 0)
			{
				_undoStack.Clear();
				if (!suppressEvents)
					OnUndoChanged();
			}
			if (_redoStack.Count > 0)
			{
				_redoStack.Clear();
				if (!suppressEvents)
					OnRedoChanged();
			}
		}

		/// <summary>
		/// Clears out the Undo stacks
		/// </summary>
		public void Clear()
		{
			Clear(false);
		}

		/// <summary>
		/// Copy the data from the source object to this one.
		/// </summary>
		/// <param name="source">Object to copy from</param>
		public void CopyFrom(UndoController source)
		{
			this.SuppressEvents = true;

			_lastSnapshot = source._lastSnapshot;
			var Temp = new Stack<UndoRedo>();

			// Flip the Undo stack over onto temp.
			while (_undoStack.Count > 0)
				Temp.Push(source._undoStack.Pop());

			// Now push all the items in temp into the original stack and this one.
			while (Temp.Count > 0)
			{
				var Item = Temp.Pop();
				_undoStack.Push(Item);
				source._undoStack.Push(Item);
			}

			// Flip the Redo stack over onto temp.
			while (_redoStack.Count > 0)
				Temp.Push(source._redoStack.Pop());

			// Now push all the items in temp into the original stack and this one.
			while (Temp.Count > 0)
			{
				var Item = Temp.Pop();
				_redoStack.Push(Item);
				source._redoStack.Push(Item);
			}

			this.SuppressEvents = false;
		}

		/// <summary>
		/// Clean up all child objects here, unlink all events and dispose
		/// </summary>
		protected override void DisposeChildObjects()
		{
			base.DisposeChildObjects();

			_lastSnapshot?.Dispose();
			if (_undoStack != null)
			{
				while (_undoStack.Count > 0)
				{
					var Data = _undoStack.Pop();
					Data?.Dispose();
				}
				_undoStack = null;
			}
			if (_redoStack != null)
			{
				_redoStack = null;
				while (_redoStack.Count > 0)
				{
					var Data = _redoStack.Pop();
					Data?.Dispose();
				}
			}
		}

		/// <summary>
		/// Compares the current snapshot to the last one, records the differences in a ChangeSet
		/// </summary>
		/// <param name="action">Text to indicate the last operation the program performed to warrent undoing</param>
		/// <param name="current">The current Snapshot of the data</param>
		/// <returns>ChangeSet object that holds the differences between the current snapshot and the last one</returns>
		private UndoRedo FindSnapshotChanges(string action, Snapshot current)
		{
			var Changes = new UndoRedo(action);
			var FoundSerialized = string.Empty;

			// Background
			//if (current.Data.Background != _lastSnapshot.Data.Background)
			//{
			//	Changes.Undo.Background = _lastSnapshot.Data.Background;
			//	Changes.Redo.Background = current.Data.Background;
			//}

			// Scaling
			if (current.Data.Scaling.LatticeSize != _lastSnapshot.Data.Scaling.LatticeSize)
			{
				Changes.Undo.Scaling.LatticeSize = _lastSnapshot.Data.Scaling.LatticeSize;
				Changes.Redo.Scaling.LatticeSize = current.Data.Scaling.LatticeSize;
			}

			if (current.Data.Scaling.CellSize != _lastSnapshot.Data.Scaling.CellSize)
			{
				Changes.Undo.Scaling.CellSize = _lastSnapshot.Data.Scaling.CellSize;
				Changes.Redo.Scaling.CellSize = current.Data.Scaling.CellSize;
			}

			if (current.Data.Scaling.ShowGridLines != _lastSnapshot.Data.Scaling.ShowGridLines)
			{
				Changes.Undo.Scaling.ShowGridLines = _lastSnapshot.Data.Scaling.ShowGridLines;
				Changes.Redo.Scaling.ShowGridLines = current.Data.Scaling.ShowGridLines;
			}

			if (current.Data.Scaling.Zoom != _lastSnapshot.Data.Scaling.Zoom)
			{
				Changes.Undo.Scaling.Zoom = _lastSnapshot.Data.Scaling.Zoom;
				Changes.Redo.Scaling.Zoom = current.Data.Scaling.Zoom;
			}
			
			return Changes;
		}

		/// <summary>
		/// Creates the initial snapshot of the data
		/// </summary>
		public void GetInitialSnapshot()
		{
			_lastSnapshot = new Snapshot(_Document);
		}

		/// <summary>
		/// Rewinds the last Undo performed, reapplying the changes
		/// </summary>
		public void Redo()
		{
			if (_redoStack.Count == 0)
			{
				return;
			}

			UndoRedo Changes = _redoStack.Pop();
			_undoStack.Push(Changes);

			ApplyChangeset(Changes, Activity.Redo);

			// Get a new Snapshot of the data
			_lastSnapshot = new Snapshot(_Document);

			// Fire the events
			OnRedoChanged();
			OnUndoChanged();
			OnCompleted();
		}

		/// <summary>
		/// The program has just performed an operation that can be undone. Grab a snapshot of the data
		/// and save the differences between this and the last as a Changeset
		/// </summary>
		/// <param name="action">Text of the operation complete, this will appear in the Undo menu in the Editor</param>
		public void SaveUndo(string action)
		{
			if (_applyingChangeSet)
			{
				return;
			}

			Snapshot Current = new Snapshot(_Document);

			// Find the changes between the current Snapshot and the last snapshot taken.
			UndoRedo Changes = FindSnapshotChanges(action, Current);

			if (!Changes.IsEmpty)
			{
				_undoStack.Push(Changes);
				_redoStack.Clear();
			}

			_lastSnapshot = Current;
			Current = null;

			// Fire the events
			OnRedoChanged();
			OnUndoChanged();
		}

		/// <summary>
		/// Looks at the last set of changes and applies the old values.
		/// </summary>
		public void Undo()
		{
			if (_undoStack.Count == 0)
			{
				return;
			}

			UndoRedo Changes = _undoStack.Pop();
			_redoStack.Push(Changes);

			ApplyChangeset(Changes, Activity.Undo);

			// Get a new Snapshot of the data
			_lastSnapshot = new Snapshot(_Document);

			// Fire the events
			OnRedoChanged();
			OnUndoChanged();
			OnCompleted();
		}

		#region [ Debug Methods ]

		/// <summary>
		/// Returns a text representative of the data in the Redo stack.
		/// </summary>
		public string Debug_RedoStack()
		{
			StringBuilder Output = new StringBuilder();
			Stack<UndoRedo> TempStack = new Stack<UndoRedo>();
			UndoRedo Data = null;
			// Pop out each data item, get the contents, and then push it onto the temporary stack.
			while (_redoStack.Count > 0)
			{
				Data = _redoStack.Pop();
				if (Output.Length > 0)
					Output.AppendLine("-----------------------------------------------------------------");
				Output.AppendLine(Data.ToDebugString(false));
				TempStack.Push(Data);
			}
			// Push the data back onto the original stack.
			while (TempStack.Count > 0)
				_redoStack.Push(TempStack.Pop());

			TempStack = null;
			Data = null;
			return Output.ToString();
		}

		/// <summary>
		/// Returns a text representative of the data in the Undo stack.
		/// </summary>
		public string Debug_UndoStack()
		{
			StringBuilder Output = new StringBuilder();
			Stack<UndoRedo> TempStack = new Stack<UndoRedo>();
			UndoRedo Data = null;
			// Pop out each data item, get the contents, and then push it onto the temporary stack.
			while (_undoStack.Count > 0)
			{
				Data = _undoStack.Pop();
				if (Output.Length > 0)
					Output.AppendLine("-----------------------------------------------------------------");
				Output.AppendLine(Data.ToDebugString(true));
				TempStack.Push(Data);
			}
			// Push the data back onto the original stack.
			while (TempStack.Count > 0)
				_undoStack.Push(TempStack.Pop());

			TempStack = null;
			Data = null;
			return Output.ToString();
		}

		/// <summary>
		/// Returns a text version of the current Snapshot
		/// </summary>
		/// <returns></returns>
		public string Debug_UndoSnapshot()
		{
			return _lastSnapshot.Data.ToDebugString();
		}

		#endregion [ Debug Methods ]

		#endregion [ Methods ]

		#region [ Events ]

		#region [ Event Triggers ]

		/// <summary>
		/// Occurs when the Redo stack changes
		/// </summary>
		private void OnRedoChanged()
		{
			if (RedoChanged == null)
				return;
			bool HasData = (_redoStack.Count > 0);
			string TopMost = HasData ? _redoStack.Peek().ToString() : string.Empty;
			RedoChanged(this, new UndoEventArgs(TopMost, HasData));
		}

		/// <summary>
		/// Occurs when the Undo stack changes
		/// </summary>
		private void OnUndoChanged()
		{
			if (UndoChanged == null)
				return;
			bool HasData = (_undoStack.Count > 0);
			string TopMost = HasData ? _undoStack.Peek().ToString() : string.Empty;
			UndoChanged(this, new UndoEventArgs(TopMost, HasData));
		}

		/// <summary>
		/// Occurs when an Undo or Redo event is triggered.
		/// </summary>
		private void OnCompleted()
		{
			if (Completed != null)
				Completed(this, new System.EventArgs());
		}

		#endregion [ Event Triggers ]

		#region [ Event Handlers ]

		/// <summary>
		/// Occurs when either an Undo or a Redo is triggered.
		/// </summary>
		public EventHandler Completed;

		/// <summary>
		/// Occurs when a new item is the top item on the Redo stack
		/// </summary>
		public EventHandlers.UndoEventHandler RedoChanged;

		/// <summary>
		/// Occurs when a new item is the top item on the Undo stack
		/// </summary>
		public EventHandlers.UndoEventHandler UndoChanged;

		#endregion [ Event Handlers ]

		#endregion [ Events ]
	}
}
