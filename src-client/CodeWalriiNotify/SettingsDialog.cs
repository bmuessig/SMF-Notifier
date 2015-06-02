using System;
using System.Diagnostics;
using System.Reflection;

namespace CodeWalriiNotify
{
	public partial class SettingsDialog : Gtk.Dialog
	{
		MainWindow winMain;

		public SettingsDialog(MainWindow MainWindow)
		{
			this.Build();

			winMain = MainWindow;
			ReadSettings(SettingsProvider.CurrentSettings);
		}

		protected void ReadSettings(SettingsData CurrentSettings)
		{
			// General
			feedUrlTxt.Text = CurrentSettings.FeedURL;
			feedTitleTxt.Text = CurrentSettings.FeedTitle;
			iconFileSel.SetFilename(CurrentSettings.IconFile);

			// Query
			queryIntervalDec.Value = CurrentSettings.QueryInterval;
			maxPostsDec.Value = CurrentSettings.MaximumPosts;

			// Notification
			visualNotifyCb.Active = CurrentSettings.VisualNotify;
			audioNotifyCb.Active = CurrentSettings.AudioNotify;
			audioFileSel.SetFilename(CurrentSettings.AudioFile);

			// Colors
			headerBgColorBtn.Color = CurrentSettings.HeaderBackcolor;
			timeFgColorBtn.Color = CurrentSettings.TimestampForecolor;
			titleFgColorBtn.Color = CurrentSettings.TitleForecolor;
			bodyBgColorBtn.Color = CurrentSettings.BodyBackcolor;
			footerBgColorBtn.Color = CurrentSettings.FooterBackcolor;
			authorFgColorBtn.Color = CurrentSettings.AuthorForecolor;

			// Fonts
			titleFontBtn.SetFontName(CurrentSettings.TitleFont);
			detailFontBtn.SetFontName(CurrentSettings.DetailFont);
			bodyFormatTxt.Buffer.Text = CurrentSettings.BodyFormat;

			// Rendering
			bodyAntiAliasCb.Active = CurrentSettings.BodyUseAntiAlias;
		}

		protected void WriteSettings(SettingsData CurrentSettings)
		{
			// General
			CurrentSettings.FeedURL = feedUrlTxt.Text;
			CurrentSettings.FeedTitle = feedTitleTxt.Text;
			CurrentSettings.IconFile = iconFileSel.Filename;

			// Query
			CurrentSettings.QueryInterval = (uint)queryIntervalDec.Value;
			CurrentSettings.MaximumPosts = (byte)maxPostsDec.Value;

			// Notification
			CurrentSettings.VisualNotify = visualNotifyCb.Active;
			CurrentSettings.AudioNotify = audioNotifyCb.Active;
			CurrentSettings.AudioFile = audioFileSel.Filename;

			// Colors
			CurrentSettings.HeaderBackcolor = headerBgColorBtn.Color;
			CurrentSettings.TimestampForecolor = timeFgColorBtn.Color;
			CurrentSettings.TitleForecolor = titleFgColorBtn.Color;
			CurrentSettings.BodyBackcolor = bodyBgColorBtn.Color;
			CurrentSettings.FooterBackcolor = footerBgColorBtn.Color;
			CurrentSettings.AuthorForecolor = authorFgColorBtn.Color;

			// Fonts
			CurrentSettings.TitleFont = titleFontBtn.FontName;
			CurrentSettings.DetailFont = detailFontBtn.FontName;
			CurrentSettings.BodyFormat = bodyFormatTxt.Buffer.Text;

			// Rendering
			CurrentSettings.BodyUseAntiAlias = bodyAntiAliasCb.Active;
		}

		protected void OnOkButtonClicked(object sender, EventArgs e)
		{
			WriteSettings(SettingsProvider.CurrentSettings);
			SettingsProvider.ToFile(SettingsProvider.CurrentFilename);
			MessageBox.Show(
				"The Notifier needs to be restarted in order to apply the new Settings.",
				"Restart required",
				Gtk.MessageType.Info,
				Gtk.ButtonsType.Ok,
				new Gtk.ResponseHandler(delegate(object o, Gtk.ResponseArgs args) {
					this.Destroy();
					winMain.Shutdown();
					Process.Start(Assembly.GetExecutingAssembly().Location);
				})
			);
		}

		protected void OnCancelButtonClicked(object sender, EventArgs e)
		{
			this.Destroy();
		}

		protected void OnDefaultsButtonClicked(object sender, EventArgs e)
		{
			ReadSettings(new SettingsData());
		}
	}
}

