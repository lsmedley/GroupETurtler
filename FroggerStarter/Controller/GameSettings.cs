using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Holds information about the game settings.
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Gets the player lives.
        /// </summary>
        /// <value>
        /// The player lives.
        /// </value>
        public int PlayerLives { get; }
        /// <summary>
        /// Gets the timer length in seconds.
        /// </summary>
        /// <value>
        /// The timer length in seconds.
        /// </value>
        public int TimerLengthSeconds { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettings" /> class.
        /// </summary>
        /// <param name="lives">The lives that the player will have.</param>
        /// <param name="time">The time that the player will have to get home.</param>
        public GameSettings(int lives, int time)
        {
            this.PlayerLives = lives;
            this.TimerLengthSeconds = time;
        }
    }
}
