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

        public int size;

        public float[] pos;
        private float[,] shadowpos;

        public float rad;

        private Color color;

        private int value;

        private float vx;
        private float vy;
        private float angle;
        private float blastamt;

        public float health;

        private float mass;

        //set the shadow position array to all possible corresponding positions outside the field of view.
        public void setshadow(float[] position)
        {
            shadowpos[0, 0] = position[0];
            shadowpos[0, 1] = position[1] + 2.0f;

            shadowpos[1, 0] = position[0] + 2.0f;
            shadowpos[1, 1] = position[1];

            shadowpos[2, 0] = position[0];
            shadowpos[2, 1] = position[1] - 2.0f;

            shadowpos[3, 0] = position[0] - 2.0f;
            shadowpos[3, 1] = position[1];

        }

        // use barycentric coordinates to check if P is inside triangle A,B,C.
        // from "http://www.blackpawn.com/texts/pointinpoly"
        public bool intriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            Vector2 v0 = C - A;
            Vector2 v1 = B - A;
            Vector2 v2 = P - A;

            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);
            float invDenom = -1.0f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            return (u >= 0) && (v >= 0) && (u + v < 1);
        }

        // check if a point is inside the meteor or it's shadows
        public bool isWithin(float x, float y)
        {
            if (isWithinPos(x, y, pos[0], pos[1]))
                return true;

            for (int i = 0; i < 4; i++)
            {
                if (isWithinPos(x, y, shadowpos[i, 0], shadowpos[i, 1]))
                    return true;
            }
            return false;
        }
        private bool isWithinPos(float x, float y, float posx, float posy)
        {
            //check if inside the possible radius of the meteor
            if (Engine.dist2(posx, posy, x, y) < (rad + 0.5f * rad))
            {
                //check each triangluar wedge
                Vector2 A = new Vector2(posx, posy);
                Vector2 P = new Vector2(x, y);
                Vector2 B, C;

                for (int i = 1; i < size; i++)
                {
                    B = new Vector2(pts[i, 0]+posx, pts[i, 1]+posy);
                    C = new Vector2(pts[i - 1, 0]+posx, pts[i - 1, 1]+posy);
                    if (intriangle(A, B, C, P))
                        return true;

                }
                //check the last wedge
                B = new Vector2(pts[size - 1, 0]+posx, pts[size - 1, 1]+posy);
                C = new Vector2(pts[0, 0]+posx, pts[0, 1]+posy);
                if (intriangle(A, B, C, P))
                    return true;
            }
            return false;
        }

        // check if any of the meteor's lines are visible
        public bool onscreen(float xpos, float ypos)
        {
            int i = 0;
            float xcoord = Math.Abs(pts[i, 0] + xpos);
            float ycoord = Math.Abs(pts[i, 1] + ypos);

            for (i = 0; i < this.size; i++)
            {
                if ((xcoord < 1.01f) || (ycoord < 1.01f))
                    return true;
            }

            return false;
        }

        //this is used to put an impulse on the meteor
        public void blast(float angle, float blastamt = Engine.METEOR_BLAST)
        {
            this.angle = angle;
            this.blastamt = blastamt;
        }

        //compute the next position of the meteor
        public void nextframe(float dt = Engine.DT)
        {

            this.blastamt *= 1.0f - Engine.METEOR_DRAG;

            this.blastamt /= mass;

            float fx = blastamt * (float)Math.Cos(angle * Engine.PI / 180f);
            float fy = blastamt * (float)Math.Sin(angle * Engine.PI / 180f);


            float ax = fx / size;
            float ay = fy / size;

            float dvx, dvy;
            dvx = ax * dt;
            dvy = ay * dt;

            vx += dvx;
            vy += dvy;

            if (vx > Engine.MAX_VEL)
                vx = .05f;
            if (vy > Engine.MAX_VEL)
                vy = .05f;


            float dsx, dsy;
            dsx = vx * dt;
            dsy = vy * dt;

            //compute the new positions and check if off the edge of the screen.
            pos[0] = Engine.glrange(pos[0] + dsx);
            pos[1] = Engine.glrange(pos[1] + dsy);

            //reset the off screen shadow origins
            setshadow(pos);
        }

        //draw the meteor and any visible shadow copies
        public void draw()
        {
            //always draw the origin position, it is always on the screen.
            drawpos(this.pos[0], this.pos[1]);

            //loop over the possible intruding edges, checking if any are visible and if so, draw them.
            for (int i = 0; i < 4; i++)
            {
                if (onscreen(shadowpos[i, 0], shadowpos[i, 1]))
                {
                    drawpos(shadowpos[i, 0], shadowpos[i, 1]);
                }
            }
        }

        //draw the meteor at the given position
        private void drawpos(float xpos, float ypos)
        {

            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < size; i++)
            {
                GL.Vertex2(pts[i, 0] + xpos, pts[i, 1] + ypos);
            }

            GL.Vertex2(pts[0, 0] + xpos, pts[0, 1] + ypos);
            GL.End();

            GL.LineWidth(1.0f);
            GL.Color3(this.color);
            GL.Begin(PrimitiveType.LineStrip);

            for (int i = 0; i < size; i++)
            {
                GL.Vertex2(pts[i, 0] + xpos, pts[i, 1] + ypos);
            }

            GL.Vertex2(pts[0, 0] + xpos, pts[0, 1] + ypos);

            GL.End();


        }


        public Meteor(int size = Engine.METEOR_PTS, float x = 0.0f, float y = 0.0f, float b_angle = 0f)
        {
            pts = new float[size, 2];
            pos = new float[2];
            shadowpos = new float[4, 2];

            this.size = size;
            this.color = Color.White;
            this.value = Engine.METEOR_VALUE;
            this.health = Engine.METEOR_HEALTH;

            if (size < Engine.METEOR_MINSIZE)
                size = Engine.METEOR_MINSIZE;

            pos[0] = x;
            pos[1] = y;
            this.setshadow(pos);

            mass = size;

            //health and radius of the meteor are proportional to size
            rad = 0.2f * ((float)size / Engine.METEOR_PTS);
            health = Engine.METEOR_HEALTH * ((float)size / (float)Engine.METEOR_PTS);

            //generate the randomized surface of the meteor
            float angle = 0;
            for (int i = 1; i <= size; i++)
            {
                angle = i * (360 / size);

                float p_rand = ((float)Engine.rand.NextDouble() * 0.5f + 0.5f) * rad;

                float p_x = p_rand * (float)Math.Cos(angle * Engine.PI / 180f);
                float p_y = p_rand * (float)Math.Sin(angle * Engine.PI / 180f);

                pts[i - 1, 0] = p_x;
                pts[i - 1, 1] = p_y;
            }

            //give the meteor a random starting blast angle
            if (b_angle == 0)
                b_angle = Engine.rand.Next() % 360;

            this.blast(b_angle);

        }

    }
}
