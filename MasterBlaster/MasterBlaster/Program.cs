

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

		[STAThread]
		public static void Main ()
		{
			Engine engine = new Engine (8);
			using (var game = new MBGameWindow ()) {
				game.Load += (sender, e) => {
					// setup settings, load textures, sounds
					game.VSync = VSyncMode.On;
					game.paneSetup ();
					game.Mode = WindowMode.Game;
				};

				game.Resize += (sender, e) => {
                    

					if (game.Height < 600)
						game.Height = 600;
					if (game.Width < 600)
						game.Width = 600;

					Engine.ViewY = 1.0f;
					Engine.ViewX = 1.0f * game.Width / game.Height;
					//game.scoreWriter.Dispose ();
					//game.buttonWriter.Dispose ();
					//game.menuPane.Dispose ();

					//game.paneSetup ();

					//game.scoreWriter.SetClientSize(game.Size);
					//game.testlabel.SetClientSize(game.Size);
					//game.buttonWriter.setClientSize (new Size (game.Width, game.Height));
					////GL.Viewport(0, 0, game.Width, game.Height);

				};
				game.KeyDown += (sender, e) =>
				{
					switch (e.Key)
					{
						case Key.Space:
							engine.startfiring();
							break;
						case Key.W:
							engine.startThrusting(1.0f);
							break;
						case Key.S:
							engine.startThrusting(-1.0f);
							break;
						case Key.A:
							engine.startTurning(1.0f);
							break;
						case Key.D:
							engine.startTurning(-1.0f);
							break;
						case Key.M:
							game.Mode = WindowMode.Menu;
							break;
						case Key.G:
							game.Mode = WindowMode.Game;
							break;
						default:
							break;
					}
					game.menuPane.KeyHandler(e.Key);
				};
				game.KeyUp += (sender, e) => {
					switch (e.Key) {
					case Key.Space:
						engine.stopfiring ();
						break;
					case Key.W:
						engine.stopThrusting ();
						break;
					case Key.S:
						engine.stopThrusting ();
						break;
					case Key.A:
						engine.stopTurning ();
						break;
					case Key.D:
						engine.stopTurning ();
						break;
					case Key.Escape:
						game.Exit();
						break;
					case Key.F:
						if(game.WindowState == WindowState.Normal){
							game.WindowState = WindowState.Fullscreen;
						} else {
							game.WindowState = WindowState.Normal;
						}
						break;
					default:
						break;
					}

				};
				game.UpdateFrame += (sender, e) => {
				
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



					if (engine.scorechanged) {
 						game.scoreWriter.SetText(Convert.ToString(engine.score));
					}



					GL.Disable (EnableCap.Blend);


					if (game.Mode == WindowMode.Game) {
					
						engine.draw ();

						game.scoreWriter.Draw (game.Size);
						//game.testlabel.Draw();


					}
					if (game.Mode == WindowMode.Menu) {
						
						game.menuPane.Draw ();

						//GL.BlendFunc (BlendingFactorSrc.SrcAlphaSaturate, BlendingFactorDest.OneMinusSrcAlpha);
						//GL.Enable (EnableCap.Blend);

						game.menuPane.DrawText (game.Size);
						engine.draw();

					}

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
