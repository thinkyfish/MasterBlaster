﻿using System;
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
        private int bulletcycle;
        private bool firing;

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
        public const int METEOR_BLAST = 500;
        public const float METEOR_RFUDGE = 0.9f;
        public const int METEOR_MINSIZE = 4;

        public const float BULLET_SIZE = 3.0f;
        public const float BULLET_VEL = 2.0f;
        public const float BULLET_RANGE = 10f;
        public const int BULLET_DAMAGE = 300;
        public const int FIRE_CYCLE = 100;
        public const float SHIP_TURN = 4.0f;
        public const float SHIP_THRUST = 1.4f;
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


        

        public static float dist2(float x1, float y1, float x2, float y2)
        {
            double distx = x2 - x1;
            double disty = y2 - y1;
            return (float)Math.Sqrt(distx * distx + disty * disty);
        }


        public static float dist(float[] dot, float x, float y)
        {
            double distx = dot[0] - x;
            double disty = dot[1] - y;
            return (float)Math.Sqrt(distx * distx + disty * disty);
        }


        public static float glrange(float a)
        {
            if (a > 1.01f)
                a -= 2.02f;
            else if (a < -1.01f)
                a += 2.02f;
            if (a < 1.01f && a > -1.01f)
                return a;
            else
                return -2f;
        }

        



        public Engine()
        {
            this.bulletlist = new List<Bullet>();
            this.meteorlist = new List<Meteor>();
            this.explosionlist = new List<Explosion>();
            ship = new Ship();
            this.bulletcycle = 0;
        }
        private bool cyclebullet()
        {
            bool fire = false;
            if (bulletcycle == 0)
                fire = true;
            // cycle the value 
            this.bulletcycle += 1;
            this.bulletcycle %= FIRE_CYCLE;
            return fire;
        }
        public void startfiring()
        {
            //reset bullet cycle
            if(!firing)
                bulletcycle = 0;
            firing = true;
        }
        public void stopfiring()
        {
            //bulletcycle = 0;
            firing = false;
        }
        public void fire_bullet() {
            if (firing && cyclebullet())
            {
                float x = ship.pts[0, 0] + ship.pos[0];
                float y = ship.pts[0, 1] + ship.pos[1];

                float vel = BULLET_VEL;
                Bullet b = new Bullet(x, y, ship.angle, vel);
                b.ax = ship.ax + ship.fx;
                bulletlist.Add(b);
            }
        }
        
        public void nextframe(float dt = Engine.DT)
        {
            //fire bullets if needed
            if (firing) fire_bullet();

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
            foreach (Bullet b in bulletlist)
            {
                foreach (Meteor m in meteorlist)
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

                                    int newsize = (m.size / numkids);
                                    if (newsize < 4) { newsize = 4; }

                                    kidlist.Add(new Meteor(newsize, kx, ky, angle + r_ang));
                                }

                            }



                            
                        }

                        
                    }
                }
                //add the kids to the list of meteors *outside* of the foreach
                if (kidlist.Count > 0)
                {
                    meteorlist.AddRange(kidlist);
                }


            }

            //remove dead bullets
            bulletlist.RemoveAll(b => b.health <= 0);

            //remove dead meteors
            meteorlist.RemoveAll(m => m.health <= 0.0f);
            //check for meteor collisions
            //TODO

            //compute ship position
            if (ship != null) ship.nextframe(dt);

        }
        public void draw()
        {
            bulletlist.ForEach(b => b.draw());
            meteorlist.ForEach(m => m.draw());
            explosionlist.ForEach(e => e.draw());
            if(ship != null) ship.draw();
        }
        public void addBullet(Bullet b)
        {
            bulletlist.Add(b);
        }
        public void newMeteor(int size = METEOR_PTS)
        {
            float mx = ship.pos[0];
            float my = ship.pos[1];

            // generate a position -1 to 1 that is at least .25 away from the ship
            while (dist(ship.pos, mx, my) < 0.25f)
            {
                
                mx = (float)(rand.NextDouble() * 2.0 - 1.0); 
                my = (float)(rand.NextDouble() * 2.0 - 1.0);

            }
            meteorlist.Add(new Meteor(size, mx, my));
        }
        public void addShip(float x = 0.0f, float y = 0.0f)
        {
            ship = new Ship(x, y) ;
        }

    }
}