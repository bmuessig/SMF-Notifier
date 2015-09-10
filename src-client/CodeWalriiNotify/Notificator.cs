using System;
using System.Reflection;
using System.Media;
using Gtk;

namespace CodeWalriiNotify
{
	public class Notificator
	{
		readonly SettingsData settings;
		readonly MainWindow winMain;
		readonly NotifierCore notifier;

		public Notificator(SettingsData Settings, MainWindow MainWindow, NotifierCore Notifier)
		{
			settings = Settings;
			winMain = MainWindow;
			notifier = Notifier;
		}

		private bool IsMainWindowVisible {
			get {
				return winMain.Visible && winMain.HasToplevelFocus;
			}
		}

		public void NewPost(PostMeta Post)
		{
			if (settings.Notifications.VisualNotifyEnable && !IsMainWindowVisible) {
				var nwin = new NotificationWindow(Post.Subject, "by " + Post.Poster, winMain, notifier);
				nwin.ShowMe();
			}

			if (IsMainWindowVisible)
				winMain.ShowLatestPost();

			if (settings.Notifications.AudioNotifyEnable)
				PlayAudio();
		}

		public void NewPosts(PostMeta[] Posts)
		{
			if (Posts == null)
				return;
			if (Posts.Length == 0)
				return;
			if (Posts.Length == 1) {
				NewPost(Posts[0]);
				return;
			}
				
			if (settings.Notifications.VisualNotifyEnable && !IsMainWindowVisible) {
				var nwin = new NotificationWindow(Posts[Posts.Length - 1].Subject, string.Format("and {0} other new posts", (Posts.Length - 1)), winMain, notifier);
				nwin.ShowMe();
			}

			if (IsMainWindowVisible)
				winMain.ShowLatestPost();

			if (settings.Notifications.AudioNotifyEnable)
				PlayAudio();
		}

		protected void PlayAudio()
		{
			SoundPlayer player;
			if (settings.Notifications.AudioNotifyUseCustomAudio) {
				try {
					player = new SoundPlayer(settings.Notifications.AudioNotifyFile);
				} catch (Exception) {
					return;
				}
			} else
				player = new SoundPlayer(Assembly.GetExecutingAssembly().GetManifestResourceStream(
					"Chime.wav"
				));
			player.Play();
		}
	}
}

