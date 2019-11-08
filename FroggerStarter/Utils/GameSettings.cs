using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FroggerStarter.Utils
{
    /// <summary>
    ///     Holds information about the game settings.
    /// </summary>
    public abstract class GameSettings
    {
        #region Data members

        /// <summary>
        ///     Gets the player lives.
        /// </summary>
        /// <value>
        ///     The player lives.
        /// </value>
        public const int PlayerLives = 4;

        /// <summary>
        ///     Gets the level timer length in seconds.
        /// </summary>
        /// <value>
        ///     The level timer length in seconds.
        /// </value>
        public const int TimerLengthSeconds = 20;

        /// <summary>
        ///     Gets the number of times the player has to score to win.
        /// </summary>
        /// <value>
        ///     The scores to win.
        /// </value>
        public const int ScoresToWin = 5;

        /// <summary>
        ///     The collection of lane settings
        /// </summary>
        public static readonly ICollection<LaneSettings> LaneSettingsCollection = new ReadOnlyCollection<LaneSettings>
        (new List<LaneSettings> {
            new LaneSettings(1, Direction.Left, 3, VehicleType.Car),
            new LaneSettings(3, Direction.Right, 2, VehicleType.Bus),
            new LaneSettings(5, Direction.Left, 4, VehicleType.Car),
            new LaneSettings(7, Direction.Left, 3, VehicleType.Bus),
            new LaneSettings(9, Direction.Right, 5, VehicleType.Car)
        });

        #endregion
    }
}