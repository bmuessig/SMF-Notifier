using System;

namespace CodeWalriiNotify
{
	public class QuerySettingsMeta
	{
		public string FeedURL {
			get;
			set;
		}

		public uint QueryInterval {
			get;
			set;
		}

		public byte MaximumPosts {
			get;
			set;
		}

		public QuerySettingsMeta()
		{
			FeedURL = "https://codewalr.us/notifier/";
			QueryInterval = 50;
			MaximumPosts = 10;
		}
	}
}

