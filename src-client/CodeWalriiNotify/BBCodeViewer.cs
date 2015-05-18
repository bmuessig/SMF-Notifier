using System;
using Cairo;

namespace CodeWalriiNotify
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class BBCodeViewer : Gtk.Bin
	{
		private string bbcode;
		private ImageSurface surface;

		public BBCodeViewer()
		{
			this.Build();
		}

		public string BBCode {
			get { return bbcode; }
			set {
				bbcode = value;
				int width;
				int height;
				Canvas.GetSizeRequest(out width, out height);
				surface = Render(bbcode, (uint)width, (uint)height);
				Canvas.Pixbuf = new Gdk.Pixbuf(surface.Data, surface.Width, surface.Height);
			}
		}

		private ImageSurface Render(string BBCode, uint CanvasWidth, uint CanvasHeight)
		{
			var imgSurface = new ImageSurface(Format.Rgb24, (int)CanvasWidth, (int)CanvasHeight);
			var cairoContext = new Context(imgSurface);

			return null;
		}
	}
}

