

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
        public static void Main()
        {
            Engine engine = new Engine(8);


            //Bullet b = new Bullet(100.0f, 100.0f, 35f);
            using (var game = new MBGameWindow())
            {
				game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                    //game.WindowBorder = WindowBorder.Hidden;
					//game.fontSetup();
					game.textSetup();
					game.paneSetup();

                };

                game.Resize += (sender, e) =>
                {
                    
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
                    //GL.Viewport(0, 0, game.Width, game.Height);
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
                        default:
                            break;
                    }

                };
                game.KeyUp += (sender, e) =>
                {
                    switch (e.Key)
                    {
                        case Key.Space:
                            engine.stopfiring();
                            break;
                        case Key.W:
                            engine.stopThrusting();
                            break;
                        case Key.A:
                            engine.stopTurning();
                            break;
                        case Key.D:
                            engine.stopTurning();
                            break;
                        default:
                            break;
                    }

                };
                game.UpdateFrame += (sender, e) =>
                {

                    // add game logic, input handling
                    if (game.Keyboard[Key.Escape])
                    {
                        game.Exit();
                    }
                    engine.nextframe(Engine.DT);
                    //b.nextframe();

                };

                game.RenderFrame += (sender, e) =>
                {

					if (game.Height < 600)
						game.Height = 600;
					if (game.Width < 600)
						game.Width = 600;
					// render graphics
					GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
					GL.Viewport(0, 0, game.Width, game.Height);
					GL.Ortho(-Engine.ViewX, Engine.ViewX, -Engine.ViewY, Engine.ViewY, -1.0, 1.0);
					//GL.Disable( EnableCap.Blend );
					//GL.Enable(EnableCap.Blend);
					//GL.BlendFunc(

                    //GL.Begin(PrimitiveType.Triangles);
                    //GL.Color3(Color.MidnightBlue);
                    //GL.Vertex2(-1.0f, 1.0f);
                    //GL.Color3(Color.SpringGreen);
                    //GL.Vertex2(0.0f, -1.0f);
                    //GL.Color3(Color.Ivory);
                    //GL.Vertex2(1.0f, 1.0f);

                    //GL.End();

					engine.draw();
					/////
					GL.ClearDepth(1.0f);
					GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrc1Alpha);
					GL.Enable(EnableCap.Blend);
					//QFont.Begin();
					//string text = Convert.ToString(engine.score);
					//SizeF len = game.scoreFont.Measure(text);
					//Label score = new Label(text,game.scoreFont, new Vector2(game.Width - (len.Width + 50f), 20.0f));
					//score.draw();
					//if(engine.scorechanged){
					game.scoreWriter.Clear();

					game.scoreWriter.AddLine(Convert.ToString(engine.score),
							new PointF(game.Width - 50f, 20.0f),
							new SolidBrush(Color.White));

					game.scoreWriter.Draw();
					//GL.Enable(EnableCap.Texture2D);

					//GL.Clear(ClearBufferMask.DepthBufferBit);
					GL.Disable(EnableCap.Blend);
					//QFont.Begin();
					game.menuPane.draw();
					//GL.Disable(EnableCap.Texture2D);
				
					//GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    game.SwapBuffers();
                };
                game.Width = 600;
                game.Height = 600;
                // Run the game at 60 updates per second
                game.Run(60.0);
            }
        }
    }
}
