using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines basic properties and behavior of vehicle objects.
    /// </summary>
    public class Vehicle : GameObject
    {
        private readonly Direction direction;

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type of vehicle.
        /// </value>
        public VehicleType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="type">The type of vehicle.</param>
        /// <param name="direction">The direction of the vehicle.</param>
        /// <param name="spd">The speed of the vehicle.</param>
        public Vehicle(VehicleType type, Direction direction, int spd)
        {
            this.Type = type;
            switch (type)
            {
                case VehicleType.Car:
                    Sprite = new CarSprite();
                    break;
                case VehicleType.Bus:
                    Sprite = new BusSprite();
                    break;
                default:
                    Sprite = null;
                    break;
            }

            if (direction == Direction.Right)
            {
                var baseSprite = Sprite;
                if (baseSprite != null)
                {
                    baseSprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    baseSprite.RenderTransform = new ScaleTransform {ScaleX = -1};
                }
            }

            this.direction = direction;

            SetSpeed(spd, 0);
        }
        /// <summary>
        /// Moves this vehicle forward.
        /// Postcondition: if this.direction == Direction.Left, this.X -= this.Speed.
        /// else this.X += this.Speed.
        /// </summary>
        public void MoveForward()
        {
            if (this.direction == Direction.Left)
            {
                MoveLeft();
            }
            else
            {
                MoveRight();
            }
        }
    }
}
