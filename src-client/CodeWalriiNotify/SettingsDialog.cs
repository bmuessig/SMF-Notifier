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
			customIconCb.Active = CurrentSettings.UseCustomIcon;
			iconFileSel.SetFilename(CurrentSettings.IconFile);

			// Query
			queryIntervalDec.Value = CurrentSettings.QueryInterval;
			maxPostsDec.Value = CurrentSettings.MaximumPosts;

			// Notification
			visualNotifyEnabledCb.Active = CurrentSettings.VisualNotifyEnable;
			visualNotifyVerticalAlignmentSlide.Value = CurrentSettings.VisualNotifyVerticalAlignment;
			visualNotifyAnimationCb.Active = CurrentSettings.VisualNotifyDoAnimate;
			visualNotifyAnimationIntervalDec.Value = CurrentSettings.VisualNotifyAnimationInterval;
			visualNotifyTimeoutDec.Value = CurrentSettings.VisualNotifyTimeout;
			audioNotifyCb.Active = CurrentSettings.AudioNotifyEnable;
			customAudioCb.Active = CurrentSettings.AudioNotifyUseCustomAudio;
			audioFileSel.SetFilename(CurrentSettings.AudioNotifyFile);

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
			CurrentSettings.UseCustomIcon = customIconCb.Active;
			CurrentSettings.IconFile = iconFileSel.Filename;

			// Query
			CurrentSettings.QueryInterval = (uint)queryIntervalDec.ValueAsInt;
			CurrentSettings.MaximumPosts = (byte)maxPostsDec.ValueAsInt;

			// Notification
			CurrentSettings.VisualNotifyEnable = visualNotifyEnabledCb.Active;
			CurrentSettings.VisualNotifyVerticalAlignment = (float)visualNotifyVerticalAlignmentSlide.Value;
			CurrentSettings.VisualNotifyDoAnimate = visualNotifyAnimationCb.Active;
			CurrentSettings.VisualNotifyAnimationInterval = (uint)visualNotifyAnimationIntervalDec.ValueAsInt;
			CurrentSettings.VisualNotifyTimeout = (uint)visualNotifyTimeoutDec.ValueAsInt;
			CurrentSettings.AudioNotifyEnable = audioNotifyCb.Active;
			CurrentSettings.AudioNotifyUseCustomAudio = customAudioCb.Active;
			CurrentSettings.AudioNotifyFile = audioFileSel.Filename;

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
				"The notifier needs to be restarted in order to apply the new settings.",
				"Configuration change requires restart",
				Gtk.MessageType.Info,
				Gtk.ButtonsType.Ok,
				new Gtk.ResponseHandler((o, args) => Gtk.Application.Invoke(delegate {
					this.Destroy();
					winMain.Shutdown();
					Process.Start(Assembly.GetExecutingAssembly().Location);
				}))
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

		/*public static void PromptInvalidSetting(string SettingName, MainWindow MainWindow)
		{
			MainWindow.Hide();
			MessageBox.Show(
				String.Format("The {0} setting is invalid. Press OK to change it or Cancel to terminate the program.", SettingName),
				"Invalid Configuration", 
				Gtk.MessageType.Error,
				Gtk.ButtonsType.OkCancel,
				new Gtk.ResponseHandler(delegate(object o, Gtk.ResponseArgs args) {
					if (args.ResponseId == Gtk.ResponseType.Ok) {
						Gtk.Application.Invoke(delegate {
							using (var settings = new SettingsDialog(MainWindow)) {
								settings.Close += delegate {
									Gtk.Application.Invoke(delegate {
										MainWindow.Shutdown();
									});
								};
								settings.Show();
							}
						});
					} else {
						Gtk.Application.Invoke((object sender, EventArgs e) => MainWindow.Shutdown());
					}
				})
			);
		}*/
	}
}

