using System;
using System.IO;

//using System.Web.Script.Serialization;
using System.Dynamic;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CodeWalriiNotify
{
	public static class SettingsProvider
	{
		static SettingsProvider()
		{
			CurrentFilename = "settings.json";
			FromFile(CurrentFilename);
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

			try {
				CurrentSettings = (SettingsData)JsonConvert.DeserializeObject(rawSettingsJson, typeof(SettingsData));
			} catch (Exception ex) {
				if (DefaultOnError) {
					CurrentSettings = new SettingsData();
					ToFile(Path);
				} else
					throw ex;
			}
		}

		public static void ToFile(string Path)
		{
			File.WriteAllText(Path, JsonConvert.SerializeObject(CurrentSettings, Formatting.Indented));
		}

		public static void SetDefaults()
		{
			CurrentSettings = new SettingsData();
		}

		public static SettingsData CurrentSettings {
			get;
			private set;
		}

		public static string CurrentFilename {
			get;
			private set;
		}
	}
}

