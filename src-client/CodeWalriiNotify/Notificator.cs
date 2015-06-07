using System;
using System.Reflection;
using System.Media;
using Gtk;

namespace CodeWalriiNotify
{
	public class Notificator
	{
		SettingsData settings;
		MainWindow winMain;

		StatusIcon notificationIcon;

		public Notificator(SettingsData Settings, MainWindow MainWindow)
		{
			settings = Settings;
			winMain = MainWindow;

			notificationIcon = Settings.UseCustomIcon ? new StatusIcon(Settings.IconFile) : new StatusIcon(Gdk.Pixbuf.LoadFromResource("Bell.png"));
			notificationIcon.Tooltip = Settings.FeedTitle + (Settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
			notificationIcon.Visible = false;
		}

		public void NewPost(PostMeta Post)
		{
			var nwin = new NotificationWindow(Post.Subject, "by " + Post.Poster, winMain);
			nwin.ShowMe();

			if (!winMain.HasFocus) {
				notificationIcon.Visible = true;
				notificationIcon.Blinking = true;
			}
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
				
			var nwin = new NotificationWindow(Posts[0].Subject, string.Format("and {0} other new Posts", (Posts.Length - 1).ToString()), winMain);
			nwin.ShowMe();
		}

		protected void PlayAudio()
		{
			SoundPlayer player;
			if (settings.AudioNotifyUseCustomAudio) {
				try {
					player = new SoundPlayer(settings.AudioNotifyFile);
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

