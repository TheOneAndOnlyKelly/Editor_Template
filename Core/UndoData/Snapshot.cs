using Editor_Template.Interfaces;

namespace Editor_Template.Core.UndoData
{
	/// <summary>
	/// Current status of the current Document and settings, saved in a ChangeSet object.
	/// </summary>
	public class Snapshot : Base
	{
		#region [ Public Fields ]

		public ChangeSet Data { get; private set; }

		#endregion [ Public Fields ]

		#region [ Constructor ]

		/// <summary>
		/// Initializes the object by taking a snapshot of the data.
		/// </summary>
		/// <param name="Document">Document data to capture</param>
		public Snapshot(IDocument Document)
			: base()
		{
			Data = new ChangeSet();

			string List = string.Empty;
			string Serialized = string.Empty;

			// Record all the current UI settings into the ChangeSet

			Data.Scaling = new Scaling
			{
				LatticeSize = Document.Scaling.LatticeSize,
				CellSize = Document.Scaling.CellSize,
				ShowGridLines = Document.Scaling.ShowGridLines,
				Zoom = Document.Scaling.Zoom
			};
		}

		#endregion [ Constructor ]

	}
}
