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
    class Ship
    {
        private float angle;

        private float[] pos;

        private float vx;
        private float vy;

        private float ax;
        private float ay;

        private float fx;
        private float fy;

        private float mass;

        private float drag;

        private float thrust;

        private float[,] pts;

        private Color color;

        public float health;

        public Ship(float x = 0.0f, float y = 0.0f)
        {
            this.vx = 0.0f;
            this.vy = 0.0f;
            this.ax = 0.0f;
            this.pts = new float[4, 2];
            this.pos = new float[2];
            this.pos[0] = x;
            this.pos[1] = y;
            this.angle = 0.0f;
            this.drag = Engine.SHIP_DRAG;
            this.color = Color.White;
            this.health = 1;
        }

        public void draw()
        {
            //define the size and shape of the ship
            float rad = 0.06f;
            float angle1 = angle;
            float angle2 = angle + 130;
            float angle3 = angle - 130;

            // Recompute the ships lines based on the angle
            pts[0, 0] = rad * (float)Math.Cos(angle1 * Engine.PI / 180.0f);
            pts[0, 1] = rad * (float)Math.Sin(angle1 * Engine.PI / 180.0f);
            pts[1, 0] = rad * (float)Math.Cos(angle2 * Engine.PI / 180.0f);
            pts[1, 1] = rad * (float)Math.Sin(angle2 * Engine.PI / 180.0f);
            pts[2, 0] = 0.0f;
            pts[2, 1] = 0.0f;
            pts[3, 0] = rad * (float)Math.Cos(angle3 * Engine.PI / 180.0f);
            pts[3, 1] = rad * (float)Math.Sin(angle3 * Engine.PI / 180.0f);

            GL.LineWidth(1.0f);
            GL.Color3(this.color);

            //draw the edge lines
            GL.Begin(PrimitiveType.LineStrip);
            for (int i = 0; i < 4; i++)
                GL.Vertex2(pos[0] + pts[i, 0],
                   pos[1] + pts[i, 1]);

            //connect last to first
            GL.Vertex2(pos[0] + pts[0, 0],
                   pos[1] + pts[0, 1]);

            GL.End();
        }

        public void engine(float thrust = Engine.SHIP_THRUST)
        {
            this.thrust = thrust;
        }

        public void nextframe(float dt = Engine.DT)
        {

            //compute engine force based on ship direction and thrust
            fx = thrust * (float)Math.Cos(angle * Engine.PI / 180.0f);
            fy = thrust * (float)Math.Sin(angle * Engine.PI / 180.0f);
            

            ax = fx / Engine.MASS;
            ay = fy / Engine.MASS;

            float dvx, dvy;
            dvx = ax * dt;
            dvy = ay * dt;

            vx += dvx;
            vy += dvy;

            //drag
            vx *= 1.0f - drag;
            vy *= 1.0f - drag;


            float dsx, dsy;
            dsx = vx * dt;
            dsy = vy * dt;

            // warp at edge of screen
            pos[0] = Engine.glrange(pos[0] + dsx);
            pos[1] = Engine.glrange(pos[1] + dsy);


        }

    }
}
