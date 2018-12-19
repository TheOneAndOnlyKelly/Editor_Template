using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

/// <summary>
/// TreeNode class customized for the Channel Explorer Tree
/// </summary>
public class DEL_TreeNode : System.Windows.Forms.TreeNode
{
	#region [ Private Variables ]

	private bool _enabled = true;
	private bool _included = true;
	private object _storedObject = null;

	#endregion [ Private Variables ]

	#region [ Properties ]

	public bool Enabled
	{
		get { return _enabled; }
		set { _enabled = value; }
	}

	public bool Included
	{
		get { return _included; }
		set { _included = value; }
	}

	public object StoredObject
	{
		get { return _storedObject; }
		set { _storedObject = value; }
	}

	#endregion [ Properties ]

	#region [ Constructors ]

	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class.
	public TreeNode() : base() { }

	//
	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class with
	//     the specified label text.
	//
	// Parameters:
	//   text:
	//     The label System.Windows.Forms.TreeNode.Text of the new tree node.

	public TreeNode(string text)
		: base(text)
	{ }

	//
	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class using
	//     the specified serialization information and context.
	//
	// Parameters:
	//   serializationInfo:
	//     A System.Runtime.Serialization.SerializationInfo containing the data to deserialize
	//     the class.
	//
	//   context:
	//     The System.Runtime.Serialization.StreamingContext containing the source and
	//     destination of the serialized stream.
	protected TreeNode(SerializationInfo serializationInfo, StreamingContext context)
		: base(serializationInfo, context)
	{ }

	//
	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class with
	//     the specified label text and child tree nodes.
	//
	// Parameters:
	//   text:
	//     The label System.Windows.Forms.TreeNode.Text of the new tree node.
	//
	//   children:
	//     An array of child System.Windows.Forms.TreeNode objects.
	public TreeNode(string text, System.Windows.Forms.TreeNode[] children)
		: base(text, children)
	{ }

	//
	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class with
	//     the specified label text and images to display when the tree node is in a
	//     selected and unselected state.
	//
	// Parameters:
	//   text:
	//     The label System.Windows.Forms.TreeNode.Text of the new tree node.
	//
	//   imageIndex:
	//     The index value of System.Drawing.Image to display when the tree node is
	//     unselected.
	//
	//   selectedImageIndex:
	//     The index value of System.Drawing.Image to display when the tree node is
	//     selected.
	public TreeNode(string text, int imageIndex, int selectedImageIndex)
		: base(text, imageIndex, selectedImageIndex)
	{ }

	//
	// Summary:
	//     Initializes a new instance of the System.Windows.Forms.TreeNode class with
	//     the specified label text, child tree nodes, and images to display when the
	//     tree node is in a selected and unselected state.
	//
	// Parameters:
	//   text:
	//     The label System.Windows.Forms.TreeNode.Text of the new tree node.
	//
	//   imageIndex:
	//     The index value of System.Drawing.Image to display when the tree node is
	//     unselected.
	//
	//   selectedImageIndex:
	//     The index value of System.Drawing.Image to display when the tree node is
	//     selected.
	//
	//   children:
	//     An array of child System.Windows.Forms.TreeNode objects.
	public TreeNode(string text, int imageIndex, int selectedImageIndex, System.Windows.Forms.TreeNode[] children)
		: base(text, imageIndex, selectedImageIndex, children)
	{ }

	#endregion [ Constructors ]
}