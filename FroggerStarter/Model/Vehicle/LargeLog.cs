using FroggerStarter.Utils;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    ///     Holds functionality for large logs.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle.Vehicle" />
    public class LargeLog : Vehicle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LargeLog" /> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        public LargeLog(int speed, Direction direction)
        {
            Speed = speed;
            Direction = direction;
            Sprite = new LargeLogSprite();
            RenderSprite();
        }

        #endregion
    }
}