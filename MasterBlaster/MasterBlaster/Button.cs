using System;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MasterBlaster
{
	public class Button : MasterBlaster.Label
	{
		private RectangleF bounds;
		public Color bordercolor = Color.White;
		public float linewidth = 1.0f;
		public Button (string text, TextWriter writer, Vector2 location, float bordersize = 1.0f) : base (text, writer, location)
		{
			//SizeF textsize = font.Measure (text);
			//bounds = new RectangleF (location.X - bordersize, location.Y - bordersize,
//								   textsize.Width + 2 * bordersize, textsize.Height + 2 * bordersize);

		}

		public override void draw(){

//			GL.Disable(EnableCap.Texture2D);
//			GL.Color4 (bordercolor);
//			GL.LineWidth (linewidth);
//			GL.Begin(PrimitiveType.LineLoop);
//			GL.Vertex3(bounds.X, bounds.Y, 0f);
//			GL.Vertex3(bounds.X + bounds.Width, bounds.Y, 0f);
//			GL.Vertex3(bounds.X + bounds.Width, bounds.Y + bounds.Height, 0f);
//			GL.Vertex3(bounds.X, bounds.Y + bounds.Height, 0f);
//			GL.End();
			base.draw();


		}
	}
}

