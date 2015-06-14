using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace CodeWalriiNotify
{
	public class APIPostQueryMeta
	{
		public APIPostQueryMeta(string Json)
		{
			try {
				dynamic apiObj = JObject.Parse(Json);

				if ((bool)apiObj.success) {

					// Var declarations
					var posts = new List<PostMeta>();
					var exceptions = new List<APIException>();

					// Process Informations
					bool cached = (bool)apiObj.cached;
					ulong timestamp = (ulong)apiObj.timestamp;
					ulong changed = (ulong)apiObj.changed;

					// Process Posts
					foreach (dynamic postObj in apiObj.data) {
						dynamic postFields = postObj.post;
						dynamic posterFields = postObj.poster;
						dynamic topicFields = postObj.topic;
						dynamic starterFields = postObj.starter;
						dynamic boardFields = postObj.board;

						var post = new PostMeta((string)postFields.subject, (string)topicFields.subject, (uint)topicFields.id, (string)posterFields.name, (uint)posterFields.id, (string)postFields.body, UnixTimeStampToDateTime((ulong)postFields.time), (string)postFields.link);
						posts.Add(post);
					}
						
					// Process Exceptions
					foreach (dynamic excObj in apiObj.exceptions) {
						string type = (string)excObj.type;
						string what = (string)excObj.what;
						bool critical = (bool)excObj.critical;

						var exception = new APIException(type, what, critical);
						exceptions.Add(exception);
					}

					// Build API Object
					this.Success = true;
					this.Posts = posts.ToArray();
					this.Exceptions = exceptions.ToArray();
					this.Cached = cached;
					this.Timestamp = timestamp;
					this.Changed = changed;
				} else {
					this.Success = false;
				}
			} catch (Exception ex) {
				this.Success = false;
				this.Exceptions = new []{ new APIException(ex.GetType().ToString(), ex.Message, true) };
			}
		}

		public APIPostQueryMeta(bool Success, bool Cached, uint Timestamp, uint Changed, APIException[] Exceptions, PostMeta[] Posts)
		{
			this.Success = Success;
			this.Cached = Cached;
			this.Timestamp = Timestamp;
			this.Changed = Changed;
			this.Exceptions = Exceptions;
			this.Posts = Posts;
		}

		protected DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		public ulong Timestamp {
			get;
			private set;
		}

		public ulong Changed {
			get;
			private set;
		}

		public bool Success {
			get;
			private set;
		}

		public bool Cached {
			get;
			private set;
		}

		public APIException[] Exceptions {
			get;
			private set;
		}

		public PostMeta[] Posts {
			get;
			private set;
		}
	}
}

