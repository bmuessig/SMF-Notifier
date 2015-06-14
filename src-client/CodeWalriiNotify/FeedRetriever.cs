using System;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace CodeWalriiNotify
{
	public static class FeedRetriever
	{
		public static string RetrieveFeedInfo(SettingsData Settings)
		{
			var wc = new WebClient();
			return wc.DownloadString(Settings.Query.FeedURL);
		}

		public static string RetrieveFeedInfo()
		{
			return RetrieveFeedInfo(SettingsProvider.CurrentSettings);
		}

		public static string RetrieveFeedStyles(SettingsData Settings)
		{
			var wc = new WebClient();
			var queryString = new StringBuilder(Settings.Query.FeedURL);
			queryString.Append("?styles");

			return wc.DownloadString(queryString.ToString());
		}

		public static string RetrieveFeedStyles()
		{
			return RetrieveFeedStyles(SettingsProvider.CurrentSettings);
		}

		public static string RetrieveFeedData(SettingsData Settings, KeyValuePair<string, string>[] Opts = null)
		{
			var wc = new WebClient();
			var queryString = new StringBuilder(Settings.Query.FeedURL);
			queryString.Append("?query");

			if (Opts != null) {
				foreach (KeyValuePair<string, string> opt in Opts)
					queryString.AppendFormat("&{0}={1}", opt.Key, opt.Value);
			}

			return wc.DownloadString(queryString.ToString());
		}

		public static string RetrieveFeedData(KeyValuePair<string, string>[] Opts = null)
		{
			return RetrieveFeedData(SettingsProvider.CurrentSettings, Opts);
		}
	}
}

