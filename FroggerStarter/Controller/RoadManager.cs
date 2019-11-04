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
        /// Occurs when [car added].
        /// </summary>
        public event EventHandler<EventArgs> CarAdded;

        private readonly IList<LaneManager> lanes;
        private readonly LaneSettings laneSettings;
        private int currentTick;
        private const int AddCarTick = 300;

        private const int LaneHeight = 50;
        private const int VehicleOffset = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadManager"/> class.
        /// </summary>
        /// <param name="laneSettings">The settings for this roadmanager's lanes.</param>
        /// <param name="totalHeight">The total height of the road.</param>
        /// <param name="laneLength">Length of the lanes.</param>
        /// <exception cref="Exception">Each lane must be fully defined</exception>
        public RoadManager(LaneSettings laneSettings, double totalHeight, double laneLength)
        {
            if (laneSettings.Vehicles.Count != laneSettings.TrafficDirections.Count 
                || laneSettings.Vehicles.Count != laneSettings.TrafficSpeeds.Count 
                || laneSettings.TrafficDirections.Count != laneSettings.TrafficSpeeds.Count)
            {
                throw new Exception("Each lane must be fully defined");
            }

            this.laneSettings = laneSettings;
            this.currentTick = 0;

            this.lanes = new List<LaneManager>();
            this.SetUpLanes(totalHeight, laneLength);
        }

        /// <summary>
        /// Sets up lanes.
        /// <param name="totalHeight">The total height of the road.</param>
        /// <param name="laneLength">Length of the lane.</param>
        /// </summary>
        public void SetUpLanes(double totalHeight, double laneLength)
        {
            this.lanes.Clear();
            for (var i = 0; i < this.laneSettings.Vehicles.Count; i++)
            {
                var lane = new LaneManager(this.laneSettings.TrafficSpeeds[i], this.laneSettings.TrafficDirections[i], this.laneSettings.Vehicles[i].Item1);

                lane.AddVehicle(this.laneSettings.Vehicles[i].Item2);

                this.lanes.Add(lane);
            }
            this.placeVehiclesInLanes(totalHeight, laneLength);
        }

        private void placeVehiclesInLanes(double totalHeight, double laneLength)
        {
            for (var i = 0; i < this.lanes.Count; i++)
            {
                this.lanes[i].SetVehicleYs(totalHeight - LaneHeight * (i + 1) + VehicleOffset);
                this.lanes[i].PlaceAllVehiclesInLane(laneLength);
            }
        }

        /// <summary>
        /// Called when [tick]. Adds a vehicle if currentTick == AddCarTick, then moves the vehicles
        /// in each lane, wrapping vehicles that are out of bounds back to the other side of the game.
        /// Postcondition: Each lane has one more care if this.currentTick == AddCarTick and the requirements
        /// to add a new vehicle are met.
        /// </summary>
        /// <param name="laneLen">Length of the lane.</param>
        public void OnTick(double laneLen)
        {
            this.currentTick++;
            if (this.currentTick >= AddCarTick)
            { 
                this.addVehicles(laneLen);
                this.onCarAdded();
                this.currentTick = 0;
            }

            this.moveLaneVehicles(laneLen);
        }

        private void onCarAdded()
        {
            this.CarAdded?.Invoke(this, EventArgs.Empty);
        }

        private void addVehicles(double laneLength)
        {
            foreach (var lane in this.lanes)
            {
                var vehicleType = VehicleType.Car;
                foreach (var vehicle in lane)
                {
                    vehicleType = vehicle.Type;
                }

                lane.AddVehicle(vehicleType, laneLength);

            }

        }

        private void moveLaneVehicles(double laneLen)
        {
            foreach (var lane in this.lanes)
            {
                lane.MoveVehicles(laneLen);
            }
        }

        /// <summary>
        /// Checks to see if there is a collision.
        /// </summary>
        /// <param name="g">The game object being checked for collisions with a vehicle.</param>
        /// <returns>true if there is a collision, false otherwise.</returns>
        public bool CheckCollision(GameObject g)
        {
            var hasCollided = false;

            foreach (var lane in this.lanes)
            {
                if (lane.CheckCollision(g))
                {
                    hasCollided = true;
                }
            }

            return hasCollided;
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
