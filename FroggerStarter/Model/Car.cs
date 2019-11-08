using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// Handles Cars, which are Vehicles
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.Vehicle" />
    public class Car : Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
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

    }
}
