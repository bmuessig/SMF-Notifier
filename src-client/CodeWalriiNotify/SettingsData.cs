using System;
using Gdk;

namespace CodeWalriiNotify
{
	public class SettingsData
	{
		public GeneralSettingsMeta General { get; set; }

		public QuerySettingsMeta Query { get; set; }

		public ContentSettingsMeta Content { get; set; }

		public NotificationSettingsMeta Notifications { get; set; }

		public StyleSettingsMeta Styles { get; set; }

		public SettingsData()
		{
			General = new GeneralSettingsMeta();
			Query = new QuerySettingsMeta();
			Content = new ContentSettingsMeta();
			Notifications = new NotificationSettingsMeta();
			Styles = new StyleSettingsMeta();
		}
	}
}

