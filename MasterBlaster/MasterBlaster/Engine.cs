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
    class Engine
    {
        //entit
        private List<Bullet> bulletlist;
        private List<Meteor> meteorlist;
        private List<Explosion> explosionlist;
        private Ship ship;
        private int bulletcycle = 0;
        private bool firing;
		private bool fire;
        private bool thrusting;
        private float turning;
        public int level = 8;
        private bool gameover;
        public int startinglevel = 8;
		public int score;
		public bool scorechanged = true;
        //This is the rng for the whole game
        public static Random rand = new Random();

        public const float DOT_SIZE = 8.0f;
        public const int MAX_NUMDOTS = 100;
        public const int GRABNEAR = 6;
        public const float PI = 3.14159265f;

        public static Color TEXT_COLOR = Color.White;
        public const float TIMEOUT = 0.05f;

        public const int METEOR_PTS = 16;
        public const int METEOR_VALUE = 100;
        public const float METEOR_HEALTH = 300.0f;
        public const float METEOR_DRAG = 0.1f;
        public const float METEOR_BLAST = 80000.0f;
        public const float METEOR_RFUDGE = 0.9f;
        public const int METEOR_MINSIZE = 4;

        public const float BULLET_SIZE = 3.0f;
        public const float BULLET_VEL = 10.0f;
        public const float BULLET_RANGE = 0.008f;  //this is range squared
        public const int BULLET_DAMAGE = 100;
        public const int FIRE_CYCLE = 8;
        public const float SHIP_TURN = 3.0f;
        public const float SHIP_THRUST = 100.4f;
        public const float SHIP_DRAG = 0.008f;

        public const float MAX_VEL = 3;

        public const float EASY = 5;
        public const float MEDIUM = 10;
        public const float HARD = 20;

        public const float OVERDELAY = 6;


        //PHYSICS STUFF

        public const float DROPOFF = 0.1f;
        public const float MASS = 1;

        public const float DT = 0.001f;

        public const int NAMELENGTH = 3;
        public const int NUMSCORES = 8;
        public const string SCOREFILE = "scores.dat";
        public const string SCOREFILEBAK = "scores.dat~";
		public static float ViewX = 2.0f;
		
		public static float ViewY = 1.0f;

        public void nextlevel(int l)
        {
            ship = new Ship();
            meteorlist = new List<Meteor>();
            bulletlist = new List<Bullet>();
            int i;
            for(i = 0; i < l; i++)
            {
                newMeteor((l + 8) / 2);
            }
        }

        public static bool dist2(float x1, float y1, float x2, float y2, float test )
        {
            float distx = x2 - x1;
            float disty = y2 - y1;
            return (test * test) > (distx * distx + disty * disty);
        }


        public static float distsq(float[] dot, float x, float y)
        {
            float distx = dot[0] - x;
            float disty = dot[1] - y;
            return distx * distx + disty * disty;
        }


		public static float glrange(float a, float bound = 1.0f)
        {
            //if (a > 1.015625f)
            //    a -= 2.03125f;
            //else if (a < -1.015625f)
            //    a += 2.03125f;
            //if (a < 1.015625f && a > -1.015625f)
            //    return a;
            //else
            //    return -2f;
            if (a > bound)
                a -= 2.0f*bound;
            else if (a < -bound)
                a += 2.0f*bound;

            return a;

        }

        public static float anglerange(float angle)
        {
            float newangle = angle % 360.0f;
            if (newangle < 0.0f)
                newangle = 360.0f - newangle;
            return newangle;
        }



        public Engine(int level = 8)
        {
            
            this.explosionlist = new List<Explosion>();
            this.bulletcycle = 0;
            this.startinglevel = level;
            this.nextlevel(level);
        }
        private bool cyclebullet()
        {
            bool fire = false;

			if (bulletcycle == 0)
				fire = true;
			else if( this.bulletcycle > 0 )
				this.bulletcycle -= 1;
            
            return fire;
        }
        public void startfiring()
        {
            //reset bullet cycle
            ////if (!firing)
            //    bulletcycle = 0;
            firing = true;
			fire_bullet ();

        }
        public void stopfiring()
        {
            //bulletcycle = 0;

				
				firing = false;
		
        }
        public void fire_bullet()
        {
			if (cyclebullet() && firing)
            {
                float x = ship.pts[0, 0] + ship.position[0];
                float y = ship.pts[0, 1] + ship.position[1];

                float vel = BULLET_VEL;
                Bullet b = new Bullet(x, y, ship.angle, vel);
                b.ax = ship.ax + ship.fx;
                bulletlist.Add(b);
				bulletcycle = FIRE_CYCLE;
				score -= 1;
				//firing = false;
            }
        }
        public void startThrusting(float direction = 1.0f)
        {
            thrusting = true;
            ship.engine(direction * SHIP_THRUST);
        }
        public void stopThrusting()
        {
            thrusting = false;
            ship.engine(0);
        }
        public void startTurning(float direction)
        {
            turning = direction;
        }
        public void stopTurning()
        {
            turning = 0.0f;
        }
        public void nextframe(float dt = Engine.DT)
        {
            //fire bullets if needed
            fire_bullet();
            if(turning != 0.0f)
            {
                ship.angle += turning * SHIP_TURN;
            }

            //remove dead meteors
            meteorlist.RemoveAll(m => m.health <= 0.0f);

            //remove dead bullets
            bulletlist.RemoveAll(b => b.health <= 0);

            //remove dead explosions
            explosionlist.RemoveAll(e => e.health <= 0);

            //compute new bullet positions
            bulletlist.ForEach(b => b.nextframe(dt));

            //compute new meteor positions
            meteorlist.ForEach(m => m.nextframe(dt));

            List<Meteor> kidlist = new List<Meteor>();

            //check for bullet collisions with meteors
            foreach (Meteor m in meteorlist) 
            {
                foreach (Bullet b in bulletlist)
                {
                    //resolve the effects of a collision
                    if ((m.health > 0.0f) && (b.health > 0) && m.isWithin(b.pos[0], b.pos[1]))
                    {
                        //knock the meteor
                        m.health = m.health - b.damage;
                        m.blast(b.angle);

                        //add an explosion effect
                        explosionlist.Add(new Explosion(b.pos[0], b.pos[1], b.angle));

                        //bullet is dead
                        b.health = 0;

                        //check for dead meteors and if so, split into smaller ones.
                        if (m.health <= 0.0f)
                        {
                            int numkids = (Engine.rand.Next() % 2) + 2;
                            float angle = b.angle;
							score += m.value;
							scorechanged = true;

                            // large meteors get kids, small ones don't
                            if (m.size > 6)
                            {
                                //initialize each kid meteor and add to the kidlist
                                for (int i = 0; i < numkids; i++)
                                {
                                    float r_ang = (float)(Engine.rand.Next() % 160 - 80);

                                    float kx = m.rad * (float)Math.Cos((angle + r_ang) * Engine.PI / 180f);
                                    float ky = m.rad * (float)Math.Sin((angle + r_ang) * Engine.PI / 180f);
                                    kx += m.pos[0];
                                    ky += m.pos[1];

                                    int newsize = (m.size / numkids) ;
                                    if (newsize < 4) { newsize = 4; }

                                    kidlist.Add(new Meteor(newsize, kx, ky, angle + r_ang));
                                }

                            }




                        }


                    }
                }

                //check for ship collisions
				if (dist2 (m.pos [0], m.pos [1], ship.position [0], ship.position [1], m.rad + ship.rad)) {
					for (int i = 0; i < ship.pts.GetLength (0); i++) {
						if ((m.health > 0.0f) && (ship.health > 0.0f) &&
						                  m.isWithin (ship.position [0] + ship.pts [i, 0], ship.position [1] + ship.pts [i, 1])) {
							ship.health = 0;                       
						}
					}
				}

            }
            //add the kids to the list of meteors *outside* of the foreach
            if (kidlist.Count > 0)
            {
                meteorlist.AddRange(kidlist);
            }
            //remove dead bullets
            bulletlist.RemoveAll(b => b.health <= 0);

            //remove dead meteors
            meteorlist.RemoveAll(m => m.health <= 0.0f);
            //check for meteor collisions
            //TODO

            //go up to the next level if there are no meteors left
            if(meteorlist.Count == 0)
            {
                // delete any active bullets
                nextlevel(++level);
            }

            //if ship is dead, start at first level
            if (ship.health == 0.0f)
                nextlevel(startinglevel);
            //compute ship position
            if (ship != null) ship.nextframe(dt);

        }
        public void draw()
        {
            bulletlist.ForEach(b => b.draw());
            meteorlist.ForEach(m => m.draw());
            explosionlist.ForEach(e => e.draw());
            if (ship != null) ship.draw();
        }
        public void addBullet(Bullet b)
        {
            bulletlist.Add(b);
        }
        public void newMeteor(int size = METEOR_PTS)
        {
            float mx = ship.position[0];
            float my = ship.position[1];

			float maxrad =  0.2f * ((float)size / (float)Engine.METEOR_PTS);
            // generate a position -1 to 1 that is at least .1 away from the ship
			while (dist2(ship.position[0],ship.position[1], mx, my, maxrad + 0.1f))
            {
				float width = ViewY * 2;
				float height = ViewX * 2;
                mx = (float)(rand.NextDouble() * width - ViewX);
				my = (float)(rand.NextDouble() * height - ViewY);

            }
            meteorlist.Add(new Meteor(size, mx, my));
        }
        public void addShip(float x = 0.0f, float y = 0.0f)
        {
            ship = new Ship(x, y);
        }

    }
}