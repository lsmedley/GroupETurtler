using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Handles Busses, which are Vehicles
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle" />
    public class Bus : Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bus"/> class.
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
    }
}
