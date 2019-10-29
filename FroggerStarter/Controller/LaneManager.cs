using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        public int Speed { get; set; }
        /// <summary>
        /// The start speed of vehicles in this lane.
        /// </summary>
        public readonly int StartSpeed;
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
        /// <param name="startSpeed">The start speed.</param>
        /// <param name="dir">The dir.</param>
        public LaneManager(int startSpeed, Direction dir, int maxV)
        {
            this.Speed = startSpeed;
            this.StartSpeed = startSpeed;
            this.maxVehicles = maxV;
            this.Direction = dir;
            this.vehicles = new List<Vehicle>();
        }

        /// <summary>
        /// Adds a vehicle to the lane.
        /// Postcondition: If this.vehicles.Count < this.maxVehicles, then this.vehicles += a vehicle with the given type and the same speed and direction as this lane.
        /// Else none.
        /// </summary>
        /// <param name="type">The type of the vehicle.</param>
        public void AddVehicle(VehicleType type)
        {
            if (this.vehicles.Count < this.maxVehicles)
            {
                this.vehicles.Add(new Vehicle(type, this.Direction, this.Speed));
            }
        }

        /// <summary>
        /// Adds a vehicle to the appropriate place in the lane.
        /// Postcondition: If this.vehicles.Count < this.maxVehicles, then this.vehicles += a vehicle with the given type and the same speed and direction as this lane,
        /// appropriately spaced behind the last vehicle. Else none.
        /// </summary>
        /// <param name="type">The type of the vehicle.</param>
        /// <param lanelen="double">The length of this lane</param>
        public void AddVehicle(VehicleType type, double lanelen)
        {
            if (this.vehicles.Count < this.maxVehicles)
            {
                var space = this.GetSpacing(lanelen);
                Vehicle prevV = this.vehicles[this.vehicles.Count - 1];
                if (this.Direction == Direction.Right)
                {
                    foreach (var v in this.vehicles)
                    {
                        //if (v.X <= 0 - v.Width && 0 <= v.X && v != curV)
                        if (v.X <= 0 || v.X >= lanelen - v.Width)
                        {
                            return;
                        }
                    }
                    this.vehicles.Add(new Vehicle(type, this.Direction, this.Speed));
                    this.vehicles[this.vehicles.Count - 1].X = 0 - this.vehicles[this.vehicles.Count - 1].Width;
                }
                else {
                    foreach (var v in this.vehicles)
                    {
                        if (v.X >= lanelen - v.Width || v.X <= v.Width)
                        {
                            return;
                        }
                    }
                    this.vehicles.Add(new Vehicle(type, this.Direction, this.Speed));
                    this.vehicles[this.vehicles.Count - 1].X = lanelen;
                }

                this.vehicles[this.vehicles.Count - 1].Y = prevV.Y;
            }

        }

        /// <summary>
        /// Determines whether this instance is full.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is full; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFull()
        {
            return this.vehicles.Count == this.maxVehicles;
        }

        /// <summary>
        /// Sets the location of each vehicle along the X axis.
        /// Postcondition: Each vehicle is at an algorithmically computed X value.
        /// </summary>
        /// <param name="laneLength">Length of the lane.</param>
        public void PlaceAllVehiclesInLane(double laneLength)
        {
            var space = this.GetSpacing(laneLength);
            double curX;
            if (this.Direction == Direction.Left)
            {
                curX = 0;
            }
            else
            {
                curX = laneLength;
            }

            foreach (var v in this.vehicles)
            {
                v.X = curX;

                if (this.Direction == Direction.Left)
                {
                    curX += v.Width + space;
                }
                else
                {
                    curX -= v.Width + space;
                }
            }

        }

        /// <summary>
        /// Sets the vehicle ys.
        /// Postcondition: Each vehicle is at the specified Y value.
        /// </summary>
        /// <param name="y">The y.</param>
        public void SetVehicleYs(double y)
        {
            foreach (var v in this.vehicles)
            {
                v.Y = y;
            }
        }

        /// <summary>
        /// Gets the space between vehicles.
        /// </summary>
        /// <param name="laneLength">Length of the lane.</param>
        /// <returns>The space between vehicles</returns>
        public double GetSpacing(double laneLength)
        {
            double totalCarLen = 0;
            foreach (var v in this.vehicles)
            {
                totalCarLen += v.Width;
            }

            return (laneLength - totalCarLen) / this.vehicles.Count;
        }

        /// <summary>
        /// Moves the vehicles.
        /// Postcondition: Each vehicle has been moved an amount equal to its SpeedX in the direction of the vehicle.
        /// </summary>
        public void MoveVehicles(double laneLength)
        {
            foreach (var v in this.vehicles)
            {
                v.MoveForward();
                if (v.X < 0 - v.Width)
                {
                    v.X = laneLength;
                }

                if (v.X > laneLength)
                {
                    v.X = 0 - v.Width;
                }
            }
        }

        /// <summary>
        /// Resets the vehicles, removing all but 1 vehicle from this lane.
        /// </summary>
        public void ResetVehicles()
        {
            for (int i = 1; i < this.vehicles.Count; i++)
            {
                this.vehicles.Remove(this.vehicles[i]);
            }
        }

        /// <summary>
        /// Speeds up all vehicles in the lane.
        /// Postcondition: This.Speed += 1, all vehicles are moving at the new speed.
        /// </summary>
        public void SpeedUp(int speedInc)
        {
            this.Speed += speedInc;
            this.updateVehicleSpeed();
        }

        /// <summary>
        /// Resets the speed of all vehicles to the starting speed.
        /// Postcondition: All vehicles are going the starting speed for this lane.
        /// </summary>
        public void ResetSpeed()
        {
            this.Speed = this.StartSpeed;
            this.updateVehicleSpeed();
        }

        private void updateVehicleSpeed()
        {
            foreach (var v in this.vehicles)
            {
                v.ChangeSpeed(this.Speed);
            }
        }

        /// <summary>
        /// Checks if there is a collision.
        /// </summary>
        /// <param name="g">The g.</param>
        /// <returns>True if one of the vehicles has collided with g, false otherwise.</returns>
        public bool CheckCollision(GameObject g)
        {

            foreach (var v in this.vehicles)
            {
                if (v.IsColliding(g))
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
