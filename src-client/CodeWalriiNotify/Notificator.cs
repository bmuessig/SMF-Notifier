using System;
using System.Reflection;
using System.Media;
using Gtk;
using System.Drawing;
using System.IO;

namespace CodeWalriiNotify
{
	public class Notificator
	{
		private SettingsData settings;
		private MainWindow winMain;
		private NotifierCore notifier;
		private System.Drawing.Image icon;

		public Notificator(SettingsData Settings, MainWindow MainWindow, NotifierCore Notifier, System.Drawing.Image Icon)
		{
			settings = Settings;
			winMain = MainWindow;
			notifier = Notifier;
			icon = Icon;
		}

		private bool IsMainWindowVisible {
			get {
				return winMain.Visible && winMain.HasToplevelFocus;
			}
		}

		public void NewPost(PostMeta Post)
		{
			if (settings.Notifications.VisualNotifyEnable && !IsMainWindowVisible) {
				var nwin = new NotificationWindow(Post.Subject, "", winMain, notifier);
				nwin.ShowMe();
			}
				
			if (IsMainWindowVisible)
				winMain.ShowLatestPost();
			else
				UpdateIcon(1);

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
				var nwin = new NotificationWindow(Posts[0].Subject, string.Format("and {0} other new posts", (Posts.Length - 1)), winMain, notifier);
				nwin.ShowMe();
			}

			if (IsMainWindowVisible)
				winMain.ShowLatestPost();
			else
				UpdateIcon((uint)Posts.Length);

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

		public void UpdateIcon(uint UnreadCount)
		{
			Application.Invoke(delegate {
				winMain.Icon = GenerateIcon(UnreadCount);
			});
		}

		private Gdk.Pixbuf GenerateIcon(uint UnreadCount)
		{
			var bmp = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Color backcolor = MyToolbox.GdkToDrawingColor(SettingsProvider.CurrentSettings.Styles.IconBackcolor);
			System.Drawing.Color forecolor = MyToolbox.GdkToDrawingColor(SettingsProvider.CurrentSettings.Styles.IconForecolor);
			System.Drawing.Font font = MyToolbox.PangoToDrawingFont(Pango.FontDescription.FromString(SettingsProvider.CurrentSettings.Styles.IconFont));

			Graphics graphics = Graphics.FromImage(bmp);
			SizeF stringSize = graphics.MeasureString(UnreadCount.ToString(), font);

			graphics.DrawImage(icon, 0, 0, bmp.Width, bmp.Height);

			if (UnreadCount > 0 && SettingsProvider.CurrentSettings.Notifications.TaskbarIconUnreadCounterEnable) {
				Align align = SettingsProvider.CurrentSettings.Notifications.TaskbarIconUnreadCounterAlignment;
				int pos_x = ((((int)align & 0x1) > 0) ? (bmp.Width - (int)stringSize.Width) : 2);
				int pos_y = ((((int)align & 0x2) > 0) ? (bmp.Height - (int)stringSize.Height) : 2);
				graphics.FillRectangle(new SolidBrush(backcolor), pos_x - 2, pos_y - 2, stringSize.Width + 2, stringSize.Height + 2);
				graphics.DrawString(UnreadCount.ToString(), font, new SolidBrush(forecolor), pos_x - 1, pos_y - 1);
			}

			graphics.Dispose();

			return MyToolbox.ImageToPixbuf(bmp);
		}
	}
}
	