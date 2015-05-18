using System;
using Cairo;

namespace CodeWalriiNotify
{
	public class CairoHTMLRenderer
	{
		public CairoHTMLRenderer()
		{
			var ps = new ImageSurface(Format.Argb32, 100, 100);
			var cr = new Context(ps);

			
			//cr.SelectFontFace("Tahoma", FontSlant.Normal, FontWeight.Normal);
			cr.SetSourceRGB(255, 255, 255);
			cr.Paint();

			cr.SetSourceRGB(0, 0, 0);

			DrawText(cr, 0, 0, Pango.FontDescription.FromString("Tahoma 15.6"), "Hello World!");

			cr.Save();

			//ps.WriteToPng("lol.png");
		}

		private void DrawText(Context cairoContext, double x, double y, Pango.FontDescription font, string markup)
		{
			Pango.Layout layout = Pango.CairoHelper.CreateLayout(cairoContext);
			layout.SetMarkup(markup);
			layout.FontDescription = font;

			int textWidth;
			int textHeight;
			layout.GetPixelSize(out textWidth, out textHeight);

			cairoContext.MoveTo(x, y);

			Pango.CairoHelper.ShowLayout(cairoContext, layout);
		}
	}
}

