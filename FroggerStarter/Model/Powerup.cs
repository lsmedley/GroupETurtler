using Windows.UI.Xaml;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Holds functionality for a generic powerup object.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    internal class Powerup : GameObject
    {
        #region Properties

        /// <summary>
        ///     Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Powerup" /> class.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public Powerup(BaseSprite sprite)
        {
            Sprite = sprite;
            this.IsActive = false;
            Sprite.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Activates this instance.
        ///     Postcondition: This.IsActive == true, this.Sprite is visible.
        /// </summary>
        public void Activate()
        {
            this.IsActive = true;
            Sprite.Visibility = Visibility.Visible;
        }

        /// <summary>
        ///     Deactivates this instance.
        ///     Postcondition: this.IsActive == false, this.Sprite is collapsed.
        /// </summary>
        public void Deactivate()
        {
            this.IsActive = false;
            Sprite.Visibility = Visibility.Collapsed;
        }

        #endregion
    }
}