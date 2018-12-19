using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CoreObjects.ListBoxControls;
using Editor_Template.Singletons;
using Editor_Template.Interfaces;
using WeifenLuo.WinFormsUI.Docking;

namespace Editor_Template.Forms
{
	public partial class ExplorerTree : DockContent
	{
		#region [ Constants ]

		private const string LOCKED = "LOCKED";
		private const string BACKGROUND = "BACKGROUND";
		private const string NOT_VISIBLE = "NOT_VISIBLE";
		private const string NOT_VISIBLE_LOCKED = "NOT_VISIBLE_LOCKED";

		#endregion [ Constants ]

		#region [ Private Variables ]

		private GlobalController _global = GlobalController.Instance;
		private bool _shown = false;
		private KellyControls.TreeNode _clickedNode = null;
		private ListBoxUtil _listBoxUtil = null;
		private SortedList<string, int> _scrollPositions = new SortedList<string, int>();
		private bool _disposed = false;

		/// <summary>
		/// Indicates whether select events should be listened to coming from the Controller
		/// When setting the items to be selected in this form, this flag should be set so that an infinite loop is
		/// not generated.
		/// </summary>
		private bool SuppressEvents = false;

		#endregion [ Private Variables ]

		#region [ Properties ]

		/// <summary>
		/// Link to the Editor form that owns this form.
		/// </summary>
		public Editor Editor { get; set; }

		/// <summary>
		/// Gets the Active Document
		/// </summary>
		private IDocument Document
		{
			get { return DocumentController.Instance.Active; }
		}

		#endregion [ Properties ]

		#region [ Constructors ]

		public ExplorerTree()
		{
			InitializeComponent();
			SuppressEvents = false;

			_listBoxUtil = new ListBoxUtil();

			cmdAdd.Enabled = false;
			cmdEdit.Enabled = false;
			cmdDelete.Enabled = false;
			cboDropdown.Enabled = false;
			ItemTree.Enabled = false;

			LoadIconsIntoListView();
			BuildMenuImages();
		}

		#endregion [ Constructors ]

		#region [ Methods ]

		/// <summary>
		/// Construct the custom images for the menu items
		/// </summary>
		private void BuildMenuImages()
		{
			
		}
		

		/// <summary>
		/// Populate the tree control with the Items.
		/// </summary>
		private void BuildTree()
		{
			BuildTree(false);
		}

		/// <summary>
		/// Populate the tree control with the Items. If rebuilding, then it should remember all expanded nodes
		/// </summary>
		/// <param name="rebuild">Indicates whether expanded nodes should be recalled.</param>
		public void BuildTree(bool rebuild)
		{
			_clickedNode = null;
			//KellyControls.TreeNode Node = null;
			var SelectedNodes = new List<KellyControls.TreeNode>();
			var ExpandedNodePaths = new List<string>();

			if (rebuild)
			{
				// Find all the paths of nodes that have their Expanded flags set to True.
				ExpandedNodePaths.AddRange(GetExpandedNodePaths(ItemTree.Nodes));
			}

			ItemTree.Nodes.Clear();
			FontStyle FontStyle = ItemTree.Font.Style;

			//...

			if (ItemTree.SelectedNodes.Count > 0)
				_clickedNode = ItemTree.SelectedNodes[0];

			ItemTree.SelectedNodes = SelectedNodes;

			if (rebuild)
			{
				foreach (string FullPath in ExpandedNodePaths)
				{
					var FoundNodes = ItemTree.Nodes.Find(FullPath, true);
					if (FoundNodes != null)
					{
						for (int i = 0; i < FoundNodes.Length; i++)
							FoundNodes[i].Expand();
					}
				}
			}

			//int Scroll = 0;
			//if (_scrollPositions.Keys.Contains(Document.GUID))
			//	Scroll = _scrollPositions[Document.GUID];
			//ExplorerTree.VScrollPos = Scroll;
		}

		///// <summary>
		///// Finds the KellyControls.TreeNode associated with this item.
		///// </summary>
		///// <param name="item">Item to search for in the tree control.</param>
		///// <param name="nodes">Collection of nodes to search.</param>
		///// <returns>Returns the KellyControls.TreeNode if found, null otherwise.</returns>
		//private KellyControls.TreeNode FindTreeNode(object item, TreeNodeCollection nodes)
		//{
		//	if (item == null)
		//		throw new ArgumentNullException("item is null.");
		//	if (nodes == null)
		//		return null;

		//	foreach (KellyControls.TreeNode Node in nodes)
		//	{
		//		if (Object.ReferenceEquals(Node.StoredObject, item))
		//			return Node;
		//		if (Node.Nodes != null)
		//		{
		//			KellyControls.TreeNode FoundIt = FindTreeNode(item, Node.Nodes);
		//			if (FoundIt != null)
		//				return FoundIt;
		//		}
		//	}
		//	return null;
		//}

		/// <summary>
		/// Loops through the collection, looking for the paths of all nodes that are expanded. If the node is expanded and has child nodes, then recursively navigate 
		/// into the nodes collection and gets that list as well.
		/// </summary>
		/// <param name="nodes">Collection of KellyControls.TreeNodes. Can be null.</param>
		/// <returns>List of paths to all KellyControls.TreeNodes that are expanded</returns>
		private List<string> GetExpandedNodePaths(TreeNodeCollection nodes)
		{
			var NodeList = new List<string>();

			if (nodes == null)
				return NodeList;

			foreach (TreeNode Node in nodes)
			{
				if (Node.IsExpanded)
					NodeList.Add(Node.FullPath);
				NodeList.AddRange(GetExpandedNodePaths(Node.Nodes));
			}

			return NodeList;
		}

		/// <summary>
		/// Load up the image list with the images stored in the Resources.
		/// </summary>
		private void LoadIconsIntoListView()
		{
			ilstIcons.ColorDepth = ColorDepth.Depth32Bit;
			//ilstState.ColorDepth = ColorDepth.Depth32Bit;

			// Build the list of State Icons
			//ilstState.Images.Add(LOCKED, ERes.lock_state);

			ilstIcons.Images.Clear();
			//ilstIcons.Images.Add(THING, ERes.thing);
		}

		/// <summary>
		/// Finds the node for the active item and begins the edit process on it.
		/// </summary>
		internal void Rename()
		{
			//TreeNode Node = FindTreeNode(Document.Items.Active, ExplorerTree.Nodes);
			//if (!_clickedNode.IsEditing)
			//{
			//	string Text = _clickedNode.Text;
			//	Node.Text = Text.Substring(Text.IndexOf(" ") + 1);
			//	Node.BeginEdit();
			//}
		}

		/// <summary>
		/// Inserts a new node at the position of the old one and deletes the old node.
		/// </summary>
		/// <param name="oldNode">The TreeNode object to replace</param>
		/// <param name="newNode">The new TreeNode object to add</param>
		private void ReplaceTreeNode(TreeNode oldNode, TreeNode newNode)
		{
			bool Expanded = oldNode.IsExpanded;
			bool ParentExpanded = oldNode.Parent.IsExpanded;
			var TreeView = oldNode.TreeView;

			oldNode.Parent.Nodes.Insert(oldNode.Index, newNode);
			oldNode.Parent.Nodes.Remove(oldNode);
			TreeView.SelectedNode = newNode;

			if (Expanded)
				newNode.Expand();
			if (ParentExpanded)
				newNode.Parent.Expand();
			else
				newNode.Parent.Collapse();

			TreeView = null;
		}
		

		/// <summary>
		/// Checks the item on the node specified to see if it is visible or not, updates the icon accordingly.
		/// </summary>
		private void UpdateIcon(KellyControls.TreeNode node)
		{ }

		#endregion [ Methods ]

		#region [ Events ]

		#region [ Event Triggers ]

		/// <summary>
		/// Occurs when the form is resized.
		/// </summary>
		private void OnResized()
		{
			if (!_shown)
				return;

			//_listBoxUtil.SetDropDownWidth(this.cboDropDown);

			ExplorerResized?.Invoke(this, new EventArgs());
		}

		#endregion [ Event Triggers ]

		#region [ Event Delegates ]

		#region [ Form Events ]

		/// <summary>
		/// Form is closing, clean up any objects and detach events.
		/// </summary>
		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{ }

		private void Form_DockStateChanged(object sender, EventArgs e)
		{
			if (!_disposed)
				OnResized();
		}

		private void Form_SizeChanged(object sender, EventArgs e)
		{
			if (!_disposed)
				OnResized();
		}

		private void Form_Shown(object sender, EventArgs e)
		{
			_shown = true;

			//Debug.WriteLine("Form_Shown" + "\t" + this.Width + "\t" + this.DockState.ToString());
		}

		#endregion [ Form Events ]

		#region [ Context Menu Events ]

		private void Context_AddNew_Click(object sender, EventArgs e)
		{ }

		private void Context_Properties_Click(object sender, EventArgs e)
		{ }

		private void Context_Clear_Click(object sender, EventArgs e)
		{ }

		private void Context_ClearAll_Click(object sender, EventArgs e)
		{ }

		private void Context_Delete_Click(object sender, EventArgs e)
		{ }

		private void Context_Disable_Click(object sender, EventArgs e)
		{ }

		private void Context_Enable_Click(object sender, EventArgs e)
		{ }

		private void Context_Exclude_Click(object sender, EventArgs e)
		{ }

		private void Context_Group_Click(object sender, EventArgs e)
		{ }

		private void Context_Hide_Click(object sender, EventArgs e)
		{ }

		private void Context_HideOthers_Click(object sender, EventArgs e)
		{ }

		private void Context_Include_Click(object sender, EventArgs e)
		{ }

		private void Context_Lock_Click(object sender, EventArgs e)
		{
			Editor.Item_Lock_Click(sender, e);
		}

		private void Context_MoveToBottom_Click(object sender, EventArgs e)
		{ }

		private void Context_MoveToTop_Click(object sender, EventArgs e)
		{ }

		private void Context_MoveDown_Click(object sender, EventArgs e)
		{ }

		private void Context_MoveUp_Click(object sender, EventArgs e)
		{ }

		private void Context_Rename_Click(object sender, EventArgs e)
		{ }

		private void Context_ChangeRendColor_Click(object sender, EventArgs e)
		{ }

		private void Context_ChangeSeqColor_Click(object sender, EventArgs e)
		{ }

		private void Context_Show_Click(object sender, EventArgs e)
		{ }

		private void Context_ShowAll_Click(object sender, EventArgs e)
		{ }

		private void Context_Ungroup_Click(object sender, EventArgs e)
		{ }

		private void Context_Unlock_Click(object sender, EventArgs e)
		{ }


		#endregion [ Context Menu Events ]

		#region [ TreeView events ]

		/// <summary>
		/// Event that fires after one or more TreeNodes have been selected. Set the IsSelected flag on the attached object
		/// </summary>
		private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			SuppressEvents = true;
			
			// Do stuff here

			SuppressEvents = false;
		}

		/// <summary>
		/// Event that fires after the edit of the KellyControls.TreeNode label is complete.
		/// </summary>
		private void Tree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{ }

		/// <summary>
		/// Record the position of the mouse down event.
		/// </summary>
		private void Tree_MouseDown(object sender, MouseEventArgs e)
		{
			_clickedNode = (KellyControls.TreeNode)ItemTree.GetNodeAt(e.X, e.Y);
		}

		/// <summary>
		/// Record the scroll position of the tree.
		/// </summary>
		private void Tree_Scroll(object sender, ScrollEventArgs e)
		{ }

		#endregion [ TreeView events ]

		/// <summary>
		/// Occurs when an UNDO or REDO has happened.
		/// </summary>
		private void Undo_Completed(object sender, EventArgs e)
		{
			BuildTree();
		}

		#region [ Sort Order Control Events ]

		/// <summary>
		/// Changes the currently selected Shuffle.
		/// </summary>
		private void DropDown_SelectedIndexChanged(object sender, EventArgs e)
		{ }

		/// <summary>
		/// Deletes the currently selected item.
		/// </summary>
		private void Delete_Click(object sender, EventArgs e)
		{ }

		/// <summary>
		/// Edits the currently selected item.
		/// </summary>
		private void Edit_Click(object sender, EventArgs e)
		{ }

		/// <summary>
		/// Adds a new item.
		/// </summary>
		private void Add_Click(object sender, EventArgs e)
		{ }

		#endregion [ Sort Order Control Events ]

		private void Focus_Tick(object sender, EventArgs e)
		{
			tmrFocus.Enabled = false;
			ItemTree.Focus();
		}

		#endregion [ Event Delegates ]

		#region [ Event Handlers ]

		public EventHandler ExplorerResized;

		#endregion [ Event Handlers ]

		#endregion [ Events ]

	}

}


