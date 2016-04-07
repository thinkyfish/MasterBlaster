using System;
using QuickFont;
using System.Collections;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace MasterBlaster
{
	public class Label
	{
		private string text;
		private TextWriter writer;
		private PointF location;
		private SizeF size;
		private Brush brush;
		public Label (string text, TextWriter writer, Vector2 location)
		{
			this.writer = writer;
			this.text = text;
			this.location = new PointF (location.X, location.Y);
			this.brush = new SolidBrush (Color.White);
			//this.writer.AddLine (text, this.location, brush);

		}
		public virtual void draw(){
			writer.Clear ();
			writer.AddLine (text, location, brush);
			GL.Disable(EnableCap.DepthTest);
			//GL.ClearDepth(1.0f);
			writer.Draw ();
//				GL.ClearDepth(0.0);
//				GL.Disable(EnableCap.Texture2D);
//				GL.Disable( EnableCap.Blend );
//				//GL.Enable(EnableCap.Texture2D);

				//font.Options.UseDefaultBlendFunction = true;
				//GL.Enable( EnableCap.Blend );

			//GL.Disable(EnableCap.Texture2D);	

		}
		public void Dispose(){
			this.writer.Dispose ();
		}

	}
}

