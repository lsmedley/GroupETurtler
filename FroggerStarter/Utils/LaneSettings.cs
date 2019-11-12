using System;

namespace FroggerStarter.Utils
{
    /// <summary>
    ///     Defines settings for lanes.
    /// </summary>
    public class LaneSettings
    {
        #region Properties

        /// <summary>
        ///     Gets the start speed.
        /// </summary>
        /// <value>
        ///     The start speed.
        /// </value>
        public int StartSpeed { get; set; }

        /// <summary>
        ///     Gets the direction.
        /// </summary>
        /// <value>
        ///     The direction.
        /// </value>
        public Direction Direction { get; set; }

        /// <summary>
        ///     Gets or sets the maximum number vehicles.
        /// </summary>
        /// <value>
        ///     The maximum number vehicles.
        /// </value>
        public int MaxNumVehicles { get; set; }

        /// <summary>
        ///     Gets the type of the vehicle.
        /// </summary>
        /// <value>
        ///     The type of the vehicle.
        /// </value>
        public VehicleType VehicleType { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LaneSettings" /> class.
        /// </summary>
        /// <param name="startSpeed">The start speed.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="maxNumVehicles">The maximum number vehicles.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        /// <exception cref="ArgumentOutOfRangeException">startSpeed - must be at least 1</exception>
        public LaneSettings(int startSpeed, Direction direction, int maxNumVehicles, VehicleType vehicleType)
        {
            if (startSpeed < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(startSpeed), "must be at least 1");
            }

            this.StartSpeed = startSpeed;
            this.Direction = direction;
            this.MaxNumVehicles = maxNumVehicles;
            this.VehicleType = vehicleType;
        }

        #endregion
    }
}