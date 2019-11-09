using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Holds functionality for a generic powerup object.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    class Powerup : GameObject
    {
        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Powerup" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public Powerup(BaseSprite sprite)
        {
            this.Sprite = sprite;
            this.IsActive = false;
            this.Sprite.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Activates this instance.
        /// Postcondition: This.IsActive == true, this.Sprite is visible.
        /// </summary>
        public void Activate()
        {
            this.IsActive = true;
            this.Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Deactivates this instance.
        /// Postcondition: this.IsActive == false, this.Sprite is collapsed.
        /// </summary>
        public void Deactivate()
        {
            this.IsActive = false;
            this.Sprite.Visibility = Visibility.Collapsed;
        }
    }
}
