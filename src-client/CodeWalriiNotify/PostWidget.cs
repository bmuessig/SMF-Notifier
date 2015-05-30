using System;
using Gdk;
using System.Diagnostics;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PostWidget : Gtk.Bin
	{
		private string BodyHTML = "";

		public PostWidget()
		{
			this.Build();

			var headerBackcolor = SettingsProvider.CurrentSettings.HeaderBackcolor;
			var timeForecolor = SettingsProvider.CurrentSettings.TimestampForecolor;
			var titleForecolor = SettingsProvider.CurrentSettings.TitleForecolor;
			var bodyBackcolor = SettingsProvider.CurrentSettings.BodyBackcolor;
			var footerBackcolor = SettingsProvider.CurrentSettings.FooterBackcolor;
			var authorForecolor = SettingsProvider.CurrentSettings.AuthorForecolor;

			var titleFont = Pango.FontDescription.FromString(SettingsProvider.CurrentSettings.TitleFont);
			var detailFont = Pango.FontDescription.FromString(SettingsProvider.CurrentSettings.DetailFont);

			headerBox.ModifyBg(Gtk.StateType.Normal, headerBackcolor);
			topicLabel.ModifyFg(Gtk.StateType.Normal, titleForecolor);
			topicLabel.ModifyFont(titleFont);
			timeLabel.ModifyFg(Gtk.StateType.Normal, timeForecolor);
			timeLabel.ModifyFont(detailFont);
			bodyBox.ModifyBg(Gtk.StateType.Normal, bodyBackcolor);
			mainBox.ModifyBg(Gtk.StateType.Normal, bodyBackcolor);
			footerBox.ModifyBg(Gtk.StateType.Normal, footerBackcolor);
			posterLabel.ModifyFg(Gtk.StateType.Normal, authorForecolor);
			posterLabel.ModifyFont(detailFont);

			headerBox.Realized += delegate {
				headerBox.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
			};
			headerBox.ButtonPressEvent += delegate {
				var urlInfo = new ProcessStartInfo(URL);
				Process.Start(urlInfo);
			};
			bodyBox.Realized += delegate {
				bodyBox.GdkWindow.Cursor = new Cursor(CursorType.Arrow);
			};

			this.ShowAll();
		}

		public String Topic {
			get {
				return topicLabel.Text;
			}

			set {
				topicLabel.Text = value;
			}
		}

		public String Body {
			get {
				return BodyHTML;
			}

			set {
				BodyHTML = value;
				UpdateBody();
			}
		}

		public String Poster {
			get {
				return posterLabel.Text;
			}

			set {
				posterLabel.Text = value;
			}
		}

		public String Time {
			get {
				return timeLabel.Text;
			}

			set {
				timeLabel.Text = value;
			}
		}

		public String URL {
			get;
			set;
		}

		public void UpdateBody()
		{
			Gtk.Requisition size = bodyRenderarea.SizeRequest();
			bodyRenderarea.Pixbuf = HTMLRenderer.RenderHTML(
				SettingsProvider.CurrentSettings.BodyFormat.Replace("<post>", BodyHTML),
				(uint)size.Width,
				0,
				SettingsProvider.CurrentSettings.BodyUseAntiAlias
			);
			bodyRenderarea.HeightRequest = bodyRenderarea.Pixbuf.Height + 20;
		}
	}
}

