using FroggerStarter.Utils;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    ///     Handles Busses, which are Vehicles
    /// </summary>
    /// <seealso cref="Vehicle" />
    public class Bus : Vehicle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Bus" /> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        public Bus(int speed, Direction direction)
        {
            Speed = speed;
            Direction = direction;
            Sprite = new BusSprite();
            RenderSprite();
        }

        #endregion
    }
}