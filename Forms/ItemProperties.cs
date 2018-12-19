using System.Windows.Forms;

namespace Editor_Template.Forms
{
	public partial class ItemProperties : Form
	{
		private Core.Properties _properties;

		public ItemProperties()
		{
			InitializeComponent();
		}

		public Core.Properties Properties
		{
			get { return _properties; }
			set
			{
				_properties = value;
				this.propertyGrid1.SelectedObject = _properties;
			}
		}
	

	}
}
