using System;
using System.Net;

namespace CodeWalriiNotify
{
	public class FeedRetriever
	{
		readonly private String apiUrl;

		public FeedRetriever(String ApiURL)
		{
			this.apiUrl = ApiURL;
		}

		public String RetrieveData(String opts)
		{
			var wc = new WebClient();
			return wc.DownloadString(apiUrl + opts);
		}
	}
}

