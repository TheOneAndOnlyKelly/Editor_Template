namespace Editor_Template.Forms
{
	partial class Editor
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.OpenImageFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.SaveImageFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.FileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.File_New = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.File_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Revert = new System.Windows.Forms.ToolStripMenuItem();
			this.File_SaveStandalone = new System.Windows.Forms.ToolStripMenuItem();
			this.File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Sep5 = new System.Windows.Forms.ToolStripSeparator();
			this.File_Settings1 = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Sep2 = new System.Windows.Forms.ToolStripSeparator();
			this.File_Recent = new System.Windows.Forms.ToolStripMenuItem();
			this.File_Sep3 = new System.Windows.Forms.ToolStripSeparator();
			this.File_ExitStandalone = new System.Windows.Forms.ToolStripMenuItem();
			this.EditMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Undo = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Redo = new System.Windows.Forms.ToolStripMenuItem();
			this.EditSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Edit_Cut = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Copy = new System.Windows.Forms.ToolStripMenuItem();
			this.Edit_Paste = new System.Windows.Forms.ToolStripMenuItem();
			this.SettingsMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Settings_KeyConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.SettingsSep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Settings_ShowRuler = new System.Windows.Forms.ToolStripMenuItem();
			this.WindowMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.AboutMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStrip_Edit = new System.Windows.Forms.ToolStrip();
			this.ToolBar_Undo = new System.Windows.Forms.ToolStripButton();
			this.ToolBar_Redo = new System.Windows.Forms.ToolStripButton();
			this.ToolBar_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolBar_Cut = new System.Windows.Forms.ToolStripButton();
			this.ToolBar_Copy = new System.Windows.Forms.ToolStripButton();
			this.ToolBar_Paste = new System.Windows.Forms.ToolStripButton();
			this.ToolBox_Main = new System.Windows.Forms.ToolStrip();
			this.StatusBar = new System.Windows.Forms.StatusStrip();
			this.tssStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this._tssImageSize = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssImageSize = new System.Windows.Forms.ToolStripStatusLabel();
			this._tssZoom = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssZoom = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tmrFlyout = new System.Windows.Forms.Timer(this.components);
			this.OpenDocumentDialog = new System.Windows.Forms.OpenFileDialog();
			this.DockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.pnlHoldToolStrips = new System.Windows.Forms.Panel();
			this.ToolStrip_Blank = new System.Windows.Forms.ToolStrip();
			this.SaveDocumentDialog = new System.Windows.Forms.SaveFileDialog();
			this.pnlToolStripDocker = new System.Windows.Forms.Panel();
			this._tssTool = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssTool = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssMouseCoords = new System.Windows.Forms.ToolStripStatusLabel();
			this.MainMenu.SuspendLayout();
			this.ToolStrip_Edit.SuspendLayout();
			this.StatusBar.SuspendLayout();
			this.pnlHoldToolStrips.SuspendLayout();
			this.SuspendLayout();
			// 
			// OpenImageFileDialog
			// 
			this.OpenImageFileDialog.Filter = "Bitmap File (*.bmp)|*.bmp|JPEG File (*.jpg)|*.jpg|PNG File (*.png)|*.png|GIF File" +
    " (*.gif)|*.gif|All Files (*.*)|*.*";
			this.OpenImageFileDialog.FilterIndex = 5;
			// 
			// SaveImageFileDialog
			// 
			this.SaveImageFileDialog.Filter = "Bitmap File (*.bmp)|*.bmp|JPEG File (*.jpg)|*.jpg|PNG File (*.png)|*.png|GIF File" +
    " (*.gif)|*.gif|All Files (*.*)|*.*";
			this.SaveImageFileDialog.FilterIndex = 5;
			// 
			// MainMenu
			// 
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenu,
            this.EditMenu,
            this.SettingsMenu,
            this.WindowMenu,
            this.AboutMenu});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(1167, 24);
			this.MainMenu.TabIndex = 0;
			this.MainMenu.Text = "menuStrip1";
			// 
			// FileMenu
			// 
			this.FileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File_New,
            this.File_Open,
            this.File_Sep1,
            this.File_Close,
            this.File_Revert,
            this.File_SaveStandalone,
            this.File_SaveAs,
            this.File_Sep5,
            this.File_Settings1,
            this.File_Sep2,
            this.File_Recent,
            this.File_Sep3,
            this.File_ExitStandalone});
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(37, 20);
			this.FileMenu.Text = "&File";
			// 
			// File_New
			// 
			this.File_New.Image = global::Editor_Template.Properties.Resources.create;
			this.File_New.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_New.Name = "File_New";
			this.File_New.Size = new System.Drawing.Size(173, 22);
			this.File_New.Text = "&New Item";
			this.File_New.Click += new System.EventHandler(this.File_New_Click);
			// 
			// File_Open
			// 
			this.File_Open.Image = global::Editor_Template.Properties.Resources.file_open;
			this.File_Open.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_Open.Name = "File_Open";
			this.File_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.File_Open.Size = new System.Drawing.Size(173, 22);
			this.File_Open.Text = "&Open Item";
			this.File_Open.Click += new System.EventHandler(this.File_Open_Click);
			// 
			// File_Sep1
			// 
			this.File_Sep1.Name = "File_Sep1";
			this.File_Sep1.Size = new System.Drawing.Size(170, 6);
			// 
			// File_Close
			// 
			this.File_Close.Image = global::Editor_Template.Properties.Resources.close;
			this.File_Close.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_Close.Name = "File_Close";
			this.File_Close.Size = new System.Drawing.Size(173, 22);
			this.File_Close.Text = "&Close Item";
			this.File_Close.Click += new System.EventHandler(this.File_Close_Click);
			// 
			// File_Revert
			// 
			this.File_Revert.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_Revert.Name = "File_Revert";
			this.File_Revert.Size = new System.Drawing.Size(173, 22);
			this.File_Revert.Text = "&Revert to Saved";
			this.File_Revert.Click += new System.EventHandler(this.File_Revert_Click);
			// 
			// File_SaveStandalone
			// 
			this.File_SaveStandalone.Image = global::Editor_Template.Properties.Resources.save;
			this.File_SaveStandalone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_SaveStandalone.Name = "File_SaveStandalone";
			this.File_SaveStandalone.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.File_SaveStandalone.Size = new System.Drawing.Size(173, 22);
			this.File_SaveStandalone.Text = "&Save";
			this.File_SaveStandalone.Click += new System.EventHandler(this.File_SaveStandalone_Click);
			// 
			// File_SaveAs
			// 
			this.File_SaveAs.Image = global::Editor_Template.Properties.Resources.save_as;
			this.File_SaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_SaveAs.Name = "File_SaveAs";
			this.File_SaveAs.Size = new System.Drawing.Size(173, 22);
			this.File_SaveAs.Text = "Save &As...";
			this.File_SaveAs.Click += new System.EventHandler(this.File_SaveAs_Click);
			// 
			// File_Sep5
			// 
			this.File_Sep5.Name = "File_Sep5";
			this.File_Sep5.Size = new System.Drawing.Size(170, 6);
			// 
			// File_Settings1
			// 
			this.File_Settings1.Image = global::Editor_Template.Properties.Resources.settings;
			this.File_Settings1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_Settings1.Name = "File_Settings1";
			this.File_Settings1.Size = new System.Drawing.Size(173, 22);
			this.File_Settings1.Text = "Settings...";
			this.File_Settings1.Click += new System.EventHandler(this.File_Settings_Click);
			// 
			// File_Sep2
			// 
			this.File_Sep2.Name = "File_Sep2";
			this.File_Sep2.Size = new System.Drawing.Size(170, 6);
			// 
			// File_Recent
			// 
			this.File_Recent.Image = global::Editor_Template.Properties.Resources.recent;
			this.File_Recent.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_Recent.Name = "File_Recent";
			this.File_Recent.Size = new System.Drawing.Size(173, 22);
			this.File_Recent.Text = "Recent";
			// 
			// File_Sep3
			// 
			this.File_Sep3.Name = "File_Sep3";
			this.File_Sep3.Size = new System.Drawing.Size(170, 6);
			// 
			// File_ExitStandalone
			// 
			this.File_ExitStandalone.Image = global::Editor_Template.Properties.Resources.exit;
			this.File_ExitStandalone.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.File_ExitStandalone.Name = "File_ExitStandalone";
			this.File_ExitStandalone.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.File_ExitStandalone.Size = new System.Drawing.Size(173, 22);
			this.File_ExitStandalone.Text = "&Exit";
			this.File_ExitStandalone.Click += new System.EventHandler(this.File_ExitStandalone_Click);
			// 
			// EditMenu
			// 
			this.EditMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit_Undo,
            this.Edit_Redo,
            this.EditSep1,
            this.Edit_Cut,
            this.Edit_Copy,
            this.Edit_Paste});
			this.EditMenu.Name = "EditMenu";
			this.EditMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.EditMenu.Size = new System.Drawing.Size(39, 20);
			this.EditMenu.Text = "&Edit";
			// 
			// Edit_Undo
			// 
			this.Edit_Undo.Enabled = false;
			this.Edit_Undo.Image = global::Editor_Template.Properties.Resources.undo;
			this.Edit_Undo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.Edit_Undo.Name = "Edit_Undo";
			this.Edit_Undo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.Edit_Undo.Size = new System.Drawing.Size(174, 22);
			this.Edit_Undo.Text = "&Undo";
			this.Edit_Undo.Click += new System.EventHandler(this.Edit_Undo_Click);
			// 
			// Edit_Redo
			// 
			this.Edit_Redo.Enabled = false;
			this.Edit_Redo.Image = global::Editor_Template.Properties.Resources.redo;
			this.Edit_Redo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.Edit_Redo.Name = "Edit_Redo";
			this.Edit_Redo.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.Edit_Redo.Size = new System.Drawing.Size(174, 22);
			this.Edit_Redo.Text = "&Redo";
			this.Edit_Redo.Click += new System.EventHandler(this.Edit_Redo_Click);
			// 
			// EditSep1
			// 
			this.EditSep1.Name = "EditSep1";
			this.EditSep1.Size = new System.Drawing.Size(171, 6);
			// 
			// Edit_Cut
			// 
			this.Edit_Cut.Enabled = false;
			this.Edit_Cut.Image = global::Editor_Template.Properties.Resources.cut;
			this.Edit_Cut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.Edit_Cut.Name = "Edit_Cut";
			this.Edit_Cut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.Edit_Cut.Size = new System.Drawing.Size(174, 22);
			this.Edit_Cut.Text = "C&ut";
			this.Edit_Cut.Click += new System.EventHandler(this.Edit_Cut_Click);
			// 
			// Edit_Copy
			// 
			this.Edit_Copy.Enabled = false;
			this.Edit_Copy.Image = global::Editor_Template.Properties.Resources.copy;
			this.Edit_Copy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.Edit_Copy.Name = "Edit_Copy";
			this.Edit_Copy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.Edit_Copy.Size = new System.Drawing.Size(174, 22);
			this.Edit_Copy.Text = "&Copy";
			this.Edit_Copy.Click += new System.EventHandler(this.Edit_Copy_Click);
			// 
			// Edit_Paste
			// 
			this.Edit_Paste.Enabled = false;
			this.Edit_Paste.Image = global::Editor_Template.Properties.Resources.paste;
			this.Edit_Paste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.Edit_Paste.Name = "Edit_Paste";
			this.Edit_Paste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.Edit_Paste.Size = new System.Drawing.Size(174, 22);
			this.Edit_Paste.Text = "&Paste";
			this.Edit_Paste.Click += new System.EventHandler(this.Edit_Paste_Click);
			// 
			// SettingsMenu
			// 
			this.SettingsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Settings_KeyConfig,
            this.SettingsSep1,
            this.Settings_ShowRuler});
			this.SettingsMenu.Name = "SettingsMenu";
			this.SettingsMenu.Size = new System.Drawing.Size(61, 20);
			this.SettingsMenu.Text = "&Settings";
			this.SettingsMenu.Visible = false;
			// 
			// Settings_KeyConfig
			// 
			this.Settings_KeyConfig.Image = global::Editor_Template.Properties.Resources.keyboard;
			this.Settings_KeyConfig.Name = "Settings_KeyConfig";
			this.Settings_KeyConfig.Size = new System.Drawing.Size(179, 22);
			this.Settings_KeyConfig.Text = "&Key Configuration...";
			this.Settings_KeyConfig.Visible = false;
			this.Settings_KeyConfig.Click += new System.EventHandler(this.Settings_KeyConfig_Click);
			// 
			// SettingsSep1
			// 
			this.SettingsSep1.Name = "SettingsSep1";
			this.SettingsSep1.Size = new System.Drawing.Size(176, 6);
			this.SettingsSep1.Visible = false;
			// 
			// Settings_ShowRuler
			// 
			this.Settings_ShowRuler.Image = global::Editor_Template.Properties.Resources.ruler;
			this.Settings_ShowRuler.Name = "Settings_ShowRuler";
			this.Settings_ShowRuler.Size = new System.Drawing.Size(179, 22);
			this.Settings_ShowRuler.Text = "Show Ru&ler";
			this.Settings_ShowRuler.Click += new System.EventHandler(this.Settings_ShowRuler_Click);
			// 
			// WindowMenu
			// 
			this.WindowMenu.Name = "WindowMenu";
			this.WindowMenu.Size = new System.Drawing.Size(63, 20);
			this.WindowMenu.Text = "&Window";
			// 
			// AboutMenu
			// 
			this.AboutMenu.Name = "AboutMenu";
			this.AboutMenu.Size = new System.Drawing.Size(61, 20);
			this.AboutMenu.Text = "About...";
			this.AboutMenu.Click += new System.EventHandler(this.AboutMenu_Click);
			// 
			// ToolStrip_Edit
			// 
			this.ToolStrip_Edit.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip_Edit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBar_Undo,
            this.ToolBar_Redo,
            this.ToolBar_Sep1,
            this.ToolBar_Cut,
            this.ToolBar_Copy,
            this.ToolBar_Paste});
			this.ToolStrip_Edit.Location = new System.Drawing.Point(0, 24);
			this.ToolStrip_Edit.Name = "ToolStrip_Edit";
			this.ToolStrip_Edit.Size = new System.Drawing.Size(1167, 25);
			this.ToolStrip_Edit.TabIndex = 1;
			this.ToolStrip_Edit.Text = "ToolStrip_Edit";
			// 
			// ToolBar_Undo
			// 
			this.ToolBar_Undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolBar_Undo.Enabled = false;
			this.ToolBar_Undo.Image = global::Editor_Template.Properties.Resources.undo;
			this.ToolBar_Undo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolBar_Undo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolBar_Undo.Name = "ToolBar_Undo";
			this.ToolBar_Undo.Size = new System.Drawing.Size(23, 22);
			this.ToolBar_Undo.Text = "Reverses the last operation";
			this.ToolBar_Undo.Click += new System.EventHandler(this.Edit_Undo_Click);
			// 
			// ToolBar_Redo
			// 
			this.ToolBar_Redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolBar_Redo.Enabled = false;
			this.ToolBar_Redo.Image = global::Editor_Template.Properties.Resources.redo;
			this.ToolBar_Redo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolBar_Redo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolBar_Redo.Name = "ToolBar_Redo";
			this.ToolBar_Redo.Size = new System.Drawing.Size(23, 22);
			this.ToolBar_Redo.Text = "Redoes the last Undo operation";
			this.ToolBar_Redo.Click += new System.EventHandler(this.Edit_Redo_Click);
			// 
			// ToolBar_Sep1
			// 
			this.ToolBar_Sep1.Name = "ToolBar_Sep1";
			this.ToolBar_Sep1.Size = new System.Drawing.Size(6, 25);
			// 
			// ToolBar_Cut
			// 
			this.ToolBar_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolBar_Cut.Enabled = false;
			this.ToolBar_Cut.Image = global::Editor_Template.Properties.Resources.cut;
			this.ToolBar_Cut.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolBar_Cut.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolBar_Cut.Name = "ToolBar_Cut";
			this.ToolBar_Cut.Size = new System.Drawing.Size(23, 22);
			this.ToolBar_Cut.Text = "Cuts the selection and places it in the Clipboard (Ctrl+X)";
			this.ToolBar_Cut.Click += new System.EventHandler(this.Edit_Cut_Click);
			// 
			// ToolBar_Copy
			// 
			this.ToolBar_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolBar_Copy.Enabled = false;
			this.ToolBar_Copy.Image = global::Editor_Template.Properties.Resources.copy;
			this.ToolBar_Copy.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolBar_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolBar_Copy.Name = "ToolBar_Copy";
			this.ToolBar_Copy.Size = new System.Drawing.Size(23, 22);
			this.ToolBar_Copy.Text = "Copies the selection and places it in the Clipboard (Ctrl+C)";
			this.ToolBar_Copy.Click += new System.EventHandler(this.Edit_Copy_Click);
			// 
			// ToolBar_Paste
			// 
			this.ToolBar_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ToolBar_Paste.Enabled = false;
			this.ToolBar_Paste.Image = global::Editor_Template.Properties.Resources.paste;
			this.ToolBar_Paste.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolBar_Paste.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ToolBar_Paste.Name = "ToolBar_Paste";
			this.ToolBar_Paste.Size = new System.Drawing.Size(23, 22);
			this.ToolBar_Paste.Text = "Paste the contents of the clipboard onto the document";
			this.ToolBar_Paste.Click += new System.EventHandler(this.Edit_Paste_Click);
			// 
			// ToolBox_Main
			// 
			this.ToolBox_Main.AutoSize = false;
			this.ToolBox_Main.Dock = System.Windows.Forms.DockStyle.Left;
			this.ToolBox_Main.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolBox_Main.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.ToolBox_Main.Location = new System.Drawing.Point(0, 49);
			this.ToolBox_Main.Name = "ToolBox_Main";
			this.ToolBox_Main.Padding = new System.Windows.Forms.Padding(0, 2, 1, 0);
			this.ToolBox_Main.Size = new System.Drawing.Size(32, 564);
			this.ToolBox_Main.TabIndex = 3;
			this.ToolBox_Main.TabStop = true;
			// 
			// StatusBar
			// 
			this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssStatus,
            this.tssMouseCoords,
            this._tssTool,
            this.tssTool,
            this._tssImageSize,
            this.tssImageSize,
            this._tssZoom,
            this.tssZoom});
			this.StatusBar.Location = new System.Drawing.Point(0, 613);
			this.StatusBar.Name = "StatusBar";
			this.StatusBar.Size = new System.Drawing.Size(1167, 22);
			this.StatusBar.TabIndex = 7;
			// 
			// tssStatus
			// 
			this.tssStatus.Name = "tssStatus";
			this.tssStatus.Size = new System.Drawing.Size(744, 17);
			this.tssStatus.Spring = true;
			this.tssStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _tssImageSize
			// 
			this._tssImageSize.BackColor = System.Drawing.SystemColors.Control;
			this._tssImageSize.Name = "_tssImageSize";
			this._tssImageSize.Size = new System.Drawing.Size(66, 17);
			this._tssImageSize.Text = "Image Size:";
			// 
			// tssImageSize
			// 
			this.tssImageSize.BackColor = System.Drawing.SystemColors.Control;
			this.tssImageSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tssImageSize.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
			this.tssImageSize.Name = "tssImageSize";
			this.tssImageSize.Size = new System.Drawing.Size(42, 17);
			this.tssImageSize.Text = "64x32";
			// 
			// _tssZoom
			// 
			this._tssZoom.BackColor = System.Drawing.SystemColors.Control;
			this._tssZoom.Name = "_tssZoom";
			this._tssZoom.Size = new System.Drawing.Size(42, 17);
			this._tssZoom.Text = "Zoom:";
			// 
			// tssZoom
			// 
			this.tssZoom.BackColor = System.Drawing.SystemColors.Control;
			this.tssZoom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tssZoom.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
			this.tssZoom.Name = "tssZoom";
			this.tssZoom.Size = new System.Drawing.Size(38, 17);
			this.tssZoom.Text = "100%";
			// 
			// tmrFlyout
			// 
			this.tmrFlyout.Interval = 200;
			this.tmrFlyout.Tick += new System.EventHandler(this.tmrFlyout_Tick);
			// 
			// OpenDocumentDialog
			// 
			this.OpenDocumentDialog.DefaultExt = "pro";
			this.OpenDocumentDialog.FilterIndex = 0;
			this.OpenDocumentDialog.Title = "Open Document";
			// 
			// DockPanel
			// 
			this.DockPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DockPanel.DockRightPortion = 200D;
			this.DockPanel.Location = new System.Drawing.Point(32, 49);
			this.DockPanel.Name = "DockPanel";
			this.DockPanel.Size = new System.Drawing.Size(1135, 564);
			this.DockPanel.TabIndex = 7;
			// 
			// pnlHoldToolStrips
			// 
			this.pnlHoldToolStrips.Controls.Add(this.ToolStrip_Blank);
			this.pnlHoldToolStrips.Location = new System.Drawing.Point(217, 65);
			this.pnlHoldToolStrips.Name = "pnlHoldToolStrips";
			this.pnlHoldToolStrips.Size = new System.Drawing.Size(536, 202);
			this.pnlHoldToolStrips.TabIndex = 8;
			this.pnlHoldToolStrips.Visible = false;
			// 
			// ToolStrip_Blank
			// 
			this.ToolStrip_Blank.Location = new System.Drawing.Point(0, 0);
			this.ToolStrip_Blank.Name = "ToolStrip_Blank";
			this.ToolStrip_Blank.Size = new System.Drawing.Size(536, 25);
			this.ToolStrip_Blank.TabIndex = 13;
			// 
			// SaveDocumentDialog
			// 
			this.SaveDocumentDialog.DefaultExt = "pro";
			this.SaveDocumentDialog.FilterIndex = 0;
			this.SaveDocumentDialog.Title = "Save Document";
			// 
			// pnlToolStripDocker
			// 
			this.pnlToolStripDocker.Location = new System.Drawing.Point(128, 25);
			this.pnlToolStripDocker.Name = "pnlToolStripDocker";
			this.pnlToolStripDocker.Size = new System.Drawing.Size(1039, 25);
			this.pnlToolStripDocker.TabIndex = 10;
			// 
			// _tssTool
			// 
			this._tssTool.BackColor = System.Drawing.SystemColors.Control;
			this._tssTool.Name = "_tssTool";
			this._tssTool.Size = new System.Drawing.Size(76, 17);
			this._tssTool.Text = "Current Tool:";
			// 
			// tssTool
			// 
			this.tssTool.BackColor = System.Drawing.SystemColors.Control;
			this.tssTool.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tssTool.Margin = new System.Windows.Forms.Padding(0, 3, 5, 2);
			this.tssTool.Name = "tssTool";
			this.tssTool.Size = new System.Drawing.Size(66, 17);
			this.tssTool.Text = "Paintbrush";
			// 
			// tssMouseCoords
			// 
			this.tssMouseCoords.BackColor = System.Drawing.SystemColors.Control;
			this.tssMouseCoords.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tssMouseCoords.Name = "tssMouseCoords";
			this.tssMouseCoords.Size = new System.Drawing.Size(32, 17);
			this.tssMouseCoords.Text = "{0,0}";
			// 
			// Editor
			// 
			this.AllowDrop = true;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1167, 635);
			this.Controls.Add(this.pnlToolStripDocker);
			this.Controls.Add(this.pnlHoldToolStrips);
			this.Controls.Add(this.DockPanel);
			this.Controls.Add(this.ToolBox_Main);
			this.Controls.Add(this.ToolStrip_Edit);
			this.Controls.Add(this.MainMenu);
			this.Controls.Add(this.StatusBar);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.MainMenu;
			this.MinimumSize = new System.Drawing.Size(20, 73);
			this.Name = "Editor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
			this.Load += new System.EventHandler(this.Form_Load);
			this.Shown += new System.EventHandler(this.Form_Shown);
			this.ResizeBegin += new System.EventHandler(this.Form_ResizeBegin);
			this.ResizeEnd += new System.EventHandler(this.Form_ResizeEnd);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form_KeyUp);
			this.Resize += new System.EventHandler(this.Form_Resize);
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.ToolStrip_Edit.ResumeLayout(false);
			this.ToolStrip_Edit.PerformLayout();
			this.StatusBar.ResumeLayout(false);
			this.StatusBar.PerformLayout();
			this.pnlHoldToolStrips.ResumeLayout(false);
			this.pnlHoldToolStrips.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog OpenImageFileDialog;
		private System.Windows.Forms.SaveFileDialog SaveImageFileDialog;
		private WeifenLuo.WinFormsUI.Docking.DockPanel DockPanel;
		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ToolStripMenuItem EditMenu;
		private System.Windows.Forms.ToolStripMenuItem Edit_Undo;
		private System.Windows.Forms.ToolStripMenuItem Edit_Redo;
		private System.Windows.Forms.ToolStripSeparator EditSep1;
		private System.Windows.Forms.ToolStripMenuItem Edit_Cut;
		private System.Windows.Forms.ToolStripMenuItem Edit_Copy;
		private System.Windows.Forms.ToolStripMenuItem Edit_Paste;
		private System.Windows.Forms.ToolStrip ToolStrip_Edit;
		private System.Windows.Forms.ToolStripButton ToolBar_Undo;
		private System.Windows.Forms.ToolStripButton ToolBar_Redo;
		private System.Windows.Forms.ToolStripSeparator ToolBar_Sep1;
		private System.Windows.Forms.ToolStripButton ToolBar_Cut;
		private System.Windows.Forms.ToolStripButton ToolBar_Copy;
		private System.Windows.Forms.ToolStripButton ToolBar_Paste;
		private System.Windows.Forms.ToolStrip ToolBox_Main;
		private System.Windows.Forms.StatusStrip StatusBar;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem SettingsMenu;
		private System.Windows.Forms.ToolStripMenuItem Settings_KeyConfig;
		private System.Windows.Forms.ToolStripStatusLabel tssStatus;
		private System.Windows.Forms.ToolStripStatusLabel _tssZoom;
		private System.Windows.Forms.ToolStripStatusLabel tssZoom;
		private System.Windows.Forms.ToolStripStatusLabel _tssImageSize;
		private System.Windows.Forms.ToolStripStatusLabel tssImageSize;
		private System.Windows.Forms.Timer tmrFlyout;
		private System.Windows.Forms.OpenFileDialog OpenDocumentDialog;
		private System.Windows.Forms.ToolStripSeparator SettingsSep1;
		private System.Windows.Forms.ToolStripMenuItem Settings_ShowRuler;
		private System.Windows.Forms.Panel pnlHoldToolStrips;
		private System.Windows.Forms.ToolStrip ToolStrip_Blank;
		private System.Windows.Forms.ToolStripMenuItem FileMenu;
		private System.Windows.Forms.ToolStripMenuItem File_New;
		private System.Windows.Forms.ToolStripMenuItem File_Open;
		private System.Windows.Forms.ToolStripSeparator File_Sep1;
		private System.Windows.Forms.ToolStripMenuItem File_Close;
		private System.Windows.Forms.ToolStripMenuItem File_Revert;
		private System.Windows.Forms.ToolStripMenuItem File_SaveStandalone;
		private System.Windows.Forms.ToolStripMenuItem File_SaveAs;
		private System.Windows.Forms.ToolStripSeparator File_Sep2;
		private System.Windows.Forms.ToolStripMenuItem File_Recent;
		private System.Windows.Forms.ToolStripSeparator File_Sep3;
		private System.Windows.Forms.ToolStripMenuItem File_ExitStandalone;
		private System.Windows.Forms.SaveFileDialog SaveDocumentDialog;
		private System.Windows.Forms.ToolStripMenuItem WindowMenu;
		private System.Windows.Forms.ToolStripMenuItem AboutMenu;
		private System.Windows.Forms.ToolStripSeparator File_Sep5;
		private System.Windows.Forms.ToolStripMenuItem File_Settings1;
		private System.Windows.Forms.Panel pnlToolStripDocker;
		private System.Windows.Forms.ToolStripStatusLabel tssMouseCoords;
		private System.Windows.Forms.ToolStripStatusLabel _tssTool;
		private System.Windows.Forms.ToolStripStatusLabel tssTool;
	}
}