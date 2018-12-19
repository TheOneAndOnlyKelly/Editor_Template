using System.Text;

namespace Editor_Template.Core.UndoData
{
	/// <summary>
	/// Stores the differences between one snapshot and another.
	/// </summary>
	public class ChangeSet : Base
	{
		#region [ Constants ]

		private const string EMPTY = "EMPTY";
		private const string TAB = "\t";

		#endregion [ Constants ]

		#region [ Properties ]

		/// <summary>
		/// Serialized UI data.
		/// </summary>
		public Scaling Scaling { get; set; }

		/// <summary>
		/// Gets a value indicating whether this ChangeSet is empty. If any of the objects or data that was 
		/// initially set to null is not null, then returns false. 
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				if (!Scaling.IsEmpty)
					return false;

				return true;
			}
		}

		#endregion [ Properties ]

		#region [ Constructor ]

		public ChangeSet()
			: base()
		{
			this.Scaling = new Scaling();
		}

		#endregion [ Constructor ]

		#region [ Methods ]

		/// <summary>
		/// Clean up all child objects here, unlink all events and dispose
		/// </summary>
		protected override void DisposeChildObjects()
		{
			base.DisposeChildObjects();
			Scaling?.Dispose();
		}

		/// <summary>
		/// Returns a string representation of the data contained in this object.
		/// </summary>
		public override string ToDebugString()
		{
			if (IsEmpty)
				return EMPTY;

			var Output = new StringBuilder();

			Output.AppendLine("SCALING:");
			if (this.Scaling.IsEmpty)
				Output.AppendLine(TAB + EMPTY);
			else
				Output.AppendLine(this.Scaling.ToDebugString(1));

			return Output.ToString();
		}

		/// <summary>
		/// Clean up the Xml for displaying for the Diagnostic panes.
		/// </summary>
		private string FormatForDebug(string value)
		{
			if (value == null)
				return TAB + EMPTY;
			value = TAB + value.Replace(TAB, string.Empty).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("    ", " ");
			value = value.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
			value = value.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
			return value;
		}

		#endregion [ Methods ]
	}
}
