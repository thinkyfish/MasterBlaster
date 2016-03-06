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
    class Meteor
    {
        private float[,] pts;

        private int size;

        private float[] pos;

        private float rad;

        private Color color;

        private int value;

        private float vx;
        private float vy;
        private float angle;
        private float blastamt;

        private int health;

        private float mass;

        public int isWithin(float x, float y)
        {
            if (common.dist(pos, x, y) < (rad * common.METEOR_RFUDGE))
            {

                // if(fabs(pos[0]-x) < rad && fabs(pos[1]-y) < rad){

                return 1;

            }
            else return 0;
        }


        public void blast(float angle, float blastamt = common.METEOR_BLAST)
        {
            this.angle = angle;
            this.blastamt = blastamt;
        }


        public void nextframe(float dt = common.DT)
        {

            this.blastamt *= 1.0f - common.METEOR_DRAG;

            this.blastamt /= mass;

            float fx = blastamt * (float)Math.Cos(angle * common.PI / 180f);
            float fy = blastamt * (float)Math.Sin(angle * common.PI / 180f);


            float ax = fx / size;
            float ay = fy / size;

            float dvx, dvy;
            dvx = ax * dt;
            dvy = ay * dt;

            vx += dvx;
            vy += dvy;

            if (vx > common.MAX_VEL)
                vx = .05f;
            if (vy > common.MAX_VEL)
                vy = .05f;


            float dsx, dsy;
            dsx = vx * dt;
            dsy = vy * dt;




            pos[0] = common.glrange(pos[0] + dsx);
            pos[1] = common.glrange(pos[1] + dsy);


            if (pos[0] == -2)
                health = 0;
            if (pos[1] == -2)
                health = 0;


        }


        public void draw()
        {

            GL.LineWidth(1.0f);
            GL.Color3(this.color);

            GL.Begin(PrimitiveType.LineStrip);

            for (int i = 0; i < size; i++)
            {
                GL.Vertex2(pts[i,0] + pos[0], pts[i,1] + pos[1]);
            }

            GL.Vertex2(pts[0,0] + pos[0], pts[0,1] + pos[1]);

            GL.End();


        }


        public Meteor(int size = common.METEOR_PTS, float x = 0.0f, float y = 0.0f, float b_angle = 0f)
        {
            pts = new float[size, 2];
            pos = new float[2];

            this.size = size;
            color = Color.White;
            value = common.METEOR_VALUE;
            health = common.METEOR_HEALTH;

            if (size < common.METEOR_MINSIZE)
                size = common.METEOR_MINSIZE;

            pos[0] = x;
            pos[1] = y;

            mass = size;

            rad = 0.2f * ((float)size / common.METEOR_PTS);

            health = common.METEOR_HEALTH * (size / common.METEOR_PTS);

            float angle = 0;
            for (int i = 1; i <= size; i++)
            {

                angle = i * (360 / size);

                float p_rand = ((float)common.rand.NextDouble() * 0.5f + 0.5f) * rad;



                float p_x = p_rand * (float)Math.Cos(angle * common.PI / 180f);
                float p_y = p_rand * (float)Math.Sin(angle * common.PI / 180f);

                pts[i - 1,0] = p_x;
                pts[i - 1,1] = p_y;
            }

            //give the meteor a random starting blast angle
            if (b_angle == 0)
                b_angle = common.rand.Next() % 360;

            this.blast(b_angle);

        }
        
    }
}
