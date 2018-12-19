using System;
using Editor_Template.Singletons;
using Editor_Template.Interfaces;

namespace Editor_Template.Singletons
{
	public class ClipboardController
	{
		#region [ Constants ]

		public const string UNDO_CUT = "Cut";
		public const string UNDO_COPY = "Copy";
		public const string UNDO_PASTE = "Paste";
		public const string UNDO_DELETE = "Delete";

		#endregion [ Constants ]

		#region [ Private Static Variables ]

		private static readonly Lazy<ClipboardController> lazy = new Lazy<ClipboardController>(() => new ClipboardController());

		#endregion [ Private Static Variables ]

		#region [ Properties ]

		/// <summary>
		/// Returns true if there is any data we might be holding
		/// </summary>
		public bool HasData
		{
			get
			{
				// ... Do Stuff Here
				return false;
			}
		}

		public static ClipboardController Instance { get { return lazy.Value; } }

		/// <summary>
		/// Indicates whether events should be suppressed from firing. Use sparingly.
		/// </summary>
		public bool SuppressEvents { get; set; }

		#endregion [ Properties ]

		#region [ Constructors ]

		public ClipboardController()
		{ }

		static ClipboardController()
		{ }

		#endregion [ Constructors ]

		#region [ Methods ]

		public bool Copy()
		{
			// ... Do Stuff Here

			GlobalController.Instance.Document.SaveUndo(UNDO_COPY);
			return true;
		}

		public bool Cut()
		{
			if (!Copy())
				return false;
			Delete(false);
			GlobalController.Instance.Document.SaveUndo(UNDO_CUT);
			return true;
		}

		/// <summary>
		/// Do a CUT without actually moving anything into the clipboard, without copying into the clipboard
		/// </summary>
		public void Delete(bool saveUndo = true)
		{
			// ... Do Stuff Here

			if (saveUndo)
				GlobalController.Instance.Document.SaveUndo(UNDO_DELETE);
		}

		internal void DisplayDiagnosticData()
		{
#if DEBUG
			
#endif
		}

		public void Paste()
		{
			if (!this.HasData)
				return;

			// ... Do Stuff Here

			GlobalController.Instance.Document.SaveUndo(UNDO_PASTE);
		}

		#endregion [ Methods ]

		#region [ Events ]

		#region [ Event Triggers ]

		private void OnChanged()
		{
			Changed?.Invoke(this, new EventArgs());
		}

		#endregion [ Event Triggers ]

		#region [ Event Handlers ]

		/// <summary>
		/// Occurs when data is put into the clipboard buffer
		/// </summary>
		public EventHandler Changed;

		#endregion [ Event Handlers ]

		#endregion [ Events ]
	}
}
