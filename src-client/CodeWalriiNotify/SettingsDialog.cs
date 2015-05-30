using System;

namespace CodeWalriiNotify
{
	public partial class SettingsDialog : Gtk.Dialog
	{
		public SettingsDialog()
		{
			this.Build();

			// General
			feedUrlTxt.Text = SettingsProvider.CurrentSettings.FeedURL;
			feedTitleTxt.Text = SettingsProvider.CurrentSettings.FeedTitle;

			// Query
			queryIntervalDec.Value = SettingsProvider.CurrentSettings.QueryInterval;
			maxPostsDec.Value = SettingsProvider.CurrentSettings.MaximumPosts;

			// Notification
			visualNotifyCb.Active = SettingsProvider.CurrentSettings.VisualNotify;
			audioNotifyCb.Active = SettingsProvider.CurrentSettings.AudioNotify;
			audioFileSel.SetFilename(SettingsProvider.CurrentSettings.AudioFile);

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

			// Rendering
			bodyAntiAliasCb.Active = SettingsProvider.CurrentSettings.BodyUseAntiAlias;
		}

		protected void OnCancelButtonClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}
	}
}

