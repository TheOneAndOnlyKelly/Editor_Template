using System.ComponentModel;
using Editor_Template.Interfaces;
using Editor_Template.Singletons;
using Editor_Template.Utilities;
using KellyControls.PlugInToolBtn.Interfaces;

namespace Editor_Template.Tools
{
	public class BaseTool : KellyControls.PlugInToolBtn.Tools.BaseTool, IPlugInTool
	{
		#region [ Private Variables ]

		protected GlobalController _global = GlobalController.Instance;
		protected Settings _settings = Settings.Instance;
		protected IDocument Document;

		#endregion [ Private Variables ]

		#region [ Properties ]

		#endregion [ Properties ]

		#region [ Constructors ]

		public BaseTool()
		{
			this.Document = _global.Document;
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Concatenates the path with a name, delimited by the pre-defined path delimiter character.
		/// </summary>
		/// <param name="path">Pre-built path</param>
		/// <param name="newNode">Name to be appended</param>
		protected override string AppendPath(string path, string newNode)
		{
			return _settings.AppendPath(path, newNode);
		}

		/// <summary>
		/// Attaches or detaches events to objects, such as Click events to buttons.
		/// </summary>
		/// <param name="attach">Indicates that the events should be attached. If false, then detaches the events</param>
		protected override void AttachEvents(bool attach)
		{
			// if we are attaching and already attached, or if we are detaching and already detached, then leave.
			if (attach & _eventsAttached)
				return;

			if (attach)
			{
				if (_eventsAttached)
					return;

				DocumentController.Instance.Switched += new EventHandlers.DocumentEventHandler(this.Documents_Switched);

				if (Document != null)
					Document.PropertyChanged += new PropertyChangedEventHandler(this.Document_PropertyChanged);
			}
			else
			{
				if (!_eventsAttached)
					return;

				DocumentController.Instance.Switched -= this.Documents_Switched;

				if (Document != null)
					Document.PropertyChanged -= this.Document_PropertyChanged;
			}
			_eventsAttached = attach;
		}

		/// <summary>
		/// Pass through to the CaptureCanvas method on the Workshop object
		/// </summary>
		protected override void CaptureCanvas()
		{
			_capturedCanvas = _global.CaptureCanvas();
		}

		/// <summary>
		/// Occurs when this Tool has been selected from the main Toolbar
		/// </summary>
		public override void OnSelected()
		{
			// Set the cursor for the Canvas. This is important because there could have been UI changes since this Select method
			// was called initially on Tool_Click, and some tool's cursors are sensitive to UI settings (ie Paint, Erase, etc)
			if (Document != null)
				Document.Cursor = this.Cursor;

			base.OnSelected();
		}

		/// <summary>
		/// Release the capture of the mouse cursor
		/// </summary>
		protected override void ReleaseMouse()
		{
			Document.ReleaseMouse();
		}

		/// <summary>
		/// Trap the mouse to only live inside of the canvas, so we don't get weird effects, like drawings starting outside, or ending outside the pictureBox.
		/// Call ReleaseMouse() on the MouseUp event to allow the cursor to act normal.
		/// </summary>
		protected override void TrapMouse()
		{
			Document.TrapMouse();
		}

		#region [ LoadValue ]

		/// <summary>
		/// Returns the string value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		/// <returns>Setting value as a string</returns>
		protected override string LoadValue(string pathName, string defaultValue, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			return _settings.GetValue(Path, defaultValue);
		}

		/// <summary>
		/// Returns the string value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <returns>Setting value as a string</returns>
		protected override string LoadValue(string pathName, string defaultValue)
		{
			return _settings.GetValue(AppendPath(_savePath, pathName), defaultValue);
		}

		/// <summary>
		/// Returns the integer value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		/// <returns>Setting value as an integer</returns>
		protected override int LoadValue(string pathName, int defaultValue, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			return _settings.GetValue(Path, defaultValue);
		}

		/// <summary>
		/// Returns the integer value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <returns>Setting value as an integer</returns>
		protected override int LoadValue(string pathName, int defaultValue)
		{
			return _settings.GetValue(AppendPath(_savePath, pathName), defaultValue);
		}

		/// <summary>
		/// Returns the floating point value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		/// <returns>Setting value as a float</returns>
		protected override float LoadValue(string pathName, float defaultValue, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			return _settings.GetValue(Path, defaultValue);
		}

		/// <summary>
		/// Returns the floating point value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <returns>Setting value as a float</returns>
		protected override float LoadValue(string pathName, float defaultValue)
		{
			return _settings.GetValue(AppendPath(_savePath, pathName), defaultValue);
		}

		/// <summary>
		/// Returns the boolean value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		/// <returns>Setting value as a boolean</returns>
		protected override bool LoadValue(string pathName, bool defaultValue, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			return _settings.GetValue(Path, defaultValue);
		}

		/// <summary>
		/// Returns the boolean value stored in Settings
		/// </summary>
		/// <param name="pathName">Path to find the setting value</param>
		/// <param name="defaultValue">Value to return if the setting value was not present</param>
		/// <returns>Setting value as a boolean</returns>
		protected override bool LoadValue(string pathName, bool defaultValue)
		{
			return _settings.GetValue(AppendPath(_savePath, pathName), defaultValue);
		}

		#endregion [ LoadValue ]

		#region [ SaveValue ]

		/// <summary>
		/// Saves the value to the settings object as a string.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		protected override void SaveValue(string pathName, string value, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			_settings.SetValue(Path, value);
		}

		/// <summary>
		/// Saves the value to the settings object as a string.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		protected override void SaveValue(string pathName, string value)
		{
			SaveValue(pathName, value, true);
		}

		/// <summary>
		/// Saves the value to the settings object as an integer.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		protected override void SaveValue(string pathName, int value, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			_settings.SetValue(Path, value);
		}

		/// <summary>
		/// Saves the value to the settings object as an integer.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		protected override void SaveValue(string pathName, int value)
		{
			SaveValue(pathName, value, true);
		}

		/// <summary>
		/// Saves the value to the settings object as a float.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		protected override void SaveValue(string pathName, float value, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			_settings.SetValue(Path, value);
		}

		/// <summary>
		/// Saves the value to the settings object as an float.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		protected override void SaveValue(string pathName, float value)
		{
			SaveValue(pathName, value, true);
		}

		/// <summary>
		/// Saves the value to the settings object as a boolean.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		/// <param name="appendPath">Indicates that we should append the path passed in with the _savePath property of this Tool</param>
		protected override void SaveValue(string pathName, bool value, bool appendPath)
		{
			string Path = pathName;
			if (appendPath)
				Path = AppendPath(_savePath, pathName);
			_settings.SetValue(Path, value);
		}

		/// <summary>
		/// Saves the value to the settings object as a boolean.
		/// </summary>
		/// <param name="pathName">Path to the place to store the setting value</param>
		/// <param name="value">Value to store</param>
		protected override void SaveValue(string pathName, bool value)
		{
			SaveValue(pathName, value, true);
		}

		#endregion [ SaveValue ]

		#endregion [ Methods ]

		#region [ Event Delegates ]

		/// <summary>
		/// Occurs when the first Document is loaded, a Document closes, or one Document becomes Active, replacing another.
		/// </summary>
		protected virtual void Documents_Switched(object sender, DocumentEventArgs e)
		{
			if (e.OldDocument != null)
			{
				e.OldDocument.PropertyChanged -= Document_PropertyChanged;
			}
			if (e.Document != null)
			{
				e.Document.PropertyChanged += new PropertyChangedEventHandler(this.Document_PropertyChanged);
			}
		}

		protected virtual void Document_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{ }

		#endregion [ Event Delegates ]

	}
}
