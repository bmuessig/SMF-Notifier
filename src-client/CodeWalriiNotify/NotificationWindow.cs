using System;
using Gtk;

namespace CodeWalriiNotify
{
	public partial class NotificationWindow : Gtk.Window
	{
		MainWindow winMain;
		bool hasTimeout;
		bool restartTimeout;
		int targetX;

		public NotificationWindow(PostMeta Post, MainWindow MainWindow)
			: base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			winMain = MainWindow;

			var headerBackcolor = SettingsProvider.CurrentSettings.HeaderBackcolor;
			var titleForecolor = SettingsProvider.CurrentSettings.TitleForecolor;
			var bodyBackcolor = SettingsProvider.CurrentSettings.BodyBackcolor;
			var footerBackcolor = SettingsProvider.CurrentSettings.FooterBackcolor;
			var authorForecolor = SettingsProvider.CurrentSettings.AuthorForecolor;

			var titleFont = Pango.FontDescription.FromString(SettingsProvider.CurrentSettings.TitleFont);
			var detailFont = Pango.FontDescription.FromString(SettingsProvider.CurrentSettings.DetailFont);

			headerBox.ModifyBg(Gtk.StateType.Normal, headerBackcolor);
			topicLabel.ModifyFg(Gtk.StateType.Normal, titleForecolor);
			topicLabel.ModifyFont(titleFont);
			mainBox.ModifyBg(Gtk.StateType.Normal, bodyBackcolor);
			footerBox.ModifyBg(Gtk.StateType.Normal, footerBackcolor);
			actionBox.ModifyBg(Gtk.StateType.Normal, footerBackcolor);
			posterLabel.ModifyFg(Gtk.StateType.Normal, authorForecolor);
			posterLabel.ModifyFont(detailFont);

			topicLabel.Text = Post.Subject;
			posterLabel.Text = Post.Poster;

			this.KeepAbove = true;

			this.Move(this.Screen.Width + this.WidthRequest, (int)((this.Screen.Height * SettingsProvider.CurrentSettings.VisualNotifyVerticalAlignment) - this.HeightRequest));

			hasTimeout = false;
			restartTimeout = false;
		}

		public void ShowMe()
		{
			this.Show();
			Begin();
			StartTimeout();
			this.GrabFocus();
		}

		protected void OnViewButtonClicked(object sender, EventArgs e)
		{
			StopTimeout();
			winMain.Show();
			winMain.GrabFocus();
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
			GLib.Timeout.Add(SettingsProvider.CurrentSettings.VisualNotifyTimeout * 1000, new GLib.TimeoutHandler(() => {
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

			if (SettingsProvider.CurrentSettings.VisualNotifyDoAnimate) {
				GLib.Timeout.Add(SettingsProvider.CurrentSettings.VisualNotifyAnimationInterval, new GLib.TimeoutHandler(() => {
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

			if (SettingsProvider.CurrentSettings.VisualNotifyDoAnimate) {
				GLib.Timeout.Add(SettingsProvider.CurrentSettings.VisualNotifyAnimationInterval, new GLib.TimeoutHandler(() => {
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

