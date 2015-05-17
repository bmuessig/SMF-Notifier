using System;
using Gdk;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PostWidget : Gtk.Bin
	{
		public PostWidget()
		{
			this.Build();

			var headerBackcolor = new Color(110, 180, 137);
			var dateForecolor = new Color(216, 216, 216);
			var authorForecolor = new Color(198, 198, 198);
			var titleForecolor = new Color(255, 255, 255);
			var bodyBackcolor = new Color(250, 250, 250);
			var bodyForecolor = new Color(0, 0, 0);

			headerContainer.ModifyBg(Gtk.StateType.Normal, headerBackcolor);
			this.ShowAll();
		}
	}
}

