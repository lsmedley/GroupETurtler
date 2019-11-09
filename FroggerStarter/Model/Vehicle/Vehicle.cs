using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.Utils;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    ///     Defines basic properties and behavior of vehicle objects.
    /// </summary>
    public abstract class Vehicle : GameObject
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the speed.
        /// </summary>
        /// <value>
        ///     The speed.
        /// </value>
        public int Speed
        {
            get => SpeedX;
            set => SetSpeed(value, 0);
        }

        /// <summary>
        ///     Gets or sets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        protected Direction Direction { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Renders the sprite.
        /// </summary>
        protected void RenderSprite()
        {
            if (this.Direction == Direction.Right)
            {
                var baseSprite = Sprite;
                if (baseSprite != null)
                {
                    baseSprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    baseSprite.RenderTransform = new ScaleTransform {ScaleX = -1};
                }
            }
        }

        /// <summary>
        ///     Moves this vehicle forward.
        ///     Postcondition: if this.Direction == Direction.Left, this.X -= this.Speed.
        ///     else this.X += this.Speed.
        /// </summary>
        public void MoveForward()
        {
            if (this.Direction == Direction.Left)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }

        #endregion
    }
}