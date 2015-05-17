using System;
using Gtk;
using System.Web.Script.Serialization;
using System.Net;

namespace CodeWalriiNotify
{
	public partial class MainWindow: Window
	{
		public MainWindow()
			: base(WindowType.Toplevel)
		{
			Build();
		}

		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal = true;
		}

		protected void OnRefreshButtonClicked(object sender, EventArgs e)
		{
			RefreshPosts();
		}

		protected void RefreshPosts()
		{
			var wc = new WebClient();
			String json = wc.DownloadString("http://api.muessigb.net/walrus_notify.php");

			var serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

			dynamic obj = serializer.Deserialize(json, typeof(object));
		}
	}
}