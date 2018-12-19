namespace Editor_Template.Forms
{
	partial class ExplorerTree
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
			_disposed = true;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerTree));
			this.Explorer_ContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Context_Properties = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_AddNew = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Delete = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Sep2 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_Clear = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_ClearAll = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Sep_Clear = new System.Windows.Forms.ToolStripSeparator();
			this.Context_Rename = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Sep_Visible = new System.Windows.Forms.ToolStripSeparator();
			this.Context_DisplaySubMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Hide = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_HideOthers = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Show = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_ShowAll = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Display_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_Enable = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Disable = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Display_Sep2 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_Lock = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Unlock = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Display_Sep3 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_Include = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Exclude = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Arrange = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Group = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Ungroup = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_Arrange_Sep1 = new System.Windows.Forms.ToolStripSeparator();
			this.Context_MoveToTop = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_MoveUp = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_MoveDown = new System.Windows.Forms.ToolStripMenuItem();
			this.Context_MoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStrip = new System.Windows.Forms.ToolStrip();
			this.TB_Group = new System.Windows.Forms.ToolStripButton();
			this.ItemTree = new KellyControls.MultiSelectTreeview();
			this.ilstIcons = new System.Windows.Forms.ImageList(this.components);
			this.ilstState = new System.Windows.Forms.ImageList(this.components);
			this.tmrFocus = new System.Windows.Forms.Timer(this.components);
			this.pnlShuffles = new System.Windows.Forms.Panel();
			this.cboDropdown = new System.Windows.Forms.ComboBox();
			this.pnlSep4 = new System.Windows.Forms.Panel();
			this.pnlSep3 = new System.Windows.Forms.Panel();
			this.cmdAdd = new System.Windows.Forms.Button();
			this.pnlSep2 = new System.Windows.Forms.Panel();
			this.cmdEdit = new System.Windows.Forms.Button();
			this.pnlSep1 = new System.Windows.Forms.Panel();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.pnlBottomSpacer = new System.Windows.Forms.Panel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.Explorer_ContextMenu.SuspendLayout();
			this.ToolStrip.SuspendLayout();
			this.pnlShuffles.SuspendLayout();
			this.SuspendLayout();
			// 
			// Explorer_ContextMenu
			// 
			this.Explorer_ContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Properties,
            this.Context_Sep1,
            this.Context_AddNew,
            this.Context_Delete,
            this.Context_Sep2,
            this.Context_Clear,
            this.Context_ClearAll,
            this.Context_Sep_Clear,
            this.Context_Rename,
            this.Context_Sep_Visible,
            this.Context_DisplaySubMenu,
            this.Context_Arrange});
			this.Explorer_ContextMenu.Name = "Explorer_ContextMenu";
			this.Explorer_ContextMenu.Size = new System.Drawing.Size(133, 204);
			// 
			// Context_Properties
			// 
			this.Context_Properties.Name = "Context_Properties";
			this.Context_Properties.Size = new System.Drawing.Size(132, 22);
			this.Context_Properties.Text = "&Properties";
			this.Context_Properties.Click += new System.EventHandler(this.Context_Properties_Click);
			// 
			// Context_Sep1
			// 
			this.Context_Sep1.Name = "Context_Sep1";
			this.Context_Sep1.Size = new System.Drawing.Size(129, 6);
			// 
			// Context_AddNew
			// 
			this.Context_AddNew.Image = global::Editor_Template.Properties.Resources.add;
			this.Context_AddNew.Name = "Context_AddNew";
			this.Context_AddNew.Size = new System.Drawing.Size(132, 22);
			this.Context_AddNew.Text = "Add Ne&w...";
			this.Context_AddNew.Click += new System.EventHandler(this.Context_AddNew_Click);
			// 
			// Context_Delete
			// 
			this.Context_Delete.Image = global::Editor_Template.Properties.Resources.delete;
			this.Context_Delete.Name = "Context_Delete";
			this.Context_Delete.Size = new System.Drawing.Size(132, 22);
			this.Context_Delete.Text = "Dele&te";
			this.Context_Delete.Click += new System.EventHandler(this.Context_Delete_Click);
			// 
			// Context_Sep2
			// 
			this.Context_Sep2.Name = "Context_Sep2";
			this.Context_Sep2.Size = new System.Drawing.Size(129, 6);
			// 
			// Context_Clear
			// 
			this.Context_Clear.Image = global::Editor_Template.Properties.Resources.clear;
			this.Context_Clear.Name = "Context_Clear";
			this.Context_Clear.Size = new System.Drawing.Size(132, 22);
			this.Context_Clear.Text = "&Clear";
			this.Context_Clear.Click += new System.EventHandler(this.Context_Clear_Click);
			// 
			// Context_ClearAll
			// 
			this.Context_ClearAll.Image = global::Editor_Template.Properties.Resources.clear;
			this.Context_ClearAll.Name = "Context_ClearAll";
			this.Context_ClearAll.Size = new System.Drawing.Size(132, 22);
			this.Context_ClearAll.Text = "C&lear All";
			this.Context_ClearAll.Click += new System.EventHandler(this.Context_ClearAll_Click);
			// 
			// Context_Sep_Clear
			// 
			this.Context_Sep_Clear.Name = "Context_Sep_Clear";
			this.Context_Sep_Clear.Size = new System.Drawing.Size(129, 6);
			// 
			// Context_Rename
			// 
			this.Context_Rename.Image = global::Editor_Template.Properties.Resources.rename;
			this.Context_Rename.Name = "Context_Rename";
			this.Context_Rename.Size = new System.Drawing.Size(132, 22);
			this.Context_Rename.Text = "&Rename";
			this.Context_Rename.Click += new System.EventHandler(this.Context_Rename_Click);
			// 
			// Context_Sep_Visible
			// 
			this.Context_Sep_Visible.Name = "Context_Sep_Visible";
			this.Context_Sep_Visible.Size = new System.Drawing.Size(129, 6);
			// 
			// Context_DisplaySubMenu
			// 
			this.Context_DisplaySubMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Hide,
            this.Context_HideOthers,
            this.Context_Show,
            this.Context_ShowAll,
            this.Context_Display_Sep1,
            this.Context_Enable,
            this.Context_Disable,
            this.Context_Display_Sep2,
            this.Context_Lock,
            this.Context_Unlock,
            this.Context_Display_Sep3,
            this.Context_Include,
            this.Context_Exclude});
			this.Context_DisplaySubMenu.Name = "Context_DisplaySubMenu";
			this.Context_DisplaySubMenu.Size = new System.Drawing.Size(132, 22);
			this.Context_DisplaySubMenu.Text = "&Display";
			// 
			// Context_Hide
			// 
			this.Context_Hide.Name = "Context_Hide";
			this.Context_Hide.Size = new System.Drawing.Size(201, 22);
			this.Context_Hide.Text = "&Hide Item(s)";
			this.Context_Hide.Click += new System.EventHandler(this.Context_Hide_Click);
			// 
			// Context_HideOthers
			// 
			this.Context_HideOthers.Name = "Context_HideOthers";
			this.Context_HideOthers.Size = new System.Drawing.Size(201, 22);
			this.Context_HideOthers.Text = "Hide &All Other Items";
			this.Context_HideOthers.Click += new System.EventHandler(this.Context_HideOthers_Click);
			// 
			// Context_Show
			// 
			this.Context_Show.Name = "Context_Show";
			this.Context_Show.Size = new System.Drawing.Size(201, 22);
			this.Context_Show.Text = "&Show Item(s)";
			this.Context_Show.Click += new System.EventHandler(this.Context_Show_Click);
			// 
			// Context_ShowAll
			// 
			this.Context_ShowAll.Name = "Context_ShowAll";
			this.Context_ShowAll.Size = new System.Drawing.Size(201, 22);
			this.Context_ShowAll.Text = "Sho&w All Item(s)";
			this.Context_ShowAll.Click += new System.EventHandler(this.Context_ShowAll_Click);
			// 
			// Context_Display_Sep1
			// 
			this.Context_Display_Sep1.Name = "Context_Display_Sep1";
			this.Context_Display_Sep1.Size = new System.Drawing.Size(198, 6);
			// 
			// Context_Enable
			// 
			this.Context_Enable.Name = "Context_Enable";
			this.Context_Enable.Size = new System.Drawing.Size(201, 22);
			this.Context_Enable.Text = "&Enable Item(s)";
			this.Context_Enable.Click += new System.EventHandler(this.Context_Enable_Click);
			// 
			// Context_Disable
			// 
			this.Context_Disable.Name = "Context_Disable";
			this.Context_Disable.Size = new System.Drawing.Size(201, 22);
			this.Context_Disable.Text = "&Disable Item(s)";
			this.Context_Disable.Click += new System.EventHandler(this.Context_Disable_Click);
			// 
			// Context_Display_Sep2
			// 
			this.Context_Display_Sep2.Name = "Context_Display_Sep2";
			this.Context_Display_Sep2.Size = new System.Drawing.Size(198, 6);
			// 
			// Context_Lock
			// 
			this.Context_Lock.Name = "Context_Lock";
			this.Context_Lock.Size = new System.Drawing.Size(201, 22);
			this.Context_Lock.Text = "&Lock Item(s)";
			this.Context_Lock.Click += new System.EventHandler(this.Context_Lock_Click);
			// 
			// Context_Unlock
			// 
			this.Context_Unlock.Name = "Context_Unlock";
			this.Context_Unlock.Size = new System.Drawing.Size(201, 22);
			this.Context_Unlock.Text = "&Unlock Item(s)";
			this.Context_Unlock.Click += new System.EventHandler(this.Context_Unlock_Click);
			// 
			// Context_Display_Sep3
			// 
			this.Context_Display_Sep3.Name = "Context_Display_Sep3";
			this.Context_Display_Sep3.Size = new System.Drawing.Size(198, 6);
			// 
			// Context_Include
			// 
			this.Context_Include.Name = "Context_Include";
			this.Context_Include.Size = new System.Drawing.Size(201, 22);
			this.Context_Include.Text = "&Include Item(s)";
			this.Context_Include.Click += new System.EventHandler(this.Context_Include_Click);
			// 
			// Context_Exclude
			// 
			this.Context_Exclude.Name = "Context_Exclude";
			this.Context_Exclude.Size = new System.Drawing.Size(201, 22);
			this.Context_Exclude.Text = "E&xclude Item(s)";
			this.Context_Exclude.Click += new System.EventHandler(this.Context_Exclude_Click);
			// 
			// Context_Arrange
			// 
			this.Context_Arrange.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Context_Group,
            this.Context_Ungroup,
            this.Context_Arrange_Sep1,
            this.Context_MoveToTop,
            this.Context_MoveUp,
            this.Context_MoveDown,
            this.Context_MoveToBottom});
			this.Context_Arrange.Name = "Context_Arrange";
			this.Context_Arrange.Size = new System.Drawing.Size(132, 22);
			this.Context_Arrange.Text = "&Arrange";
			// 
			// Context_Group
			// 
			this.Context_Group.Name = "Context_Group";
			this.Context_Group.Size = new System.Drawing.Size(181, 22);
			this.Context_Group.Text = "&Group Item(s)";
			this.Context_Group.Visible = false;
			this.Context_Group.Click += new System.EventHandler(this.Context_Group_Click);
			// 
			// Context_Ungroup
			// 
			this.Context_Ungroup.Name = "Context_Ungroup";
			this.Context_Ungroup.Size = new System.Drawing.Size(181, 22);
			this.Context_Ungroup.Text = "U&ngroup Item(s)";
			this.Context_Ungroup.Visible = false;
			this.Context_Ungroup.Click += new System.EventHandler(this.Context_Ungroup_Click);
			// 
			// Context_Arrange_Sep1
			// 
			this.Context_Arrange_Sep1.Name = "Context_Arrange_Sep1";
			this.Context_Arrange_Sep1.Size = new System.Drawing.Size(178, 6);
			this.Context_Arrange_Sep1.Visible = false;
			// 
			// Context_MoveToTop
			// 
			this.Context_MoveToTop.Name = "Context_MoveToTop";
			this.Context_MoveToTop.Size = new System.Drawing.Size(181, 22);
			this.Context_MoveToTop.Text = "Move to &Top";
			this.Context_MoveToTop.Click += new System.EventHandler(this.Context_MoveToTop_Click);
			// 
			// Context_MoveUp
			// 
			this.Context_MoveUp.Name = "Context_MoveUp";
			this.Context_MoveUp.Size = new System.Drawing.Size(181, 22);
			this.Context_MoveUp.Text = "Move U&p";
			this.Context_MoveUp.Click += new System.EventHandler(this.Context_MoveUp_Click);
			// 
			// Context_MoveDown
			// 
			this.Context_MoveDown.Name = "Context_MoveDown";
			this.Context_MoveDown.Size = new System.Drawing.Size(181, 22);
			this.Context_MoveDown.Text = "Move &Down";
			this.Context_MoveDown.Click += new System.EventHandler(this.Context_MoveDown_Click);
			// 
			// Context_MoveToBottom
			// 
			this.Context_MoveToBottom.Name = "Context_MoveToBottom";
			this.Context_MoveToBottom.Size = new System.Drawing.Size(181, 22);
			this.Context_MoveToBottom.Text = "Move to &Bottom";
			this.Context_MoveToBottom.Click += new System.EventHandler(this.Context_MoveToBottom_Click);
			// 
			// ToolStrip
			// 
			this.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TB_Group});
			this.ToolStrip.Location = new System.Drawing.Point(5, 5);
			this.ToolStrip.Name = "ToolStrip";
			this.ToolStrip.Size = new System.Drawing.Size(235, 25);
			this.ToolStrip.TabIndex = 1;
			this.ToolStrip.Text = "toolStrip1";
			this.ToolStrip.Visible = false;
			// 
			// TB_Group
			// 
			this.TB_Group.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.TB_Group.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.TB_Group.Name = "TB_Group";
			this.TB_Group.Size = new System.Drawing.Size(23, 22);
			this.TB_Group.Text = "toolStripButton1";
			// 
			// ItemTree
			// 
			this.ItemTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.ItemTree.ContextMenuStrip = this.Explorer_ContextMenu;
			this.ItemTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ItemTree.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ItemTree.FullRowSelect = true;
			this.ItemTree.HideSelection = false;
			this.ItemTree.ImageIndex = 0;
			this.ItemTree.ImageList = this.ilstIcons;
			this.ItemTree.LabelEdit = true;
			this.ItemTree.LineColor = System.Drawing.Color.SaddleBrown;
			this.ItemTree.Location = new System.Drawing.Point(3, 28);
			this.ItemTree.Name = "ItemTree";
			this.ItemTree.SelectedImageIndex = 0;
			this.ItemTree.ShowRootLines = false;
			this.ItemTree.Size = new System.Drawing.Size(698, 291);
			this.ItemTree.StateImageList = this.ilstState;
			this.ItemTree.TabIndex = 0;
			this.ItemTree.Scroll += new System.Windows.Forms.ScrollEventHandler(this.Tree_Scroll);
			this.ItemTree.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.Tree_AfterLabelEdit);
			this.ItemTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.Tree_AfterSelect);
			this.ItemTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tree_MouseDown);
			// 
			// ilstIcons
			// 
			this.ilstIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilstIcons.ImageSize = new System.Drawing.Size(16, 16);
			this.ilstIcons.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ilstState
			// 
			this.ilstState.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilstState.ImageSize = new System.Drawing.Size(8, 10);
			this.ilstState.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// tmrFocus
			// 
			this.tmrFocus.Enabled = true;
			this.tmrFocus.Interval = 500;
			this.tmrFocus.Tick += new System.EventHandler(this.Focus_Tick);
			// 
			// pnlShuffles
			// 
			this.pnlShuffles.Controls.Add(this.cboDropdown);
			this.pnlShuffles.Controls.Add(this.pnlSep4);
			this.pnlShuffles.Controls.Add(this.pnlSep3);
			this.pnlShuffles.Controls.Add(this.cmdAdd);
			this.pnlShuffles.Controls.Add(this.pnlSep2);
			this.pnlShuffles.Controls.Add(this.cmdEdit);
			this.pnlShuffles.Controls.Add(this.pnlSep1);
			this.pnlShuffles.Controls.Add(this.cmdDelete);
			this.pnlShuffles.Controls.Add(this.pnlBottomSpacer);
			this.pnlShuffles.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlShuffles.Location = new System.Drawing.Point(3, 3);
			this.pnlShuffles.Name = "pnlShuffles";
			this.pnlShuffles.Size = new System.Drawing.Size(698, 25);
			this.pnlShuffles.TabIndex = 3;
			// 
			// cboDropdown
			// 
			this.cboDropdown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cboDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDropdown.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDropdown.FormattingEnabled = true;
			this.cboDropdown.Location = new System.Drawing.Point(1, 0);
			this.cboDropdown.Name = "cboDropdown";
			this.cboDropdown.Size = new System.Drawing.Size(628, 21);
			this.cboDropdown.TabIndex = 3;
			this.cboDropdown.SelectedIndexChanged += new System.EventHandler(this.DropDown_SelectedIndexChanged);
			// 
			// pnlSep4
			// 
			this.pnlSep4.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlSep4.Location = new System.Drawing.Point(0, 0);
			this.pnlSep4.Name = "pnlSep4";
			this.pnlSep4.Size = new System.Drawing.Size(1, 21);
			this.pnlSep4.TabIndex = 9;
			// 
			// pnlSep3
			// 
			this.pnlSep3.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSep3.Location = new System.Drawing.Point(629, 0);
			this.pnlSep3.Name = "pnlSep3";
			this.pnlSep3.Size = new System.Drawing.Size(1, 21);
			this.pnlSep3.TabIndex = 9;
			// 
			// cmdAdd
			// 
			this.cmdAdd.Dock = System.Windows.Forms.DockStyle.Right;
			this.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdAdd.Image = global::Editor_Template.Properties.Resources.add;
			this.cmdAdd.Location = new System.Drawing.Point(630, 0);
			this.cmdAdd.Name = "cmdAdd";
			this.cmdAdd.Size = new System.Drawing.Size(22, 21);
			this.cmdAdd.TabIndex = 4;
			this.toolTip1.SetToolTip(this.cmdAdd, "Add new Sort Order");
			this.cmdAdd.UseVisualStyleBackColor = true;
			this.cmdAdd.Click += new System.EventHandler(this.Add_Click);
			// 
			// pnlSep2
			// 
			this.pnlSep2.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSep2.Location = new System.Drawing.Point(652, 0);
			this.pnlSep2.Name = "pnlSep2";
			this.pnlSep2.Size = new System.Drawing.Size(1, 21);
			this.pnlSep2.TabIndex = 9;
			// 
			// cmdEdit
			// 
			this.cmdEdit.Dock = System.Windows.Forms.DockStyle.Right;
			this.cmdEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdEdit.Image = global::Editor_Template.Properties.Resources.edit;
			this.cmdEdit.Location = new System.Drawing.Point(653, 0);
			this.cmdEdit.Name = "cmdEdit";
			this.cmdEdit.Size = new System.Drawing.Size(22, 21);
			this.cmdEdit.TabIndex = 5;
			this.toolTip1.SetToolTip(this.cmdEdit, "Edit Sort Order");
			this.cmdEdit.UseVisualStyleBackColor = true;
			this.cmdEdit.Click += new System.EventHandler(this.Edit_Click);
			// 
			// pnlSep1
			// 
			this.pnlSep1.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlSep1.Location = new System.Drawing.Point(675, 0);
			this.pnlSep1.Name = "pnlSep1";
			this.pnlSep1.Size = new System.Drawing.Size(1, 21);
			this.pnlSep1.TabIndex = 8;
			// 
			// cmdDelete
			// 
			this.cmdDelete.Dock = System.Windows.Forms.DockStyle.Right;
			this.cmdDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdDelete.Image = global::Editor_Template.Properties.Resources.delete;
			this.cmdDelete.Location = new System.Drawing.Point(676, 0);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(22, 21);
			this.cmdDelete.TabIndex = 6;
			this.toolTip1.SetToolTip(this.cmdDelete, "Delete Sort Order");
			this.cmdDelete.UseVisualStyleBackColor = true;
			this.cmdDelete.Click += new System.EventHandler(this.Delete_Click);
			// 
			// pnlBottomSpacer
			// 
			this.pnlBottomSpacer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottomSpacer.Location = new System.Drawing.Point(0, 21);
			this.pnlBottomSpacer.Name = "pnlBottomSpacer";
			this.pnlBottomSpacer.Size = new System.Drawing.Size(698, 4);
			this.pnlBottomSpacer.TabIndex = 10;
			// 
			// ExplorerTree
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(704, 322);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.ItemTree);
			this.Controls.Add(this.pnlShuffles);
			this.Controls.Add(this.ToolStrip);
			this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight;
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExplorerTree";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
			this.TabText = "Explorer";
			this.Text = "Explorer";
			this.DockStateChanged += new System.EventHandler(this.Form_DockStateChanged);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
			this.Shown += new System.EventHandler(this.Form_Shown);
			this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
			this.Explorer_ContextMenu.ResumeLayout(false);
			this.ToolStrip.ResumeLayout(false);
			this.ToolStrip.PerformLayout();
			this.pnlShuffles.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip ToolStrip;
		private System.Windows.Forms.ToolStripButton TB_Group;
		private KellyControls.MultiSelectTreeview ItemTree;
		private System.Windows.Forms.ImageList ilstState;
		private System.Windows.Forms.ImageList ilstIcons;
		private System.Windows.Forms.Timer tmrFocus;
		private System.Windows.Forms.Panel pnlShuffles;
		private System.Windows.Forms.Button cmdAdd;
		private System.Windows.Forms.ComboBox cboDropdown;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button cmdDelete;
		private System.Windows.Forms.Button cmdEdit;
		private System.Windows.Forms.Panel pnlSep4;
		private System.Windows.Forms.Panel pnlSep3;
		private System.Windows.Forms.Panel pnlSep2;
		private System.Windows.Forms.Panel pnlSep1;
		private System.Windows.Forms.Panel pnlBottomSpacer;
		internal System.Windows.Forms.ContextMenuStrip Explorer_ContextMenu;
		internal System.Windows.Forms.ToolStripMenuItem Context_Group;
		internal System.Windows.Forms.ToolStripMenuItem Context_Ungroup;
		internal System.Windows.Forms.ToolStripSeparator Context_Sep2;
		internal System.Windows.Forms.ToolStripMenuItem Context_Clear;
		internal System.Windows.Forms.ToolStripSeparator Context_Sep_Visible;
		internal System.Windows.Forms.ToolStripMenuItem Context_Hide;
		internal System.Windows.Forms.ToolStripMenuItem Context_Show;
		internal System.Windows.Forms.ToolStripMenuItem Context_ShowAll;
		internal System.Windows.Forms.ToolStripMenuItem Context_HideOthers;
		internal System.Windows.Forms.ToolStripMenuItem Context_Rename;
		internal System.Windows.Forms.ToolStripMenuItem Context_Lock;
		internal System.Windows.Forms.ToolStripMenuItem Context_Unlock;
		internal System.Windows.Forms.ToolStripSeparator Context_Sep_Clear;
		internal System.Windows.Forms.ToolStripMenuItem Context_MoveUp;
		internal System.Windows.Forms.ToolStripMenuItem Context_MoveDown;
		internal System.Windows.Forms.ToolStripMenuItem Context_AddNew;
		internal System.Windows.Forms.ToolStripMenuItem Context_Delete;
		internal System.Windows.Forms.ToolStripMenuItem Context_ClearAll;
		internal System.Windows.Forms.ToolStripMenuItem Context_MoveToTop;
		internal System.Windows.Forms.ToolStripMenuItem Context_MoveToBottom;
		internal System.Windows.Forms.ToolStripSeparator Context_Sep1;
		internal System.Windows.Forms.ToolStripMenuItem Context_Properties;
		internal System.Windows.Forms.ToolStripMenuItem Context_DisplaySubMenu;
		internal System.Windows.Forms.ToolStripMenuItem Context_Enable;
		internal System.Windows.Forms.ToolStripMenuItem Context_Disable;
		internal System.Windows.Forms.ToolStripMenuItem Context_Arrange;
		internal System.Windows.Forms.ToolStripSeparator Context_Arrange_Sep1;
		internal System.Windows.Forms.ToolStripSeparator Context_Display_Sep1;
		internal System.Windows.Forms.ToolStripSeparator Context_Display_Sep2;
		internal System.Windows.Forms.ToolStripSeparator Context_Display_Sep3;
		internal System.Windows.Forms.ToolStripMenuItem Context_Include;
		internal System.Windows.Forms.ToolStripMenuItem Context_Exclude;
	}
}