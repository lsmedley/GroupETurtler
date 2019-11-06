using System.Collections.Generic;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Defines settings for lanes.
    /// </summary>
    public class LaneSettings
    {
        public int StartSpeed { get; }
        public Direction Direction { get; }
        public int MaxNumVehicles { get; }
        public VehicleType VehicleType { get; }

        public LaneSettings(int startSpeed, Direction direction, int maxNumVehicles, VehicleType vehicleType)
        {
            this.StartSpeed = startSpeed;
            this.Direction = direction;
            this.MaxNumVehicles = maxNumVehicles;
            this.VehicleType = vehicleType;
        }
    }
}
