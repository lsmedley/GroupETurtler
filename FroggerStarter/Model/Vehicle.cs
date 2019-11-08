using Windows.Foundation;
using Windows.UI.Xaml.Media;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Defines basic properties and behavior of vehicle objects.
    /// </summary>
    public abstract class Vehicle : GameObject
    {
        protected int Speed
        {
            get => this.SpeedX;
            set => this.SetSpeed(value, 0);
        }
        protected Direction Direction { get; set; }

        protected void RenderSprite()
        {
            if (this.Direction == Direction.Right)
            {
                var baseSprite = Sprite;
                if (baseSprite != null)
                {
                    baseSprite.RenderTransformOrigin = new Point(0.5, 0.5);
                    baseSprite.RenderTransform = new ScaleTransform { ScaleX = -1 };
                }
            }
        }

        /// <summary>
        /// Moves this vehicle forward.
        /// Postcondition: if this.Direction == Direction.Left, this.X -= this.Speed.
        /// else this.X += this.Speed.
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
    }
}
