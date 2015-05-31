using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;

namespace CodeWalriiNotify
{
	public class NotifierCore
	{
		//private uint lastTimeStamp;
		private Window mainWindow;
		private RecyclerView postsView;
		private SettingsData settings;
		private BackgroundWorker asyncThread;

		public NotifierCore(Window MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			/*
			GLib.Timeout.Add(settings.QueryInterval * 100, new GLib.TimeoutHandler(delegate {
				DoQuery();
				return true;
			}));
			*/

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;
		}

		void AsyncThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			mainWindow.Title = "Almost done!";
			lock (mainWindow) {
				lock (postsView) {
					if (e.Result.GetType() == typeof(List<Widget>)) {
						List <Widget> widgets = ((List<Widget>)e.Result);
						postsView.Clear();
						foreach (Widget widget in widgets) {
							postsView.InsertFirst(widget);
						}
						mainWindow.Title = "Done.";
					} else {
						mainWindow.Title = "Failed!";
					}
				}
			}
		}

		void AsyncThread_DoWork(object sender, DoWorkEventArgs e)
		{
			e.Result = DoRefresh((SettingsData)e.Argument);
		}

		protected void DoQuery()
		{
			ForceRefresh();
		}

		public void Run()
		{
			//queryTimer.Start();
		}

		public void Pause()
		{
			//queryTimer.Stop();
		}

		public void Shutdown()
		{
			Pause();
		}

		public void ForceRefresh()
		{
			DoRefreshAsync();
		}

		protected void DoRefreshAsync()
		{
			if (asyncThread.IsBusy)
				return;
			mainWindow.Title = "Refreshing...";
			asyncThread.RunWorkerAsync(settings);
		}

		protected object DoRefresh(SettingsData Settings)
		{
			try {
				var fdr = new FeedRetriever(Settings.FeedURL);
				String js = fdr.RetrieveData("?html_stripmode=none");
				List<PostMeta> posts = PostMeta.FromJSON(js);

				var widgets = new List<Widget>();

				foreach (PostMeta post in posts) {
					var pw = new PostWidget();
					pw.Topic = post.Subject;
					pw.Body = post.Body;
					pw.Poster = post.Poster;
					pw.Time = post.Time.ToString();
					pw.URL = post.Link;
					widgets.Add(pw);
				}

				return widgets;
			} catch (Exception ex) {
				return ex;
			}
		}
	}
}

