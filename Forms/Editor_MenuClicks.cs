using System;
using System.Windows.Forms;

namespace Editor_Template.Forms
{
	partial class Editor
	{
		#region [ Context Menu Events ]

		/// <summary>
		/// Adds a new Item to the Document.
		/// </summary>
		internal void Item_AddNew_Click(object sender, EventArgs e)
		{ }

		/// <summary>
		/// Deletes the currently selected Item(s) from the Document.
		/// </summary>
		internal void Item_Delete_Click(object sender, EventArgs e)
		{
			string Message = "Really delete {0}?";
			bool Multiple = false;  //	(Document.Items.Selected.Count > 1);
			if (Multiple)
				Message = string.Format(Message, "these Items");
			else
				Message = string.Format(Message, "this Item");
			if (MessageBox.Show(Message, "Delete Item", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
				return;

			// ...

			Document.Refresh();
			Document.SaveUndo("Delete Item" + (Multiple ? "s" : string.Empty));
		}

		/// <summary>
		/// Makes all Items visible
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		internal void Item_AllVisible_Click(object sender, EventArgs e)
		{
			//Document.Items.SetVisible(Document.Items.List, true);
			Document.Refresh();
		}

		/// <summary>
		/// Clear out the Things of all the Items
		/// </summary>
		internal void Item_Clear_AllTheThings_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Really clear the Things from ALL Items?", "Clear All Items", MessageBoxButtons.OKCancel, 
								MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
				return;

			//Document.Items.EmptyAllThings();
			Document.SaveUndo("Clear All The Things");
		}

		/// <summary>
		/// Clear out the Thing for the selected Item(s)
		/// </summary>
		internal void Item_Clear_Click(object sender, EventArgs e)
		{
			string PluralText = "Things"; //Document.Items.PluralText;
			string Title =  $"Clear {PluralText}";

			if (MessageBox.Show("Really clear the Things from " + ((Document.Items.Count == 1) ? string.Empty : "all ") + "the selected " + PluralText + "?", 
								Title, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
				return;

			//Document.Items.EmptyThings(Document.Items);
			Document.SaveUndo(Title);
		}

		/// <summary>
		/// Disables the selected Items.
		/// </summary>
		internal void Item_Disable_Click(object sender, EventArgs e)
		{
			string PluralText = "Things"; //Document.Items.PluralText;
			string Title = string.Format("Disable {0}", PluralText);
			if (MessageBox.Show(string.Format("Really disable the selected {0}?", PluralText), Title,
								MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
				return;
			//foreach (var SelectedItem in Document.Items.Selected)
			//	SelectedItem.Enabled = false;
			Document.SaveUndo(Title);
		}

		/// <summary>
		/// Set the selected Items to be Enabled.
		/// </summary>
		internal void Item_Enable_Click(object sender, EventArgs e)
		{
			string PluralText = "Things"; //Document.Items.PluralText;
			string Title = string.Format("Enable {0}", PluralText);
			//foreach (var SelectedItem in Document.Items.Selected)
			//	SelectedItem.Enabled = true;
			Document.SaveUndo(Title);
		}

		/// <summary>
		/// Excludes the selected Items
		/// </summary>
		internal void Item_Exclude_Click(object sender, EventArgs e)
		{
			string PluralText = "Things"; //Document.Items.PluralText;
			string Title = string.Format("Exclude {0}", PluralText);
			if (MessageBox.Show(string.Format("Really exclude the selected {0}?", PluralText), Title,
								MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.OK)
				return;
			//foreach (var Item in Document.Items.Selected)
				//Item.Included = false;
			Document.SaveUndo(Title);
		}

		/// <summary>
		/// Groups the currently selected Items together
		/// </summary>
		internal void Item_Group_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Sets the currently selected Items Visible property to false.
		/// </summary>
		internal void Item_Hide_Click(object sender, EventArgs e)
		{
			//Document.Items.SetVisible(Document.Items.Selected, false);
		}

		/// <summary>
		/// Sets the Visible property to false to all Items not currently selected.
		/// </summary>
		internal void Item_HideOthers_Click(object sender, EventArgs e)
		{
			//Document.Items.SetVisible(Document.Items.Unselected, false);
		}

		/// <summary>
		/// Import Item data from another Document
		/// </summary>
		internal void Item_Import_Click(object sender, EventArgs e)
		{
			if (Document == null)
				return;

			if (OpenDocumentDialog.ShowDialog() == DialogResult.Cancel)
				return;

			// ...

			//Document.SaveUndo("Import Stuff", string.Empty));
			Document.Refresh();
		}

		/// <summary>
		/// Includes the selected Items
		/// </summary>
		internal void Item_Include_Click(object sender, EventArgs e)
		{
			string Title = string.Format("Include {0}", "Things");//Document.Items.PluralText);
			//foreach (var SelectedItem in Document.Items.Selected)
				//SelectedItem.Included = true;
			Document.SaveUndo(Title);
		}
		

		/// <summary>
		/// Locks all the currently selected Items.
		/// </summary>
		internal void Item_Lock_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Moves the selected Items to the bottom of the current Order
		/// </summary>
		internal void Item_MoveToBottom_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Moves the selected Items to the top of the current Order
		/// </summary>
		internal void Item_MoveToTop_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Moves the selected Items down one position within the current Order
		/// </summary>
		internal void Item_MoveDown_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Moves the selected Items up one position within the current Order
		/// </summary>
		internal void Item_MoveUp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Allows the user to view and change Item properties directly.
		/// </summary>
		internal void Item_Properties_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Allows the user to rename the Active Item.
		/// </summary>
		internal void Item_Rename_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Saves the selected Item to a bitmap
		/// </summary>
		internal void Item_SaveToBitmap_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}
		

		/// <summary>
		/// Sets the currently selected Item Visible property to true.
		/// </summary>
		internal void Item_Show_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Sets all Item Visible property to true.
		/// </summary>
		internal void Item_ShowAll_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Ungroups all groups that are currently selected
		/// </summary>
		internal void Item_Ungroup_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		/// <summary>
		/// Unlocks all the currently selected Item.
		/// </summary>
		internal void Item_Unlock_Click(object sender, EventArgs e)
		{
			MessageBox.Show("NYI");
		}

		#endregion [ Context Menu Events ]
		
	}
}
