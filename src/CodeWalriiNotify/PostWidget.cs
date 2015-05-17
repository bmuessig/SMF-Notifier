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

			var titleFont = Pango.FontDescription.FromString("Tahoma 15.6");

			headerBox.ModifyBg(Gtk.StateType.Normal, headerBackcolor);
			titleLabel.ModifyFg(Gtk.StateType.Normal, titleForecolor);
			titleLabel.ModifyFont(titleFont);
			AuthorLabel.ModifyFg(Gtk.StateType.Normal, authorForecolor);
			bodyBox.ModifyBg(Gtk.StateType.Normal, bodyBackcolor);
			mainBox.ModifyBg(Gtk.StateType.Normal, bodyBackcolor);

			this.ShowAll();
		}
	}
}

