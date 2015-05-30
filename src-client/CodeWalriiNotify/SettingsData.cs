using System;
using Gdk;

namespace CodeWalriiNotify
{
	public class SettingsData
	{
		public string FeedURL {
			get;
			set;
		}

		public string FeedTitle {
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

		public bool AudioNotify {
			get;
			set;
		}

		public string AudioFile {
			get;
			set;
		}

		public bool VisualNotify {
			get;
			set;
		}

		public Color HeaderBackcolor {
			get;
			set;
		}

		public Color TimestampForecolor {
			get;
			set;
		}

		public Color AuthorForecolor {
			get;
			set;
		}

		public Color TitleForecolor {
			get;
			set;
		}

		public Color BodyBackcolor {
			get;
			set;
		}

		public Color FooterBackcolor {
			get;
			set;
		}

		public string TitleFont {
			get;
			set;
		}

		public string DetailFont {
			get;
			set;
		}

		public bool BodyUseAntiAlias {
			get;
			set;
		}

		public string BodyFormat {
			get;
			set;
		}

		public SettingsData()
		{
			FeedURL = "http://api.muessigb.net/smf_notifier.php";
			FeedTitle = "CodeWalr.us";
			QueryInterval = 50;
			MaximumPosts = 10;
			AudioNotify = false;
			AudioFile = "";
			VisualNotify = true;
			BodyUseAntiAlias = true;
			HeaderBackcolor = new Color(110, 180, 137);
			TimestampForecolor = new Color(216, 216, 216);
			TitleForecolor = new Color(255, 255, 255);
			BodyBackcolor = new Color(250, 250, 250);
			FooterBackcolor = new Color(250, 250, 250);
			AuthorForecolor = new Color(198, 198, 198);
			TitleFont = "Tahoma 16";
			DetailFont = "Tahoma 11";
			BodyFormat = "<html><head><style>img{max-width:440px;}\na{text-decoration:none;}</style></head><body><post></body></html>";
		}
	}
}

