using System;
using Gtk;

namespace CodeWalriiNotify
{
	public static class MessageBox
	{
		public static void Show(string Message, string Title = "", MessageType Type = MessageType.Info, ButtonsType Buttons = ButtonsType.Ok, ResponseHandler Callback = null, Window Self = null)
		{
			var md = new MessageDialog(Self, DialogFlags.DestroyWithParent, Type, Buttons, Message);
			md.Title = Title;
			if (Callback != null)
				md.Response += Callback;
			md.Run();
			md.Destroy();
		}
	}
}
