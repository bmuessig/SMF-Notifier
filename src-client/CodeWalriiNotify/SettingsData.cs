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

		public IgnoredEntity[] IgnoredTopics {
			get;
			set;
		}

		public IgnoredEntity[] IgnoredUsers {
			get;
			set;
		}

		public bool HideIgnoredPosts {
			get;
			set;
		}

		public bool AudioNotifyEnable {
			get;
			set;
		}

		public bool AudioNotifyUseCustomAudio {
			get;
			set;
		}

		public string AudioNotifyFile {
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

		public bool VisualNotifyEnable {
			get;
			set;
		}

		public float VisualNotifyVerticalAlignment {
			get;
			set;
		}

		public bool VisualNotifyDoAnimate {
			get;
			set;
		}

		public uint VisualNotifyAnimationInterval {
			get;
			set;
		}

		public uint VisualNotifyTimeout {
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
			FeedURL = "http://codewalr.us/notifier/";
			FeedTitle = "CodeWalr.us";
			QueryInterval = 50;
			MaximumPosts = 10;
			IgnoredTopics = new IgnoredEntity[]{ };
			IgnoredUsers = new IgnoredEntity[]{ };
			HideIgnoredPosts = false;
			AudioNotifyEnable = false;
			AudioNotifyUseCustomAudio = false;
			AudioNotifyFile = "";
			UseCustomIcon = true;
			IconFile = "walrii.gif";
			VisualNotifyEnable = true;
			VisualNotifyVerticalAlignment = 0.9f;
			VisualNotifyDoAnimate = true;
			VisualNotifyAnimationInterval = 10;
			VisualNotifyTimeout = 10;
			BodyUseAntiAlias = true;
			HeaderBackcolor = new Color(110, 180, 137);
			TimestampForecolor = new Color(216, 216, 216);
			TitleForecolor = new Color(255, 255, 255);
			BodyBackcolor = new Color(250, 250, 250);
			FooterBackcolor = new Color(250, 250, 250);
			AuthorForecolor = new Color(198, 198, 198);
			TitleFont = "Tahoma 16";
			DetailFont = "Tahoma 11";
			BodyFormat = @"<html>
<head>
<style>
body {
 font-family: 'Tahoma';
}

img {
 max-width:100%;
 height:auto;
}

a {
 text-decoration:none;
}
</style>
</head>
<body>
<post>
</body>
</html>";
		}
	}
}

