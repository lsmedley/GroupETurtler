using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Utils;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.VehicleSprites;

namespace FroggerStarter.Model.Vehicle
{
    /// <summary>
    /// Holds functionality for a car towing a trailer.
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle.Vehicle" />
    public class TowingCar : Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TowingCar"/> class.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="direction">The direction.</param>
        public TowingCar(int speed, Direction direction)
        {
            Speed = speed;
            Direction = direction;
            Sprite = new TowingCarSprite();
            RenderSprite();
        }
    }
}
