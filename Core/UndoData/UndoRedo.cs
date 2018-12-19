using System.Text;

namespace Editor_Template.Core.UndoData
{
	/// <summary>
	/// Stores the differences between one snapshot and another.
	/// </summary>
	public class UndoRedo : Base
	{
		#region [ Properties ]

		/// <summary>
		/// Text of the action to undo, to be displayed in the Undo menu under Edit
		/// </summary>
		public string Action { get; set; }

		/// <summary>
		/// Indicates whether the ChangeSets are empty.
		/// </summary>
		public bool IsEmpty
		{
			get { return Undo.IsEmpty && Redo.IsEmpty; }
		}

		/// <summary>
		/// Undo data
		/// </summary>
		public ChangeSet Undo { get; set; }

		/// <summary>
		/// Redo data
		/// </summary>
		public ChangeSet Redo { get; set; }

		#endregion [ Properties ]

		#region [ Constructor ]

		public UndoRedo()
			: base()
		{
			Action = string.Empty;
			Undo = new ChangeSet();
			Redo = new ChangeSet();
		}

		public UndoRedo(string action)
			: this()
		{
			Action = action;
		}

		#endregion [ Constructor ]

		#region [ Methods ]
		
		/// <summary>
		/// User interface
		/// </summary>
		public Scaling Scaling(bool undoData)
		{
			return undoData ? Undo.Scaling : Redo.Scaling;
		}
		
		public override string ToString()
		{
			return Action ?? string.Empty;
		}

		/// <summary>
		/// Returns a string representation of the data contained in this object.
		/// </summary>
		/// <param name="showUndoData">Indicates if the Undo part of the dataset should be output.</param>
		public string ToDebugString(bool showUndoData)
		{
			StringBuilder Output = new StringBuilder();

			Output.AppendLine("ACTION: " + this.Action);
			if (showUndoData)
				Output.Append(Undo.ToDebugString());
			else
				Output.Append(Redo.ToDebugString());
			return Output.ToString();
		}

		#endregion [ Methods ]
	}
}
