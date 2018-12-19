using System.Drawing;
using System.Windows.Forms;
using Editor_Template.Utilities;
using KellyControls.PlugInToolBtn.Interfaces;
using KellyControls.PlugInToolBtn.Tools;
using ImageHandler = Editor_Template.Singletons.ImageController;

namespace Editor_Template.Tools
{
	[Tool("Select")]
	public class Select
		: BaseTool, IPlugInTool
	{
		#region [ Constructors ]

		public Select()
			: base()
		{
			this.ID = (int)ToolID.Select;
			this.Name = "Select";
			this.ToolBoxImage = ImageHandler.Instance.GetBitmap(ImageType.Select);
			this.ToolBoxImageSelected = ImageHandler.Instance.GetBitmap(ImageType.Select_Selected);
			this.Cursor = Cursors.Arrow;
			this.DoesSelection = true;
		}

		#endregion [ Constructors ]


		#region [ Methods ]

		/// <summary>
		/// Create the Toolstrip control for this Tool
		/// </summary>
		protected override void CreateToolStrip()
		{
			_toolStrip = new ToolStrip
			{
				BackColor = Color.White,
				GripStyle = ToolStripGripStyle.Hidden,
				Name = "Select_ToolStrip"
			};

			_toolStrip.Items.AddRange(new ToolStripItem[] {
				new ToolStripLabel()
				{
					Name = "lblToolName",
					BackColor = SystemColors.ControlDark,
					Font = new Font("Segoe UI", 9F, FontStyle.Bold),
					Image = ImageHandler.Instance.GetBitmap(ImageType.Cursor),
					Size = new Size(102, 22),
					Text = "Select"
				}
			});
		}
		#endregion [ Methods ]
	}

}