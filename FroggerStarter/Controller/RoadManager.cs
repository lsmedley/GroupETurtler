using FroggerStarter.Model;
using System;
using System.Collections.Generic;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages lanes in a road.
    /// </summary>
    public class RoadManager
    {
        /// <summary>
        /// The lanes
        /// </summary>
        public readonly IList<LaneManager> Lanes;
        private int currentTick;
        private const int SpeedUpTick = 150;

        private const int LaneHeight = 50;
        private const int VehicleOffset = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadManager"/> class.
        /// Postcondition: Lanes.count == numLanes, Lanes[i] contains vehicles matching vehicles[i],
        /// and Lanes[i].Direction == trafficFlow[i].Direction and Lanes[i].startSpeed == trafficFlow[i].int
        /// </summary>
        /// <param name="numLanes">The number Lanes.</param>
        /// <param name="vehicles">The number and type of vehicles for each lane.</param>
        /// <param name="trafficFlow">The direction and speed of traffic for each lane.</param>
        /// <exception cref="Exception">Number of Lanes must equal number of lane definitions</exception>
        public RoadManager(int numLanes, IList<(int, VehicleType)> vehicles, IList<(int, Direction)> trafficFlow)
        {
            if (vehicles.Count != numLanes || trafficFlow.Count != numLanes)
            {
                throw new Exception("Number of Lanes must equal number of lane definitions");
            }

            this.currentTick = 0;

            this.Lanes = new List<LaneManager>();
            for (var i = 0; i < numLanes; i++)
            {
                var lane = new LaneManager(trafficFlow[i].Item1, trafficFlow[i].Item2);
                for (var j = 0; j < vehicles[i].Item1; j++)
                {
                    lane.AddVehicle(vehicles[i].Item2);
                }

                this.Lanes.Add(lane);
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
            for (var i = 0; i < this.Lanes.Count; i++)
            {
                this.Lanes[i].SetVehicleYs(totalHeight - LaneHeight * (i + 1) + VehicleOffset);
                this.Lanes[i].PlaceAllVehiclesInLane(laneLength);
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
                this.speedUpLanes();
                this.currentTick = 0;
            }

            this.moveLaneVehicles(laneLen);
        }

        private void moveLaneVehicles(double laneLen)
        {
            foreach (var lane in this.Lanes)
            {
                lane.MoveVehicles(laneLen);
            }
        }

        private void speedUpLanes()
        {
            foreach (var lane in this.Lanes)
            {
                lane.SpeedUp();
            }
        }

        /// <summary>
        /// Resets the speed of the lanes.
        /// </summary>
        public void ResetLanes()
        {
            foreach (var lane in this.Lanes)
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

            foreach (var lane in this.Lanes)
            {
                if (lane.CheckCollision(g))
                {
                    isCol = true;
                }
            }

            return isCol;
        }

    }
}
