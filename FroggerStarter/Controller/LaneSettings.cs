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
        /// The max number and type of vehicles in each lane.
        /// </summary>
        /// <value>
        /// The max number and type of vehicles.
        /// </value>
        public IList<(int, VehicleType)> Vehicles { get; }
        /// <summary>
        /// The traffic direction for each lane.
        /// </summary>
        /// <value>
        /// The traffic directions for each lane.
        /// </value>
        public IList<Direction> TrafficDirections { get; }
        /// <summary>
        /// Gets the traffic speeds for each lane.
        /// </summary>
        /// <value>
        /// The traffic speeds for each lane.
        /// </value>
        public IList<int> TrafficSpeeds { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaneSettings" /> class.
        /// </summary>
        /// <param name="vehicles">The type and max number of vehicles in this lane.</param>
        /// <param name="flows">The direction of traffic of each lane.</param>
        /// <param name="speeds">The speed of each lane.</param>
        public LaneSettings(IList<(int, VehicleType)> vehicles, IList<Direction> flows, IList<int> speeds)
        {
            this.Vehicles = vehicles;
            this.TrafficDirections = flows;
            this.TrafficSpeeds = speeds;
        }
    }
}
