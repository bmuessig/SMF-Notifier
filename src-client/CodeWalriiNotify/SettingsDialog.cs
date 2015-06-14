using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Gtk;

namespace CodeWalriiNotify
{
	public partial class SettingsDialog : Dialog
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
			feedUrlTxt.Text = CurrentSettings.Query.FeedURL;
			feedTitleTxt.Text = CurrentSettings.General.FeedTitle;
			customIconCb.Active = CurrentSettings.General.UseCustomIcon;
			iconFileSel.SetFilename(CurrentSettings.General.IconFile);

			// Query
			queryIntervalDec.Value = CurrentSettings.Query.QueryInterval;
			maxPostsDec.Value = CurrentSettings.Query.MaximumPosts;

			// Content
			SetComboboxEntries(ignTopicsComboEntry, GetReadableEntries(new List<IgnoredEntity>(CurrentSettings.Content.IgnoredTopics)));
			SetComboboxEntries(ignUsersComboEntry, GetReadableEntries(new List<IgnoredEntity>(CurrentSettings.Content.IgnoredUsers)));
			hideIgnoredPostsCb.Active = CurrentSettings.Content.HideIgnoredPosts;
			minWordsDec.Value = CurrentSettings.Content.MinimumWordcount;

			// Notification
			visualNotifyEnabledCb.Active = CurrentSettings.Notifications.VisualNotifyEnable;
			visualNotifyVerticalAlignmentSlide.Value = CurrentSettings.Notifications.VisualNotifyVerticalAlignment;
			visualNotifyAnimationCb.Active = CurrentSettings.Notifications.VisualNotifyDoAnimate;
			visualNotifyAnimationIntervalDec.Value = CurrentSettings.Notifications.VisualNotifyAnimationInterval;
			visualNotifyTimeoutDec.Value = CurrentSettings.Notifications.VisualNotifyTimeout;
			audioNotifyEnabledCb.Active = CurrentSettings.Notifications.AudioNotifyEnable;
			audioNotifyCustomAudioCb.Active = CurrentSettings.Notifications.AudioNotifyUseCustomAudio;
			audioNotifyFileSel.SetFilename(CurrentSettings.Notifications.AudioNotifyFile);

			// Colors
			headerBgColorBtn.Color = CurrentSettings.Styles.HeaderBackcolor;
			timeFgColorBtn.Color = CurrentSettings.Styles.TimestampForecolor;
			titleFgColorBtn.Color = CurrentSettings.Styles.TitleForecolor;
			bodyBgColorBtn.Color = CurrentSettings.Styles.BodyBackcolor;
			footerBgColorBtn.Color = CurrentSettings.Styles.FooterBackcolor;
			authorFgColorBtn.Color = CurrentSettings.Styles.AuthorForecolor;

			// Fonts
			titleFontBtn.SetFontName(CurrentSettings.Styles.TitleFont);
			detailFontBtn.SetFontName(CurrentSettings.Styles.DetailFont);
			bodyFormatTxt.Buffer.Text = CurrentSettings.Styles.BodyFormat;

			// Rendering
			bodyAntiAliasCb.Active = CurrentSettings.Styles.BodyUseAntiAlias;
		}

		protected void WriteSettings(SettingsData CurrentSettings)
		{
			// General
			CurrentSettings.Query.FeedURL = feedUrlTxt.Text;
			CurrentSettings.General.FeedTitle = feedTitleTxt.Text;
			CurrentSettings.General.UseCustomIcon = customIconCb.Active;
			CurrentSettings.General.IconFile = iconFileSel.Filename;

			// Query
			CurrentSettings.Query.QueryInterval = (uint)queryIntervalDec.ValueAsInt;
			CurrentSettings.Query.MaximumPosts = (byte)maxPostsDec.ValueAsInt;

			// Content
			CurrentSettings.Content.IgnoredTopics = ParseEntries(GetComboboxEntries(ignTopicsComboEntry)).ToArray();
			CurrentSettings.Content.IgnoredUsers = ParseEntries(GetComboboxEntries(ignUsersComboEntry)).ToArray();
			CurrentSettings.Content.HideIgnoredPosts = hideIgnoredPostsCb.Active;
			CurrentSettings.Content.MinimumWordcount = (uint)minWordsDec.ValueAsInt;

			// Notification
			CurrentSettings.Notifications.VisualNotifyEnable = visualNotifyEnabledCb.Active;
			CurrentSettings.Notifications.VisualNotifyVerticalAlignment = (float)visualNotifyVerticalAlignmentSlide.Value;
			CurrentSettings.Notifications.VisualNotifyDoAnimate = visualNotifyAnimationCb.Active;
			CurrentSettings.Notifications.VisualNotifyAnimationInterval = (uint)visualNotifyAnimationIntervalDec.ValueAsInt;
			CurrentSettings.Notifications.VisualNotifyTimeout = (uint)visualNotifyTimeoutDec.ValueAsInt;
			CurrentSettings.Notifications.AudioNotifyEnable = audioNotifyEnabledCb.Active;
			CurrentSettings.Notifications.AudioNotifyUseCustomAudio = audioNotifyCustomAudioCb.Active;
			CurrentSettings.Notifications.AudioNotifyFile = audioNotifyFileSel.Filename;

			// Colors
			CurrentSettings.Styles.HeaderBackcolor = headerBgColorBtn.Color;
			CurrentSettings.Styles.TimestampForecolor = timeFgColorBtn.Color;
			CurrentSettings.Styles.TitleForecolor = titleFgColorBtn.Color;
			CurrentSettings.Styles.BodyBackcolor = bodyBgColorBtn.Color;
			CurrentSettings.Styles.FooterBackcolor = footerBgColorBtn.Color;
			CurrentSettings.Styles.AuthorForecolor = authorFgColorBtn.Color;

			// Fonts
			CurrentSettings.Styles.TitleFont = titleFontBtn.FontName;
			CurrentSettings.Styles.DetailFont = detailFontBtn.FontName;
			CurrentSettings.Styles.BodyFormat = bodyFormatTxt.Buffer.Text;

			// Rendering
			CurrentSettings.Styles.BodyUseAntiAlias = bodyAntiAliasCb.Active;
		}

		protected void OnOkButtonClicked(object sender, EventArgs e)
		{
			MessageBox.Show(
				"The notifier needs to be restarted in order to apply the new settings.\nDo you want to save and restart now?",
				"Configuration change requires restart",
				MessageType.Question,
				ButtonsType.OkCancel,
				new ResponseHandler((o, args) => Application.Invoke(delegate {
					if (args.ResponseId == ResponseType.Ok) {
						WriteSettings(SettingsProvider.CurrentSettings);
						SettingsProvider.ToFile(SettingsProvider.CurrentFilename);
						this.Destroy();
						winMain.Shutdown();
						Process.Start(Assembly.GetExecutingAssembly().Location);
					}
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

		public static void PromptInvalidSetting(string Message, MainWindow MainWindow)
		{
			MainWindow.Hide();
			MessageBox.Show(
				Message,
				"Invalid Configuration", 
				MessageType.Error,
				ButtonsType.OkCancel,
				new ResponseHandler(delegate(object o, ResponseArgs args) {
					if (args.ResponseId == ResponseType.Ok) {
						Application.Invoke(delegate {
							using (var settings = new SettingsDialog(MainWindow)) {
								settings.Close += delegate {
									Application.Invoke(delegate {
										MainWindow.Shutdown();
									});
								};
								settings.Show();
							}
						});
					} else {
						Application.Invoke((object sender, EventArgs e) => MainWindow.Shutdown());
					}
				})
			);
		}

		protected void OnIgnTopicAddBtnClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(ignTopicsComboEntry.Entry.Text))
				return;

			IgnoredEntity entity;
			if (!IgnoredEntity.TryParse(ignTopicsComboEntry.Entry.Text, out entity))
				return;
			if (GetComboboxEntries(ignTopicsComboEntry).Contains(entity.ToString()))
				return;

			ignTopicsComboEntry.AppendText(entity.ToString());
			ignTopicsComboEntry.Entry.Text = "";
		}

		protected void OnIgnTopicsRemoveBtnClicked(object sender, EventArgs e)
		{
			if (ignTopicsComboEntry.Active < 0)
				return;
			ignTopicsComboEntry.RemoveText(ignTopicsComboEntry.Active);
			ignTopicsComboEntry.Entry.Text = "";
		}

		protected void OnIgnUsersAddBtnClicked(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(ignUsersComboEntry.Entry.Text))
				return;

			IgnoredEntity entity;
			if (!IgnoredEntity.TryParse(ignUsersComboEntry.Entry.Text, out entity))
				return;
			if (GetComboboxEntries(ignUsersComboEntry).Contains(entity.ToString()))
				return;

			ignUsersComboEntry.AppendText(entity.ToString());
			ignUsersComboEntry.Entry.Text = "";
		}

		protected void OnIgnUsersRemoveBtnClicked(object sender, EventArgs e)
		{
			if (ignUsersComboEntry.Active < 0)
				return;
			ignUsersComboEntry.RemoveText(ignUsersComboEntry.Active);
			ignUsersComboEntry.Entry.Text = "";
		}

		protected List<IgnoredEntity> ParseEntries(List<string> RawEntries, bool SuppressErrors = true)
		{
			if (RawEntries == null) {
				if (SuppressErrors)
					return null;
				else
					throw new ArgumentNullException("RawEntries");
			}

			var entities = new List<IgnoredEntity>();

			foreach (string entry in RawEntries) {
				try {
					IgnoredEntity entity;
					if (IgnoredEntity.TryParse(entry, out entity))
						entities.Add(entity);
				} catch (Exception ex) {
					if (!SuppressErrors)
						throw ex;
				}
			}

			return entities;
		}

		protected List<string> GetReadableEntries(List<IgnoredEntity> Entities)
		{
			if (Entities == null)
				throw new ArgumentNullException("Entities");

			var entries = new List<string>();
			foreach (IgnoredEntity entity in Entities) {
				string entry = entity.ToString();
				if (!string.IsNullOrWhiteSpace(entry))
					entries.Add(entry);
			}

			return entries;
		}

		protected List<string> GetComboboxEntries(ComboBoxEntry EntryBox)
		{
			if (EntryBox == null)
				throw new ArgumentNullException("EntryBox");
			var listStore = EntryBox.Model as ListStore;
			return (listStore != null) ? (MyToolbox.ListStoreToList(listStore) ?? new List<string>()) : new List<string>();
		}

		protected void ClearComboboxEntries(ComboBoxEntry EntryBox)
		{
			if (EntryBox == null)
				throw new ArgumentNullException("EntryBox");
			EntryBox.Model = new ListStore(typeof(string));
			EntryBox.Entry.Text = "";
		}

		protected void SetComboboxEntries(ComboBoxEntry EntryBox, List<string> Entries)
		{
			if (EntryBox == null)
				throw new ArgumentNullException("EntryBox");
			if (Entries == null)
				throw new ArgumentNullException("Entries");
			ClearComboboxEntries(EntryBox);

			foreach (string entry in Entries) {
				EntryBox.AppendText(entry);
			}
		}

		protected void OnNotificationLoadDefaultsBtnClicked(object sender, EventArgs e)
		{
			var currentSettings = new SettingsData();
			var newSettings = new SettingsData();

			WriteSettings(currentSettings);

			newSettings.Content = currentSettings.Content;
			newSettings.General = currentSettings.General;
			newSettings.Query = currentSettings.Query;
			newSettings.Styles = currentSettings.Styles;

			ReadSettings(newSettings);
		}

		protected void OnContentLoadDefaultsBtnClicked(object sender, EventArgs e)
		{
			var currentSettings = new SettingsData();
			var newSettings = new SettingsData();

			WriteSettings(currentSettings);

			newSettings.Notifications = currentSettings.Notifications;
			newSettings.General = currentSettings.General;
			newSettings.Query = currentSettings.Query;
			newSettings.Styles = currentSettings.Styles;

			ReadSettings(newSettings);
		}

		protected void OnStylesLoadDefaultsBtnClicked(object sender, EventArgs e)
		{
			var currentSettings = new SettingsData();
			var newSettings = new SettingsData();

			WriteSettings(currentSettings);

			newSettings.Content = currentSettings.Content;
			newSettings.General = currentSettings.General;
			newSettings.Query = currentSettings.Query;
			newSettings.Notifications = currentSettings.Notifications;

			ReadSettings(newSettings);
		}

		protected void OnGeneralLoadDefaultsClicked(object sender, EventArgs e)
		{
			var currentSettings = new SettingsData();
			var newSettings = new SettingsData();

			WriteSettings(currentSettings);

			newSettings.Content = currentSettings.Content;
			newSettings.Notifications = currentSettings.Notifications;
			newSettings.Styles = currentSettings.Styles;

			ReadSettings(newSettings);
		}
	}
}

