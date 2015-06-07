﻿using System;
using Gtk;
using System.Collections.Generic;
using System.Reflection;

namespace CodeWalriiNotify
{
	public partial class MainWindow: Window
	{
		NotifierCore notifier;

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
			notifier.TimerRunningChanged += Notifier_TimerRunningChanged;

			string feedTitle = SettingsProvider.CurrentSettings.FeedTitle;
			this.Title = feedTitle + (feedTitle.Length > 0 ? " " : "") + "Post Notifier";

			notifier.Run();
			notifier.RefreshPosts();
		}

		protected void OnFocusInEvent(object o, FocusInEventArgs args)
		{
			if (notifier != null)
				notifier.MarkPostsRead();
		}

		protected void Notifier_TimerRunningChanged(object sender, NotifierCore.TimerRunningEventArgs e)
		{
			autoRefreshAction.Active = e.IsRunning;
		}

		public void ShowLatestPost()
		{
			this.Show();
			if (!this.Visible)
				this.Visible = true;
			this.GrabFocus();
			this.mainRecyclerview.ScrollUp();
		}

		public void Shutdown()
		{
			notifier.Shutdown();
			this.Destroy();
			Application.Quit();
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			a.RetVal = true;
			this.Iconify();
		}

		protected void OnRefreshActionActivated(object sender, EventArgs e)
		{
			notifier.RefreshPosts();
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

		protected void OnAutoRefreshActionActivated(object sender, EventArgs e)
		{
			if (autoRefreshAction.Active)
				notifier.Run();
			else
				notifier.Pause();
		}
	}
}