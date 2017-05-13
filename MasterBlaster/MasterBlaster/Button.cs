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
		public Button (string text, Font font, PointF location, float bordersize = 1.0f) : base (text, font, new PointF(location.X + 2 * bordersize, location.Y + 2 * bordersize))
		{
			//nothing here

		}


		public override void Draw(Size clientSize, float Depth = 1.0f){
		
			//GL.Disable(EnableCap.Texture2D);
			GL.Color4 (bordercolor);
			GL.LineWidth (linewidth);
			GL.Begin(PrimitiveType.LineLoop);
			GL.Vertex3(bounds.X, bounds.Y, 0f);
			GL.Vertex3(bounds.X + bounds.Width, bounds.Y, 0f);
			GL.Vertex3(bounds.X + bounds.Width, bounds.Y + bounds.Height, 0f);
			GL.Vertex3(bounds.X, bounds.Y + bounds.Height, 0f);
			GL.End();
			base.Draw(clientSize, Depth);


		}
	}
}

