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

        public VehicleType Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vehicle"/> class.
        /// </summary>
        /// <param name="type">The type of vehicle.</param>
        /// <param name="dir">The direction of the vehicle.</param>
        /// <param name="spd">The speed of the vehicle.</param>
        public Vehicle(VehicleType type, Direction dir, int spd)
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

            if (dir == Direction.Right)
            {
                var baseSprite = Sprite;
                if (baseSprite != null)
                {
                    baseSprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    baseSprite.RenderTransform = new ScaleTransform {ScaleX = -1};
                }
            }

            this.direction = dir;

            SetSpeed(spd, 0);
        }
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
