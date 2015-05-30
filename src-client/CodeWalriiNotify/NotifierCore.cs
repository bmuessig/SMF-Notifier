using System;
using System.Collections.Generic;

namespace CodeWalriiNotify
{
	public class NotifierCore
	{
		private uint lastTimeStamp;
		private RecyclerView postsView;

		public NotifierCore(RecyclerView PostsView)
		{
			postsView = PostsView;
		}

		public void ForceRefresh()
		{
			RefreshPosts();
		}

		protected void RefreshPosts()
		{
			var fdr = new FeedRetriever(SettingsProvider.FeedURL);
			String js = fdr.RetrieveData("?html_stripmode=none");
			List<PostMeta> posts = PostMeta.FromJSON(js);

			postsView.Clear();

			foreach (PostMeta post in posts) {
				var pw = new PostWidget();
				pw.Topic = post.Subject;
				pw.Body = post.Body;
				pw.Poster = post.Poster;
				pw.Time = post.Time.ToString();
				pw.URL = post.Link;
				postsView.InsertFirst(pw);
			}
		}
	}
}

