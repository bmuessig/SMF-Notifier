using System;
using Gtk;
using System.Collections.Generic;
using System.Reflection;

namespace CodeWalriiNotify
{
	public partial class MainWindow: Window
	{
		NotifierCore notifier;

		Notificator nfc;

		public MainWindow()
			: base(WindowType.Toplevel)
		{
			Build();

			this.Icon = Stetic.IconLoader.LoadIcon(this, "gtk-execute", IconSize.Dnd);
			this.Title = "Loading...";

			while (Application.EventsPending())
				Application.RunIteration();

			string iconFileName = SettingsProvider.CurrentSettings.IconFile;
			this.Icon = SettingsProvider.CurrentSettings.UseCustomIcon ? new Gdk.Pixbuf(iconFileName) : Gdk.Pixbuf.LoadFromResource("Bell.png");

			notifier = new NotifierCore(this, mainRecyclerview, SettingsProvider.CurrentSettings);

			string feedTitle = SettingsProvider.CurrentSettings.FeedTitle;
			this.Title = feedTitle + (feedTitle.Length > 0 ? " " : "") + "Post Notifier";

			notifier.Run();
			notifier.ForceRefresh();

			nfc = new Notificator(SettingsProvider.CurrentSettings, this);
			nfc.NewPost(new PostMeta("Sample Subject", "Muessigb", "Lol lol loll lllolll", DateTime.Now, "http://google.de/"));
		}

		public void Shutdown()
		{
			notifier.Shutdown();
			this.Destroy();
			Application.Quit();
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Shutdown();
			a.RetVal = true;
		}

		protected void OnRefreshActionActivated(object sender, EventArgs e)
		{
			notifier.ForceRefresh();
			this.Hide();
		}

		protected void OnQuitActionActivated(object sender, EventArgs e)
		{
			Shutdown();
		}

		protected void OnPreferencesActionActivated(object sender, EventArgs e)
		{
			var dialog = new SettingsDialog(this);
			dialog.Show();
		}
	}
}