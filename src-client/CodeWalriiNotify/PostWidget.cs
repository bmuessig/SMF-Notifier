using System;
using Gdk;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
				URL = "notepad.exe " + URL;
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

