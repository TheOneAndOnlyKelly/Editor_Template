namespace Editor_Template.Utilities
{
	public class EventHandlers
	{
		public delegate void DirtyEventHandler(object sender, DirtyEventArgs e);
		public delegate void UndoEventHandler(object sender, UndoEventArgs e);
		public delegate void ZoomEventHandler(object sender, ZoomEventArgs e);
		public delegate void DocumentEventHandler(object sender, DocumentEventArgs e);
	}
}
