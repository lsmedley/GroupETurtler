using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines the player model
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 50;
        private const int SpeedYDirection = 50;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Changes the sprite.
        ///     Postcondition: this.Sprite = the specified sprite.
        /// </summary>
        /// <param name="sprite">The sprite.</param>
        public void ChangeSprite(BaseSprite sprite)
        {
            Sprite = sprite;
        }

        #endregion
    }
}