using FroggerStarter.Utils;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    ///     Holds functionality for a small log.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle.Vehicle" />
    public class SmallLog : Vehicle
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SmallLog" /> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        public SmallLog(int speed, Direction direction)
        {
            Speed = speed;
            Direction = direction;
            Sprite = new SmallLogSprite();
            RenderSprite();
        }

        #endregion
    }
}