using System;

namespace CodeWalriiNotify
{
	public class PostMeta
	{
		public String Subject { get; set; }

		public String Poster { get; set; }

		public String Body { get; set; }

		public DateTime Time { get; set; }

		public String Link { get; set; }

		public String FormatTime(String Format)
		{
			return Time.ToString();
		}

		public PostMeta()
		{
			Subject = "";
			Poster = "";
			Body = "";
			Time = DateTime.Now;
			Link = "";
		}

		public PostMeta(String Subject, String Poster, String Body, DateTime Time, String Link)
		{
			this.Subject = Subject;
			this.Poster = Poster;
			this.Body = Body;
			this.Time = Time;
			this.Link = Link;
		}
	}
}

