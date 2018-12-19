using System;
using System.Drawing;
using Editor_Template.Interfaces;

namespace Editor_Template.Utilities
{

	public class DirtyEventArgs : EventArgs
	{
		#region [ Properties ]

		public bool IsDirty { get; private set; }

		#endregion [ Properties ]

		#region [ Constructors ]

		public DirtyEventArgs(bool isDirty)
		{
			this.IsDirty = isDirty;
		}

		#endregion [ Constructors ]
	}

	public class KeyChordEventArgs : EventArgs
	{
		#region [ Properties ]

		public string MethodName { get; private set; }

		#endregion [ Properties ]

		#region [ Constructors ]

		public KeyChordEventArgs(string methodName)
		{
			this.MethodName = methodName;
		}

		#endregion [ Constructors ]}
	}
	
	public class UndoEventArgs : EventArgs
	{
		#region [ Properties ]

		public string Text { get; set; }
		public bool HasData { get; set; }

		#endregion [ Properties ]

		#region [ Constructors ]

		public UndoEventArgs()
		{
			this.Text = string.Empty;
			this.HasData = false;
		}

		public UndoEventArgs(string text, bool hasData)
		{
			this.Text = text;
			this.HasData = hasData;
		}

		#endregion [ Constructors ]
	}

	public class ZoomEventArgs : EventArgs
	{
		#region [ Properties ]

		public Point ZoomPoint = new Point();
		public float ZoomLevel = 1.0f;

		#endregion [ Properties ]

		#region [ Constructors ]

		public ZoomEventArgs()
		{ }

		public ZoomEventArgs(Point zoomPoint, float zoomLevel)
		{
			this.ZoomPoint = zoomPoint;
			this.ZoomLevel = zoomLevel;
		}

		#endregion [ Constructors ]
	}

	public class DocumentEventArgs : EventArgs
	{
		#region [ Properties ]

		public IDocument Document { get; private set; }
		public IDocument OldDocument { get; private set; }

		#endregion [ Properties ]

		#region [ Constructors ]

		public DocumentEventArgs(IDocument Document)
		{
			this.Document = Document;
			this.OldDocument = null;
		}

		public DocumentEventArgs(IDocument newDocument, IDocument oldDocument)
		{
			this.Document = newDocument;
			this.OldDocument = oldDocument;
		}

		#endregion [ Constructors ]
	}
}