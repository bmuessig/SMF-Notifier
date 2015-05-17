using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace CodeWalriiNotify
{
	public class PostMeta
	{
		public String Subject { get; set; }

		public String Poster { get; set; }

		public String Body { get; set; }

		public DateTime Time { get; set; }

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
		}

		public PostMeta(String Subject, String Poster, String Body, DateTime Time)
		{
			this.Subject = Subject;
			this.Poster = Poster;
			this.Body = Body;
			this.Time = Time;
		}

		/// <summary>
		/// Retrieves the PostMeta objects from a JSON-String
		/// </summary>
		/// <returns>A list of the PostMeta elements</returns>
		/// <param name="json">JSON API response string</param>
		public static List<PostMeta> FromJSON(String json)
		{
			var posts = new List<PostMeta>();
			var serializer = new JavaScriptSerializer();

			serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
			dynamic jsonObj = serializer.Deserialize(json, typeof(object));

			if ((bool)jsonObj.success) {
				List<object> data = jsonObj.data;
				foreach (dynamic postObj in data) {
					dynamic postFields = postObj.post;
					dynamic posterFields = postObj.poster;
					dynamic topicFields = postObj.topic;
					dynamic starterFields = postObj.starter;
					dynamic boardFields = postObj.board;

					var post = new PostMeta((string)postFields.subject, (string)posterFields.name, (string)postFields.body, UnixTimeStampToDateTime((ulong)postFields.time));
					posts.Add(post);
				}

			} else {
				// API ERROR!
				return null;
			}

			return posts;
		}

		public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

	}
}

