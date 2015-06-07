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
		private DateTime lastPostTime;

		public bool TimerAlive{ get; private set; }

		public bool TimerRunning { get; private set; }

		public PostMeta[] CurrentPosts { get; private set; }

		public PostMeta[] NewPosts { get; private set; }

		public event EventHandler<TimerRunningEventArgs> TimerRunningChanged;
		public event EventHandler<PostsArrivedEventArgs> PostsArrived;

		public NotifierCore(MainWindow MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;

			lastChanged = 0;
			lastPostTime = DateTime.Now;

			notificator = new Notificator(Settings, MainWindow, this);

			TimerAlive = false;
		}

		protected void RunTimer()
		{
			if (TimerRunning && TimerAlive)
				return;
			if (!TimerAlive) {
				GLib.Timeout.Add(settings.QueryInterval * 1000, new GLib.TimeoutHandler(delegate {
					if (!TimerRunning)
						return true;
				
					RefreshPosts();
					return TimerAlive;
				}));
				TimerAlive = true;
			}

			TimerRunning = true;
			OnRaiseTimerRunningChanged(TimerRunning);
		}

		protected void KillTimer()
		{
			TimerRunning = false;
			TimerAlive = false;

			OnRaiseTimerRunningChanged(TimerRunning);
		}

		protected void PauseTimer()
		{
			TimerRunning = false;

			OnRaiseTimerRunningChanged(TimerRunning);
		}

		protected virtual void OnRaiseTimerRunningChanged(bool IsRunning)
		{
			EventHandler<TimerRunningEventArgs> handler = TimerRunningChanged;

			if (handler != null)
				handler(this, new TimerRunningEventArgs(IsRunning));
		}

		protected virtual void OnRaisePostsArrived(PostMeta[] Posts)
		{
			EventHandler<PostsArrivedEventArgs> handler = PostsArrived;

			if (handler != null)
				handler(this, new PostsArrivedEventArgs(Posts));
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
					PauseTimer();
				}
			});
		}

		void AsyncThread_DoWork(object sender, DoWorkEventArgs e)
		{
			var opts = (RefreshOpts)e.Argument;
			var apiResult = new APIQueryResult();
			var postResult = new PostRefreshResult();

			if (opts.DoInfo)
				apiResult = DoAPIInfoQuery(opts.Settings, 3, 500);
			if (opts.DoPosts && (!opts.DoInfo || apiResult.Success))
				postResult = DoPostsRefresh(opts.Settings, opts.LastChanged, opts.LastPostTime);

			e.Result = new RefreshResult(opts, apiResult, postResult);
		}

		public void Run()
		{
			RunTimer();
		}

		public void Pause()
		{
			PauseTimer();
		}

		public void Shutdown()
		{
			KillTimer();
		}

		public void MarkPostsRead()
		{
			if (NewPosts != null) {
				if (NewPosts.Length > 0) {
					lastPostTime = NewPosts[0].Time;
					NewPosts = new PostMeta[]{ };
				}
			}
		}

		protected bool ThreadSafeInfoSync(APIQueryResult Result)
		{
			if (Result.Success) {
				apiInfo = Result.Meta;
				return true;
			} else {
				return false;
			}
		}

		protected bool ThreadSafePostSync(PostRefreshResult Result)
		{
			if (Result.Success) {
				if (Result.Refresh) {
					CurrentPosts = Result.Meta.Posts;
					lastChanged = Result.Meta.Changed;

					// Add the UI widgets
					postsView.Clear();
					foreach (Widget widget in Result.Widgets)
						postsView.InsertFirst(widget);

					// Are there any new posts?
					if (Result.NewPosts != null) {
						if (Result.NewPosts.Length > 0) {
							//lastPostTime = Result.NewPosts[0].Time;
							notificator.NewPosts(Result.NewPosts);

							OnRaisePostsArrived(Result.NewPosts);
						}
					}
				}
				return true;
			}
			return false;
		}

		public bool RefreshPosts()
		{
			if (asyncThread.IsBusy)
				return false;
			mainWindow.Title = settings.FeedTitle + (settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier - Synchronizing...";
			asyncThread.RunWorkerAsync(new RefreshOpts(apiInfo == null, true, lastChanged, lastPostTime, settings));
			return true;
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

		protected PostRefreshResult DoPostsRefresh(SettingsData Settings, ulong LastChanged, DateTime LastPostTime)
		{
			try {
				String js = FeedRetriever.RetrieveFeedData(Settings); // We need to request data from the API

				var query = new APIQueryMeta(js);

				if (query.Success) {
					if (query.Changed > LastChanged) {
						var widgets = new List<Widget>();
						var newPosts = new List<PostMeta>();

						foreach (PostMeta post in query.Posts) {
							var pw = new PostWidget();
							pw.Topic = post.Subject;
							pw.Body = post.Body;
							pw.Poster = post.Poster;
							pw.Time = post.Time.ToString();
							pw.URL = post.Link;
							widgets.Add(pw);

							if (post.Time > LastPostTime)
								newPosts.Add(post);
						}

						return new PostRefreshResult(query, newPosts.ToArray(), widgets);
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
			public ulong LastChanged;
			public DateTime LastPostTime;
			public SettingsData Settings;

			public RefreshOpts(bool DoInfo, bool DoPosts, ulong LastChanged, DateTime LastPostTime, SettingsData Settings)
			{
				this.DoInfo = DoInfo;
				this.DoPosts = DoPosts;
				this.LastChanged = LastChanged;
				this.LastPostTime = LastPostTime;
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
			public APIQueryMeta Meta;
			public PostMeta[] NewPosts;

			public PostRefreshResult(bool Success)
			{
				this.Success = Success;
				this.Refresh = false;
				Exception = null;
				Meta = null;
				NewPosts = null;
				Widgets = null;
			}

			public PostRefreshResult(Exception Exception)
			{
				Success = false;
				Refresh = false;
				this.Exception = Exception;
				Meta = null;
				NewPosts = null;
				Widgets = null;
			}

			public PostRefreshResult(APIQueryMeta Meta, PostMeta[] NewPosts, List<Widget> Widgets)
			{
				Success = Meta != null;
				Refresh = Widgets != null;
				this.Meta = Meta;
				this.NewPosts = NewPosts;
				this.Widgets = Widgets;
				Exception = null;
			}
		}

		[Serializable]
		public sealed class TimerRunningEventArgs : EventArgs
		{
			public bool IsRunning { get; private set; }

			public TimerRunningEventArgs(bool IsRunning)
			{
				this.IsRunning = IsRunning;
			}
		}

		[Serializable]
		public sealed class PostsArrivedEventArgs : EventArgs
		{
			public PostMeta[] Posts { get; private set; }

			public PostsArrivedEventArgs(PostMeta[] Posts)
			{
				this.Posts = Posts;
			}
		}
	}
}

