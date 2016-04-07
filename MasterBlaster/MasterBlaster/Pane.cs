using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
namespace MasterBlaster
{
	public class Pane
	{
		public RectangleF bounds;
		private Color bordercolor;
		private Color backgroundcolor;
		private float linewidth;
		private List<Label> contents;
		private float Depth = 0.5f;
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
	
			//GL.Disable (EnableCap.Texture2D);
			//GL.ClearDepth (1.0f);
			//draw background square
			//GL.Disable (EnableCap.Blend);
			GL.Color3 (backgroundcolor);
			GL.Begin (PrimitiveType.Polygon);
			GL.Vertex3 (bounds.X, bounds.Y, Depth);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y, Depth);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y + bounds.Height, Depth);
			GL.Vertex3 (bounds.X, bounds.Y + bounds.Height, Depth);
			GL.Vertex3 (bounds.X, bounds.Y, Depth);
			GL.End ();

			//draw border
			GL.Color4 (bordercolor);
			GL.LineWidth (linewidth);
			GL.Begin (PrimitiveType.LineLoop);
			GL.Vertex3 (bounds.X, bounds.Y, Depth);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y, Depth);
			GL.Vertex3 (bounds.X + bounds.Width, bounds.Y + bounds.Height, Depth);
			GL.Vertex3 (bounds.X, bounds.Y + bounds.Height, Depth);
			GL.End ();
			//GL.ClearDepth (0.7);
			//GL.Clear(ClearBufferMask.DepthBufferBit);

			//GL.Enable (EnableCap.Blend);
			//;
			//draw labels
			//contents.ForEach (c => c.draw ());

		}
		public void DrawText(){
			contents.ForEach (c => c.draw ());

		}
		public void Dispose(){
			contents.ForEach (c => c.Dispose ());
		}

	}
}

