using System;
using System.IO;

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

		public static bool FromFile(string Path, bool DefaultOnError = true)
		{
			string rawSettingsJson;
			var defaultConfig = new SettingsData();
			bool errorEncountered = false;

			if (!File.Exists(Path)) {
				if (!DefaultOnError)
					throw new FileNotFoundException("The JSON file couldn't be found!", Path);
				else {
					CurrentSettings = defaultConfig;
					ToFile(Path);
					return false;
				}
			} else
				rawSettingsJson = File.ReadAllText(Path);

			try {
				CurrentSettings = (SettingsData)JsonConvert.DeserializeObject(rawSettingsJson, typeof(SettingsData));
				CurrentFilename = Path;
				if (!File.Exists(CurrentSettings.IconFile)) {
					if (DefaultOnError) {
						CurrentSettings.UseCustomIcon = false;
					} else
						throw new FileNotFoundException("The Icon file couldn't be found!", CurrentSettings.IconFile);
					errorEncountered = true;
				}
				if (!File.Exists(CurrentSettings.AudioNotifyFile) && CurrentSettings.AudioNotifyEnable) {
					if (DefaultOnError) {
						CurrentSettings.AudioNotifyUseCustomAudio = false;
					} else
						throw new FileNotFoundException("The Audio file couldn't be found!", CurrentSettings.AudioNotifyFile);
					errorEncountered = true;
				}
			} catch (Exception ex) {
				if (DefaultOnError) {
					CurrentSettings = new SettingsData();
					ToFile(Path);
					errorEncountered = true;
				} else
					throw ex;
			}
			return !errorEncountered;
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

