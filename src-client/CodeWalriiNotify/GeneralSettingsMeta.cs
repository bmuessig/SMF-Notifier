using System;

namespace CodeWalriiNotify
{
	public class GeneralSettingsMeta
	{
		public string FeedTitle {
			get;
			set;
		}

		public bool UseCustomIcon {
			get;
			set;
		}

		public string IconFile {
			get;
			set;
		}

		public GeneralSettingsMeta()
		{
			FeedTitle = "CodeWalr.us";
			UseCustomIcon = true;
			IconFile = "walrii.gif";
		}
	}
}

