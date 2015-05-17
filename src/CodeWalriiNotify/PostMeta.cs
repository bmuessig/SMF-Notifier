using System;

namespace CodeWalriiNotify
{
	public struct PostMeta
	{
		public String Title { get; set; }

		public String Poster { get; set; }

		public String Body { get; set; }

		public DateTime Time { get; set; }

		public String FormatTime(String Format)
		{
			return Time.ToString();
		}
	}
}

