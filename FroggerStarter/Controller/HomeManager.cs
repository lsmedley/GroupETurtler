using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{

    /// <summary>
    /// Manages logic pertaining to home spaces.
    /// </summary>
    public class HomeManager : IEnumerable<HomeSpace>
    {
        private readonly IList<HomeSpace> homes;
        private const int HomeTileWidth = 50;
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
                var x = Math.Round((wallWidth - spacing * (i + 1) + HomeTileWidth) / HomeTileWidth) * HomeTileWidth;
                this.homes[i].X = x;
                this.homes[i].Y = heightOffset;
            }

            this.homes[0].X -= HomeTileWidth;
            this.homes[1].X -= HomeTileWidth;
            this.homes[this.homes.Count - 1].X += HomeTileWidth;
        }

        /// <summary>
        /// Checks if one of the home spaces has been collided with.
        /// </summary>
        /// <param name="gameObject">The gameObject.</param>
        /// <returns>Whether one of the home spaces has been collided with.</returns>
        public bool CheckCollision(GameObject gameObject)
        {
            foreach (var home in this.homes)
            {
                if (!home.IsTaken && home.IsColliding(gameObject))
                {
                    return true;
                }
            }

            return false;
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
