using System;

namespace CodeWalriiNotify
{
	public class ContentSettingsMeta
	{
		public IgnoredEntity[] IgnoredTopics {
			get;
			set;
		}

		public IgnoredEntity[] IgnoredUsers {
			get;
			set;
		}

		public bool HideIgnoredPosts {
			get;
			set;
		}

		public uint MinimumWordcount {
			get;
			set;
		}

		public ContentSettingsMeta()
		{
			IgnoredTopics = new IgnoredEntity[]{ };
			IgnoredUsers = new IgnoredEntity[]{ };
			HideIgnoredPosts = false;
			MinimumWordcount = 0;
		}
	}
}

