using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing.Imaging;
using System.Drawing.Text;
// code originally from http://www.opentk.com/node/1554#comment-10625

namespace MasterBlaster
{


	public class TextWriter
	{
		private readonly Font TextFont = new Font(FontFamily.GenericMonospace, 20);
		private readonly Bitmap TextBitmap;
		private List<PointF> _positions;
		private List<string> _lines;
		private List<Brush> _colours;
		private int _textureId;
		private Size _clientSize;
		private StringFormatFlags flags;
		private float Depth = 1.0f;
		public enum Alignment { Left, Center, Right };

		private Alignment alignment;
		public void Update(int ind, string newText)
		{
			if (ind < _lines.Count)
			{
				_lines[ind] = newText;
				UpdateText();
			}
		}

		public void setClientSize(Size size){
			_clientSize = size;
		}

		public TextWriter(Font f, Size ClientSize, Size areaSize, Alignment alignment = Alignment.Left)
		{
			_positions = new List<PointF>();
			_lines = new List<string>();
			_colours = new List<Brush>();

			TextBitmap = new Bitmap(areaSize.Width, areaSize.Height);
			this._clientSize = ClientSize;
			_textureId = CreateTexture();
			TextFont = f;
			this.flags = StringFormatFlags.NoFontFallback;
			this.alignment = alignment;


		}

		private int CreateTexture()
		{
			int textureId;
			GL.TexEnv(TextureEnvTarget.TextureEnv, TextureEnvParameter.TextureEnvMode, (float)TextureEnvMode.Replace);//Important, or wrong color on some computers
			Bitmap bitmap = TextBitmap;
			GL.GenTextures(1, out textureId);
			GL.BindTexture(TextureTarget.Texture2D, textureId);

			BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.Finish();
			bitmap.UnlockBits(data);
			return textureId;
		}

		public void Dispose()
		{
			if (_textureId > 0)
				GL.DeleteTexture(_textureId);
		}

		public void Clear()
		{
			_lines.Clear();
			_positions.Clear();
			_colours.Clear();
		}

		public void AddLine(string s, PointF pos, Brush col)
		{
			_lines.Add(s);
			_positions.Add(pos);
			_colours.Add(col);
			UpdateText();
		}

		public void UpdateText()
		{
			if (_lines.Count > 0)
			{
				using (Graphics gfx = Graphics.FromImage(TextBitmap))
				{
					gfx.Clear(Color.Transparent);
					gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

					for (int i = 0; i < _lines.Count; i++) {
						SizeF drawsize = gfx.MeasureString (_lines [i], TextFont);
						PointF position;
						switch (alignment) {
						case Alignment.Right:
							{
								position = new PointF (_positions [i].X - drawsize.Width, _positions [i].Y);
								gfx.DrawString (_lines [i], TextFont, _colours [i], position,
									new StringFormat (flags));
								break;
							}
						case Alignment.Center:
							{
								position = new PointF (_positions [i].X - (0.5f * drawsize.Width), _positions [i].Y);
								gfx.DrawString (_lines [i], TextFont, _colours [i], position,
									new StringFormat (flags));
								break;
							}
						case Alignment.Left:
							{
								gfx.DrawString (_lines [i], TextFont, _colours [i], _positions [i],
									new StringFormat (flags));
								break;
							}
						}
					}

				}

				System.Drawing.Imaging.BitmapData data = TextBitmap.LockBits(new Rectangle(0, 0, TextBitmap.Width, TextBitmap.Height),
					System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, TextBitmap.Width, TextBitmap.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
				TextBitmap.UnlockBits(data);
			}
		}

		public void Draw()
		{


			Matrix4 ortho_projection = Matrix4.CreateOrthographicOffCenter(0, _clientSize.Width, _clientSize.Height, 0, -1, 1);
			GL.MatrixMode(MatrixMode.Projection);

			GL.PushMatrix();
			GL.LoadMatrix(ref ortho_projection);
			//GL.Disable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			GL.Enable(EnableCap.Texture2D);
			GL.BindTexture(TextureTarget.Texture2D, _textureId);
			//GL.Enable(EnableCap.DepthTest);
			//GL.Clear(ClearBufferMask.DepthBufferBit);

			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(0, 0); GL.Vertex3(0, 0, Depth);
			GL.TexCoord2(1, 0); GL.Vertex3(TextBitmap.Width, 0, Depth);
			GL.TexCoord2(1, 1); GL.Vertex3(TextBitmap.Width, TextBitmap.Height, Depth);
			GL.TexCoord2(0, 1); GL.Vertex3(0, TextBitmap.Height, Depth);
			//GL.Disable(EnableCap.DepthTest);
			GL.End();
			//GL.ClearDepth(1.0f);
			//GL.ClearDepth(0.5f);
			//GL.Clear(ClearBufferMask.DepthBufferBit);
			GL.PopMatrix();

			GL.Disable(EnableCap.Texture2D);

		}
	}
}

