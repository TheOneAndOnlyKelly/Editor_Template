using System.Windows.Forms;

// Found at: http://www.codeproject.com/KB/cs/mrutoolstripmenu.aspx
namespace Editor_Template.Utilities
{
	/// <summary>
	/// Represents a most recently used (MRU) menu.
	/// </summary>
	/// <remarks>This class shows the MRU list in a popup menu. To display
	/// the MRU list "inline" use <see labelName="MruMenuInline" />.
	/// <para>The class will optionally load the last set of files from the registry
	/// on construction and store them when instructed by the main program.</para>
	/// <para>Internally, this class uses zero-based numbering for the items.
	/// The displayed numbers, however, will start with one.</para></remarks>
	public class MruStripMenu : KellyControls.CommonClasses.MRU.MruStripMenu
	{
		#region [ Constructors ]

		/// <summary>
		/// Initializes a new instance of the MruMenu class.
		/// </summary>
		/// <param labelName="recentFileMenuItem">The temporary menu item which will be replaced with the MRU list.</param>
		/// <param labelName="clickedHandler">The delegate to handle the item selection (click) event.</param>
		public MruStripMenu(ToolStripMenuItem recentFileMenuItem, ClickedHandler clickedHandler)
			: base(recentFileMenuItem, clickedHandler)
		{ }

		#endregion [ Constructors ]

		#region [ Settings Methods ]

		/// <summary>
		/// Saves the MRU data to the Settings xml file.
		/// </summary>
		public void SaveToSettings(Singletons.Settings settings)
		{
			string SavePath = "MRU";

			settings.SetValue(settings.AppendPath(SavePath, "Max"), _maxEntries);

			int number = 1;
			int i = StartIndex;
			for (; i < EndIndex; i++, number++)
			{
				settings.SetValue(settings.AppendPath(SavePath, "File" + number), ((MruMenuItem)MenuItems[i]).Filename);
			}

			for (; number <= 16; number++)
			{
				settings.RemoveValue(settings.AppendPath(SavePath, "File" + number));
			}
		}

		/// <summary>
		/// Loads the entries from the Settings object
		/// </summary>
		public void LoadFromSettings(Singletons.Settings settings)
		{
			RemoveAll();
			string SavePath = "MRU";
			string filename = string.Empty;

			MaxEntries = settings.GetValue(settings.AppendPath(SavePath, "Max"), 4);

			for (int number = _maxEntries; number > 0; number--)
			{
				filename = settings.GetValue(settings.AppendPath(SavePath, "File" + number), string.Empty);
				if (filename.Length > 0)
					AddFile(filename);
			}
		}

		#endregion [ Settings Methods ]
	}
	
}