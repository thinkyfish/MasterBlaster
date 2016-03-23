using System;
using System.Collections.Generic;
using QuickFont;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace MasterBlaster
{
	public class Pane
	{
		private RectangleF bounds;
		private Color bordercolor;
		private Color backgroundcolor;
		private float linewidth;
		private List<MasterBlaster.Label> contents;
		public Pane (RectangleF bounds, float linewidth = 1.0f)
		{
			this.bordercolor = Color.White;
			this.backgroundcolor = Color.Black;
			this.bounds = bounds;
			this.contents = new List<Label> ();
			this.linewidth = linewidth;
		}
		public void addLabel(Label l){
			contents.Add (l);
		}
		public void draw(){
	
			GL.Disable (EnableCap.Texture2D);

			//draw background square
			GL.Color3 (backgroundcolor);
			GL.Begin (PrimitiveType.Polygon);
			GL.Vertex2 (bounds.X, bounds.Y);
			GL.Vertex2 (bounds.X + bounds.Width, bounds.Y);
			GL.Vertex2 (bounds.X + bounds.Width, bounds.Y + bounds.Height);
			GL.Vertex2 (bounds.X, bounds.Y + bounds.Height);
			GL.Vertex2 (bounds.X, bounds.Y);
			GL.End ();

			//draw border
			GL.Color4 (bordercolor);
			GL.LineWidth (linewidth);
			GL.Begin (PrimitiveType.LineLoop);
			GL.Vertex3 (bounds.X, bounds.Y, 0f);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y, 0f);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y + bounds.Height, 0f);
			GL.Vertex3 (bounds.X, bounds.Y + bounds.Height, 0f);
			GL.End ();

			//draw labels
			contents.ForEach (c => c.draw ());

		}
	}
}

