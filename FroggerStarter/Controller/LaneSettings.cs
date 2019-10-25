using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Defines settings for lanes.
    /// </summary>
    public class LaneSettings
    {
        /// <summary>
        /// The types of vehicles in each lane.
        /// </summary>
        /// <value>
        /// The types of vehicles.
        /// </value>
        public IList<(int, VehicleType)> Vehicles { get; }
        /// <summary>
        /// The traffic direction for each lane.
        /// </summary>
        /// <value>
        /// The traffic directions.
        /// </value>
        public IList<Direction> TrafficDirections { get; }
        /// <summary>
        /// Gets the traffic speeds.
        /// </summary>
        /// <value>
        /// The traffic speeds.
        /// </value>
        public IList<int> TrafficSpeeds { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaneSettings" /> class.
        /// </summary>
        /// <param name="vehicles">The vehicles.</param>
        /// <param name="flows">The directions of traffic.</param>
        /// <param name="speeds">The speeds of each lane.</param>
        public LaneSettings(IList<(int, VehicleType)> vehicles, IList<Direction> flows, IList<int> speeds)
        {
            this.Vehicles = vehicles;
            this.TrafficDirections = flows;
            this.TrafficSpeeds = speeds;
        }
    }
}
