using System;
using Gtk;
using System.Collections.Generic;

namespace CodeWalriiNotify
{
	public partial class MainWindow: Window
	{
		NotifierCore notifier;

		public MainWindow()
			: base(WindowType.Toplevel)
		{
			Build();
			this.Title = "Loading...";


			notifier = new NotifierCore(mainRecyclerview);
			this.Title = SettingsProvider.CurrentSettings.ApplicationTitle;
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void OnRefreshActionActivated(object sender, EventArgs e)
		{
			notifier.ForceRefresh();
		}

		protected void OnQuitActionActivated(object sender, EventArgs e)
		{
			this.Destroy();
			Environment.Exit(0);
		}
	}
}