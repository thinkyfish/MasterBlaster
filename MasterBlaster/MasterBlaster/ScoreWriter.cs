using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.Drawing.Text;
using StringTextureGL;
namespace MasterBlaster
{
	public class ScoreWriter
	{

		private Font scorefont;
		private StringTexture scoretexture;
		private Color background = Color.Transparent;
		private Color foreground = Color.White;

		public ScoreWriter ()
		{
			//var fontFile = new FontFamily("pack://application:,,,/Resources/Simplex.ttf");
			this.scorefont = StringTexture.NewFont("Fonts/Simplex.ttf", "Simplex", 30, FontStyle.Bold);
			//this.scorefont = new Font(fontFile, 30);
			//Console.WriteLine("Family:f" + fontFile.Name);
			scoretexture = new StringTexture("0", scorefont, foreground, background);
		}

		public void SetText(String text){
			//delete the current texture
			scoretexture = null;
;
			scoretexture = new StringTexture(text, scorefont, foreground, background);
		}

		public void Draw(Size clientSize, float Depth = 1.0f)
		{
			Size size = scoretexture.Size();
			PointF location = new PointF(clientSize.Width - 20 - size.Width, 20);


			
			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, clientSize.Width, clientSize.Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);

			GL.PushMatrix();
			GL.LoadMatrix(ref ortho_projection);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);



			GL.BindTexture(TextureTarget.Texture2D, scoretexture.TextureId());
			GL.Begin (PrimitiveType.Quads);
			GL.TexCoord2 (0, 0);
			GL.Vertex3 (location.X , location.Y, Depth);
			GL.TexCoord2 (1, 0);
			GL.Vertex3 (location.X + size.Width, location.Y, Depth);
			GL.TexCoord2 (1, 1);
			GL.Vertex3 (location.X + size.Width, location.Y + size.Height, Depth);
			GL.TexCoord2 (0, 1);
			GL.Vertex3 (location.X , location.Y + size.Height, Depth);

			GL.End ();

			GL.PopMatrix();

			GL.Disable(EnableCap.Texture2D);

		}
	}
}

