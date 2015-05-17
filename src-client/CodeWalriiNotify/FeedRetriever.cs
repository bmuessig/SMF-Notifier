using System;
using System.Net;
using Newtonsoft.Json.Linq;

namespace CodeWalriiNotify
{
	public class FeedRetriever
	{
		readonly private String apiUrl;

		public FeedRetriever(String ApiUrl, String FeedBaseUrl)
		{
			apiUrl = String.Format("{0}?feed_base_url={1}", ApiUrl, FeedBaseUrl);
		}

		public String RetrieveData(String opts)
		{
			var wc = new WebClient();
			String json = wc.DownloadString(apiUrl + opts);

			return json;

			/*var serializer = new JavaScriptSerializer();
			serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

			dynamic obj = serializer.Deserialize(json, typeof(object));

			return obj;*/

			//return JObject.Parse(json);
		}
	}
}

