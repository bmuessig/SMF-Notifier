using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using System.Threading;

namespace CodeWalriiNotify
{
	public class NotifierCore
	{
		private MainWindow mainWindow;
		private RecyclerView postsView;
		private SettingsData settings;
		private BackgroundWorker asyncThread;

		private APIMeta apiInfo;

		private Notificator notificator;

		private ulong lastChanged;

		public NotifierCore(MainWindow MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;

			lastChanged = 0;
			TimerRunning = false;

			notificator = new Notificator(Settings, MainWindow);
		}

		public bool TimerRunning { get; private set; }

		protected void RunTimer()
		{
			TimerRunning = true;
			GLib.Timeout.Add(settings.QueryInterval * 1000, new GLib.TimeoutHandler(delegate {
				DoRefresh();
				return TimerRunning;
			}));
		}

		protected void StopTimer()
		{
			TimerRunning = false;
		}

		protected void AsyncThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			Application.Invoke(delegate {
				mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Synchronizing... (Almost done, hold on!)";

				var result = (RefreshResult)e.Result;
				bool postRefreshSuccess = true;
				bool infoQuerySuccess = true;

				if (result.Opts.DoInfo)
					infoQuerySuccess = ThreadSafeInfoSync(result.APIQuery);
				if (result.Opts.DoPosts && infoQuerySuccess)
					postRefreshSuccess = ThreadSafePostSync(result.PostRefresh);

				if (infoQuerySuccess) {
					if (postRefreshSuccess)
						mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier";
					else
						mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Refreshing failed!";
				} else {
					mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Invalid API, please check your settings!";
					StopTimer();
				}
			});
		}

		void AsyncThread_DoWork(object sender, DoWorkEventArgs e)
		{
			var opts = (RefreshOpts)e.Argument;
			var apiResult = new APIQueryResult();
			var postResult = new PostRefreshResult();

			if (opts.DoInfo)
				apiResult = DoAPIInfoQuery(opts.Settings);
			if (opts.DoPosts && (!opts.DoInfo || apiResult.Success))
				postResult = DoPostsRefresh(opts.Settings);

			e.Result = new RefreshResult(opts, apiResult, postResult);
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
			DoRefresh();
		}

		protected bool ThreadSafeInfoSync(APIQueryResult Result)
		{
			if (Result.Success) {
				apiInfo = Result.Meta;
				return true;
			} else {
				//SettingsDialog.PromptInvalidSetting("Invalid API!\nYou might want to change the settings!", (MainWindow)mainWindow);
				return false;
			}
		}

		protected bool ThreadSafePostSync(PostRefreshResult Result)
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

		protected void DoRefresh()
		{
			if (asyncThread.IsBusy)
				return;
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Synchronizing...";
			asyncThread.RunWorkerAsync(new RefreshOpts(apiInfo == null, true, settings));
		}

		protected APIQueryResult DoAPIInfoQuery(SettingsData Settings, uint MaxTries = 3, uint Sleep = 500)
		{
			byte repeat = 0;
			APIMeta apiMeta = null;
			Exception lastErr = null;

			while (apiMeta == null && repeat < MaxTries) {
				if (repeat > 0)
					Thread.Sleep((int)Sleep); // Wait a moment for things to settle
				bool success;
				object result = GetAPIInfo(Settings, out success); 

				if (success) {
					apiMeta = (APIMeta)result;
				} else {
					lastErr = (Exception)result;
				}

				repeat++;
			}

			return apiMeta != null ? new APIQueryResult(apiMeta, repeat) : new APIQueryResult(lastErr, repeat);
		}

		protected object GetAPIInfo(SettingsData Settings, out bool Success)
		{
			try {
				String js = FeedRetriever.RetrieveFeedInfo(Settings); // We need to request info from the API

				Success = true;
				return new APIMeta(js);
			} catch (Exception ex) {
				Success = false;
				return ex;
			}
		}

		protected PostRefreshResult DoPostsRefresh(SettingsData Settings)
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

						return new PostRefreshResult(widgets);
					} else
						return new PostRefreshResult(true);
				} else
					return new PostRefreshResult(false);
			} catch (Exception ex) {
				return new PostRefreshResult(ex);
			}
		}

		protected struct RefreshOpts
		{
			public bool DoInfo;
			public bool DoPosts;
			public SettingsData Settings;

			public RefreshOpts(bool DoInfo, bool DoPosts, SettingsData Settings)
			{
				this.DoInfo = DoInfo;
				this.DoPosts = DoPosts;
				this.Settings = Settings;
			}
		}

		protected struct RefreshResult
		{
			public RefreshOpts Opts;
			public APIQueryResult APIQuery;
			public PostRefreshResult PostRefresh;

			public RefreshResult(RefreshOpts Opts, APIQueryResult APIQuery, PostRefreshResult PostRefresh)
			{
				this.Opts = Opts;
				this.APIQuery = APIQuery;
				this.PostRefresh = PostRefresh;
			}
		}

		protected struct APIQueryResult
		{
			public bool Success;
			public uint Tries;
			public Exception Exception;
			public APIMeta Meta;

			public APIQueryResult(Exception Exception, uint Tries)
			{
				Success = false;
				this.Exception = Exception;
				this.Tries = Tries;
				Meta = null;
			}

			public APIQueryResult(APIMeta Meta, uint Tries)
			{
				Success = Meta != null;
				Exception = null;
				this.Tries = Tries;
				this.Meta = Meta;
			}
		}

		protected struct PostRefreshResult
		{
			public bool Success;
			public bool Refresh;
			public Exception Exception;
			public List<Widget> Widgets;

			public PostRefreshResult(bool Success)
			{
				this.Success = Success;
				this.Refresh = false;
				Exception = null;
				Widgets = null;
			}

			public PostRefreshResult(Exception Exception)
			{
				Success = false;
				Refresh = false;
				this.Exception = Exception;
				Widgets = null;
			}

			public PostRefreshResult(List<Widget> Widgets)
			{
				Success = true;
				Refresh = Widgets != null;
				this.Widgets = Widgets;
				Exception = null;
			}
		}
	}
}

