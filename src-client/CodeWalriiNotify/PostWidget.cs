using System;
using Gdk;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Gtk;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class PostWidget : Gtk.Bin
	{
		private string BodyHTML = "";
		private uint LastRenderWidth;

		public PostWidget()
		{
			this.Build();

			LastRenderWidth = 0;

			SettingsData settings = SettingsProvider.CurrentSettings;

			var headerBackcolor = settings.Styles.HeaderBackcolor;
			var timeForecolor = settings.Styles.TimestampForecolor;
			var titleForecolor = settings.Styles.TitleForecolor;
			var bodyBackcolor = settings.Styles.BodyBackcolor;
			var footerBackcolor = settings.Styles.FooterBackcolor;
			var authorForecolor = settings.Styles.AuthorForecolor;

			var titleFont = Pango.FontDescription.FromString(settings.Styles.TitleFont);
			var detailFont = Pango.FontDescription.FromString(settings.Styles.DetailFont);

			headerBox.ModifyBg(StateType.Normal, headerBackcolor);
			topicLabel.ModifyFg(StateType.Normal, titleForecolor);
			topicLabel.ModifyFont(titleFont);
			timeLabel.ModifyFg(StateType.Normal, timeForecolor);
			timeLabel.ModifyFont(detailFont);
			bodyBox.ModifyBg(StateType.Normal, bodyBackcolor);
			mainBox.ModifyBg(StateType.Normal, bodyBackcolor);
			footerBox.ModifyBg(StateType.Normal, footerBackcolor);
			posterLabel.ModifyFg(StateType.Normal, authorForecolor);
			posterLabel.ModifyFont(detailFont);

			headerBox.Realized += delegate {
				headerBox.GdkWindow.Cursor = new Cursor(CursorType.Hand1);
			};

			headerBox.ButtonPressEvent += delegate {
				if (MyToolbox.CheckUrl(URL)) {
					try {
						var urlInfo = new ProcessStartInfo(URL); // I am fully aware of this possible security breach; it's fixed now, as only correct urls will pass
						Process.Start(urlInfo);
					} catch (Exception) {
						return;
					}
				} else {
					MessageBox.Show(
						String.Format("The post URL \"{0}\" could not be parsed as an URL.\nOpening it is a possible security risk.\n\nDo you want to proceed?",
							URL),
						"Open URL",
						Gtk.MessageType.Warning,
						Gtk.ButtonsType.YesNo,
						new Gtk.ResponseHandler(delegate(object o, Gtk.ResponseArgs args) {
							if (args.ResponseId == Gtk.ResponseType.Yes) {
								try {
									var urlInfo = new ProcessStartInfo(URL); // I am fully aware of this imense security breach; see the note above
									Process.Start(urlInfo);
								} catch (Exception) {
									return;
								}
							}
						})
					);
				}
				
			};
			bodyBox.Realized += delegate {
				bodyBox.GdkWindow.Cursor = new Cursor(CursorType.Arrow);
			};
			bodyRenderarea.SizeAllocated += delegate(object o, SizeAllocatedArgs args) {
				//		UpdateBody(args.Allocation);
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

		protected void UpdateBody(uint Width)
		{
			UpdateRenderImage(Width);
			bodyRenderarea.HeightRequest = bodyRenderarea.Pixbuf.Height + 20;
		}

		protected void UpdateRenderImage(uint Width)
		{
			bodyRenderarea.Pixbuf = HTMLRenderer.RenderHTML(
				SettingsProvider.CurrentSettings.Styles.BodyFormat.Replace("<post>", BodyHTML),
				Width,
				0,
				SettingsProvider.CurrentSettings.Styles.BodyUseAntiAlias
			);
		}

		protected void OnBodyRenderareaExposeEvent(object o, ExposeEventArgs args)
		{
			if (args.Args.Length == 0)
				return;
			if (args.Args[0].GetType() != typeof(EventExpose))
				return;

			var expose = (EventExpose)args.Args[0];
			if (expose.Area.Width == LastRenderWidth)
				return;
			LastRenderWidth = (uint)expose.Area.Width;
			UpdateBody(LastRenderWidth);
		}
	}
}

