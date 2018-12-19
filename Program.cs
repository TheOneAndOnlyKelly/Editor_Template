using System;
using System.Windows.Forms;
using Editor_Template.Singletons;

namespace Editor_Template
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Settings Settings = Settings.Instance;
			Settings.Style = Settings.SettingsStyle.Xml;

			GlobalController Global = GlobalController.Instance;
			Global.Initialize();

			ImageController.Instance.BuildJoinedImagesList();

			Application.Run(new Forms.Editor(args));

			Settings.Save();
			Global?.Dispose();
		}
	}
}
