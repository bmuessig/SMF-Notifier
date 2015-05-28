using System;
using System.IO;
using System.Web.Script.Serialization;

namespace CodeWalriiNotify
{
	public static class SettingsProvider
	{
		static JavaScriptSerializer jsonDecoder;

		static SettingsProvider()
		{
			FromFile("config.json");
		}


		public static void FromFile(string Path, bool DefaultOnError = true)
		{
			string rawSettingsJson;

			if (!File.Exists(Path)) {
				if (!DefaultOnError)
					throw new FileNotFoundException("The JSON file couldn't be found and defaulting is disabled!", Path);
				else
					rawSettingsJson = ""; // default json settings
			} else
				rawSettingsJson = File.ReadAllText(Path);
			
			jsonDecoder = new JavaScriptSerializer();
			jsonDecoder.RegisterConverters(new[] { new DynamicJsonConverter() });
			dynamic jsonObj = jsonDecoder.Deserialize(rawSettingsJson, typeof(object));


		}
		
	}
}

