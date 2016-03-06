using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterBlaster
{
    class Engine
    {
        private List<Bullet> bulletlist;
        private List<Meteor> meteorlist;

        public Engine()
        {
            bulletlist = new List<Bullet>();
            meteorlist = new List<Meteor>();

        }
        public void nextframe()
        {
            //compute new bullet positions
            bulletlist.ForEach(b => b.nextframe());

            //remove dead bullets
            bulletlist.RemoveAll(b => b.health <= 0);

            //compute new meteor positions
            meteorlist.ForEach(m => m.nextframe());

        }
        public void draw()
        {
            bulletlist.ForEach(b => b.draw());
            meteorlist.ForEach(m => m.draw());
        }
        public void addBullet(Bullet b)
        {
            bulletlist.Add(b);
        }
        public void addMeteor()
        {
            meteorlist.Add(new Meteor());
        }
    }
}