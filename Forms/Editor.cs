using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Editor_Template.Interfaces;
using Editor_Template.Singletons;
using Editor_Template.Utilities;
using EnumerationLib;
using KellyControls.PlugInToolBtn;
using KellyControls.PlugInToolBtn.Interfaces;
using WeifenLuo.WinFormsUI.Docking;
using XmlHelperLib;
using ERes = Editor_Template.Properties.Resources;
using ImageHandler = Editor_Template.Singletons.ImageController;

namespace Editor_Template.Forms
{
	public partial class Editor : Form
	{
		#region [ Constants ]

		private const string LAST_TOOL = "LastToolUsed";

		private const string EXPLORER_WIDTH = /*Constants.SAVE_PATH_DELIMITER +*/ "ExplorerWidth";

		#endregion [ Constants ]

		#region [ Private Variables ]

		/// <summary>
		/// GlobalController is a Singleton type of class. Simply getting the Instance variable from the Static object will get the object already loaded with our data
		/// </summary>
		private GlobalController _global = GlobalController.Instance;
		private Settings _settings = Settings.Instance;

		private ExplorerTree _explorerTree = null;
		private XmlHelper _xmlHelper = new XmlHelper();
		private ToolStrip _flyoutToolStrip = null;
		private MruStripMenu _mruMenu;

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Internal property to get the Active Document.
		/// </summary>
		[DebuggerHidden()]
		public IDocument Document
		{
			get { return DocumentController.Instance.Active; }
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		public Editor()
		{
			//DocumentSplash Splash = new DocumentSplash(true);
			//Splash.Show();

			InitializeComponent();

			ConfigureRunMode();

			BuildFileDialogFilters();

			_mruMenu = new MruStripMenu(File_Recent, new MruStripMenu.ClickedHandler(OnMruFile_Data));
			_mruMenu.LoadFromSettings(_settings);

			BuildMenuImages();
			AttachEvents();

			_explorerTree = new ExplorerTree
			{
				Editor = this
			};

			LoadPlugIns();

			// Load in the remembered last settings
			LoadSettings();
			_explorerTree.Show(DockPanel);

			if (Document != null)
			{
				UpdateControls_Document();
				Documents_Added(this, new DocumentEventArgs(Document));
				DocumentController.Instance.FireSwitched();
			}

			// Call the select event for the current tool to let it know there is a Document now.
			_global.CurrentTool.Tool.OnSelected();
		}

		public Editor(string[] args) : this()
		{
			if ((args != null) && (args.Length > 0))
			{
				DocumentController.Instance.Load(args);
			}
			else
				Documents_Switched(null, new DocumentEventArgs(null));
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		#region [ ToolBox Methods ]

		/// <summary>
		/// Configures the form and the DockPanel to either run as a Single Document Interface (PlugIn mode), or as a
		/// Multiple Document Interface (stand-alone mode)
		/// </summary>
		private void ConfigureRunMode()
		{
			this.AllowDrop = true;
			this.IsMdiContainer = true;
			DockPanel.DocumentStyle = DocumentStyle.DockingMdi;
			WindowMenu.Visible = true;
		}

		/// <summary>
		/// Combines a tool image with the flyout image.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		private Bitmap Flyout(Bitmap source)
		{
			if (source == null)
				return null;

			//return ImageHandler.Instance.AddAnnotation(source, Annotation.Flyout);
			Bitmap FlyoutImage = ERes.annotation_flyout;
			if (FlyoutImage != null)
			{
				Graphics g = Graphics.FromImage(source);
				g.DrawImage(FlyoutImage, new Rectangle(0, 0, 16, 16));
				g.Dispose();
			}
			return source;
		}

		/// <summary>
		/// Load up all the Tools derived from PlugIns
		/// </summary>
		private void LoadPlugIns()
		{
			_global.ToolGroups = PlugInController.PlugInToolGroupList;
			_global.Tools = PlugInController.PlugInToolList;

			if (_global.Tools == null)
				return;

			int MinID = Int32.MaxValue;
			int MaxID = Int32.MinValue;

			// Go through all the Tools in the list that are not in ToolGroups, create their buttons and assign the button's click delegate. If the tool has
			// a control toolstrip, add it to the hidden panel control.
			foreach (PlugInTool pTool in _global.Tools.WhereList(false))
			{
				pTool.Tool.OperationCompleted += new EventHandler(this.Tool_OperationCompleted);
				pTool.Button.Click += new EventHandler(this.Tool_Click);
				pTool.Tool.Selected += new EventHandler(this.Tool_Selected);
				pTool.Initialize();
				MinID = Math.Min(MinID, pTool.ID);
				MaxID = Math.Max(MaxID, pTool.ID);

				// If this tool has a ToolStrip that holds settings for it, add that control to the hidden panel on this form.
				if (pTool.SettingsToolStrip != null)
					pnlHoldToolStrips.Controls.Add(pTool.SettingsToolStrip);
			}

			// Loop through the ToolGroups, setting up their child tools, etc.
			foreach (PlugInToolGroup pToolGroup in _global.ToolGroups)
			{
				MinID = Math.Min(MinID, pToolGroup.ID);
				MaxID = Math.Max(MaxID, pToolGroup.ID);

				// Create a button for this ToolGroup. This button will initially have to image or tooltip, as there are no child tools yet.
				// Assign the button delegate.
				// Note, this button is created in the property_get method.
				pToolGroup.Button.Click += new EventHandler(this.Tool_Click);
				pToolGroup.Button.MouseDown += new MouseEventHandler(this.ToolGroup_MouseDown);
				pToolGroup.Button.MouseUp += new MouseEventHandler(this.ToolGroup_MouseUp);

				// Create the toolstrip control for this ToolGroup and add it to Editor's list of controls.
				this.Controls.Add(pToolGroup.ChildToolBox);

				// Sort all the Tools into their various ToolGroups, and initialize them like their non-grouped siblings
				foreach (PlugInTool pTool in _global.Tools.WhereList(pToolGroup.Name))
				{
					pToolGroup.ToolGroup.Add(pTool);
					pTool.Tool.OperationCompleted += new EventHandler(this.Tool_OperationCompleted);
					pTool.Tool.Selected += new EventHandler(this.Tool_Selected);
					pTool.Initialize();
					pTool.ParentButton = pToolGroup.Button;
					pTool.ToolBox = pToolGroup.ChildToolBox;
					pToolGroup.ChildToolBox.Items.Add(pTool.Button);
				}
				pToolGroup.Initialize();
			}

			// Now that all the Tools and ToolGroups are roughly initialized, we need to sort them by their ID order, and add the appropriate buttons
			// to the main ToolBox.
			// Sort them by looping from the minimum ID to the maximum ID. If the number value equals one or the other, add the button to the main ToolBox.
			// If the ID is the same between them, add the tool first, then the toolgroup.
			for (int i = MinID; i <= MaxID; i++)
			{
				//var Tool = _global.Tools.Where(t => t.InToolGroup == false && t.ID == i).FirstOrDefault();
				var Tool = _global.Tools.Where(false, i);
				if (Tool != null)
				{
					ToolBox_Main.Items.Add(Tool.Button);
					Tool.ToolBox = ToolBox_Main;
					Tool.Index = ToolBox_Main.Items.Count - 1;
				}

				//var ToolGroup = _global.ToolGroups.Where(t => t.ID == i).FirstOrDefault();
				var ToolGroup = _global.ToolGroups.Where(i);
				if (ToolGroup != null)
				{
					ToolBox_Main.Items.Add(ToolGroup.Button);
					ToolGroup.ToolBox = ToolBox_Main;
					ToolGroup.Index = ToolBox_Main.Items.Count - 1;
				}
			}

			// Now that all the proper tools and ToolGroups are installed on the main ToolBox, go through the ToolGroups and position their child ToolBoxes so they
			// line up to the buttons
			foreach (PlugInToolGroup pToolGroup in _global.ToolGroups)
			{
				pToolGroup.LineUpToolBox();
			}
		}

		#endregion [ ToolBox Methods ]

		/// <summary>
		/// Attach the menu events as well as some Workshop events.
		/// </summary>
		private void AttachEvents()
		{
			ClipboardController.Instance.Changed += new EventHandler(Clipboard_Changed);

			DocumentController.Instance.Added += new EventHandlers.DocumentEventHandler(Documents_Added);
			DocumentController.Instance.Removed += new EventHandlers.DocumentEventHandler(Documents_Removed);
			DocumentController.Instance.Switched += new EventHandlers.DocumentEventHandler(Documents_Switched);
			_global.UI.PropertyChanged += new PropertyChangedEventHandler(UI_PropertyChanged);
		}

		/// <summary>
		/// Determine which Document types we are currently supporting and create the filterstring for the Open and Save dialogs to use. 
		/// </summary>
		private void BuildFileDialogFilters()
		{
			string FilterFormat = "{0} (*.{1})|*.{1}";
			string Filter = string.Empty;

			//foreach (SupportedDocument sDocument in _global.SupportedDocuments)
			//{
			//	Filter += ((Filter.Length > 0) ? "|" : string.Empty) + string.Format(FilterFormat, sDocument.Name, sDocument.Extension);
			//}
			SaveDocumentDialog.Filter = Filter;

			// Append All Files at the end of the filter.
			Filter += ((Filter.Length > 0) ? "|" : string.Empty) + string.Format(FilterFormat, "All Files", "*");
			OpenDocumentDialog.Filter = Filter;
		}

		/// <summary>
		/// Construct the custom images for the menu items, such as adding the Create annotation image to the Document image for the New Document menu item.
		/// </summary>
		private void BuildMenuImages()
		{
			
		}

		/// <summary>
		/// Updates the items in the Windows menu, making only the item associated with the Active Document checked.
		/// </summary>
		private void CheckActiveDocumentWindowMenuItem()
		{
			foreach (ToolStripMenuItem Menu in WindowMenu.DropDownItems)
				Menu.Checked = (Object.ReferenceEquals(Menu.Tag, Document));
		}

		/// <summary>
		/// If the menu is checked, then superimposes the check annotation image over the passed in image.
		/// </summary>
		private Bitmap GetMenuCheckedImage(Bitmap menuImage, bool? isChecked)
		{
			return GetMenuCheckedImage(menuImage, (isChecked ?? false));
		}

		/// <summary>
		/// If the menu is checked, then superimposes the check annotation image over the passed in image.
		/// </summary>
		private Bitmap GetMenuCheckedImage(Bitmap menuImage, bool isChecked)
		{
			if (isChecked)
				return ImageHandler.Instance.AddAnnotation(menuImage, Annotation.Check);
			else
				return menuImage;
		}

		/// <summary>
		/// http://stackoverflow.com/questions/6469027/call-methods-using-names-in-c-sharp
		/// Calls a method within this form by name. Used for the shortcuts.
		/// </summary>
		/// <param name="methodName">Name of the method to invoke</param>
		/// <param name="args">Parameters to use with the method invocation.</param>
		public void InvokeMethod(string methodName, List<object> args)
		{
			GetType().GetMethod(methodName).Invoke(this, args.ToArray());
		}

		/// <summary>
		/// Brings up the Open File dialog to import a bitmap.
		/// </summary>
		/// <param name="filename">Output parameter, returns the filename chosen.</param>
		/// <returns>Bitmap that was loaded. If the dialog was canceled, returns null.</returns>
		private Bitmap LoadBitmap(out string filename)
		{
			filename = string.Empty;
			if (OpenImageFileDialog.ShowDialog() != DialogResult.OK)
				return null;

			var bmp = ImageHandler.Instance.LoadBitmapFromFile(OpenImageFileDialog.FileName);
			if (bmp == null)
			{
				MessageBox.Show("Unable to load this file.", "Load Image File", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return null;
			}

			filename = OpenImageFileDialog.FileName;
			return bmp;
		}

		/// <summary>
		/// Loads the window position/state values from the _settings object.
		/// </summary>
		private void LoadSettings()
		{

			this.Left = _settings.GetValue(Constants.SETUP_DIALOG + Constants.WINDOW_LEFT, this.Left);
			this.Top = _settings.GetValue(Constants.SETUP_DIALOG + Constants.WINDOW_TOP, this.Top);
			this.Width = _settings.GetValue(Constants.SETUP_DIALOG + Constants.WINDOW_WIDTH, this.Width);
			this.Height = _settings.GetValue(Constants.SETUP_DIALOG + Constants.WINDOW_HEIGHT, this.Height);
			this.WindowState = EnumHelper.GetEnumFromValue<FormWindowState>(_settings.GetValue(Constants.SETUP_DIALOG + Constants.WINDOW_STATE, (int)FormWindowState.Normal));
			DockPanel.DockRightPortion = _settings.GetValue(Constants.SETUP_DIALOG + EXPLORER_WIDTH, DockPanel.DockRightPortion);

			int SelectedToolID = _settings.GetValue(LAST_TOOL, 0);
			_global.CurrentTool = _global.GetPlugInTool(SelectedToolID);
			if (_global.CurrentTool == null)
				_global.CurrentTool = _global.Tools[0];
			_global.CurrentTool.Tool.IsSelected = true;

			Settings_ShowRuler.Image = GetMenuCheckedImage(ERes.ruler, _global.UI.ShowRuler);
		}
		

		/// <summary>
		/// Opens the Document specified.
		/// </summary>
		/// <param name="filename">Name of the file that contains the Document.</param>
		private void OpenDocuments(string filename)
		{
			List<string> Filenames = new List<string>
			{
				filename
			};
			OpenDocuments(Filenames);
			Filenames = null;
		}

		/// <summary>
		/// Opens the Documents specified in the list.
		/// </summary>
		/// <param name="fileNames">List of filenames to open</param>
		private void OpenDocuments(List<string> fileNames)
		{
			this.Cursor = Cursors.WaitCursor;
			foreach (string Filename in fileNames)
			{
				// Check to see if this Document is already open. If so, ask the user if they intended to revert it back to the saved version
				foreach (IDocument Document in DocumentController.Instance.List)
				{
					if (string.Compare(Document.Filename, Filename, true) == 0)
					{
						if (Document.Dirty)
							if (MessageBox.Show("The Document " + Document.Name + " is already loaded. Did you want to revert it back to it's saved version?", "Load Document", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
								continue;

						// Close out this existing Document
						DocumentController.Instance.Remove(Document);
						break;
					}
				}

				if (DocumentController.Instance.Load(Filename) != null)
					_mruMenu.AddFile(Filename);
			}

			this.Cursor = Cursors.Default;

			// Call the select event for the current tool to let it know there is a Document now.
			_global.CurrentTool.Tool.OnSelected();
		}

		/// <summary>
		/// Informs the various objects (including this form) to save their settings to the Settings document
		/// </summary>
		private void SaveSettings()
		{
			if (this.WindowState != FormWindowState.Minimized)
				_settings.SetValue(Constants.SETUP_DIALOG + Constants.WINDOW_STATE, (int)this.WindowState);

			if (this.WindowState == FormWindowState.Normal)
			{
				_settings.SetValue(Constants.SETUP_DIALOG + Constants.WINDOW_LEFT, this.Left);
				_settings.SetValue(Constants.SETUP_DIALOG + Constants.WINDOW_TOP, this.Top);
				_settings.SetValue(Constants.SETUP_DIALOG + Constants.WINDOW_WIDTH, this.Width);
				_settings.SetValue(Constants.SETUP_DIALOG + Constants.WINDOW_HEIGHT, this.Height);
			}

			_settings.SetValue(Constants.SETUP_DIALOG + EXPLORER_WIDTH, DockPanel.DockRightPortion);

			_settings.SetValue(LAST_TOOL, _global.CurrentTool.ID);

			// UI Settings
			_global.UI.SaveSettings();

			if (_global.Tools != null)
				foreach (PlugInTool PTool in _global.Tools)
					PTool.Tool.SaveSettings();

			if (_global.ToolGroups != null)
				foreach (PlugInToolGroup pToolGroup in _global.ToolGroups)
					pToolGroup.ToolGroup.SaveSettings();

			_mruMenu.SaveToSettings(_settings);
			_settings.Save();
		}

		/// <summary>
		/// Updates the controls to reflect the current Document.
		/// </summary>
		private void UpdateControls_Document()
		{
			UpdateControls_DocumentZoom();
			SetEditControls();
		}


		/// <summary>
		/// Updates the status bar for the current LatticeSize.
		/// </summary>
		private void UpdateControls_ImageSize()
		{
			if (Document == null)
				return;
			tssImageSize.Text = Document.Scaling.LatticeSize.Width + "x" + Document.Scaling.LatticeSize.Height;
		}

		/// <summary>
		/// Updates the status bar for the current Zoom.
		/// </summary>
		private void UpdateControls_DocumentZoom()
		{
			if (Document == null)
				return;
			tssZoom.Text = (Document.Scaling.Zoom * 100) + "%";
		}

		/// <summary>
		/// Sets the tooltip/text/enabled properties of the edit controls, based on the Undo and/or clipboard status of the current Document.
		/// </summary>
		private void SetEditControls()
		{
			// Clipboard tools
			bool HasData = false;

			//if (Document != null)
			//	HasData = Document.HasMask;

			ToolBar_Copy.Enabled = Edit_Copy.Enabled = HasData;
			ToolBar_Cut.Enabled = Edit_Cut.Enabled = HasData;

			HasData = ClipboardController.Instance.HasData;
			ToolBar_Paste.Enabled = Edit_Paste.Enabled = HasData;

			// Undo tools
			string Undo = string.Empty;
			HasData = false;
			if (Document != null)
			{
				HasData = Document.HasRedo;
				Undo = Document.GetUndoText(false);
			}
			SetRedoTools(HasData, Undo);

			Undo = string.Empty;
			HasData = false;

			if (Document != null)
			{
				HasData = Document.HasUndo;
				Undo = Document.GetUndoText(true);
			}
			SetUndoTools(HasData, Undo);
		}

		private void SetRedoTools(bool hasData, string action)
		{
			if (hasData)
			{
				Edit_Redo.Text = "&Redo " + action;
				Edit_Redo.Enabled = true;
				ToolBar_Redo.ToolTipText = "Redo " + action;
				ToolBar_Redo.Enabled = true;
			}
			else
			{
				Edit_Redo.Text = "&Redo";
				Edit_Redo.Enabled = false;
				ToolBar_Redo.ToolTipText = "Redo";
				ToolBar_Redo.Enabled = false;
			}
		}

		private void SetUndoTools(bool hasData, string action)
		{
			if (hasData)
			{
				Edit_Undo.Text = "&Undo " + action;
				Edit_Undo.Enabled = true;
				ToolBar_Undo.ToolTipText = "Undo " + action;
				ToolBar_Undo.Enabled = true;
			}
			else
			{
				Edit_Undo.Text = "&Undo";
				Edit_Undo.Enabled = false;
				ToolBar_Undo.ToolTipText = "Undo";
				ToolBar_Undo.Enabled = false;
			}
		}

		/// <summary>
		/// Perform necessary event decoupling that should occur when this form is closing
		/// </summary>
		private void ShutDown()
		{
			// Form is closed, do last minute housekeeping
			DocumentController.Instance.Switched -= Documents_Switched;
			DocumentController.Instance.Added -= Documents_Added;
			DocumentController.Instance.Removed -= Documents_Removed;
			_global.UI.PropertyChanged -= UI_PropertyChanged;
			ClipboardController.Instance.Changed -= Clipboard_Changed;

			_explorerTree.Close();
			_explorerTree.Editor = null;

			if (Document != null)
			{
				Document.PropertyChanged -= Document_PropertyChanged;
				Document.DirtyChanged -= Document_DirtyChanged;
			}

			DocumentController.Instance.Clear();
		}

		#endregion [ Methods ]

		#region [ Events ]

		#region [ CanvasWindow Events ]

		/// <summary>
		/// Fires when the mouse has enter the CanvasPane control on the Canvas Window form for the active Document.
		/// Makes the mouse coordinates panel on the status bar visible.
		/// </summary>
		private void Canvas_MouseEnter(object sender, EventArgs e)
		{
			//tssMouseCoords.Visible = true;
		}

		/// <summary>
		/// Fires when the mouse has left the CanvasPane control on the Canvas Window form for the active Document.
		/// Makes the mouse coordinates panel on the status bar invisible.
		/// </summary>
		private void Canvas_MouseLeave(object sender, EventArgs e)
		{
			//tssMouseCoords.Visible = false;
		}

		#endregion [ CanvasWindow Events ]

		#region [ Clipboard Events ]

		/// <summary>
		/// Occurs when the Clipboard contents are changed. Usually when something is Copied or Cut to the buffer
		/// </summary>
		private void Clipboard_Changed(object sender, EventArgs e)
		{
			SetEditControls();
			//Edit_Paste.Enabled = _global.Clipboard.HasData;
			//ToolBar_Paste.Enabled = _global.Clipboard.HasData;
		}

		#endregion [ Clipboard Events ]

		#region [ Form Events ]

		/// <summary>
		/// Fires when this window is closing. Save the window position/state settings.
		/// </summary>
		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveSettings();
		}

		/// <summary>
		/// The form has closed, detach all events and destroy all objects.
		/// </summary>
		private void Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			ShutDown();
		}

		/// <summary>
		/// Fires when this form begins to load into memory.
		/// </summary>
		private void Form_Load(object sender, EventArgs e)
		{ }

		/// <summary>
		/// Fires when the user taps a key on the keyboard.
		/// </summary>
		private void Form_KeyDown(object sender, KeyEventArgs e)
		{

			if (_global.CurrentTool != null)
				if (_global.CurrentTool.Tool.KeyDown(e))
				{
					e.Handled = false;
					return;
				}
		}

		/// <summary>
		/// Fires when the user releases a key on the keyboard
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_KeyUp(object sender, KeyEventArgs e)
		{
		}

		/// <summary>
		/// Fires when this form has been resized.
		/// </summary>
		private void Form_Resize(object sender, EventArgs e)
		{
			pnlToolStripDocker.Width = ToolStrip_Edit.Width - pnlToolStripDocker.Location.X;
			pnlToolStripDocker.Height = ToolStrip_Edit.Height;
		}

		/// <summary>
		/// Fires when the user/system begins to resize this form.
		/// </summary>
		private void Form_ResizeBegin(object sender, EventArgs e)
		{
			DockPanel.SuspendLayout();
		}

		/// <summary>
		/// Fires at the end of the resizing events.
		/// </summary>
		private void Form_ResizeEnd(object sender, EventArgs e)
		{
			DockPanel.ResumeLayout();
		}

		/// <summary>
		/// Occurs the first time the Editor window appears on screen
		/// </summary>
		private void Form_Shown(object sender, EventArgs e)
		{
			this.Text = (this.Text + " - " + AssemblyInfo.Trademark).TrimEnd() + " Version " + AssemblyInfo.AssemblyVersion;

			if ((_global.Document != null) && ((_global.Document.Name ?? string.Empty).Length > 0))
				this.Text += " - " + _global.Document.Name;

			if (pnlToolStripDocker.Controls.Count > 0)
				pnlToolStripDocker.Controls[0].Visible = true;
			
		}

		#endregion [ Form Events ]

		#region [ Menu Events ]

		#region [ File Menu Events ]

		#region [ PlugIn File Menu ]

		/// <summary>
		/// Exits this window and returns control back to the calling program (Vixen) with a flag indicating that
		/// the data should be saved
		/// </summary>
		private void File_SavePlugIn_Click(object sender, EventArgs e)
		{
			//this.PerformReconcile = true;
			//	Document.Save();
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Saves the data to the setup node and allows the user to continue editing.
		/// </summary>
		private void File_SaveContinue_Click(object sender, EventArgs e)
		{
			//this.PerformReconcile = true;
			//Document.Save();
		}

		/// <summary>
		/// Exits this window and returns control back to the calling program (Vixen) with a flag indicating that
		/// the data should be ignored
		/// </summary>
		private void File_ExitPlugIn_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		#endregion [ PlugIn File Menu ]

		private void File_Settings_Click(object sender, EventArgs e)
		{
			//	EditUISettings frmSettings = new EditUISettings();
			//frmSettings.ShowDialog();
			//frmSettings = null;
		}

		/// <summary>
		/// Prompt to save any dirty Documents, then exits the program
		/// </summary>
		private void File_ExitStandalone_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Save the current Document under a different name
		/// </summary>
		private void File_SaveAs_Click(object sender, EventArgs e)
		{
			//if (Document == null)
			//	return;

			////var Found = _global.SupportedDocuments.Where(Document.DocumentTypeID);
			////if (Found != null)
			////{
			////	int Index = _global.SupportedDocuments.IndexOf(Found);
			////	if (Index != -1) 
			////		SaveDocumentDialog.FilterIndex = Index + 1;
			////}

			//string Filename = Document.Filename;
			//if (Filename.Length > 0)
			//{
			//	if (Filename.Contains("\\"))
			//		SaveDocumentDialog.InitialDirectory = Path.GetFullPath(Filename);
			//	FileInfo fi = new FileInfo(Filename);
			//	SaveDocumentDialog.FileName = fi.Name;
			//}
			//if (SaveDocumentDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
			//	return;

			//Filename = SaveDocumentDialog.FileName;
			//Document.Filename = Filename;

			//// Determine what type of Document to save this as based on the Filter Index of the dialog.
			//DocumentType TargetDocumentType = DocumentType.NotSet;
			////TargetDocumentType = _global.SupportedDocuments[SaveDocumentDialog.FilterIndex - 1].DocumentType;

			//_global.DocumentController.ConvertDocument(Document, TargetDocumentType);
			//Document.Save();
			////File_Revert.Enabled = false;

			//_mruMenu.AddFile(Filename);
		}

		/// <summary>
		/// Calls the Documents save method, using the filename it already has.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void File_SaveStandalone_Click(object sender, EventArgs e)
		{
			//if (Document == null)
			//	return;

			//// If there is not a filename already defined within the Document,then call the SaveAs menu item.
			//if (Document.Filename.Length == 0)
			//	File_SaveAs_Click(sender, e);
			//else
			//{
			//	Document.Save();
			//	_mruMenu.AddFile(Document.Filename);
			//}
			////File_Revert.Enabled = false;
		}

		/// <summary>
		/// Reverts the Document to the last saved version.
		/// </summary>
		private void File_Revert_Click(object sender, EventArgs e)
		{
			//if (Document == null)
			//	return;

			//// Give the user a chance to not do this.
			//if (MessageBox.Show("Revert the Document to it's last saved version? This cannot be undone.", "Revert to Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
			//	return;

			//Document.Load();
			//Document.SetClean();

			////File_Revert.Enabled = false;
		}

		/// <summary>
		/// Close the Document. If there are changes, prompt the user to see if they want to save them first.
		/// </summary>
		private void File_Close_Click(object sender, EventArgs e)
		{
			//if (Document == null)
			//	return;

			//// Remove the Document from the list, switch to the next Document, if any
			//_global.DocumentController.Remove(Document);
		}

		/// <summary>
		/// Occurs when a user is dragging a file from Explorer and has entered the current window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_DragEnter(object sender, DragEventArgs e)
		{
			if (this.AllowDrop)
				if (e.Data.GetDataPresent(DataFormats.FileDrop))
					e.Effect = DragDropEffects.Copy;
		}

		/// <summary>
		/// Occurs when a user has dragged one or more Document file from the Explorer window to this form.
		/// </summary>
		private void Form_DragDrop(object sender, DragEventArgs e)
		{
			if (!this.AllowDrop)
				return;

			List<string> Filenames = new List<string>();
			Filenames.AddRange((string[])e.Data.GetData(DataFormats.FileDrop));
			OpenDocuments(Filenames);
		}

		/// <summary>
		/// Open a new Document, load its data from disk, and then make it the Active Document
		/// </summary>
		private void File_Open_Click(object sender, EventArgs e)
		{
			OpenDocumentDialog.Multiselect = true;
			if (OpenDocumentDialog.ShowDialog() == DialogResult.Cancel)
				return;

			List<string> Filenames = new List<string>();
			Filenames.AddRange(OpenDocumentDialog.FileNames);
			OpenDocuments(Filenames);
			Filenames = null;
		}

		/// <summary>
		/// Create a new Document
		/// </summary>
		private void File_New_Click(object sender, EventArgs e)
		{
			//Forms.NewDocument frmNew = new Forms.NewDocument();
			//if (frmNew.ShowDialog() == DialogResult.Cancel)
				//return;

			//IDocument NewDocument = _global.DocumentController.CreateNewDocument(frmNew.DocumentType);
			//NewDocument.Name = frmNew.DocumentName;
			//NewDocument.Filename = frmNew.Filename;
			//NewDocument.Scaling.LatticeSize = new Size(frmNew.CanvasWidth, frmNew.CanvasHeight);
			//NewDocument.Scaling.CellSize = frmNew.CellSize;
			//NewDocument.Scaling.ShowGridLines = frmNew.ShowGridLines;
			//NewDocument.Scaling.Zoom = Scaling.ZOOM_100;
			//int ID = 0;

			//NewDocument.InitializeUndo();
			//_global.DocumentController.Add(NewDocument);
			//_global.DocumentController.Active = NewDocument;
		}

		/// <summary>
		/// Loads the file that was selected from the MRU list.
		/// </summary>
		/// <param name="number">MRU Number of the file</param>
		/// <param name="filename">Name of the file</param>
		private void OnMruFile_Data(int number, string filename)
		{
			if (filename.Length == 0)
				return;

			FileInfo FI = new FileInfo(filename);
			if ((FI == null) || (FI.Exists == false))
			{
				MessageBox.Show(filename + " does not exist. Removing it from the Most Recent File menu.", "Open Document", MessageBoxButtons.OK, MessageBoxIcon.Error);
				_mruMenu.RemoveFile(number);
				return;
			}

			// Create a new Document object and pass in the Filename
			OpenDocuments(filename);

			//_mruMenu.RemoveFile(number);
			_mruMenu.AddFile(filename);
		}

		#endregion [ File Menu Events ]

		#region [ Edit Menu Events ]

		/// <summary>
		/// Copies the contents of the marquee from the selected item(s)
		/// </summary>
		private void Edit_Copy_Click(object sender, EventArgs e)
		{
			if (!ClipboardController.Instance.Copy())
				MessageBox.Show("Selection is Empty!", "Copy", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		/// <summary>
		/// Cuts the contents of the marquee from the selected item(s)
		/// </summary>
		private void Edit_Cut_Click(object sender, EventArgs e)
		{
			if (!ClipboardController.Instance.Cut())
				MessageBox.Show("Selection is Empty!", "Cut", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			else
				Document.Refresh();
		}

		/// <summary>
		/// Pastes the contents from the Clipboard to the selected item(s)
		/// </summary>
		private void Edit_Paste_Click(object sender, EventArgs e)
		{
			if (ClipboardController.Instance.HasData)
			{
				ClipboardController.Instance.Paste();
				Document.Refresh();
			}
		}

		/// <summary>
		/// Reapply the last operation that was recently undone.
		/// </summary>
		private void Edit_Redo_Click(object sender, EventArgs e)
		{
			Cursor LastCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			Document.Redo();
			this.Cursor = LastCursor;
		}

		/// <summary>
		/// Reverse out the last operation
		/// </summary>
		private void Edit_Undo_Click(object sender, EventArgs e)
		{
			Cursor LastCursor = this.Cursor;
			this.Cursor = Cursors.WaitCursor;
			Document.Undo();
			this.Cursor = LastCursor;
		}

		#endregion [ Edit Menu Events ]

		#region [ Document Menu Events ]

		/// <summary>
		/// Sets the Canvas Size
		/// </summary>
		private void Document_SetCanvasSize_Click(object sender, EventArgs e)
		{
			//ChangeLatticeSize frmLatticeSize = new ChangeLatticeSize();
			//if (frmLatticeSize.ShowDialog() == DialogResult.Cancel)
			//	return;
			//Document.Scaling.LatticeSize = new Size(frmLatticeSize.LatticeWidth, frmLatticeSize.LatticeHeight);
			//Document.SaveUndo("Set Canvas Size");
			//frmLatticeSize = null;
		}

		/// <summary>
		/// Sets the Cell Size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Document_CellSize_Click(object sender, EventArgs e)
		{
			//Document.Scaling.CellSize = Convert.ToInt32(((ToolStripMenuItem)sender).Tag);
			//Document.SaveUndo("Change Cell Size");
		}

		/// <summary>
		/// Indicates whether the grid should be used for displaying Cells.
		/// </summary>
		private void Document_ShowGrid_Click(object sender, EventArgs e)
		{
			//Document.Scaling.ShowGridLines = !Document.Scaling.ShowGridLines;
			//Document.SaveUndo(Document.Scaling.ShowGridLines.GetValueOrDefault(true) ? "Show Grid Lines" : "Hide Grid Lines");
		}

		/// <summary>
		/// Bring up the Background Settings dialog.
		/// </summary>
		private void Document_BackgroundSettings_Click(object sender, EventArgs e)
		{
			//Forms.BackgroundSettings frmBackground = new Forms.BackgroundSettings();
			//frmBackground.ShowDialog();
			//frmBackground = null;
		}

		#endregion [ Document Menu Events ]

		#region [ Settings Menu Events ]

		private void Settings_KeyConfig_Click(object sender, EventArgs e)
		{
			//ConfigureKeys frmConfig = new ConfigureKeys();
			//if (frmConfig.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
			//	return;
		}
		
		/// <summary>
		/// Indicates whether the rulers should show on the Canvas Window.
		/// </summary>
		private void Settings_ShowRuler_Click(object sender, EventArgs e)
		{
			_global.UI.ShowRuler = !_global.UI.ShowRuler;
		}
		
		#endregion [ Settings Menu Events ]

		#region [ Diagnostic Menu Events ]
		
		#endregion [ Diagnostic Menu Events ]

		#region [ Window Menu Events ]

		/// <summary>
		/// Makes the Document associated with this menu the Active one.
		/// </summary>
		private void Window_Document_Click(object sender, EventArgs e)
		{
			//_global.DocumentController.Active = (IDocument)((ToolStripMenuItem)sender).Tag;
			//_global.DocumentController.Active.Form.Activate();
		}

		#endregion [ Window Menu Events ]

		private void AboutMenu_Click(object sender, EventArgs e)
		{
			//DocumentSplash frmAbout = new DocumentSplash();
			//frmAbout.ShowDialog();
			//frmAbout = null;
		}

		#endregion [ Menu Events ]

		#region [ Document Controller Events ]


		/// <summary>
		/// Fires when the dirty bit for the Document changed.
		/// </summary>
		private void Document_DirtyChanged(object sender, DirtyEventArgs e)
		{
			//File_SaveStandalone.Enabled = e.IsDirty;
			//File_SaveContinue.Enabled = e.IsDirty;

			// If we've not saved this file yet, Revert is meaningless.
			File_Revert.Enabled = (e.IsDirty & (Document.Filename.Length > 0));
		}

		/// <summary>
		/// Occurs when one of the properties of the active Document (or one of its child objects) has changed.
		/// </summary>
		private void Document_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case Constants.PROPERTY_LATTICE_SIZE:
					UpdateControls_ImageSize();
					break;

				case Constants.PROPERTY_ZOOM:
					UpdateControls_DocumentZoom();
					break;

				case Constants.PROPERTY_SHOW_GRID_LINES:
					//this.Document_ShowGrid.Image = GetMenuCheckedImage(ERes.grid, Document.Scaling.ShowGridLines);
					break;
			}
		}

		/// <summary>
		/// Occurs when the Active Document changes.
		/// </summary>
		private void Documents_Switched(object sender, DocumentEventArgs e)
		{
			if (e.OldDocument != null)
			{
				// Detach the events from the old Document
				e.OldDocument.Canvas_MouseEnter -= Canvas_MouseEnter;
				e.OldDocument.Canvas_MouseLeave -= Canvas_MouseLeave;
				e.OldDocument.PropertyChanged -= Document_PropertyChanged;
				e.OldDocument.DirtyChanged -= Document_DirtyChanged;
				e.OldDocument.Undo_Changed -= Undo_Changed;
				e.OldDocument.Redo_Changed -= Redo_Changed;
				e.OldDocument.Undo_Completed -= Undo_Completed;
			}

			if (e.Document != null)
			{
				// Attach the events for the new Active Document
				e.Document.Canvas_MouseEnter += new EventHandler(Canvas_MouseEnter);
				e.Document.Canvas_MouseLeave += new EventHandler(Canvas_MouseLeave);
				e.Document.PropertyChanged += new PropertyChangedEventHandler(Document_PropertyChanged);
				e.Document.DirtyChanged += new EventHandlers.DirtyEventHandler(Document_DirtyChanged);
				e.Document.Undo_Changed += new EventHandlers.UndoEventHandler(Undo_Changed);
				e.Document.Redo_Changed += new EventHandlers.UndoEventHandler(Redo_Changed);
				e.Document.Undo_Completed += new EventHandler(Undo_Completed);
			}

			// If we are in standalone mode, check to see if the ActiveDocument is null.
			// Change the enabled state on the menus based on this.
			if (e.Document == null)
			{
				File_Close.Enabled = false;
				File_Revert.Enabled = false;
				File_SaveStandalone.Enabled = false;
				File_SaveAs.Enabled = false;
				EditMenu.Enabled = false;
				//DocumentMenu.Enabled = false;
				SettingsMenu.Enabled = false;
				WindowMenu.Enabled = false;
				ToolBox_Main.Enabled = false;

				tssMouseCoords.Visible = false;

				pnlToolStripDocker.Enabled = false;
				ToolStrip_Edit.Enabled = false;
			}
			else
			{
				File_Close.Enabled = true;
				File_Revert.Enabled = false;
				File_SaveStandalone.Enabled = true;
				File_SaveAs.Enabled = true;
				EditMenu.Enabled = true;
				//DocumentMenu.Enabled = true;
				SettingsMenu.Enabled = true;
				WindowMenu.Enabled = true;
				ToolBox_Main.Enabled = true;
				tssMouseCoords.Visible = true;

				pnlToolStripDocker.Enabled = true;
				ToolStrip_Edit.Enabled = true;

			}
			UpdateControls_Document();
			CheckActiveDocumentWindowMenuItem();
		}

		/// <summary>
		/// Occurs when a new Document is added to the Document Controller
		/// </summary>
		private void Documents_Added(object sender, DocumentEventArgs e)
		{
			// Events are attached when Document_Switched is called.

			// Add the form to the docking control
			e.Document.Form.Show(DockPanel);

			// Update the Windows menu
			ToolStripMenuItem DocumentMenuItem = new ToolStripMenuItem
			{
				Name = "DocumentMenuItem",
				Text = e.Document.Name,
				Tag = e.Document
			};
			DocumentMenuItem.Click += new EventHandler(this.Window_Document_Click);

			WindowMenu.DropDownItems.Add(DocumentMenuItem);
			CheckActiveDocumentWindowMenuItem();
		}

		/// <summary>
		/// Occurs when a Document has been from the Document Controller
		/// </summary>
		private void Documents_Removed(object sender, DocumentEventArgs e)
		{
			// Update the Windows menu
			foreach (ToolStripMenuItem Menu in WindowMenu.DropDownItems)
			{
				if (object.ReferenceEquals(Menu.Tag, e.Document))
				{
					WindowMenu.DropDownItems.Remove(Menu);
					break;
				}
			}
			CheckActiveDocumentWindowMenuItem();
			e.Document.Form.DockHandler.Close();
		}

		private void Document_Closing(object sender, EventArgs e)
		{ }

		#endregion [ Document Controller Events ]

		#region [ ToolBox Events ]

		/// <summary>
		/// Fires when the user releases the mouse from a toolgroup. Hides the flyout if it had been triggered.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolGroup_MouseUp(object sender, MouseEventArgs e)
		{
			if (tmrFlyout.Enabled)
			{
				tmrFlyout.Enabled = false;
				_flyoutToolStrip = null;
			}
			//Tool_Click(sender, new EventArgs());
		}

		/// <summary>
		/// Fires when the user mouse downs on the button holding a ToolGroup and pauses. Trigger's a flyout for that toolgroup.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ToolGroup_MouseDown(object sender, MouseEventArgs e)
		{
			PlugInToolGroup TG = ((PlugInToolStripButton)sender).PlugInToolGroup;

			_flyoutToolStrip = TG.ChildToolBox;
			tmrFlyout.Enabled = true;
		}

		/// <summary>
		/// Fires when the interval for the timer is reached. Display a flyout toolstrip.
		/// </summary>
		private void tmrFlyout_Tick(object sender, EventArgs e)
		{
			tmrFlyout.Enabled = false;
			_flyoutToolStrip.Visible = true;
			this.Controls.SetChildIndex(_flyoutToolStrip, 0); // Bring this submenu to the Front of all other controls
		}

		/// <summary>
		/// Fires when on the buttons on the main ToolBox is clicked.
		/// </summary>
		private void Tool_Click(object sender, EventArgs e)
		{
			PlugInTool SelectedTool = ((PlugInToolStripButton)sender).PlugInTool;

			// Fixes a bug that has the tool already selected if picking from a child toolbox.
			if (SelectedTool.Tool.IsSelected)
				SelectedTool.Tool.IsSelected = false;

			if (SelectedTool == null)
				throw new ArgumentNullException("SelectedTool is null");

			//Debug.WriteLine(SelectedTool.Name);

			// Unselect all tools that are not the one passed in, and select that one tool passed in.
			foreach (PlugInTool pTool in _global.Tools)
			{
				pTool.Tool.IsSelected = (pTool.ID == SelectedTool.ID);
			}
		}

		/// <summary>
		/// The operation for the current tool has finished, create the Undo for it.
		/// </summary>
		private void Tool_OperationCompleted(object sender, EventArgs e)
		{
			//Document.SaveUndo(((IPlugInTool)sender).UndoText);
		}

		/// <summary>
		/// Occurs when a Tool has been selected. Update the main ToolBox and set this tool to be the Active one in Workshop.
		/// </summary>
		private void Tool_Selected(object sender, EventArgs e)
		{
			IPlugInTool Tool = (IPlugInTool)sender;

			// Update the status bar
			tssTool.Text = Tool.Name;
			tssTool.Image = Tool.ToolBoxImage;

			// Swap out the setting toolstrips
			pnlToolStripDocker.Controls.Clear();

			ToolStrip Strip = Tool.SettingsToolStrip;
			if (Strip == null)
				Strip = ToolStrip_Blank;

			Strip.Dock = DockStyle.Top;
			pnlToolStripDocker.Controls.Add(Strip);

			// Uncheck all the other tools in the ToolBox
			ToolBox_Main.SuspendLayout();

			// Find the PlugInTool for this Tool.
			PlugInTool pTool = _global.GetPlugInTool(Tool.ID);
			if (pTool == null)
				return;
			_global.CurrentTool = pTool;

			PlugInToolStripButton Button = pTool.Button;

			// Check to see if the button clicked was on a sub menu. If so, then
			// set Button to be the ToolGroup button
			if (pTool.ParentButton != null)
				Button = pTool.ParentButton;

			//Debug.WriteLine(Button.ToolTipText);

			// On the main ToolBox, uncheck all tools that aren't this Tool, and check the button that corresponds to this Tool.
			foreach (PlugInToolStripButton tButton in ToolBox_Main.Items)
			{
				tButton.Checked = (tButton == Button);
				if (tButton.PlugInToolGroup != null)
					tButton.PlugInToolGroup.Close();

				if (tButton.Checked)
					tButton.Image = tButton.PlugInTool.ToolBoxImageSelected;
				else
					tButton.Image = tButton.PlugInTool.ToolBoxImage;
			}

			// Set the cursor for the Document.
			if (Document != null)
				Document.Cursor = Tool.Cursor;

			ToolBox_Main.ResumeLayout(true);

			Tool = null;
			pTool = null;
			Button = null;
		}

		#endregion [ ToolBox Events ]

		#region [ UI Events ]

		/// <summary>
		/// Occurs when one of the properties on the Workshop.UI object changed.
		/// </summary>
		private void UI_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Document == null)
				return;

			switch (e.PropertyName)
			{
				case Editor_Template.Core.UISettings.Property_MousePosition:
					if (_global.UI.MouseSelectionSize.IsEmpty)
						tssMouseCoords.Text = string.Format("({0}, {1})", _global.UI.MousePosition.X, _global.UI.MousePosition.Y);
					else
						tssMouseCoords.Text = string.Format("({0}, {1})-({2}, {3})", _global.UI.MouseDownPosition.X, _global.UI.MouseDownPosition.Y, _global.UI.MousePosition.X, _global.UI.MousePosition.Y);
					break;

				case Editor_Template.Core.UISettings.Property_ShowRuler:
					Settings_ShowRuler.Image = GetMenuCheckedImage(ERes.ruler, _global.UI.ShowRuler);
					break;

				case Editor_Template.Core.UISettings.Property_MouseSelectionSize:
				case Editor_Template.Core.UISettings.Property_MouseDownPosition:
					break;
			}
		}

		#endregion [ UI Events ]

		#region [ UndoController Events ]

		/// <summary>
		/// Occurs when a new item is the topmost item in the Redo stack, or the Redo stack has been emptied
		/// </summary>
		private void Redo_Changed(object sender, UndoEventArgs e)
		{
			SetRedoTools(e.HasData, e.Text);
			if (e.HasData)
			{
				Edit_Redo.Text = "&Redo " + e.Text;
				Edit_Redo.Enabled = true;
				ToolBar_Redo.ToolTipText = "Redo " + e.Text;
				ToolBar_Redo.Enabled = true;
			}
			else
			{
				Edit_Redo.Text = "&Redo";
				Edit_Redo.Enabled = false;
				ToolBar_Redo.ToolTipText = "Redo";
				ToolBar_Redo.Enabled = false;
			}
		}

		/// <summary>
		/// Occurs when a new item is the topmost item in the Undo stack, or the Undo stack has been emptied
		/// </summary>
		private void Undo_Changed(object sender, UndoEventArgs e)
		{
			SetUndoTools(e.HasData, e.Text);
			if (e.HasData)
			{
				Edit_Undo.Text = "&Undo " + e.Text;
				Edit_Undo.Enabled = true;
				ToolBar_Undo.ToolTipText = "Undo " + e.Text;
				ToolBar_Undo.Enabled = true;
			}
			else
			{
				Edit_Undo.Text = "&Undo";
				Edit_Undo.Enabled = false;
				ToolBar_Undo.ToolTipText = "Undo";
				ToolBar_Undo.Enabled = false;
			}
		}

		/// <summary>
		/// Fires when an undo or redo event has completed.
		/// </summary>
		private void Undo_Completed(object sender, EventArgs e)
		{
			SetEditControls();

			// Call the Clipboard_Changed delegate incase the clipboard was altered by the undo/redo.
			Clipboard_Changed(sender, e);
		}

		#endregion [ UndoController Events ]

		#endregion [ Events ]

	}
}
