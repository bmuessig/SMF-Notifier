using System;

namespace CodeWalriiNotify
{
	public partial class SettingsDialog : Gtk.Dialog
	{
		public SettingsDialog()
		{
			this.Build();

			// Colors
			headerBgColorBtn.Color = SettingsProvider.CurrentSettings.HeaderBackcolor;
			timeFgColorBtn.Color = SettingsProvider.CurrentSettings.TimestampForecolor;
			titleFgColorBtn.Color = SettingsProvider.CurrentSettings.TitleForecolor;
			bodyBgColorBtn.Color = SettingsProvider.CurrentSettings.BodyBackcolor;
			footerBgColorBtn.Color = SettingsProvider.CurrentSettings.FooterBackcolor;
			authorFgColorBtn.Color = SettingsProvider.CurrentSettings.AuthorForecolor;

			// Fonts
			titleFontBtn.SetFontName(SettingsProvider.CurrentSettings.TitleFont);
			detailFontBtn.SetFontName(SettingsProvider.CurrentSettings.DetailFont);
			bodyFormatTxt.Buffer.Text = SettingsProvider.CurrentSettings.BodyFormat;

			bodyAntiAliasCb.Active = SettingsProvider.CurrentSettings.BodyUseAntiAlias;
		}
	}
}

