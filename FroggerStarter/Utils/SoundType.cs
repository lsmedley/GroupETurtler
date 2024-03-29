﻿namespace FroggerStarter.Utils
{
    /// <summary>
    ///     Stores the types of sounds.
    /// </summary>
    public enum SoundType
    {
        /// <summary>
        ///     The game lost
        /// </summary>
        GameLost,

        /// <summary>
        ///     The game won
        /// </summary>
        GameWon,

        /// <summary>
        ///     Death by vehicle
        /// </summary>
        VehicleDeath,

        /// <summary>
        ///     Death by wall
        /// </summary>
        WallDeath,

        /// <summary>
        ///     Death by water
        /// </summary>
        WaterDeath,

        /// <summary>
        ///     Death by the inescapable march of time
        /// </summary>
        TimeDeath,

        /// <summary>
        ///     The time power up
        /// </summary>
        TimePowerUp,

        /// <summary>
        ///     The vehicle power up
        /// </summary>
        VehiclePowerUp
    }
}