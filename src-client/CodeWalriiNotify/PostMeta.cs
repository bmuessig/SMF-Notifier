using System;

namespace CodeWalriiNotify
{
	public class PostMeta
	{
		public string Subject { get; set; }

		public string Topic { get; set; }

		public uint TopicID { get; set; }

		public string Poster { get; set; }

		public uint PosterID { get; set; }

		public string Body { get; set; }

		public DateTime Time { get; set; }

		public string Link { get; set; }

		public string FormatTime(String Format)
		{
			return Time.ToString();
		}

		public PostMeta()
		{
			Subject = "";
			Poster = "";
			Body = "";
			Time = new DateTime(1970, 0, 0);
			Link = "";
		}

		public PostMeta(string Subject, string TopicName, uint TopicID, string Poster, uint PosterID, string Body, DateTime Time, string Link)
		{
			this.Subject = Subject;
			this.Topic = TopicName;
			this.TopicID = TopicID;
			this.Poster = Poster;
			this.PosterID = PosterID;
			this.Body = Body;
			this.Time = Time;
			this.Link = Link;
		}
	}
}

