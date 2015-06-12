/*	NotifierCore.cs
 *  Processes the data async, 
 * 
 *  (c) 2015 Benedikt Müssig <muessigb.net>
 *  Licenced under the terms of the MIT Licence
 * 
 *  See https://github.com/muessigb/CodeWalrii-Notifier/
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;
using System.Threading;

namespace CodeWalriiNotify
{
	public class NotifierCore
	{
		// The main window
		private MainWindow mainWindow;
		// The custom recycler view
		private RecyclerView postsView;
		// The application settings
		private SettingsData settings;
		// The async thread
		private BackgroundWorker asyncThread;

		// The informations about the API
		private APIMeta apiInfo;

		// The visual notificator
		private Notificator notificator;
		// The timestamp of the last feed change
		private ulong lastChanged;
		// The time when the user last read the posts
		private DateTime lastPostTime;
		// The time of the last unread post
		private DateTime lastUnreadPostTime;

		// Expose if the timer is still alive
		public bool TimerAlive{ get; private set; }

		// Expose if the timer is running
		public bool TimerRunning { get; private set; }

		// Expose the current posts
		public PostMeta[] CurrentPosts { get; private set; }

		// Expose the new posts (if any)
		public PostMeta[] NewPosts { get; private set; }

		/// <summary>
		/// Occurs when the timer's running state changed.
		/// </summary>
		public event EventHandler<TimerRunningEventArgs> TimerRunningChanged;

		/// <summary>
		/// Occurs when a new posts arrived.
		/// </summary>
		public event EventHandler<PostsArrivedEventArgs> PostsArrived;

		/// <summary>
		/// Initializes a new instance of the <see cref="CodeWalriiNotify.NotifierCore"/> class.
		/// </summary>
		/// <param name="MainWindow">Main window</param>
		/// <param name="PostsView">Posts view</param>
		/// <param name="Settings">Application settings</param>
		public NotifierCore(MainWindow MainWindow, RecyclerView PostsView, SettingsData Settings)
		{
			mainWindow = MainWindow;
			postsView = PostsView;
			settings = Settings;

			asyncThread = new BackgroundWorker();
			asyncThread.DoWork += AsyncThread_DoWork;
			asyncThread.RunWorkerCompleted += AsyncThread_RunWorkerCompleted;

			lastChanged = 0;
			lastUnreadPostTime = DateTime.Now;
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
				mainWindow.Title = MyToolbox.BuildTitle(settings, (NewPosts != null ? (uint)NewPosts.Length : 0), "Post Notifier - Synchronizing... (Almost done, hold on!)");

				var result = (RefreshResult)e.Result;
				bool postRefreshSuccess = true;
				bool infoQuerySuccess = true;

				if (result.Opts.DoInfo)
					infoQuerySuccess = ThreadSafeInfoSync(result.APIQuery);
				if (result.Opts.DoPosts && infoQuerySuccess)
					postRefreshSuccess = ThreadSafePostSync(result.PostRefresh);

				if (infoQuerySuccess) {
					mainWindow.Title = postRefreshSuccess ? MyToolbox.BuildTitle(settings, (NewPosts != null ? (uint)NewPosts.Length : 0)) : MyToolbox.BuildTitle(settings, (NewPosts != null ? (uint)NewPosts.Length : 0), "Post Notifier - Refreshing failed!");
				} else {
					mainWindow.Title = MyToolbox.BuildTitle(settings, (NewPosts != null ? (uint)NewPosts.Length : 0), "Post Notifier - Invalid API, please check your settings!");
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

		/// <summary>
		/// Run this instance.
		/// </summary>
		public void Run()
		{
			RunTimer();
		}

		/// <summary>
		/// Pause this instance.
		/// </summary>
		public void Pause()
		{
			PauseTimer();
		}

		/// <summary>
		/// Shutdown this instance.
		/// </summary>
		public void Shutdown()
		{
			KillTimer();
		}

		/// <summary>
		/// Marks all the unread posts as read.
		/// </summary>
		public void MarkPostsRead()
		{
			if (NewPosts != null) {
				if (NewPosts.Length > 0) {
					lastPostTime = NewPosts[0].Time;
					NewPosts = new PostMeta[]{ };
				} else
					lastPostTime = lastUnreadPostTime;
			} else
				lastPostTime = lastUnreadPostTime;
		}

		/// <summary>
		/// The finilizing function for the API info query. <c>Must be called in sync</c>.
		/// </summary>
		/// <returns><c>true</c>, if safe info sync was threaded, <c>false</c> otherwise.</returns>
		/// <param name="Result">Result.</param>
		protected bool ThreadSafeInfoSync(APIQueryResult Result)
		{
			if (Result.Success) {
				apiInfo = Result.Meta;
				return true;
			} else {
				return false;
			}
		}

		/// <summary>
		/// The finalizing function for the posts refresh. <c>Must be called in sync</c>.
		/// </summary>
		/// <returns><c>true</c>, if the posts were refreshed, <c>false</c> otherwise.</returns>
		/// <param name="Result">Result.</param>
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
						NewPosts = Result.NewPosts;

						if (Result.NewPosts.Length > 0) {
							lastUnreadPostTime = Result.NewPosts[0].Time;
							notificator.NewPosts(Result.NewPosts);

							OnRaisePostsArrived(Result.NewPosts);
						}
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// Forcefully refreshes the posts.
		/// </summary>
		/// <returns><c>true</c>, if posts were refreshed successfully, <c>false</c> otherwise.</returns>
		public bool RefreshPosts()
		{
			if (asyncThread.IsBusy)
				return false;
			mainWindow.Title = MyToolbox.BuildTitle(settings, (NewPosts != null ? (uint)NewPosts.Length : 0), "Synchronizing...");
			asyncThread.RunWorkerAsync(new RefreshOpts(apiInfo == null, true, lastChanged, lastPostTime, settings));
			return true;
		}

		/// <summary>
		/// Retrieves informations about the API.
		/// </summary>
		/// <returns>The API query result.</returns>
		/// <param name="Settings">Application settings</param>
		/// <param name="MaxTries">Maximum retries</param>
		/// <param name="Sleep">Sleep delay on error</param>
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

		/// <summary>
		/// Retrieves the API info (helper for DoAPIInfoQuery)
		/// </summary>
		/// <returns>An exception or the feed info.</returns>
		/// <param name="Settings">Application settings</param>
		/// <param name="Success">Was query successful</param>
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

		/// <summary>
		/// Do the actual post refresh; should be called async.
		/// </summary>
		/// <returns>The result of the refresh.</returns>
		/// <param name="Settings">The application settings.</param>
		/// <param name="LastChanged">The unix timestamp of the last MD5 mismatch (aka. update)</param>
		/// <param name="LastPostTime">Last time a post was read.</param>
		protected PostRefreshResult DoPostsRefresh(SettingsData Settings, ulong LastChanged, DateTime LastPostTime)
		{
			try {
				String js = FeedRetriever.RetrieveFeedData(Settings); // We need to request data from the API

				var query = new APIQueryMeta(js); // The .ctor of the APIQueryMeta class will parse the raw Json

				if (query.Success) { // Was the query successful
					if (query.Changed > LastChanged) { // Was the last update behind the newest change
						var widgets = new List<Widget>(); // List for the custom GTK# post Widgets
						var newPosts = new List<PostMeta>(); // List of the new posts

						foreach (PostMeta post in query.Posts) { // Go through the posts
							var pw = new PostWidget(); // Create new PostWidget
							pw.Topic = post.Subject; // Set the conent
							pw.Body = post.Body;
							pw.Poster = post.Poster;
							pw.Time = post.Time.ToString();
							pw.URL = post.Link;
							widgets.Add(pw);

							if (post.Time > LastPostTime) // If the post was never than the last seen post...
								newPosts.Add(post); //... add it to the list of new posts
						}

						return new PostRefreshResult(query, newPosts.ToArray(), widgets); // Return successful result
					} else
						return new PostRefreshResult(true); // Return success, but no need to update anything
				} else
					return new PostRefreshResult(false); // Return generic failure
			} catch (Exception ex) {
				return new PostRefreshResult(ex); // Return possible exception when parsing and handling with the query
			}
		}

		/// <summary>
		/// Asyncronous Thread Refreshing Options Struct
		/// </summary>
		protected struct RefreshOpts
		{
			public bool DoInfo;
			// Retrieve API informations (mainly for version checking)
			public bool DoPosts;
			// Refresh the posts
			public ulong LastChanged;
			// Unix timestamp since last Update
			public DateTime LastPostTime;
			// Last time all messages were read (to find out the new ones)
			public SettingsData Settings;
			// Application settings

			public RefreshOpts(bool DoInfo, bool DoPosts, ulong LastChanged, DateTime LastPostTime, SettingsData Settings)
			{
				this.DoInfo = DoInfo;
				this.DoPosts = DoPosts;
				this.LastChanged = LastChanged;
				this.LastPostTime = LastPostTime;
				this.Settings = Settings;
			}
		}

		/// <summary>
		/// The result from the async thread. It will be used to transfer the result to the sync thread.
		/// </summary>
		protected struct RefreshResult
		{
			public RefreshOpts Opts;
			// The original refreshing options
			public APIQueryResult APIQuery;
			// The result of the API query (if any)
			public PostRefreshResult PostRefresh;
			// The result of the posts retrieval (if any)

			public RefreshResult(RefreshOpts Opts, APIQueryResult APIQuery, PostRefreshResult PostRefresh)
			{
				this.Opts = Opts;
				this.APIQuery = APIQuery;
				this.PostRefresh = PostRefresh;
			}
		}

		/// <summary>
		/// The result of an API query.
		/// </summary>
		protected struct APIQueryResult
		{
			public bool Success;
			// Was the query successful
			public uint Tries;
			// How many tries did it take to retrieve the data
			public Exception Exception;
			// The thrown exceptions, on failure
			public APIMeta Meta;
			// The retrieved data, on success

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

		/// <summary>
		/// The result of the post retrieval.
		/// </summary>
		protected struct PostRefreshResult
		{
			public bool Success;
			// Was the query successful
			public bool Refresh;
			// Do the widgets need to be refreshed
			public Exception Exception;
			// What was the exception (on failure)
			public List<Widget> Widgets;
			// The list of widgets (on refresh and success)
			public APIQueryMeta Meta;
			// The raw parsed APIQueryStruct from the API
			public PostMeta[] NewPosts;
			// The list of new posts (on success and refresh)

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

		/// <summary>
		/// The EventArgs for the TimerRunning event
		/// </summary>
		[Serializable]
		public sealed class TimerRunningEventArgs : EventArgs
		{
			public bool IsRunning { get; private set; }

			public TimerRunningEventArgs(bool IsRunning)
			{
				this.IsRunning = IsRunning;
			}
		}

		/// <summary>
		/// The EventArgs for the PostsArrived event
		/// </summary>
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

