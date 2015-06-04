using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;

namespace CodeWalriiNotify
{
	public class NotifierCore
	{
		private Window mainWindow;
		private RecyclerView postsView;
		private SettingsData settings;
		private BackgroundWorker asyncThread;

		private APIMeta apiInfo;

		private ulong lastChanged;

		public NotifierCore(Window MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;

			lastChanged = 0;
			TimerRunning = false;

			InitAPIAsync(Settings);
		}

		public bool TimerRunning { get; private set; }

		protected void RunTimer()
		{
			TimerRunning = true;
			GLib.Timeout.Add(settings.QueryInterval * 1000, new GLib.TimeoutHandler(delegate {
				DoRefreshAsync();
				return TimerRunning;
			}));
		}

		protected void StopTimer()
		{
			TimerRunning = false;
		}

		protected void AsyncThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Gtk.Application.Invoke(delegate {
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing... (Almost done, hold on!)";
				if (ThreadSafeSync((RefreshResult)e.Result))
					mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
				else
					mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing failed!";
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

		BackgroundWorker apiInitAsync;

		protected void InitAPIAsync(SettingsData Settings)
		{
			apiInitAsync = new BackgroundWorker();
			apiInitAsync.DoWork += delegate(object sender, DoWorkEventArgs e) {
				byte repeat = 0;
				APIMeta apiMeta = null;
				while (apiMeta == null && repeat < 3) {
					apiMeta = GetAPIInfo((SettingsData)e.Argument);
					repeat++;
				}

				e.Result = apiMeta;
			};
			apiInitAsync.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e) {
				var apiMeta = (APIMeta)e.Result;
				if (apiMeta != null) {
					Application.Invoke(delegate {
						apiInfo = apiMeta;
					});
				} else {
					Application.Invoke(delegate {
						SettingsDialog.PromptInvalidSetting("Invalid API!\nYou might want to change the settings!", (MainWindow)mainWindow);
					});
				}
			};
			apiInitAsync.RunWorkerAsync(Settings);
		}

		protected bool ThreadSafeSync(RefreshResult Result)
		{
			if (Result.Success) {
				if (Result.Refresh) {
					postsView.Clear();
					foreach (Widget widget in Result.Widgets)
						postsView.InsertFirst(widget);
				}
				return true;
			}
			return false;
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
			if (ThreadSafeSync(DoRefresh(settings)))
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
			else
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing failed!";
		}

		protected APIMeta GetAPIInfo(SettingsData Settings)
		{
			try {
				String js = FeedRetriever.RetrieveFeedInfo(Settings); // We need to request info from the API
				return new APIMeta(js);
			} catch (Exception) {
				return null;
			}
		}

		protected RefreshResult DoRefresh(SettingsData Settings)
		{
			try {
				String js = FeedRetriever.RetrieveFeedData(Settings); // We need to request data from the API

				var query = new APIQueryMeta(js);

				if (query.Success) {
					if (query.Changed > lastChanged) {
						var widgets = new List<Widget>();

						foreach (PostMeta post in query.Posts) {
							var pw = new PostWidget();
							pw.Topic = post.Subject;
							pw.Body = post.Body;
							pw.Poster = post.Poster;
							pw.Time = post.Time.ToString();
							pw.URL = post.Link;
							widgets.Add(pw);
						}

						lastChanged = query.Changed;

						return new RefreshResult(widgets);
					} else
						return new RefreshResult(true);
				} else
					return new RefreshResult(false);
			} catch (Exception ex) {
				return new RefreshResult(ex);
			}
		}

		protected struct RefreshResult
		{
			public bool Success;
			public bool Refresh;
			public Exception Exception;
			public List<Widget> Widgets;

			public RefreshResult(bool Success)
			{
				this.Success = Success;
				this.Refresh = false;
				Exception = null;
				Widgets = null;
			}

			public RefreshResult(Exception Exception)
			{
				Success = false;
				Refresh = false;
				this.Exception = Exception;
				Widgets = null;
			}

			public RefreshResult(List<Widget> Widgets)
			{
				Success = true;
				Refresh = Widgets != null;
				this.Widgets = Widgets;
				Exception = null;
			}
		}
	}
}

