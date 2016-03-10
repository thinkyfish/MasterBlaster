﻿

using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

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
            Engine engine = new Engine(25);
            

            //Bullet b = new Bullet(100.0f, 100.0f, 35f);
            using (var game = new MBGameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                    //game.WindowBorder = WindowBorder.Hidden;
                };

                game.Resize += (sender, e) =>
                {
                    //TODO: this doesn't work.
                    int max = game.Width;
                    if (game.Height > game.Width)
                        max = game.Height;
                    //game.Width = max;
                    //game.Height = max;
                    GL.Viewport(0, 0,game.Width, game.Height);
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
                    // render graphics
                    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Viewport(0, 0, game.Width, game.Height);
                    GL.Ortho(-1.0, 1.0, -1.0, 1.0, -1.0, 1.0);

                    //GL.Begin(PrimitiveType.Triangles);
                    //GL.Color3(Color.MidnightBlue);
                    //GL.Vertex2(-1.0f, 1.0f);
                    //GL.Color3(Color.SpringGreen);
                    //GL.Vertex2(0.0f, -1.0f);
                    //GL.Color3(Color.Ivory);
                    //GL.Vertex2(1.0f, 1.0f);

                    GL.End();

                    engine.draw();
                    //b.draw();
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
