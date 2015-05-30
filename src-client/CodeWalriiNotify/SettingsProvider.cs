using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Collections.Generic;

namespace CodeWalriiNotify
{
	public static class SettingsProvider
	{
		static readonly JavaScriptSerializer javaScriptSerializer;

		static SettingsProvider()
		{
			javaScriptSerializer = new JavaScriptSerializer();
			FromFile("config.json");
		}

		public static void FromFile(string Path, bool DefaultOnError = true)
		{
			string rawSettingsJson;

			if (!File.Exists(Path)) {
				if (!DefaultOnError)
					throw new FileNotFoundException("The JSON file couldn't be found and defaulting is disabled!", Path);
				else {
					CurrentSettings = new SettingsData();
					ToFile(Path);
					return;
				}
			} else
				rawSettingsJson = File.ReadAllText(Path);

			CurrentSettings = (SettingsData)javaScriptSerializer.Deserialize(rawSettingsJson, typeof(SettingsData));
		}

		public static void ToFile(string Path)
		{
			File.WriteAllText(Path, JsonPrettify.FormatJson(javaScriptSerializer.Serialize(CurrentSettings)));
		}

		public static SettingsData CurrentSettings {
			get;
			private set;
		}
	}
}

