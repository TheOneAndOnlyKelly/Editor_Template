using System.ComponentModel;
using System.Drawing;

namespace Editor_Template.Core
{
	/// <summary>
	/// This is a, example class that holds Properties for an object
	/// </summary>
	[DefaultProperty("Name")]
	public class Properties
	{

		#region [ Private Variables ]

		private bool _enabled;
		private bool _visible;
		private string _name;

		#endregion [ Private Variables ]

		#region [ Properties ]


		[Description("Name of the Item.")]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		[Category("Behavior"), Description("Indicates whether the Item is enabled.")]
		public bool Enabled
		{
			get { return _enabled; }
			set { _enabled = value; }
		}

		[Category("Behavior"), Description("Determines whether the Item is visible.")]
		public bool Visible
		{
			get { return _visible; }
			set { _visible = value; }
		}
		

		#endregion [ Properties ]

		#region [ Constructors ]

		/// <summary>        
		/// The default Constructor.        
		/// </summary>        
		public Properties()
		{
			_name = "[New Item]";
			_visible = true;
			_enabled = true;
		}
		

		#endregion [ Constructors ]

		#region [ Methods ]
		
		#endregion [ Methods ]
	}
}
