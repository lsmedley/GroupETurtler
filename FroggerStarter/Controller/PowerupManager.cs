using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Manages powerups, including where and when they appear.
    /// </summary>
    public class PowerupManager
    {
        private Powerup timePowerup;
        private Random random;
        private int timePowerTick = 0;

        private const double TimeAppearChance = 0.01;
        private const int TimeDisappearTick = 200;
        private const int TileWidth = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerupManager"/> class.
        /// </summary>
        public PowerupManager()
        {
            this.timePowerup = new Powerup(new TimePowerUpSprite());
            this.random = new Random();
        }

        /// <summary>
        /// Called when [tick].
        /// </summary>
        /// <param name="maxRight">The maximum right.</param>
        /// <param name="maxDown">The maximum down.</param>
        public void OnTick(double maxRight, double minDown, double maxDown)
        {
            double appears = this.random.NextDouble();
            int xLocal = (this.random.Next(Convert.ToInt32(maxRight)) / TileWidth) * TileWidth;
            int yLocal = (this.random.Next(Convert.ToInt32(minDown), Convert.ToInt32(maxDown)) / TileWidth) * TileWidth;

            if (this.timePowerup.IsActive)
            {
                this.timePowerTick++;
            }

            if (appears <= TimeAppearChance && !this.timePowerup.IsActive)
            {
                this.timePowerup.X = xLocal;
                this.timePowerup.Y = yLocal;
                this.timePowerup.Activate();
            }

            if (this.timePowerTick >= TimeDisappearTick)
            {
                this.deactivateTimePowerUp();
            }
        }

        private void deactivateTimePowerUp()
        {
            this.timePowerup.Deactivate();
            this.timePowerTick = 0;
        }

        /// <summary>
        /// Gets the time sprite.
        /// </summary>
        /// <returns>the time powerup's sprite.</returns>
        public BaseSprite GetTimeSprite()
        {
            return this.timePowerup.Sprite;
        }

        public bool CheckCollision(GameObject player, bool playerManagerDisabled)
        {
            if (this.timePowerup.IsColliding(player) && !playerManagerDisabled && this.timePowerup.IsActive)
            {
                this.deactivateTimePowerUp();
                return true;
            }

            return false;
        }
    }
}
