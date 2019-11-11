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
        ///     The vehicle action tick (add car, increase speed)
        /// </summary>
        public const int VehicleActionTick = 500;

        /// <summary>
        /// The vehicle slow down length in ticks
        /// </summary>
        public const int VehicleSlowDownTickLength = 250;

        /// <summary>
        /// The time power up amount
        /// </summary>
        public const int TimePowerUpAmount = 7;

        /// <summary>
        ///     The lane height
        /// </summary>
        public const int LaneHeight = 50;

        /// <summary>
        ///     The vehicle offset spacing
        /// </summary>
        public const int VehicleOffset = 5;

        /// <summary>
        ///     The bottom lane offset
        /// </summary>
        public const int BottomLaneOffset = 5;

        /// <summary>
        ///     The timer block width
        /// </summary>
        public const int TimerBlockWidth = 5;

        /// <summary>
        ///     The timer bar height
        /// </summary>
        public const int TimerBarHeight = 10;

        /// <summary>
        ///     The top of game offset
        /// </summary>
        public const int TopOfGameOffset = 100;

        /// <summary>
        ///     The tile height
        /// </summary>
        public const int TileHeight = 50;

        /// <summary>
        ///     The additional speed to add in level 2 on tick
        /// </summary>
        public const int Level2AdditionalSpeedOnTick = 1;

        /// <summary>
        ///     The additional speed to add in level 2 on tick
        /// </summary>
        public const int Level3AdditionalSpeedOnTick = 2;

        /// <summary>
        ///     The collection of lane settings
        /// </summary>
        public static readonly ICollection<LaneSettings> LaneSettingsCollection = new ReadOnlyCollection<LaneSettings>
        (new List<LaneSettings> {
            new LaneSettings(1, Direction.Left, 3, VehicleType.Car),
            new LaneSettings(5, Direction.Right, 2, VehicleType.Bus),
            new LaneSettings(3, Direction.Left, 4, VehicleType.TowingCar),
            new LaneSettings(7, Direction.Left, 3, VehicleType.Bus),
            new LaneSettings(9, Direction.Right, 5, VehicleType.Car)
        });

        public static readonly ICollection<LaneSettings> RiverLaneSettingsCollection = new ReadOnlyCollection<LaneSettings>(new List<LaneSettings> {
            new LaneSettings(1, Direction.Left, 4, VehicleType.Car),
            new LaneSettings(1, Direction.Right, 3, VehicleType.Bus),
            new LaneSettings(1, Direction.Left, 2, VehicleType.TowingCar),
            new LaneSettings(1, Direction.Left, 3, VehicleType.Bus),
            new LaneSettings(1, Direction.Right, 4, VehicleType.Car)
        });

        #endregion
    }
}