

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

//using QuickFont;

namespace MasterBlaster
{
	class Program
	{
		//private BulletList bl = new BulletList();
		/// <summary>
		/// 
		/// </summary>
		[STAThread]
		public static void Main ()
		{
			Engine engine = new Engine (8);


			//Bullet b = new Bullet(100.0f, 100.0f, 35f);
			using (var game = new MBGameWindow ()) {
				game.Load += (sender, e) => {
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
					//game.WindowBorder = WindowBorder.Hidden;
					//game.fontSetup();
					game.LoadFonts ();
					game.paneSetup ();
					game.Mode = MBGameWindow.WindowMode.Menu;
				};

				game.Resize += (sender, e) => {
                    
					//int max = game.Width;
					if (game.Height < 600)
						game.Height = 600;
					if (game.Width < 600)
						game.Width = 600;
					//    max = game.Height;
					//game.Width = max;
					//game.Height = max;
					Engine.ViewY = 1.0f;
					Engine.ViewX = 1.0f * game.Width / game.Height;
					game.scoreWriter.Dispose ();
					game.buttonWriter.Dispose ();
					game.menuPane.Dispose ();

					//game.setupFonts();
					game.paneSetup ();
					//game.setupScoreFont();
					//game.scoreWriter.UpdateText();
					game.scoreWriter.setClientSize (new Size (game.Width, game.Height));
					game.buttonWriter.setClientSize (new Size (game.Width, game.Height));
					game.menuWriter.setClientSize (new Size (game.Width, game.Height));
					//GL.Viewport(0, 0, game.Width, game.Height);
				};
				game.KeyDown += (sender, e) => {
					switch (e.Key) {
					case Key.Space:
						engine.startfiring ();
						break;
					case Key.W:
						engine.startThrusting (1.0f);
						break;
					case Key.S:
						engine.startThrusting (-1.0f);
						break;
					case Key.A:
						engine.startTurning (1.0f);
						break;
					case Key.D:
						engine.startTurning (-1.0f);
						break;
					case Key.M:
						game.Mode = MBGameWindow.WindowMode.Menu;
						break;
					case Key.G:
						game.Mode = MBGameWindow.WindowMode.Game;
						break;
					default:
						break;
					}

				};
				game.KeyUp += (sender, e) => {
					switch (e.Key) {
					case Key.Space:
						engine.stopfiring ();
						break;
					case Key.W:
						engine.stopThrusting ();
						break;
					case Key.A:
						engine.stopTurning ();
						break;
					case Key.D:
						engine.stopTurning ();
						break;
					default:
						break;
					}

				};
				game.UpdateFrame += (sender, e) => {

					// add game logic, input handling
					if (game.Keyboard [Key.Escape]) {
						game.Exit ();
					}
					engine.nextframe (Engine.DT);
					//b.nextframe();

				};

				game.RenderFrame += (sender, e) => {

					if (game.Height < 600)
						game.Height = 600;
					if (game.Width < 600)
						game.Width = 600;
					// render graphics
					GL.ClearDepth (1.0f);
					GL.Clear (ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					GL.MatrixMode (MatrixMode.Projection);
					GL.LoadIdentity ();
					GL.Viewport (0, 0, game.Width, game.Height);
					GL.Ortho (-Engine.ViewX, Engine.ViewX, -Engine.ViewY, Engine.ViewY, -1.0, 1.0);
					GL.Disable (EnableCap.DepthTest);
					//GL.Disable( EnableCap.Blend );
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(


					if (engine.scorechanged) {
						//GL.PushMatrix();
						game.scoreWriter.Clear ();

						game.scoreWriter.AddLine (Convert.ToString (engine.score),
							new PointF (game.Width - 20f, 20.0f),
							new SolidBrush (Color.White));
					}


					//GL.End();
					GL.Disable (EnableCap.Blend);



					if (game.Mode == MBGameWindow.WindowMode.Game) {
					
						engine.draw ();
						game.scoreWriter.Draw ();
					}
					if (game.Mode == MBGameWindow.WindowMode.Menu) {

						engine.draw();

						game.menuPane.draw ();
						/////
						//GL.ClearDepth(1.0f);
						//GL.Clear(ClearBufferMask.DepthBufferBit);
						//GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrc1Alpha);
						GL.BlendFunc (BlendingFactorSrc.SrcAlphaSaturate, BlendingFactorDest.OneMinusSrcAlpha);
						//GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.DstColor);
						GL.Enable (EnableCap.Blend);
						//QFont.Begin();
						//string text = Convert.ToString(engine.score);
						//SizeF len = game.scoreFont.Measure(text);
						//Label score = new Label(text,game.scoreFont, new Vector2(game.Width - (len.Width + 50f), 20.0f));
						//score.draw();


						//game.scoreWriter.Draw();
						//GL.Enable(EnableCap.Texture2D);
						//GL.PopMatrix();
						//GL.Clear(ClearBufferMask.DepthBufferBit);
						//GL.Disable(EnableCap.Blend);
						//QFont.Begin();
//					game.buttonWriter.Clear();
//					game.buttonWriter.AddLine("test", new PointF(100.0f,20.0f), new SolidBrush(Color.White));
//					game.buttonWriter.Draw();
						game.menuPane.DrawText ();
					}
					//GL.Disable(EnableCap.Texture2D);
				
					//GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

					game.SwapBuffers ();
				};
				game.Width = 600;
				game.Height = 600;
				// Run the game at 60 updates per second
				game.Run (60.0);
			}
		}
	}
}
