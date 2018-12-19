using System;
using System.Collections.Generic;
using Editor_Template.Core;
using Editor_Template.Interfaces;
using Editor_Template.Utilities;

namespace Editor_Template.Singletons
{
	/// <summary>
	/// Controls the list of Documents and determines which is the active one. When one Document becomes active, its properties override those of the UISettings
	/// object and the User Intefaces updates with these settings.
	/// </summary>
	public class DocumentController : Base
	{
		#region [ Private Variables ]

		/// <summary>
		/// The Document that is currently active.
		/// </summary>
		private IDocument _active = null;

		/// <summary>
		/// List of all the currently load Documents
		/// </summary>
		private List<IDocument> _documents = null;
		private static readonly Lazy<DocumentController> lazy = new Lazy<DocumentController>(() => new DocumentController());

		#endregion [ Private Variables ]

		#region [ Properties ]

		public static DocumentController Instance { get { return lazy.Value; } }

		/// <summary>
		/// The active Document
		/// </summary>
		public IDocument Active
		{
			get
			{
				if ((_active == null) && (_documents.Count > 0))
					this.Active = _documents[0];
				return _active;
			}
			set
			{
				IDocument LastActive = null;
				if ((_active != value) && (_active != null))
				{
					LastActive = _active;
				}
				if (!Object.ReferenceEquals(_active, value))
				{
					_active = value;
					OnSwitched(_active, LastActive);
				}
			}
		}

		/// <summary>
		/// Returns the count of all the Documents
		/// </summary>
		public int Count
		{
			get { return _documents.Count; }
		}

		/// <summary>
		/// Indicates whether any of the Documents have been modified
		/// </summary>
		public override bool Dirty
		{
			get
			{
				foreach (IDocument Document in _documents)
				{
					if (Document.Dirty)
					{
						base.Dirty = true;
						break;
					}
				}
				return base.Dirty;
			}
			set
			{
				if (base.Dirty != value)
				{
					foreach (IDocument Document in _documents)
					{
						Document.SuppressEvents = true;
						Document.Dirty = value;
						Document.SuppressEvents = false;
					}
					if (base.Dirty != value)
					{
						base.Dirty = value;
					}
				}
			}
		}

		/// <summary>
		/// Gets the list of all Documents
		/// </summary>
		public List<IDocument> List
		{
			get { return _documents; }
		}

		/// <summary>
		/// Indicates whether events should be suppressed from firing. Use sparingly.
		/// </summary>
		public override bool SuppressEvents
		{
			get { return base.SuppressEvents; }
			set
			{
				base.SuppressEvents = value;
				foreach (IDocument Document in _documents)
					Document.SuppressEvents = value;
			}
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		/// <summary>        
		/// The default Constructor.        
		/// </summary>        
		public DocumentController()
			: base()
		{
			_documents = new List<IDocument>();
		}

		public DocumentController(string[] args) : this()
		{
			Load(args);
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Adds a new Document to the list
		/// </summary>
		/// <param name="Document">Document to add</param>
		public void Add(IDocument Document)
		{
			_documents.Add(Document);
			Document.DirtyChanged += new EventHandlers.DirtyEventHandler(this.ChildObject_Dirty);
			OnAdded(Document);

			if (_active == null)
				Active = _documents[0];
		}

		/// <summary>
		/// Remove all the Documents from this controller.
		/// </summary>
		public void Clear()
		{
			if (_documents.Count > 0)
			{
				IDocument p = null;
				for (int i = 0; i < _documents.Count; i++)
				{
					p = _documents[0];
					Remove(p);
				}
				_active = null;
			}
		}

		///// <summary>
		///// Converts the existing Document to the new type.
		///// </summary>
		///// <param name="Document">IDocument object to convert</param>
		///// <param name="targetDocumentType">DocumentType of the new object</param>
		//public void ConvertDocument(IDocument Document, DocumentType targetDocumentType)
		//{
		//	//this.SuppressEvents = true;

		//	//if (Document.DocumentTypeID == targetDocumentType)
		//	//	return;

		//	//IDocument ConvertedDocument = CreateNewDocument(targetDocumentType);
		//	//ConvertedDocument.InitializeUndo();
		//	//ConvertedDocument.CopyFrom(Document);
		//	//float Zoom = Document.Scaling.Zoom.GetValueOrDefault();
		//	//Add(ConvertedDocument);
		//	//Document.SetClean();
		//	//Remove(Document);
		//	//Active = ConvertedDocument;

		//	//this.SuppressEvents = false;
		//}

		/////// <summary>
		/////// Creates a new Document based on the type passed in.
		/////// </summary>
		/////// <param name="DocumentType">DocumentType enumeration, indicating the type of Document to create.</param>
		//public IDocument CreateNewDocument(DocumentType DocumentType)
		//{
		//	//	switch (DocumentType)
		//	//	{
		//	//		case DocumentType.Vixen21x:
		//	//			return new Documents.Vixen.Vixen21x();
		//	//		case DocumentType.Vixen25x:
		//	//			return new Documents.Vixen.Vixen25x();
		//	//		case DocumentType.VixenPlus:
		//	//			return new Documents.Vixen.VixenPlus();
		//	//		case DocumentType.NotSet:
		//	//		case DocumentType.Vixen3:
		//	//		default:
		//	return null;
		//	//	}
		//}

		///// <summary>
		///// Determine the type of Document that is being loaded by the structure of the file.
		///// </summary>
		///// <param name="filename">Name of the file to load.</param>
		///// <returns>Returns NotSet if it's not possible to determine the type, otherwise, returns the determined type.</returns>
		//public DocumentType DetectDocumentType(string filename)
		//{
		//	//if ((filename ?? string.Empty).Length == 0)
		//	//	throw new ArgumentException("Missing filename");

		//	//FileInfo FI = new FileInfo(filename);
		//	//if (!FI.Exists)
		//	//	throw new FileNotFoundException("File not found.", filename);

		//	//return Documents.Vixen.BaseVixen.DetectedDocumentType(filename);
		//	return DocumentType.NotSet;
		//}

		/// <summary>
		/// Clean up all child objects here, unlink all events and dispose
		/// </summary>
		protected override void DisposeChildObjects()
		{
			base.DisposeChildObjects();
			for (int i = _documents.Count - 1; i >= 0; i--)
			{
				if (_documents[i] != null)
				{
					((Base)_documents[i]).DirtyChanged -= this.ChildObject_Dirty;
					_documents[i].Dispose();
					_documents[i] = null;
				}
			}
		}

		/// <summary>
		/// Fires off a Switched event. This is used when we are loading the Document into the Editor and we
		/// want to let all the sundry object think a Document has been loaded.
		/// </summary>
		public void FireSwitched()
		{
			OnSwitched(this.Active, null);
		}

		/// <summary>
		/// Loads the Document from the filename
		/// </summary>
		/// <param name="filename">Name of the file to open.</param>
		/// <returns>Returns true if the file is successfully loaded.</returns>
		public IDocument Load(string filename)
		{
			IDocument Document = null;

			//...

			return Document;
		}
		

		/// <summary>
		/// Loads 1 or more Documents from a string array
		/// </summary>
		/// <param name="args">Array containing file names of Documents</param>
		public void Load(string[] args)
		{
			//...
		}

		/// <summary>
		/// Removes the first occurrence of the Document from the List.
		/// </summary>
		/// <param name="Document">Document to remove</param>
		/// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the List.
		/// </returns>
		public bool Remove(IDocument Document)
		{
			if (Document == null)
				return false;

			int Index = _documents.IndexOf(Document);

			if (_documents.Remove(Document))
			{
				OnRemoved(Document);
				Document.Close();
				Document.DirtyChanged -= this.ChildObject_Dirty;

				if (Object.ReferenceEquals(Document, _active))
				{
					// If we are removing the Active Document, see if there is another further down the list. If not, see if there is another further up the list.
					if (Index < _documents.Count)
						Active = _documents[Index];
					else if (Index > 0)
						Active = _documents[Index - 1];
					else
						Active = null;
				}

				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Removes the Document at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		public void RemoveAt(int index)
		{
			if ((index < 0) || (index >= _documents.Count))
			{
				return;
			}
			IDocument Document = _documents[index];
			Document.Close();
			Document.DirtyChanged -= this.ChildObject_Dirty;
			_documents.RemoveAt(index);
			OnRemoved(Document);

			if (Object.ReferenceEquals(Document, _active))
			{
				// If we are removing the Active Document, see if there is another further down the list. If not, see if there is another further up the list.
				if (index < _documents.Count)
					Active = _documents[index];
				else if (index > 0)
					Active = _documents[index - 1];
				else
					Active = null;
			}
		}

		#endregion [ Methods ]

		#region [ Event Handlers ]

		/// <summary>
		/// Occurs when a Document has been Added
		/// </summary>
		public EventHandlers.DocumentEventHandler Added;

		/// <summary>
		/// Occurs when a Document has been Remove
		/// </summary>
		public EventHandlers.DocumentEventHandler Removed;

		/// <summary>
		/// Occurs when the Active Document changes.
		/// </summary>
		public EventHandlers.DocumentEventHandler Switched;

		#endregion [ Event Handlers ]

		#region [ Event Triggers ]

		/// <summary>
		/// Handles the throwing of the Switched event
		/// </summary>
		/// <param name="activeDocument">The currently Active Document</param>
		/// <param name="oldDocument">The previous active Document, can be NULL</param>
		private void OnSwitched(IDocument activeDocument, IDocument oldDocument)
		{
			Switched?.Invoke(this, new DocumentEventArgs(activeDocument, oldDocument));
		}

		/// <summary>
		/// Handles the throwing of the Added event
		/// </summary>
		private void OnAdded(IDocument Document)
		{
			Added?.Invoke(this, new DocumentEventArgs(Document));
		}

		/// <summary>
		/// Handles the throwing of the Removed event
		/// </summary>
		private void OnRemoved(IDocument Document)
		{
			Removed?.Invoke(this, new DocumentEventArgs(Document));
		}

		#endregion [ Event Triggers ]

		#region [ Events ]

		/// <summary>
		/// Occurs when the Dirty flag on a given object is changed
		/// </summary>
		private void ChildObject_Dirty(object sender, DirtyEventArgs e)
		{
			if (e.IsDirty)
				this.Dirty = true;
		}

		#endregion [ Events ]
	}
}
