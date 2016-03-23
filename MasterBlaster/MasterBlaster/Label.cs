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
		private QFont font;
		private Vector2 location;
		private SizeF size;

		public Label (string text, QFont font, Vector2 location)
		{
			this.text = text;
			this.font = font;
			this.location = location;
			this.size = font.Measure (text);
		}
		public virtual void draw(){
			QFont.Begin ();
			//GL.BindTexture(EnableCap.Texture2D);
			font.Print(text,location);
				GL.ClearDepth(0.0);
//				GL.Disable(EnableCap.Texture2D);
//				GL.Disable( EnableCap.Blend );
//				//GL.Enable(EnableCap.Texture2D);
				GL.Color4(0.0f, 0.0f, 0.0f, 0.0f);
				GL.LineWidth (40);
				GL.Begin(PrimitiveType.Lines);
				GL.Vertex2 (location.X, location.Y + size.Height);
				GL.Vertex2 (location.X + size.Width, location.Y + size.Height);
				GL.End();
				//font.Options.UseDefaultBlendFunction = true;
				GL.Enable( EnableCap.Blend );
			QFont.End ();
			//GL.Disable(EnableCap.Texture2D);	

		}

	}
}

