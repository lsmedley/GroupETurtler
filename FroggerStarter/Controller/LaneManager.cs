using System.Collections.Generic;
using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages vehicles in a lane.
    /// </summary>
    public class LaneManager
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
        public readonly IList<Vehicle> Vehicles;

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
            this.Vehicles = new List<Vehicle>();
        }

        /// <summary>
        /// Adds a vehicle.
        /// Postcondition: this.Vehicles += a vehicle with the given type and the same speed and direction as this lane.
        /// </summary>
        /// <param name="type">The type of the vehicle.</param>
        public void AddVehicle(VehicleType type)
        {
            this.Vehicles.Add(new Vehicle(type, this.Direction, this.Speed));
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

            foreach (var v in this.Vehicles)
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
            foreach (var v in this.Vehicles)
            {
                v.Y = y;
            }
        }

        /// <summary>
        /// Gets the space between Vehicles.
        /// </summary>
        /// <param name="laneLength">Length of the lane.</param>
        /// <returns>The space between Vehicles</returns>
        public double GetSpacing(double laneLength)
        {
            double totalCarLen = 0;
            foreach (var v in this.Vehicles)
            {
                totalCarLen += v.Width;
            }

            return (laneLength - totalCarLen) / this.Vehicles.Count;
        }

        /// <summary>
        /// Moves the Vehicles.
        /// Postcondition: Each vehicle has been moved an amount equal to its SpeedX in the direction of the lane.
        /// </summary>
        public void MoveVehicles(double laneLength)
        {
            foreach (var v in this.Vehicles)
            {
                if (this.Direction == Direction.Left)
                {
                    v.MoveLeft();
                    if (v.X < 0 - v.Width)
                    {
                        v.X = laneLength;
                    }
                }
                else
                {
                    v.MoveRight();
                    if (v.X > laneLength)
                    {
                        v.X = 0 - v.Width;
                    }
                }
            }
        }

        /// <summary>
        /// Speeds up all Vehicles in the lane.
        /// Postcondition: This.Speed += 1, all vehicles are moving at the new speed.
        /// </summary>
        public void SpeedUp()
        {
            this.Speed += 1;
            this.updateVehicleSpeed();
        }

        /// <summary>
        /// Resets the speed of all Vehicles to the starting speed.
        /// Postcondition: All Vehicles are going the starting speed for this lane.
        /// </summary>
        public void ResetSpeed()
        {
            this.Speed = this.StartSpeed;
            this.updateVehicleSpeed();
        }

        private void updateVehicleSpeed()
        {
            foreach (var v in this.Vehicles)
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

            foreach (var v in this.Vehicles)
            {
                if (v.IsColliding(g))
                {
                    return true;
                }
            }

            return false;
        }
    
    }
}
