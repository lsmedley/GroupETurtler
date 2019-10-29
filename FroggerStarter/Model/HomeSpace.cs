using System;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Holds logic for home spaces.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class HomeSpace : GameObject
    {
        /// <summary>
        /// Gets or sets a value indicating whether this space is taken.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this space is taken; otherwise, <c>false</c>.
        /// </value>
        public bool IsTaken { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeSpace" /> class.
        /// </summary>
        public HomeSpace()
        {
            Sprite = new HomeSprite();
            this.IsTaken = false;
        }

        /// <summary>
        /// Determines whether the specified other is colliding.
        /// Postcondition: this.IsTaken == true if the specified other is colliding.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>
        ///   <c>true</c> if the specified other is colliding; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsColliding(GameObject other)
        {
            var isCol = false;

            var tolerance = 0.1;
            if (Math.Abs(other.X - X) < tolerance && Math.Abs(other.Y - Y) < tolerance)
            {
                this.IsTaken = true;
                isCol = true;
            }

            return isCol;
            //if (base.IsColliding(other))
            //{
            //    this.IsTaken = true;
            //}

            //return base.IsColliding(other);
        }
    }
}
