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
        private List<Bullet> bulletlist;
        private List<Meteor> meteorlist;
        private List<Explosion> explosionlist;

        public Engine()
        {
            bulletlist = new List<Bullet>();
            meteorlist = new List<Meteor>();
            explosionlist = new List<Explosion>();

        }
        public void nextframe(float dt = common.DT)
        {

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
                            int numkids = (common.rand.Next() % 2) + 2;
                            float angle = b.angle;

                            // large meteors get kids, small ones don't
                            if (m.size > 6)
                            {
                                //initialize each kid meteor and add to the kidlist
                                for (int i = 0; i < numkids; i++)
                                {
                                    float r_ang = (float)(common.rand.Next() % 160 - 80);

                                    float kx = m.rad * (float)Math.Cos((angle + r_ang) * common.PI / 180f);
                                    float ky = m.rad * (float)Math.Sin((angle + r_ang) * common.PI / 180f);
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



        }
        public void draw()
        {
            bulletlist.ForEach(b => b.draw());
            meteorlist.ForEach(m => m.draw());
            explosionlist.ForEach(e => e.draw());
        }
        public void addBullet(Bullet b)
        {
            bulletlist.Add(b);
        }
        public void addMeteor()
        {
            meteorlist.Add(new Meteor(16,(float)common.rand.NextDouble(), (float)common.rand.NextDouble()));
        }

    }
}