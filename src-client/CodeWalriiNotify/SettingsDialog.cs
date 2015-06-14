using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using Gtk;

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

			// Content
			SetComboboxEntries(ignTopicsComboEntry, GetReadableEntries(new List<IgnoredEntity>(CurrentSettings.IgnoredTopics)));
			SetComboboxEntries(ignUsersComboEntry, GetReadableEntries(new List<IgnoredEntity>(CurrentSettings.IgnoredUsers)));
			hideIgnoredPostsCb.Active = CurrentSettings.HideIgnoredPosts;

			// Notification
			visualNotifyEnabledCb.Active = CurrentSettings.VisualNotifyEnable;
			visualNotifyVerticalAlignmentSlide.Value = CurrentSettings.VisualNotifyVerticalAlignment;
			visualNotifyAnimationCb.Active = CurrentSettings.VisualNotifyDoAnimate;
			visualNotifyAnimationIntervalDec.Value = CurrentSettings.VisualNotifyAnimationInterval;
			visualNotifyTimeoutDec.Value = CurrentSettings.VisualNotifyTimeout;
			audioNotifyEnabledCb.Active = CurrentSettings.AudioNotifyEnable;
			audioNotifyCustomAudioCb.Active = CurrentSettings.AudioNotifyUseCustomAudio;
			audioNotifyFileSel.SetFilename(CurrentSettings.AudioNotifyFile);

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

			// Content
			CurrentSettings.IgnoredTopics = ParseEntries(GetComboboxEntries(ignTopicsComboEntry)).ToArray();
			CurrentSettings.IgnoredUsers = ParseEntries(GetComboboxEntries(ignUsersComboEntry)).ToArray();
			CurrentSettings.HideIgnoredPosts = hideIgnoredPostsCb.Active;

			// Notification
			CurrentSettings.VisualNotifyEnable = visualNotifyEnabledCb.Active;
			CurrentSettings.VisualNotifyVerticalAlignment = (float)visualNotifyVerticalAlignmentSlide.Value;
			CurrentSettings.VisualNotifyDoAnimate = visualNotifyAnimationCb.Active;
			CurrentSettings.VisualNotifyAnimationInterval = (uint)visualNotifyAnimationIntervalDec.ValueAsInt;
			CurrentSettings.VisualNotifyTimeout = (uint)visualNotifyTimeoutDec.ValueAsInt;
			CurrentSettings.AudioNotifyEnable = audioNotifyEnabledCb.Active;
			CurrentSettings.AudioNotifyUseCustomAudio = audioNotifyCustomAudioCb.Active;
			CurrentSettings.AudioNotifyFile = audioNotifyFileSel.Filename;

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
	}
}

