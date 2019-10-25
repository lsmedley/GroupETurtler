using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Holds logic for home spaces.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    class HomeSpace : GameObject
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
            this.Sprite = new HomeSprite();
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
            if (base.IsColliding(other))
            {
                this.IsTaken = false;
            }

            return base.IsColliding(other);
        }
    }
}
