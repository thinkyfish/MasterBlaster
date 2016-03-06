using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace MasterBlaster
{
    class Bullet
    {
        private float[] pos;
        private float angle;
        private float vel;
        private float ax;
        private float ay;
        private float dist;
        public int health;
        public float damage;

        public Bullet(float x, float y, float angle, float vel = common.BULLET_VEL)
        {
            this.pos = new float[2];
            this.pos[0] = x;
            this.pos[1] = y;
            this.angle = angle;
            this.vel = vel;
            this.damage = common.BULLET_DAMAGE;
            this.health = 1;
        }
        public void draw()
        {
            GL.PointSize(common.BULLET_SIZE);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(Color.White);
            GL.Vertex2(pos[0], pos[1]);
            GL.End();
        }

        public void nextframe(float dt = common.DT)
        {

            //computes the next physics step for the bullet

            float dvx, dvy, vx, vy, dsx, dsy, dr;

            vx = vel * (float)Math.Cos(angle * common.PI / 180f);
            vy = vel * (float)Math.Sin(angle * common.PI / 180f);

            dvx = ax * dt;
            dvy = ay * dt;

            ax *= 1.0f - common.DROPOFF;
            ay *= 1.0f - common.DROPOFF;

            vx += dvx;
            vy += dvy;


            dsx = vx * dt;
            dsy = vy * dt;

            //compute the distance travelled
            dr = common.dist(pos, pos[0] + dsx, pos[1] + dsy);

            // set position and account for screen edges
            pos[0] = common.glrange(pos[0] + dsx);
            pos[1] = common.glrange(pos[1] + dsy);      

            //keep track of the distance
            dist += dr;

            //if the bullet has traveled too far, kill it.
            if (dist > common.BULLET_RANGE)
            {
                this.health = 0;
            }


        }

    }
}
