using System;

namespace CodeWalriiNotify
{
	public partial class VersionDialog : Gtk.Dialog
	{
		public VersionDialog()
		{
			this.Build();

			appTitleLbl.ModifyFont(Pango.FontDescription.FromString("Sans Bold 25"));
			appTitleLbl.ModifyFg(Gtk.StateType.Normal, new Gdk.Color(85, 85, 85));
			appTitleLbl.Text = "CodeWalriiNotify";

			Pango.FontDescription tableHeaderFont = Pango.FontDescription.FromString("Sans Bold 10");

			versionLblLbl.ModifyFont(tableHeaderFont);
			dateLblLbl.ModifyFont(tableHeaderFont);
			copyrightLblLbl.ModifyFont(tableHeaderFont);

			versionLbl.Text = MyToolbox.GetVersionString();
			dateLbl.Text = MyToolbox.GetBuildDate().ToString();
			copyrightLbl.Text = @"Copyright © 2017 by Benedikt Müssig (bmuessig.eu)
Source code and binary form licenced under the terms of the MIT Licence.

Live long and prosper!";
		}

		protected void OnOkButtonClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}

