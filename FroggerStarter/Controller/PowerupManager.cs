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
        private const double VehicleAppearChance = 0.005;
        private const int PowerUpDisappearTick = 200;
        private const int TileWidth = 50;
        private readonly Powerup timePowerup;
        private readonly Powerup vehiclePowerup;
        private readonly Random random;
        private int timePowerTick;
        private int vehiclePowerTick;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PowerupManager" /> class.
        /// </summary>
        public PowerupManager()
        {
            this.timePowerup = new Powerup(new TimePowerUpSprite());
            this.vehiclePowerup = new Powerup(new VehiclePowerUpSprite());
            this.random = new Random();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Called when timer ticks
        /// </summary>
        /// <param name="maxRight">The maximum right.</param>
        /// <param name="minDown">The minimum down.</param>
        /// <param name="maxDown">The maximum down.</param>
        public void OnTick(double maxRight, double minDown, double maxDown)
        {
            this.tickTimePowerUp(maxRight, minDown, maxDown);
            this.tickVehiclePowerUp(maxRight, minDown, maxDown);
        }

        private void tickVehiclePowerUp(double maxRight, double minDown, double maxDown)
        {
            var appears = this.random.NextDouble();

            if (this.vehiclePowerup.IsActive)
            {
                this.vehiclePowerTick++;
            }

            if (appears <= VehicleAppearChance && !this.vehiclePowerup.IsActive)
            {
                this.placeVehiclePowerUp(maxRight, minDown, maxDown);
                this.vehiclePowerup.Activate();
            }

            if (this.vehiclePowerTick >= PowerUpDisappearTick)
            {
                this.deactivateVehiclePowerUp();
            }
        }

        private void placeVehiclePowerUp(double maxRight, double minDown, double maxDown)
        {
            var xLocal = this.random.Next(Convert.ToInt32(maxRight)) / TileWidth * TileWidth;
            var yLocal = this.random.Next(Convert.ToInt32(minDown), Convert.ToInt32(maxDown)) / TileWidth * TileWidth;

            this.vehiclePowerup.X = xLocal;
            this.vehiclePowerup.Y = yLocal;
        }

        private void tickTimePowerUp(double maxRight, double minDown, double maxDown)
        {
            var appears = this.random.NextDouble();

            if (this.timePowerup.IsActive)
            {
                this.timePowerTick++;
            }

            if (appears <= TimeAppearChance && !this.timePowerup.IsActive)
            {
                this.placeTimePowerUp(maxRight, minDown, maxDown);
                this.timePowerup.Activate();
            }

            if (this.timePowerTick >= PowerUpDisappearTick)
            {
                this.deactivateTimePowerUp();
            }
        }

        private void placeTimePowerUp(double maxRight, double minDown, double maxDown)
        {
            var xLocal = this.random.Next(Convert.ToInt32(maxRight)) / TileWidth * TileWidth;
            var yLocal = this.random.Next(Convert.ToInt32(minDown), Convert.ToInt32(maxDown)) / TileWidth * TileWidth;

            this.timePowerup.X = xLocal;
            this.timePowerup.Y = yLocal;
        }

        private void deactivateTimePowerUp()
        {
            this.timePowerup.Deactivate();
            this.timePowerTick = 0;
        }

        private void deactivateVehiclePowerUp()
        {
            this.vehiclePowerup.Deactivate();
            this.vehiclePowerTick = 0;
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
        ///     Gets the vehicle powerup sprite.
        /// </summary>
        /// <returns>the vehicle powerup sprite.</returns>
        public BaseSprite GetVehicleSprite()
        {
            return this.vehiclePowerup.Sprite;
        }

        /// <summary>
        ///     Checks the collision with the time powerup.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="playerManagerDisabled">if set to <c>true</c> [player manager disabled].</param>
        /// <returns>true if collision with the time powerup, false if not</returns>
        public bool CheckTimeCollision(GameObject player, bool playerManagerDisabled)
        {
            if (this.timePowerup.IsColliding(player) && !playerManagerDisabled && this.timePowerup.IsActive)
            {
                this.deactivateTimePowerUp();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Checks the collision with the vehicle powerup.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="playerManagerDisabled">if set to <c>true</c> [player manager disabled].</param>
        /// <returns>true if collision with the vehicle powerup, false if not</returns>
        public bool CheckVehicleCollision(GameObject player, bool playerManagerDisabled)
        {
            if (this.vehiclePowerup.IsColliding(player) && !playerManagerDisabled && this.vehiclePowerup.IsActive)
            {
                this.deactivateVehiclePowerUp();
                return true;
            }

            return false;
        }

        #endregion
    }
}