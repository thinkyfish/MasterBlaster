using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;

namespace MasterBlaster
{
    class Meteor
    {
        private float[,] pts;

        public int size;

        public float[] pos;
        private float[,] shadowpos;
        private const int shadows = 8;
        private bool[] shadowdraw;

        public float rad;
        private float maxrad;
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

            shadowpos[4, 0] = position[0] - 2.0f;
            shadowpos[4, 1] = position[1] + 2.0f;

            shadowpos[5, 0] = position[0] + 2.0f;
            shadowpos[5, 1] = position[1] - 2.0f;

            shadowpos[6, 0] = position[0] - 2.0f;
            shadowpos[6, 1] = position[1] - 2.0f;

            shadowpos[7, 0] = position[0] + 2.0f;
            shadowpos[7, 1] = position[1] + 2.0f;



        }

        // decides which of the shadows are possible with the given angle.
        public void setshadowdraw(float angle)
        {
            angle = Engine.anglerange(angle);
            // y + 2
            shadowdraw[0] = 180.0f < angle && angle < 360.0f;
            // x + 2
            shadowdraw[1] = 90.0f < angle && angle < 270.0f;
            // y - 2
            shadowdraw[2] = 0.0f < angle && angle < 180.0f;
            // x - 2
            shadowdraw[3] = 270.0f < angle || angle < 90.0f;
            // x - 2, y + 2
            shadowdraw[4] = 270.0f < angle && angle < 360.0f;
            // x + 2, y - 2
            shadowdraw[5] = 90.0f < angle && angle < 180.0f;
            // x - 2, y - 2
            shadowdraw[6] = 0.0f < angle && angle < 90.0f;
            // x + 2, y + 2
            shadowdraw[7] = 180.0f < angle && angle < 270.0f;
            Debug.WriteLine("call {0}", angle);
            foreach (bool s in shadowdraw)
            {

                Debug.WriteLine(s.ToString());
            }
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


            // check the triangular wedges of the meteor
            if (isWithinPos(x, y, pos[0], pos[1]))
                return true;

            // do the same checks for all the shadows
            if (pos[0] > 1.0 - maxrad || pos[0] < -1.0 + maxrad ||
                pos[1] > 1.0 - maxrad || pos[1] < -1.0 + maxrad)
                
            {
                for (int i = 0; i < shadows; i++)
                {
                    if (isWithinPos(x, y, shadowpos[i, 0], shadowpos[i, 1]))
                        return true;
                }
            }
            return false;
        }

        // check if the given point (x,y) is within the meteor if it were drawn with origin (posx, posy) 
        private bool isWithinPos(float x, float y, float posx, float posy)
        {
            //check if inside the possible radius of the meteor
            if (Engine.dist2(posx, posy, x, y) < maxrad)
            {
                Vector2 A = new Vector2(posx, posy);
                Vector2 P = new Vector2(x, y);
                Vector2 B, C;

                //check the wedges with edge points in the middle of the pts list
                for (int i = 1; i < size; i++)
                {
                    B = new Vector2(pts[i, 0] + posx, pts[i, 1] + posy);
                    C = new Vector2(pts[i - 1, 0] + posx, pts[i - 1, 1] + posy);
                    if (intriangle(A, B, C, P))
                        return true;
                }

                //check the wedge with edge points as the first and last elements of the pts list
                B = new Vector2(pts[size - 1, 0] + posx, pts[size - 1, 1] + posy);
                C = new Vector2(pts[0, 0] + posx, pts[0, 1] + posy);
                if (intriangle(A, B, C, P))
                    return true;
            }

            // no collision found
            return false;
        }
        public float shadowWarp(float position)
        {
            if (position > (1.0f + maxrad))
                position -= 2.0f;
            if (position < (-1.0f) - maxrad)
                position += 2.0f;
            return position;
        }

        // check if any of the meteor's lines are visible
        public bool onscreen(float xpos, float ypos)
        {
            int i = 0;
            float xcoord = Math.Abs(pts[i, 0] + xpos);
            float ycoord = Math.Abs(pts[i, 1] + ypos);

            for (i = 0; i < this.size; i++)
            {
                if ((xcoord < 1.0f) || (ycoord < 1.0f))
                    return true;
            }

            return false;
        }

        //this is used to put an impulse on the meteor
        public void blast(float angle, float blastamt = Engine.METEOR_BLAST)
        {
            this.angle = Engine.anglerange(angle);
            this.blastamt = blastamt;
            //this.setshadowdraw(angle);
        }

        //compute the next position of the meteor
        public void nextframe(float dt = Engine.DT)
        {

            this.blastamt *= 1.0f - Engine.METEOR_DRAG;

            this.blastamt /= mass;

            // force values
            float fx = blastamt * (float)Math.Cos(angle * Engine.PI / 180f);
            float fy = blastamt * (float)Math.Sin(angle * Engine.PI / 180f);

            // acceleration
            float ax = fx / size;
            float ay = fy / size;

            // change in velocity x and y
            float dvx, dvy;
            dvx = ax * dt;
            dvy = ay * dt;

            // velocity values
            vx += dvx;
            vy += dvy;

            // velocity cap
            if (vx > Engine.MAX_VEL)
                vx = .05f;
            if (vy > Engine.MAX_VEL)
                vy = .05f;

            //change in position
            float dpx, dpy;
            dpx = vx * dt;
            dpy = vy * dt;

            //compute the new positions and correct if off the edge of the screen.
            //pos[0] = Engine.glrange(pos[0] + dpx);
            //pos[1] = Engine.glrange(pos[1] + dpy);
            pos[0] = shadowWarp(pos[0] + dpx);
            pos[1] = shadowWarp(pos[1] + dpy);

            //reset the off screen shadow origins
            setshadow(pos);
        }

        //draw the meteor and any visible shadow copies
        public void draw()
        {
            //always draw the origin position
            drawpos(this.pos[0], this.pos[1]);

            //loop over the possible intruding edges, checking if any are visible and if so, draw them.
            if (pos[0] > 1.0f - maxrad || pos[0] < -1.0f + maxrad ||
                pos[1] > 1.0f - maxrad || pos[1] < -1.0f + maxrad)
            {
                for (int i = 0; i < shadows; i++)
                {
                    //if (onscreen(shadowpos[i, 0], shadowpos[i, 1])) // don't need this. its faster to just draw them all.
                    //{
                    drawpos(shadowpos[i, 0], shadowpos[i, 1]);
                    //}
                }
            }
        }

        // draw the meteor at the given position
        private void drawpos(float xpos, float ypos)
        {
            // draw the meteor fill in black
            GL.Color3(Color.Black);
            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < size; i++)
            {
                GL.Vertex2(pts[i, 0] + xpos, pts[i, 1] + ypos);
            }

            GL.Vertex2(pts[0, 0] + xpos, pts[0, 1] + ypos);
            GL.End();

            // draw the meteor's edge
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
            this.pts = new float[size, 2];
            this.pos = new float[2];
            this.shadowpos = new float[shadows, 2];
            this.shadowdraw = new bool[shadows];
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
            maxrad = this.rad * 1.5f;
            health = Engine.METEOR_HEALTH * ((float)size / (float)Engine.METEOR_PTS);

            //generate the randomized surface of the meteor
            float angle = 0;
            for (int i = 1; i <= size; i++)
            {
                angle = Engine.anglerange(i * (360 / size));


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
