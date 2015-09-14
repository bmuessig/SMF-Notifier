using System;

namespace CodeWalriiNotify
{
	public class NotificationSettingsMeta
	{
		public bool AudioNotifyEnable {
			get;
			set;
		}

		public bool AudioNotifyUseCustomAudio {
			get;
			set;
		}

		public string AudioNotifyFile {
			get;
			set;
		}

		public bool VisualNotifyEnable {
			get;
			set;
		}

		public float VisualNotifyVerticalAlignment {
			get;
			set;
		}

		public bool VisualNotifyDoAnimate {
			get;
			set;
		}

		public uint VisualNotifyAnimationInterval {
			get;
			set;
		}

		public uint VisualNotifyTimeout {
			get;
			set;
		}

		public bool TaskbarIconUnreadCounterEnable {
			get;
			set;
		}

		public Align TaskbarIconUnreadCounterAlignment {
			get;
			set;
		}

		public NotificationSettingsMeta()
		{
			AudioNotifyEnable = false;
			AudioNotifyUseCustomAudio = false;
			AudioNotifyFile = "";
			VisualNotifyEnable = true;
			VisualNotifyVerticalAlignment = 0.9f;
			VisualNotifyDoAnimate = true;
			VisualNotifyAnimationInterval = 10;
			VisualNotifyTimeout = 10;
			TaskbarIconUnreadCounterEnable = true;
			TaskbarIconUnreadCounterAlignment = Align.BottomRight;
		}
	}
}

