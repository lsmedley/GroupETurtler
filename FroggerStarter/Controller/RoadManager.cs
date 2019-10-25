using FroggerStarter.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages lanes in a road.
    /// </summary>
    public class RoadManager : IEnumerable<Vehicle>
    {
        /// <summary>
        /// The lanes
        /// </summary>
        private readonly IList<LaneManager> lanes;
        private int currentTick;
        private const int SpeedUpTick = 150;

        private const int LaneHeight = 50;
        private const int VehicleOffset = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadManager"/> class.
        /// Postcondition: lanes.count == numLanes, lanes[i] contains vehicles matching vehicles[i],
        /// and lanes[i].Direction == trafficFlow[i].Direction and lanes[i].startSpeed == trafficFlow[i].int
        /// </summary>
        /// <param name="numLanes">The number lanes.</param>
        /// <param name="vehicles">The number and type of vehicles for each lane.</param>
        /// <param name="trafficFlow">The direction and speed of traffic for each lane.</param>
        /// <exception cref="Exception">Number of lanes must equal number of lane definitions</exception>
        public RoadManager(LaneSettings laneset)
        {
            if (laneset.Vehicles.Count != laneset.TrafficDirections.Count 
                || laneset.Vehicles.Count != laneset.TrafficSpeeds.Count 
                || laneset.TrafficDirections.Count != laneset.TrafficSpeeds.Count)
            {
                throw new Exception("Each lane must be fully defined");
            }

            this.currentTick = 0;

            this.lanes = new List<LaneManager>();
            for (var i = 0; i < laneset.Vehicles.Count; i++)
            {
                var lane = new LaneManager(laneset.TrafficSpeeds[i], laneset.TrafficDirections[i]);
                for (var j = 0; j < laneset.Vehicles[i].Item1; j++)
                {
                    lane.AddVehicle(laneset.Vehicles[i].Item2);
                }

                this.lanes.Add(lane);
            }

        }

        /// <summary>
        /// Sets up the lanes.
        /// Postcondition: Each lane has all vehicles set to the same height and spaced apart on the x-axis.
        /// </summary>
        /// <param name="totalHeight">The total height of the road.</param>
        /// <param name="laneLength">Length of the lane.</param>
        public void SetUpLanes(double totalHeight, double laneLength)
        {
            for (var i = 0; i < this.lanes.Count; i++)
            {
                this.lanes[i].SetVehicleYs(totalHeight - LaneHeight * (i + 1) + VehicleOffset);
                this.lanes[i].PlaceAllVehiclesInLane(laneLength);
            }
        }

        /// <summary>
        /// Called when [tick]. Speeds vehicles up if currentTick == SpeedUpTick, then moves the vehicles
        /// in each lane, wrapping vehicles that are out of bounds back to the other side of the game.
        /// </summary>
        /// <param name="laneLen">Length of the lane.</param>
        public void OnTick(double laneLen)
        {
            this.currentTick++;
            if (this.currentTick == SpeedUpTick)
            {
                this.speedUpLanes(1);
                this.currentTick = 0;
            }

            this.moveLaneVehicles(laneLen);
        }

        private void moveLaneVehicles(double laneLen)
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveVehicles(laneLen);
            }
        }

        private void speedUpLanes(int inc)
        {
            foreach (var lane in this.lanes)
            {
                lane.SpeedUp(inc);
            }
        }

        /// <summary>
        /// Resets the speed of the lanes.
        /// </summary>
        public void ResetLanes()
        {
            foreach (var lane in this.lanes)
            {
                lane.ResetSpeed();
            }
        }

        /// <summary>
        /// Checks to see if there is a collision.
        /// </summary>
        /// <param name="g">The game object being checked for collisions with a vehicle.</param>
        /// <returns>true if there is a collision, false otherwise.</returns>
        public bool CheckCollision(GameObject g)
        {
            var isCol = false;

            foreach (var lane in this.lanes)
            {
                if (lane.CheckCollision(g))
                {
                    isCol = true;
                }
            }

            return isCol;
        }

        public IEnumerator<Vehicle> GetEnumerator()
        {
            foreach(var lane in this.lanes)
            {
                foreach(var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var lane in this.lanes)
            {
                foreach (var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }
    }
}
