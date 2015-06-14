using System;
using Gtk;

namespace CodeWalriiNotify
{
	public partial class NotificationWindow : Window
	{
		MainWindow winMain;
		NotifierCore notifier;
		bool hasTimeout;
		bool restartTimeout;
		int targetX;

		public NotificationWindow(string Subject, string Subtitle, MainWindow MainWindow, NotifierCore Notifier)
			: base(WindowType.Popup)
		{
			this.Build();

			winMain = MainWindow;
			notifier = Notifier;

			SettingsData settings = SettingsProvider.CurrentSettings;

			var headerBackcolor = settings.Styles.HeaderBackcolor;
			var titleForecolor = settings.Styles.TitleForecolor;
			var bodyBackcolor = settings.Styles.BodyBackcolor;
			var footerBackcolor = settings.Styles.FooterBackcolor;
			var authorForecolor = settings.Styles.AuthorForecolor;

			var titleFont = Pango.FontDescription.FromString(settings.Styles.TitleFont);
			var detailFont = Pango.FontDescription.FromString(settings.Styles.DetailFont);

			headerBox.ModifyBg(StateType.Normal, headerBackcolor);
			subjectLabel.ModifyFg(StateType.Normal, titleForecolor);
			subjectLabel.ModifyFont(titleFont);
			mainBox.ModifyBg(StateType.Normal, bodyBackcolor);
			footerBox.ModifyBg(StateType.Normal, footerBackcolor);
			actionBox.ModifyBg(StateType.Normal, footerBackcolor);
			secondLabel.ModifyFg(StateType.Normal, authorForecolor);
			secondLabel.ModifyFont(detailFont);

			subjectLabel.Text = Subject;
			secondLabel.Text = Subtitle;

			this.KeepAbove = true;

			this.Move(this.Screen.Width + this.WidthRequest, (int)((this.Screen.Height - this.HeightRequest) * SettingsProvider.CurrentSettings.Notifications.VisualNotifyVerticalAlignment));

			hasTimeout = false;
			restartTimeout = false;
		}

		public void ShowMe()
		{
			this.Show();
			Begin();
			StartTimeout();
		}

		protected void OnViewButtonClicked(object sender, EventArgs e)
		{
			StopTimeout();
			winMain.ShowLatestPost();
			notifier.MarkPostsRead();
			this.Destroy();
		}

		protected void OnDismissButtonClicked(object sender, EventArgs e)
		{
			End();
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			StopTimeout();
			a.RetVal = true;
		}

		protected void OnMotionNotifyEvent(object o, Gtk.MotionNotifyEventArgs args)
		{
			RestartTimeout();
		}

		protected void StartTimeout()
		{
			hasTimeout = true;
			restartTimeout = false;
			GLib.Timeout.Add(SettingsProvider.CurrentSettings.Notifications.VisualNotifyTimeout * 1000, new GLib.TimeoutHandler(() => {
				if (!restartTimeout)
					End();
				else {
					restartTimeout = false;
					return true;
				}
				return hasTimeout;
			}));
		}

		protected void RestartTimeout()
		{
			restartTimeout = true;
		}

		protected void StopTimeout()
		{
			hasTimeout = false;
			restartTimeout = false;
		}

		protected void Begin()
		{
			targetX = this.Screen.Width - this.WidthRequest;

			if (SettingsProvider.CurrentSettings.Notifications.VisualNotifyDoAnimate) {
				GLib.Timeout.Add(SettingsProvider.CurrentSettings.Notifications.VisualNotifyAnimationInterval, new GLib.TimeoutHandler(() => {
					int x;
					int y;
					this.GetPosition(out x, out y);
					this.Move(x - 5, y);
					return (x - 5 > targetX);
				}));
			} else {
				int x;
				int y;
				this.GetPosition(out x, out y);
				this.Move(targetX, y);
			}
		}

		protected void End()
		{
			StopTimeout();

			targetX = this.Screen.Width + this.WidthRequest;

			if (SettingsProvider.CurrentSettings.Notifications.VisualNotifyDoAnimate) {
				GLib.Timeout.Add(SettingsProvider.CurrentSettings.Notifications.VisualNotifyAnimationInterval, new GLib.TimeoutHandler(() => {
					int x;
					int y;
					this.GetPosition(out x, out y);
					this.Move(x + 5, y);
					if (x + 5 >= targetX)
						this.Destroy();
					else
						return true;
					return false;
				}));
			} else {
				int x;
				int y;
				this.GetPosition(out x, out y);
				this.Move(targetX, y);
			}
		}
	}
}

