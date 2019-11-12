using System;
using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Utils;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages lanes in a road.
    /// </summary>
    public class Road : IEnumerable<Vehicle.Vehicle>
    {
        #region Data members

        /// <summary>
        /// The lanes
        /// </summary>
        protected readonly IList<Lane> Lanes;
        private readonly ICollection<LaneSettings> laneSettingsCollection;
        private int currentTick;
        private int endSlowDownTick;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Road" /> class.
        /// </summary>
        /// <param name="laneSettingsCollection">The collection of lane settings for this road's lanes.</param>
        /// <param name="totalHeight">The total height of the road.</param>
        /// <param name="laneLength">Length of the lanes.</param>
        /// <exception cref="Exception">Each lane must be fully defined</exception>
        public Road(ICollection<LaneSettings> laneSettingsCollection, double totalHeight, double laneLength)
        {
//            if (laneSettingsCollection.Vehicles.Count != laneSettingsCollection.TrafficDirections.Count 
//                || laneSettingsCollection.Vehicles.Count != laneSettingsCollection.TrafficSpeeds.Count 
//                || laneSettingsCollection.TrafficDirections.Count != laneSettingsCollection.TrafficSpeeds.Count)
//            {
//                throw new Exception("Each lane must be fully defined");
//            } //TODO figure out this

            this.laneSettingsCollection = laneSettingsCollection;
            this.currentTick = 0;
            this.endSlowDownTick = Int32.MaxValue;

            this.Lanes = new List<Lane>();
            this.SetUpLanes(totalHeight, laneLength);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Returns an enumerator that iterates through the collections of vehicles.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collections of vehicles.
        /// </returns>
        public IEnumerator<Vehicle.Vehicle> GetEnumerator()
        {
            foreach (var lane in this.Lanes)
            {
                foreach (var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var lane in this.Lanes)
            {
                foreach (var vehicle in lane)
                {
                    yield return vehicle;
                }
            }
        }

        /// <summary>
        ///     Occurs when [car added].
        /// </summary>
        public event EventHandler<EventArgs> CarAdded;

        /// <summary>
        /// Occurs when [slowdown ended].
        /// </summary>
        public event EventHandler<EventArgs> SlowdownEnded; 

        /// <summary>
        ///     Sets up lanes.
        ///     <param name="totalHeight">The total height of the road.</param>
        ///     <param name="laneLength">Length of the lane.</param>
        /// </summary>
        public void SetUpLanes(double totalHeight, double laneLength)
        {
            this.Lanes.Clear();

            foreach (var laneSettings in this.laneSettingsCollection)
            {
                var lane = new Lane(laneSettings);
                this.Lanes.Add(lane);
            }

            this.placeVehiclesInLanes(totalHeight, laneLength);
        }

        private void placeVehiclesInLanes(double totalHeight, double laneLength)
        {
            for (var i = 0; i < this.Lanes.Count; i++)
            {
                this.Lanes[i].SetVehicleYs(totalHeight - GameSettings.LaneHeight * (i + 1) +
                                           GameSettings.VehicleOffset);
                this.Lanes[i].PlaceAllVehiclesInLane(laneLength);
            }
        }

        /// <summary>
        ///     Called when [tick]. Adds a vehicle if currentTick == VehicleActionTick, then moves the vehicles
        ///     in each lane, wrapping vehicles that are out of bounds back to the other side of the game.
        ///     Postcondition: Each lane has one more care if this.currentTick == VehicleActionTick and the requirements
        ///     to add a new vehicle are met.
        /// </summary>
        /// <param name="laneLen">Length of the lane.</param>
        /// <param name="speedToAddOnVehicleAction">The amount to add on vehicle action.</param>
        public void OnTick(double laneLen, int speedToAddOnVehicleAction)
        {
            this.currentTick++;
            if (this.currentTick >= this.endSlowDownTick)
            {
                this.ResetSpeeds();
            }
            if (this.currentTick >= GameSettings.VehicleActionTick)
            {
                this.vehicleActionOnTick(laneLen, speedToAddOnVehicleAction);
            }

            this.MoveLaneVehicles(laneLen);
        }

        private void vehicleActionOnTick(double laneLen, int speedToAddOnVehicleAction)
        {
            this.addVehicles(laneLen);
            this.onCarAdded();
            this.IncreaseAllSpeedsBy(speedToAddOnVehicleAction);
            this.currentTick = 0;
        }

        private void onCarAdded()
        {
            this.CarAdded?.Invoke(this, EventArgs.Empty);
        }

        private void addVehicles(double laneLength)
        {
            foreach (var lane in this.Lanes)
            {
                lane.AddVehicle(laneLength);
            }
        }

        /// <summary>
        /// Moves the lane vehicles.
        /// </summary>
        /// <param name="laneLen">Length of the lane.</param>
        protected void MoveLaneVehicles(double laneLen)
        {
            foreach (var lane in this.Lanes)
            {
                lane.MoveVehicles(laneLen);
            }
        }

        /// <summary>
        ///     Checks to see if there is a collision.
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

            foreach (var lane in this.Lanes)
            {
                if (lane.CheckCollision(g))
                {
                    hasCollided = true;
                }
            }

            return hasCollided;
        }

        /// <summary>
        ///     Increases all speeds by the given amount.
        /// </summary>
        /// <param name="amount">The amount to increase speed by.</param>
        public void IncreaseAllSpeedsBy(int amount)
        {
            foreach (var vehicle in this)
            {
                vehicle.Speed += amount;
            }
        }

        /// <summary>
        ///     Increases the start amount by the given amount.
        /// </summary>
        /// <param name="amount">The amount to increase amount by.</param>
        public void IncreaseStartSpeedBy(int amount)
        {
            foreach (var lane in this.Lanes)
            {
                lane.StartSpeed += amount;
            }
        }

        /// <summary>
        ///     Increases the maximum vehicles by the given amount.
        /// </summary>
        /// <param name="amount">The amount to increase amount by.</param>
        public void IncreaseMaxVehiclesBy(int amount)
        {
            foreach (var lane in this.Lanes)
            {
                lane.MaxNumVehicles += amount;
            }
        }

        /// <summary>
        ///     Resets the speeds of all vehicles.
        /// </summary>
        public void ResetSpeeds()
        {
            foreach (var lane in this.Lanes)
            {
                foreach (var vehicle in lane)
                {
                    vehicle.Speed = lane.StartSpeed;
                }
            }
            this.onSlowdownEnded();
        }

        #endregion

        /// <summary>
        /// Slows down vehicles for i ticks.
        /// </summary>
        /// <param name="i">The i.</param>
        public void SlowDownVehicles(int i)
        {
            this.currentTick = 0;
            this.endSlowDownTick = i;
            foreach (var lane in this.Lanes)
            {
                foreach (var vehicle in lane)
                {
                    vehicle.Speed = 1;
                }
            }
        }

        private void onSlowdownEnded()
        {
            this.SlowdownEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}