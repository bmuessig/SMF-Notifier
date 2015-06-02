using System;

namespace CodeWalriiNotify
{
	public class APIException
	{
		public string Type { get; private set; }

		public string What { get; private set; }

		public bool Critical { get; private set; }

		public APIException(string Type, string What = "", bool Critical = false)
		{
			this.Type = Type;
			this.What = What;
			this.Critical = Critical;
		}
	}
}

