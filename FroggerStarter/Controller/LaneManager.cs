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

        /// <summary>
        /// Initializes a new instance of the <see cref="LaneManager"/> class.
        /// </summary>
        /// <param name="startSpeed">The start speed.</param>
        /// <param name="dir">The dir.</param>
        public LaneManager(int startSpeed, Direction dir)
        {
            this.Speed = startSpeed;
            this.StartSpeed = startSpeed;
            this.Direction = dir;
            this.vehicles = new List<Vehicle>();
        }

       

        /// <summary>
        /// Adds a vehicle.
        /// Postcondition: this.vehicles += a vehicle with the given type and the same speed and direction as this lane.
        /// </summary>
        /// <param name="type">The type of the vehicle.</param>
        public void AddVehicle(VehicleType type)
        {
            this.vehicles.Add(new Vehicle(type, this.Direction, this.Speed));
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
