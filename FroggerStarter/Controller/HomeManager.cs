using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    class HomeManager : IEnumerable<HomeSpace>
    {
        private IList<HomeSpace> homes;
        public HomeManager(int heightOffset, double wallWidth, int numHomes)
        {
            this.homes = new List<HomeSpace>();
            double spacing = wallWidth / numHomes;
            for (int i = 0; i < numHomes; i++)
            {
                this.homes.Add(new HomeSpace());
                //double x = Math.Round((wallWidth - 50 - spacing * (i + 1)) / 50.0) * 50.0;
                double x = Math.Round((wallWidth - spacing * (i + 1) + 50) / 50.0) * 50.0;
                this.homes[i].X = x;
                this.homes[i].Y = heightOffset;
            }
        }

        public int CheckCollision(GameObject g)
        {
            if (g.Y < 101)
            {
                foreach (var v in this.homes)
                {
                    if (!v.IsTaken && v.IsColliding(g))
                    {
                        return this.homes.IndexOf(v);
                    }
                }
            }
            

            return -1;
        }

        public IEnumerator<HomeSpace> GetEnumerator()
        {
            return this.homes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.homes.GetEnumerator();
        }
    }
}
