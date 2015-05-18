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


			string bbcode = "hello world, lol this demo text [b]is[/b] so great! lets see [img]http://img.muessigb.net/demo.img[/img]";
			//List<BBTag> tags = BBTag.FromBBCode();
			List<IBBElement> elements = BBParser.ParseBBCode(bbcode);
			//var ab = new CairoHTMLRenderer();
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
			var fdr = new FeedRetriever("http://api.muessigb.net/walrus_notify.php");
			String js = fdr.RetrieveData("");
			List<PostMeta> posts = PostMeta.FromJSON(js);

			recyclerview1.Clear();

			foreach (PostMeta post in posts) {
				var pw = new PostWidget();
				pw.Topic = post.Subject;
				pw.Body = post.Body;
				pw.Poster = post.Poster;
				pw.Time = post.Time.ToString();
				recyclerview1.InsertTop(pw);
			}
		}
	}
}