

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
            Engine engine = new Engine();
            engine.addBullet(new Bullet(0.5f, 0.5f, 35f));
            engine.addBullet(new Bullet(0.5f, 0.5f, 40f));
            engine.addBullet(new Bullet(0.5f, 0.5f, 56f));
            engine.addBullet(new Bullet(0.5f, 0.5f, 25f));
            engine.addBullet(new Bullet(0.5f, 0.5f, 10f));
            engine.addBullet(new Bullet(0.5f, 0.5f, 300f));
            engine.newMeteor();
            engine.newMeteor();
            engine.addShip();
            //Bullet b = new Bullet(100.0f, 100.0f, 35f);
            using (var game = new MBGameWindow())
            {
                game.Load += (sender, e) =>
                {
                    // setup settings, load textures, sounds
                    game.VSync = VSyncMode.On;
                };

                game.Resize += (sender, e) =>
                {
                    GL.Viewport(0, 0, game.Width, game.Height);
                };
                game.KeyDown += (sender, e) =>
                {
                    switch (e.Key)
                    {
                        case Key.Space:
                            engine.startfiring();
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
