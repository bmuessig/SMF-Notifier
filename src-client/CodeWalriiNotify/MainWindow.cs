using System;
using Gtk;
using System.Collections.Generic;

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
			var fdr = new FeedRetriever("http://api.muessigb.net/walrus_notify.php", "http://codewalr.us/index.php?action=.xml");
			String js = fdr.RetrieveData("");
			List<PostMeta> posts = PostMeta.FromJSON(js);

			// some testing

			postwidget1.Topic = posts[0].Subject;
			postwidget1.Body = posts[0].Body;
			postwidget1.Poster = posts[0].Poster;
			postwidget1.Time = posts[0].Time.ToString();

			postwidget2.Topic = posts[1].Subject;
			postwidget2.Body = posts[1].Body;
			postwidget2.Poster = posts[1].Poster;
			postwidget2.Time = posts[1].Time.ToString();

			postwidget3.Topic = posts[2].Subject;
			postwidget3.Body = posts[2].Body;
			postwidget3.Poster = posts[2].Poster;
			postwidget3.Time = posts[2].Time.ToString();

		}
	}
}