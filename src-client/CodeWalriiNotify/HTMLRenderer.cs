using System;
using System.Drawing;
using TheArtOfDev.HtmlRenderer.WinForms;
using System.Drawing.Imaging;
using System.IO;

namespace CodeWalriiNotify
{
	public static class HTMLRenderer
	{
		public static Gdk.Pixbuf RenderHTML(string HTML, uint Width = 0, uint Height = 0, bool UseAntiAlias = true)
		{
			Image img = HtmlRender.RenderToImageGdiPlus(HTML, (int)Width, (int)Height, UseAntiAlias ? System.Drawing.Text.TextRenderingHint.AntiAlias : System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit);
			return ImageToPixbuf(img);
		}

		public static SizeF MeasureHTML(string HTML, uint Width = 0)
		{
			using (Image img = new Bitmap(1, 1)) {
				using (Graphics g = Graphics.FromImage(img)) {
					return  HtmlRender.MeasureGdiPlus(g, HTML, Width);
				}
			}
		}

		static Gdk.Pixbuf ImageToPixbuf(Image image)
		{ 
			using (var stream = new MemoryStream()) { 
				image.Save(stream, ImageFormat.Png); 
				stream.Position = 0; 
				return new Gdk.Pixbuf(stream); 
			} 
		}
	}
}

