using System.Collections.Generic;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Defines settings for lanes.
    /// </summary>
    public class LaneSettings
    {
        /// <summary>
        /// Gets the start speed.
        /// </summary>
        /// <value>
        /// The start speed.
        /// </value>
        public int StartSpeed { get; }
        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction { get; }
        /// <summary>
        /// Gets the maximum number vehicles.
        /// </summary>
        /// <value>
        /// The maximum number vehicles.
        /// </value>
        public int MaxNumVehicles { get; }
        /// <summary>
        /// Gets the type of the vehicle.
        /// </summary>
        /// <value>
        /// The type of the vehicle.
        /// </value>
        public VehicleType VehicleType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaneSettings"/> class.
        /// </summary>
        /// <param name="startSpeed">The start speed.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="maxNumVehicles">The maximum number vehicles.</param>
        /// <param name="vehicleType">Type of the vehicle.</param>
        public LaneSettings(int startSpeed, Direction direction, int maxNumVehicles, VehicleType vehicleType)
        {
            this.StartSpeed = startSpeed;
            this.Direction = direction;
            this.MaxNumVehicles = maxNumVehicles;
            this.VehicleType = vehicleType;
        }
    }
}
