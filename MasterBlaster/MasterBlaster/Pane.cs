using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
namespace MasterBlaster
{
	public class Pane
	{
		public RectangleF bounds;
		private Color bordercolor;
		private Color backgroundcolor;
		private float linewidth;
		private List<Label> labels;
		private List<Button> buttons;
		private float Depth = 0.5f;
		public int selected = 0;
		//private Selector selector;
		private float nextY =0.3f;
		private Size clientSize;
		private float layoutoffset = 0.2f;


		public Pane (RectangleF bounds, float linewidth = 1.0f)
		{
			this.bordercolor = Color.White;
			this.backgroundcolor = Color.Black;
			this.bounds = bounds;
			this.labels = new List<Label> ();
			this.buttons = new List<Button> ();
			this.linewidth = linewidth;
			//this.selector = new Selector (-0.2f, 0.0f);
			this.nextY = 0.6f;
		}

		public void SetClientSize(Size newSize){
			this.clientSize = newSize;
		}

		public void SelectNext()
		{
			if (selected < buttons.Count)
				selected += 1;
		}
		public void SelectPrev()
		{
			if (selected > 0)
				selected -= 1;
		}
		public void Select()
		{
			Console.WriteLine("Button Pressed :" + selected.ToString());
		}
		public void addLabel(Label l){
			l.location.Y = nextY;
			nextY -= layoutoffset;
			labels.Add (l);
		}
		public void KeyHandler(Key k)
		{
			switch (k)
			{
				case Key.Up:
					SelectPrev();
					break;
				case Key.Down:
					SelectNext();
					break;
				case Key.Space:
				case Key.Enter:
					Select();
					break;
			}
		}
		public void addButton(Button b){
			b.location.Y = nextY;
			nextY -= layoutoffset;
			buttons.Add (b);
		}

		public void Draw(){

			GL.Disable (EnableCap.Texture2D);
			GL.ClearDepth (1.0f);
			//draw background square
			GL.Disable (EnableCap.Blend);


			
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
				


			GL.ClearDepth (0.7);
			GL.Clear(ClearBufferMask.DepthBufferBit);

			GL.Enable (EnableCap.Blend);

			//draw labels
			labels.ForEach (c => c.Draw (clientSize));
			//selector.draw (new PointF(0.0f, 0.0f));
			//DrawText(clientSize);

		}

		public void DrawText(Size clientSize){
			labels.ForEach (c => c.Draw (clientSize));
			buttons.ForEach (b => b.Draw (clientSize));

		}

		public void Dispose(){
			labels.ForEach (c => c.Dispose ());
			buttons.ForEach (b => b.Dispose ());
		}
	}
}

