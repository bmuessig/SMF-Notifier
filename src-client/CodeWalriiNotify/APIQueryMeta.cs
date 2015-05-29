using System;

namespace CodeWalriiNotify
{
	public class APIQueryMeta
	{
		public APIQueryMeta(string JSON)
		{
			
		}

		public APIQueryMeta(bool Success, bool Cached, uint Timestamp, uint Changed, string[] Exceptions, PostMeta[] Posts)
		{
			this.Success = Success;
			this.Cached = Cached;
			this.Timestamp = Timestamp;
			this.Changed = Changed;
			this.Exceptions = Exceptions;
			this.Posts = Posts;
		}

		public uint Timestamp {
			get;
			private set;
		}

		public uint Changed {
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

		public string[] Exceptions {
			get;
			private set;
		}

		public PostMeta[] Posts {
			get;
			private set;
		}
	}
}

