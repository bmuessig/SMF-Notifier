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

			notifier = new NotifierCore(mainRecyclerview);

			// temporarily save it - to be removed later
			SettingsProvider.ToFile("lol.json");
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