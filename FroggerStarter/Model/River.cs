using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FroggerStarter.Utils;

namespace FroggerStarter.Model
{
    public class River : Road
    {
        public double StartOfRiver;
        public River(ICollection<LaneSettings> laneSettingsCollection, double totalHeight, double laneLength) : base(laneSettingsCollection, totalHeight, laneLength)
        {
            this.StartOfRiver = totalHeight;
            base.SetUpLanes(totalHeight, laneLength);
        }
        //Add all 'vehicles' to river at start

        /// <summary>
        /// Moves the objects in the river, returning the value by which the player needs to be moved if it
        /// is riding on one..
        /// </summary>
        /// <param name="laneLen">Length of the lane.</param>
        /// <param name="player">The player.</param>
        /// <returns></returns>
        public double MoveRiver(double laneLen, GameObject player)
        {
            //double prevX = 0;
            //double newX = 0;
            double changeX = 0;

            foreach (var debris in this)
            {
                if (debris.IsColliding(player))
                {
                    changeX = debris.Speed;
                }
            }
            base.MoveLaneVehicles(laneLen);

            //return newX - prevX;
            return changeX;
        }
    }
}
