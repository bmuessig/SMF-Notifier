using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace CodeWalriiNotify
{
	public static class SettingsProvider
	{
		static readonly JavaScriptSerializer javaScriptSerializer;

		static SettingsProvider()
		{
			javaScriptSerializer = new JavaScriptSerializer();
			javaScriptSerializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoJsonConverter() });
			FromFile("config.json");
		}

		public static void FromFile(string Path, bool DefaultOnError = true)
		{
			string rawSettingsJson;

			if (!File.Exists(Path)) {
				if (!DefaultOnError)
					throw new FileNotFoundException("The JSON file couldn't be found and defaulting is disabled!", Path);
				else {
					RestoreDefaults();
					ToFile(Path);
					return;
				}
			} else
				rawSettingsJson = File.ReadAllText(Path);

			dynamic settings = javaScriptSerializer.Deserialize(rawSettingsJson, typeof(ExpandoObject));

			FeedURL = (string)settings.feed_url;
			ApplicationTitle = (string)settings.app_title;
		}

		public static void RestoreDefaults()
		{
			FeedURL = "http://api.muessigb.net/smf_notifier.php";
			ApplicationTitle = "Sample";
		}

		public static void ToFile(string Path)
		{
			dynamic settings = new ExpandoObject();

			settings.feed_url = FeedURL;
			settings.app_title = FeedURL;

			File.WriteAllText(Path, javaScriptSerializer.Serialize(settings));
		}

		public static string FeedURL {
			get;
			set;
		}

		public static string ApplicationTitle {
			get;
			set;
		}

		public static uint QueryInterval {
			get;
			set;
		}

		public static byte MaximumPosts {
			get;
			set;
		}

		public static bool AudioNotify {
			get;
			set;
		}

		public static bool VisualNotify {
			get;
			set;
		}

		public static Gdk.Color HeaderBackcolor {
			get;
			set;
		}

		public static Gdk.Color TimestampForecolor {
			get;
			set;
		}

		public static Gdk.Color AuthorForecolor {
			get;
			set;
		}

		public static Gdk.Color TitleForecolor {
			get;
			set;
		}

		public static Gdk.Color BodyForecolor {
			get;
			set;
		}

		public static Gdk.Color BodyBackcolor {
			get;
			set;
		}

		public static Gdk.Color FooterBackcolor {
			get;
			set;
		}

		public static string TitleFont {
			get;
			set;
		}

		public static string DetailFont {
			get;
			set;
		}

		public static string BodyFont {
			get;
			set;
		}

		public static bool AntiAlias {
			get;
			set;
		}

		public static string CustomCss {
			get;
			set;
		}

		public static string BodyFormat {
			get;
			set;
		}
			
		/*
			var headerBackcolor = new Color(110, 180, 137);
			var timeForecolor = new Color(216, 216, 216);
			var titleForecolor = new Color(255, 255, 255);
			var bodyBackcolor = new Color(250, 250, 250);
			var bodyForecolor = new Color(0, 0, 0);
			var footerBackcolor = new Color(250, 250, 250);
			var authorForecolor = new Color(198, 198, 198);

			var titleFont = Pango.FontDescription.FromString("Tahoma 15.6");
			var detailFont = Pango.FontDescription.FromString("Tahoma 10.5");
			var bodyFont = Pango.FontDescription.FromString("Tahoma 13.6");
		*/
	}
}

