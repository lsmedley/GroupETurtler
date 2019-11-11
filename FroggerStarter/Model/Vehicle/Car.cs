using FroggerStarter.Utils;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    ///     Handles Cars, which are Vehicles
    /// </summary>
    /// <seealso cref="Vehicle" />
    public class Car : Vehicle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Car" /> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        public Car(int speed, Direction direction)
        {
            Speed = speed;
            Direction = direction;
            Sprite = new CarSprite();
            RenderSprite();
        }

        #endregion
    }
}