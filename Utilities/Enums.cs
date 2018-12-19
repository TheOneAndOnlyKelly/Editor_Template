using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor_Template.Utilities
{
	/// <summary>
	/// annotations enum indicates which small image can be added to another image to modify it, such as adding an Edit image to another image
	/// </summary>
	public enum Annotation
	{
		NotSet = -1,

		Add,
		As,
		Attribute,
		Branch,

		Calculate,
		Check,
		Clear,
		Compare,
		Copy,
		Create,

		Database,
		Delete,
		Delete_Small,
		Dislike,
		DontFind,
		Edit,
		Edit_State,
		Edit_Vertical,
		Export,
		File,
		Find,
		First,
		Flag,
		Flyout,
		From,

		Gear,
		Import,

		Last,
		Like,

		Max,
		Min,

		New_State,		
		Not,
		Not_Available,
		Note,
		Number,

		Open,

		Play,
		Document,

		Question,

		Record,
		Right,

		Stop,

		Text,
		To,
		Tool,

		Unused_State,
		Verify,
		View,

		Warning,
		Warning_State,
		Watch,
		Wrench		
	}

	public enum ConstrainDirection
	{
		NotSet,
		Horizontal,
		Vertical
	}

	/// <summary>
	/// Enumeration that holds a value for each image in the project Resources
	/// </summary>
	public enum ImageType
	{
		NotSet = -1,

		#region [ A ]

		Add,
		Asterisk,
		Attribute,

		#endregion [ A ]

		#region [ B ]

		Balloon,
		Batch,
		Bold,
		Bold_Selected,
		Branch,
		Bullet,

		#endregion [ B ]

		#region [ C ]

		Calculate,
		Calendar,
		Camera,
		Cancel,
		Case_Sensitive,
		Chart_Bar,
		Chart_Curve,
		Check,
		Check_Sql,
		Check_Xml,
		Clean,
		Clear,
		Clone,
		Close,		
		Cloud,
		Code,
		Color,
		Column,
		Combo,
		Compare,
		Complete,
		Copy,
		Create,
		CSharp,
		Cursor,
		Cut,

		#endregion [ C ]

		#region [ D ]

		Database,
		Delete,
		Description,
		Disline,

		#endregion [ D ]

		#region [ E ]

		Edit,
		Editor,
		Engine,
		Error,
		Exit,
		Export,

		#endregion [ E ]

		#region [ F ]

		Field,
		FieldValue,
		File,
		File_Open,
		Find,
		Flag,
		Flyout,
		Folder_Closed,
		Folder_Open,
		Font,
		Edit_Text,
		Font_Size,

		#endregion [ F ]

		#region [ G-L ]

		Grabbed,
		Group,

		Image,
		Image_Export,
		Image_Import,
		Image_Missing,
		Image_Resize,
		Import,
		Infinity,
		Info,
		Italic,
		Italic_Selected,

		JSON_Script,

		Key,
		Keyboard,

		Label,
		List,
		Lock,
		Lock_State,
		Log,

		#endregion [ G-L ]

		#region [ M ]

		Merge,
		Message,
		Move,
		Move_Down,
		Move_Selected,
		Move_To_Bottom,
		Move_To_Top,
		Move_Up,

		#endregion [ M ]

		#region [ N ]

		Next,
		Node,
		Not,

		#endregion [ N ]

		#region [ O ]

		Ok,
		Open,
		Option,
		Option_Checked,
		Output,

		#endregion [ O ]

		#region [ P ]

		Palette,
		Pan,
		Paste,
		PDF,
		PopUp,
		Previous,
		Print,
		Print_Preview,
		Print_Setup,
		Document,
		Properties,

		#endregion [ P ]

		#region [ Q-R ]

		Recent,
		Record,
		Redo,
		Refresh,
		Rename,
		Rotate_Left,
		Rotate_Right,
		Row,
		Ruler,
		Run,

		#endregion [ Q-R ]

		#region [ S ]

		Save,
		Save_All,
		Save_As,
		Save_Close,
		Score,
		Script,
		Section,
		Select,
		Select_All,
		Select_Selected,
		Settings,
		Snippet,
		Sort,
		Sync,

		#endregion [ S ]

		#region [ T ]

		Team,
		Text,
		Text_Tool,
		Text_Tool_Selected,
		Toggle_Guides,
		Tool,
		Tree,
		Tree_Collapse,
		Tree_Expand,

		#endregion [ T ]

		#region [ U-Z ]

		Undefined,
		Underline,
		Underline_Selected,
		Undo,
		Ungroup,
		Unlock,
		User,
		User_Blue,
		User_Gold,

		Version,
		Visible,

		Warning,

		Xml,

		Zoom,
		Zoom_Minus,
		Zoom_Original_Size,
		Zoom_Plus,
		Zoom_Selected,
		Zoom_To_Fit,

		#endregion [ U-Z ]

		FIN // represents the bottom of the list. do not add anything beyond this.
	}

	public enum ToolID : int
	{
		NotSet = 0,
		Select = 10,
		Move = 20,
		Text = 30,
		Zoom = 40,

		PlugInToolGroup = 1000,
		PlugIn = 2000
	}
}
