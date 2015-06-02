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
		private bool timerRunning;

		public NotifierCore(Window MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;
		}

		protected void RunTimer()
		{
			GLib.Timeout.Add(settings.QueryInterval * 1000, new GLib.TimeoutHandler(delegate {
				DoRefreshAsync();
				return timerRunning;
			}));
		}

		protected void StopTimer()
		{
			timerRunning = false;
		}

		protected void AsyncThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Gtk.Application.Invoke(delegate {
				ThreadSafeSync(e.Result);
			});
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
			RunTimer();
		}

		public void Pause()
		{
			StopTimer();
		}

		public void Shutdown()
		{
			Pause();
		}

		public void ForceRefresh()
		{
			DoRefreshAsync();
		}

		protected void ThreadSafeSync(object result)
		{
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing... (Almost done, hold on!)";
			if (result.GetType() == typeof(List<Widget>)) {
				List <Widget> widgets = ((List<Widget>)result);
				postsView.Clear();
				foreach (Widget widget in widgets)
					postsView.InsertFirst(widget);
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
			} else
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing failed!";
		}

		protected void DoRefreshAsync()
		{
			if (asyncThread.IsBusy)
				return;
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing...";
			asyncThread.RunWorkerAsync(settings);
		}

		protected void DoRefreshSync()
		{
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing... (Synchronized; this can take a while!)";
			AsyncThread_RunWorkerCompleted(null, new RunWorkerCompletedEventArgs(DoRefresh(settings), null, false));
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
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

