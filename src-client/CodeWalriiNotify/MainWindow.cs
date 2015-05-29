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

		protected void RefreshPosts()
		{
			var fdr = new FeedRetriever("http://api.muessigb.net/smf_notifier.php");
			String js = fdr.RetrieveData("?html_stripmode=none");
			List<PostMeta> posts = PostMeta.FromJSON(js);

			mainRecyclerview.Clear();

			foreach (PostMeta post in posts) {
				var pw = new PostWidget();
				pw.Topic = post.Subject;
				pw.Body = post.Body;
				pw.Poster = post.Poster;
				pw.Time = post.Time.ToString();
				pw.URL = post.Link;
				mainRecyclerview.InsertTop(pw);
			}
		}

		protected void OnRefreshActionActivated(object sender, EventArgs e)
		{
			RefreshPosts();
		}

		protected void OnQuitActionActivated(object sender, EventArgs e)
		{
			this.Destroy();
			Environment.Exit(0);
		}
	}
}