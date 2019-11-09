using System;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages powerups, including where and when they appear.
    /// </summary>
    public class PowerupManager
    {
        #region Data members

        private const double TimeAppearChance = 0.01;
        private const int TimeDisappearTick = 200;
        private const int TileWidth = 50;
        private readonly Powerup timePowerup;
        private readonly Random random;
        private int timePowerTick;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerupManager" /> class.
        /// </summary>
        public PowerupManager()
        {
            this.timePowerup = new Powerup(new TimePowerUpSprite());
            this.random = new Random();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when [tick].
        /// </summary>
        /// <param name="maxRight">The maximum right.</param>
        /// <param name="minDown">The minimum down.</param>
        /// <param name="maxDown">The maximum down.</param>
        public void OnTick(double maxRight, double minDown, double maxDown)
        {
            var appears = this.random.NextDouble();
            var xLocal = this.random.Next(Convert.ToInt32(maxRight)) / TileWidth * TileWidth;
            var yLocal = this.random.Next(Convert.ToInt32(minDown), Convert.ToInt32(maxDown)) / TileWidth * TileWidth;

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
        ///     Gets the time sprite.
        /// </summary>
        /// <returns>the time powerup's sprite.</returns>
        public BaseSprite GetTimeSprite()
        {
            return this.timePowerup.Sprite;
        }

        /// <summary>
        ///     Checks the collision.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="playerManagerDisabled">if set to <c>true</c> [player manager disabled].</param>
        /// <returns></returns>
        public bool CheckCollision(GameObject player, bool playerManagerDisabled)
        {
            if (this.timePowerup.IsColliding(player) && !playerManagerDisabled && this.timePowerup.IsActive)
            {
                this.deactivateTimePowerUp();
                return true;
            }

            return false;
        }

        #endregion
    }
}