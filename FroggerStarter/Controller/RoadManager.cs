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
        private readonly ICollection<LaneSettings> laneSettingsCollection;
        private int currentTick;
        private const int AddCarTick = 500;

        private const int LaneHeight = 50;
        private const int VehicleOffset = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoadManager"/> class.
        /// </summary>
        /// <param name="laneSettingsCollection">The collection of lane settings for this road's lanes.</param>
        /// <param name="totalHeight">The total height of the road.</param>
        /// <param name="laneLength">Length of the lanes.</param>
        /// <exception cref="Exception">Each lane must be fully defined</exception>
        public RoadManager(ICollection<LaneSettings> laneSettingsCollection, double totalHeight, double laneLength)
        {
//            if (laneSettingsCollection.Vehicles.Count != laneSettingsCollection.TrafficDirections.Count 
//                || laneSettingsCollection.Vehicles.Count != laneSettingsCollection.TrafficSpeeds.Count 
//                || laneSettingsCollection.TrafficDirections.Count != laneSettingsCollection.TrafficSpeeds.Count)
//            {
//                throw new Exception("Each lane must be fully defined");
//            } //TODO figure out this

            this.laneSettingsCollection = laneSettingsCollection;
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

            foreach (var laneSettings in this.laneSettingsCollection)
            {
                var lane = new LaneManager(laneSettings);
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
                lane.AddVehicle(laneLength);
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
        /// <param name="playerManagerDisabled"></param>
        /// <returns>true if there is a collision, false otherwise.</returns>
        public bool CheckCollision(GameObject g, bool playerManagerDisabled)
        {
            var hasCollided = false;

            if (playerManagerDisabled)
            {
                return false;
            }

            foreach (var lane in this.lanes)
            {
                if (lane.CheckCollision(g))
                {
                    hasCollided = true;
                }
            }

            return hasCollided;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collections of vehicles.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collections of vehicles.
        /// </returns>
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

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
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
