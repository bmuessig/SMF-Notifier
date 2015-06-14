using System;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Generic;
using Gtk;

namespace CodeWalriiNotify
{
	public static class MyToolbox
	{
		private static readonly Regex urlMatchingRegex;

		static MyToolbox()
		{
			// Regex is slightly modified from https://gist.github.com/dperini/729294
			urlMatchingRegex = new Regex("^(?:https?:\\/\\/)(?:\\S+(?::\\S*)?@)?(?:(?!(?:10|127)(?:\\.\\d{1,3}){3})(?!(?:169\\.254|192\\.168)(?:\\.\\d{1,3}){2})(?!172\\.(?:1[6-9]|2\\d|3[0-1])(?:\\.\\d{1,3}){2})(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}(?:\\.(?:[1-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))|(?:(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)(?:\\.(?:[a-z\\u00a1-\\uffff0-9]-*)*[a-z\\u00a1-\\uffff0-9]+)*(?:\\.(?:[a-z\\u00a1-\\uffff]{2,})))(?:.)?(?::\\d{2,5})?(?:\\/\\S*)?$");	
		}

		public static bool CheckUrl(string URL)
		{
			return urlMatchingRegex.Match(URL).Success;
		}

		public static DateTime GetBuildDate()
		{
			var version = Assembly.GetEntryAssembly().GetName().Version;
			return new DateTime(2000, 1, 1).Add(new TimeSpan(
				TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
				TimeSpan.TicksPerSecond * 2 * version.Revision)); // seconds since midnight, (multiply by 2 to get original)
		}

		public static string GetVersionString()
		{
			Version ver = Assembly.GetExecutingAssembly().GetName().Version;
			return string.Format("{0}.{1}", ver.Major, ver.Minor);
		}

		public static string BuildTitle(SettingsData Settings, uint UnreadPosts, string CustomStatus = "")
		{
			return Settings.FeedTitle + (Settings.FeedTitle.Length > 0 ? " " : "") + "Post Notifier" + (UnreadPosts > 0 ? string.Format(" - {0} Unread posts", UnreadPosts) : "") + (CustomStatus.Length > 0 ? " - Synchronizing..." : "");
		}

		public static List<string> ListStoreToList(ListStore Model)
		{
			if (Model == null)
				return null;
			if (Model.NColumns != 1)
				return null;

			var list = new List<string>();
			foreach (object[] row in Model) {
				if (row.Length == 1) {
					var str = row[0] as string;
					if (str != null)
						list.Add(str);
					else
						return null;
				} else
					return null;
			}
			return list;
		}
	}
}

