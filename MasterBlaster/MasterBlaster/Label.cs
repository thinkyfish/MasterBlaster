using System;

using System.Collections;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using StringTextureGL;
namespace MasterBlaster
{
	public class Label
	{
		private string text;
		private  StringTexture labeltexture;
		public PointF location;
		private SizeF labelsize;
		private Size clientSize;
		private Font labelfont;
		public Label (string text, Font font, PointF location)
		{
			
			this.text = text;
			this.location = location;
			this.labelfont = font;
			this.labeltexture = new StringTexture (this.text,font, Color.White, Color.DarkRed );

		}
		public void SetClientSize(Size newSize){
			this.clientSize = newSize;
		}

		public virtual void Draw(Size clientSize, float Depth = 1.0f)
		{
			Size size = labeltexture.Size();
			//PointF location = new PointF(clientSize.Width - 20 - size.Width, 20);



			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, clientSize.Width, clientSize.Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);

			GL.PushMatrix();
			GL.LoadMatrix(ref ortho_projection);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);



			GL.BindTexture(TextureTarget.Texture2D, labeltexture.TextureId());
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0);
			GL.Vertex3(location.X, location.Y, Depth);
			GL.TexCoord2(1, 0);
			GL.Vertex3(location.X + size.Width, location.Y, Depth);
			GL.TexCoord2(1, 1);
			GL.Vertex3(location.X + size.Width, location.Y + size.Height, Depth);
			GL.TexCoord2(0, 1);
			GL.Vertex3(location.X, location.Y + size.Height, Depth);

			GL.End();

			GL.PopMatrix();

			GL.Disable(EnableCap.Texture2D);

		}
		public void Dispose(){
			//this.writer.Dispose ();
		}

	}
}

