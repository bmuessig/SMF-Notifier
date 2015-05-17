using System;
using Gtk;

namespace CodeWalriiNotify
{
	public class MessageBox
	{
		public static void Show(String message, MessageType type, Window self = null)
		{
			var md = new MessageDialog(self, DialogFlags.DestroyWithParent, type, ButtonsType.Ok, message);
			md.Run();
			md.Destroy();
		}
	}
}

