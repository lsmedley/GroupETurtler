using System.Collections;
using System.Collections.Generic;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages vehicles in a lane.
    /// </summary>
    public class LaneManager : IEnumerable<Vehicle>
    {
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>
        /// The speed of cars in this lane.
        /// </value>
        public readonly int Speed;
        
        /// <summary>
        /// Gets the direction.
        /// </summary>
        /// <value>
        /// The direction that vehicles in this lane drive in.
        /// </value>
        public Direction Direction { get; }
        /// <summary>
        /// The vehicles driving in this lane.
        /// </summary>
        private readonly List<Vehicle> vehicles;

        private readonly int maxVehicles;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaneManager"/> class.
        /// </summary>
        /// <param name="startSpeed">The speed of the lane.</param>
        /// <param name="dir">The direction of the lane.</param>
        /// <param name="maxV">The maximum number of vehicles in this lane.</param>
        public LaneManager(int startSpeed, Direction dir, int maxV)
        {
            this.Speed = startSpeed;
            this.maxVehicles = maxV;
            this.Direction = dir;
            this.vehicles = new List<Vehicle>();
        }

        /// <summary>
        /// Adds a vehicle to this lane if this.vehicles has fewer than the maximum number of vehicles.
        /// Postcondition: If this.vehicles.Count is less than this.maxVehicles, then this.vehicles
        /// += a vehicle with the given vehicleType and the same speed and direction as this lane. Else none.
        /// </summary>
        /// <param name="vehicleType">The vehicleType of the vehicle.</param>
        public void AddVehicle(VehicleType vehicleType)
        {
            if (this.vehicles.Count < this.maxVehicles)
            {
                this.vehicles.Add(new Vehicle(vehicleType, this.Direction, this.Speed));
            }
        }

        /// <summary>
        /// Adds a vehicle to the appropriate place in the lane if this.vehicles has fewer than the maximum
        /// number of vehicles.
        /// Postcondition: If this.vehicles.Count is less than this.maxVehicles, then this.vehicles
        /// += a vehicle with the given vehicleType and the same speed and direction as this lane, appropriately
        /// spaced behind the last vehicle. Else none.
        /// </summary>
        /// <param name="vehicleType">The vehicleType of the vehicle.</param>
        /// <param name="laneLength">The length of this lane</param>
        public void AddVehicle(VehicleType vehicleType, double laneLength)
        {
            if (this.vehicles.Count < this.maxVehicles)
            {
                var prevVehicle = this.vehicles[this.vehicles.Count - 1];
                if (this.Direction == Direction.Right)
                {
                    foreach (var vehicle in this.vehicles)
                    {
                        if (vehicle.X <= 0 || vehicle.X >= laneLength - vehicle.Width)
                        {
                            return;
                        }
                    }
                    this.vehicles.Add(new Vehicle(vehicleType, this.Direction, this.Speed));
                    this.vehicles[this.vehicles.Count - 1].X = 0 - this.vehicles[this.vehicles.Count - 1].Width;
                }
                else {
                    foreach (var vehicle in this.vehicles)
                    {
                        if (vehicle.X >= laneLength - vehicle.Width || vehicle.X <= vehicle.Width)
                        {
                            return;
                        }
                    }
                    this.vehicles.Add(new Vehicle(vehicleType, this.Direction, this.Speed));
                    this.vehicles[this.vehicles.Count - 1].X = laneLength;
                }

                this.vehicles[this.vehicles.Count - 1].Y = prevVehicle.Y;
            }

        }

        /// <summary>
        /// Sets the location of each vehicle along the X axis.
        /// Postcondition: Each vehicle is at an algorithmically computed X value.
        /// </summary>
        /// <param name="laneLength">Length of the lane.</param>
        public void PlaceAllVehiclesInLane(double laneLength)
        {
            var space = this.GetSpacing(laneLength);
            double currX;
            if (this.Direction == Direction.Left)
            {
                currX = 0;
            }
            else
            {
                currX = laneLength;
            }

            foreach (var vehicle in this.vehicles)
            {
                vehicle.X = currX;

                if (this.Direction == Direction.Left)
                {
                    currX += vehicle.Width + space;
                }
                else
                {
                    currX -= vehicle.Width + space;
                }
            }

        }

        /// <summary>
        /// Sets the vehicles' locations along the Y axis.
        /// Postcondition: Each vehicle is at the specified Y value.
        /// </summary>
        /// <param name="y">The y coordinate.</param>
        public void SetVehicleYs(double y)
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.Y = y;
            }
        }

        /// <summary>
        /// Gets the space between vehicles.
        /// </summary>
        /// <param name="laneLength">Length of the lane.</param>
        /// <returns>The space between vehicles</returns>
        public double GetSpacing(double laneLength)
        {
            double totalCarLength = 0;
            foreach (var vehicle in this.vehicles)
            {
                totalCarLength += vehicle.Width;
            }

            return (laneLength - totalCarLength) / this.vehicles.Count;
        }

        /// <summary>
        /// Moves the vehicles.
        /// Postcondition: Each vehicle has been moved an amount equal to its SpeedX in the direction of the vehicle.
        /// </summary>
        public void MoveVehicles(double laneLength)
        {
            foreach (var vehicle in this.vehicles)
            {
                vehicle.MoveForward();
                if (vehicle.X < 0 - vehicle.Width)
                {
                    vehicle.X = laneLength;
                }

                if (vehicle.X > laneLength)
                {
                    vehicle.X = 0 - vehicle.Width;
                }
            }
        }

        /// <summary>
        /// Checks if there is a collision.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>True if one of the vehicles has collided with g, false otherwise.</returns>
        public bool CheckCollision(GameObject g)
        {

            foreach (var vehicle in this.vehicles)
            {
                if (vehicle.IsColliding(g))
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerator<Vehicle> GetEnumerator()
        {
            return ((IEnumerable<Vehicle>)this.vehicles).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Vehicle>)this.vehicles).GetEnumerator();
        }
    }
}
