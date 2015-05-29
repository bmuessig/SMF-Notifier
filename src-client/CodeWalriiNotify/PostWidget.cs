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

			var headerBackcolor = new Color(110, 180, 137);
			var timeForecolor = new Color(216, 216, 216);
			var titleForecolor = new Color(255, 255, 255);
			var bodyBackcolor = new Color(250, 250, 250);
			var bodyForecolor = new Color(0, 0, 0);
			var footerBackcolor = new Color(250, 250, 250);
			var authorForecolor = new Color(198, 198, 198);

			var titleFont = Pango.FontDescription.FromString("Tahoma 15.6");
			var detailFont = Pango.FontDescription.FromString("Tahoma 10.5");
			var bodyFont = Pango.FontDescription.FromString("Tahoma 13.6");

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
			bodyRenderarea.Pixbuf = HTMLRenderer.RenderHTML("<style>img{max-width:440px;}\na{text-decoration:none;}</style>" + BodyHTML, (uint)size.Width, 0);
			bodyRenderarea.HeightRequest = bodyRenderarea.Pixbuf.Height + 20;
		}
	}
}

