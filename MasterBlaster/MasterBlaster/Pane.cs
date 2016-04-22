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
		private List<Button> buttons;
		private float Depth = 0.5f;
		public int selected = 0;
		private Selector selector;
		private float nextY;
		private float layoutoffset = 0.2f;


		public Pane (RectangleF bounds, float linewidth = 1.0f)
		{
			this.bordercolor = Color.White;
			this.backgroundcolor = Color.Black;
			this.bounds = bounds;
			this.contents = new List<Label> ();
			this.buttons = new List<Button> ();
			this.linewidth = linewidth;
			this.selector = new Selector (-0.2f, 0.0f);
			this.nextY = 0.6f;
		}
		public void addLabel(Label l){
			//l.location.Y = nextY;
			//nextY -= layoutoffset;
			contents.Add (l);
		}
		public void addButton(Button b){
			//b.location.Y = nextY;
			//nextY -= layoutoffset;
			buttons.Add (b);
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
			selector.draw (new PointF(0.0f, 0.0f));

		}
		public void DrawText(){
			contents.ForEach (c => c.draw ());
			buttons.ForEach (b => b.draw ());

		}
		public void Dispose(){
			contents.ForEach (c => c.Dispose ());
			buttons.ForEach (b => b.Dispose ());
		}

	}
}

