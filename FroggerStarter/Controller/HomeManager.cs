using System;
using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{

    /// <summary>
    /// Manages logic pertaining to home spaces.
    /// </summary>
    /// <seealso cref="System.Collections.Generic.IEnumerable{FroggerStarter.Model.HomeSpace}" />
    public class HomeManager : IEnumerable<HomeSpace>
    {
        /// <summary>
        /// The homespaces
        /// </summary>
        private readonly IList<HomeSpace> homes;
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeManager"/> class.
        /// </summary>
        /// <param name="heightOffset">The height offset.</param>
        /// <param name="wallWidth">Width of the wall.</param>
        /// <param name="numHomes">The number homes.</param>
        public HomeManager(int heightOffset, double wallWidth, int numHomes)
        {
            this.homes = new List<HomeSpace>();
            var spacing = wallWidth / numHomes;
            for (var i = 0; i < numHomes; i++)
            {
                this.homes.Add(new HomeSpace());
                var x = Math.Round((wallWidth - spacing * (i + 1) + 50) / 50.0) * 50.0;
                this.homes[i].X = x;
                this.homes[i].Y = heightOffset;
            }

            this.homes[0].X -= 50;
            this.homes[1].X -= 50;
            this.homes[this.homes.Count - 1].X += 50;
        }

        /// <summary>
        /// Checks if this home space has been collided with.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns></returns>
        public int CheckCollision(GameObject g)
        {
            foreach (var v in this.homes)
            {
                if (!v.IsTaken && v.IsColliding(g))
                {
                    return this.homes.IndexOf(v);
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the homes.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the homes.
        /// </returns>
        public IEnumerator<HomeSpace> GetEnumerator()
        {
            return this.homes.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.homes.GetEnumerator();
        }
    }
}
