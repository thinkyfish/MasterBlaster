using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;


namespace MasterBlaster
{
    class Explosion
    {
        public Color color;
        public float[,] end;
        public float len;
        public float angle;
        public int size;
        public float x;
        public float y;
        public int health;

        public Explosion(float x, float y, float angle)
        {
            this.size = 10;
            this.end = new float[size, 2];
            this.len = 0.1f;
            this.x = x;
            this.y = y;
            this.color = Color.Red;
            this.health = 20;

            // flip the explosion angle around so it doesnt shoot into the meteor
            this.angle = angle - 180.0f;

            //randomly generate lines radiating backwards from the explosion site
            for (int i = 0; i < size; i++)
            {
                float a_rand = Engine.rand.Next() % 110 - 55; //range -45,45

                end[i, 0] = len * (float)Math.Cos((this.angle + a_rand) * Engine.PI / 180.0f);
                end[i, 1] = len * (float)Math.Sin((this.angle + a_rand) * Engine.PI / 180.0f);

            }
        }

        public void draw()
        {
            // check if there are more frames to draw
            if (health > 0)
            {
                GL.Color3(color);
                GL.LineWidth(1.0f);

                // draw each explosion line
                for (int i = 0; i < size; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);

                    GL.Vertex2(x, y);
                    GL.Vertex2(x + end[i, 0], y + end[i, 1]);

                    GL.End();
                }

                //decrease remaining frames by one
                health -= 1;
            }
        }
    }
}

