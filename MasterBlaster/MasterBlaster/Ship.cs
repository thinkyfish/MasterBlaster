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
        public float angle;

		public float[] position { get; set; }
		public float[,] shadowpos;
		public int shadows = 8;
		public float rad;

        private float vx;
        private float vy;

        public float ax;
        public float ay;

        public float fx;
        public float fy;

        private float mass;

        private float drag;

        private float thrust;

        public float[,] pts;

		private static Color color = Color.White;

        public float health;

        public Ship(float x = 0.0f, float y = 0.0f)
        {
            this.vx = 0.0f;
            this.vy = 0.0f;
            this.ax = 0.0f;
            this.pts = new float[4, 2];
            this.position = new float[2];
            this.position[0] = x;
            this.position[1] = y;
            this.angle = 90.0f;
            this.drag = Engine.SHIP_DRAG;
            //this.color = Color.White;
            this.health = 1.0f;
			this.rad = 0.06f;
			this.shadowpos = new float[shadows, 2];
        }

        public void draw()
        {
			drawshipimage (position[0], position[1]);
			//loop over the possible intruding edges, checking if any are visible and if so, draw them.
			if (position[0] > 1.0f - rad || position[0] < -1.0f + rad ||
				position[1] > 1.0f - rad || position[1] < -1.0f + rad)
			{
				for (int i = 0; i < shadows; i++)
				{
					//if (onscreen(shadowpos[i, 0], shadowpos[i, 1])) // don't need this. its faster to just draw them all.
					//{
					drawshipimage(shadowpos[i, 0], shadowpos[i, 1]);
					//}
				}
			}
				
        }
		public void drawshipimage(float xpos, float ypos){

			//draw black background
			GL.Color3(Color.Black);
			GL.Begin(PrimitiveType.TriangleFan);
			GL.Vertex2 (xpos, ypos);
			for (int i = 0; i < 4; i++)
			{
				GL.Vertex2(pts[i, 0] + xpos, pts[i, 1] + ypos);
			}

			GL.Vertex2(pts[0, 0] + xpos, pts[0, 1] + ypos);
			GL.End();

			//draw the edge lines
			GL.LineWidth(1.0f);
			GL.Color3(color);


			GL.Begin(PrimitiveType.LineStrip);
			for (int i = 0; i < 4; i++)
				GL.Vertex2(xpos + pts[i, 0],
					ypos + pts[i, 1]);

			//connect last to first
			GL.Vertex2(xpos + pts[0, 0],
				ypos + pts[0, 1]);

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
			position[0] = Engine.glrange(position[0] + dsx, Engine.ViewX);
			position[1] = Engine.glrange(position[1] + dsy, Engine.ViewY);
			setshadow (position);
			setAngle (angle);
            
        }

		public void setAngle(float angle){
			//define the size and shape of the ship

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


		}


		//set the shadow position array to all possible corresponding positions outside the field of view.
		public void setshadow(float[] position)
		{
			float xsize = Engine.ViewX * 2.0f;
			float ysize = Engine.ViewY * 2.0f;

			shadowpos[0, 0] = position[0];
			shadowpos[0, 1] = position[1] + ysize;

			shadowpos[1, 0] = position[0] + xsize;
			shadowpos[1, 1] = position[1];

			shadowpos[2, 0] = position[0];
			shadowpos[2, 1] = position[1] - ysize;

			shadowpos[3, 0] = position[0] - xsize;
			shadowpos[3, 1] = position[1];

			shadowpos[4, 0] = position[0] - xsize;
			shadowpos[4, 1] = position[1] + ysize;

			shadowpos[5, 0] = position[0] + xsize;
			shadowpos[5, 1] = position[1] - ysize;

			shadowpos[6, 0] = position[0] - xsize;
			shadowpos[6, 1] = position[1] - ysize;

			shadowpos[7, 0] = position[0] + xsize;
			shadowpos[7, 1] = position[1] + ysize;



		}


    }
}
