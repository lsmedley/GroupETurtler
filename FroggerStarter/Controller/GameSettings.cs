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
        /// Gets the level timer length in seconds.
        /// </summary>
        /// <value>
        /// The level timer length in seconds.
        /// </value>
        public int TimerLengthSeconds { get; }

        /// <summary>
        /// Gets the number of times the player has to score to win.
        /// </summary>
        /// <value>
        /// The scores to win.
        /// </value>
        public int ScoresToWin { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettings" /> class.
        /// </summary>
        /// <param name="lives">The lives that the player will have.</param>
        /// <param name="time">The time that the player will have to get home.</param>
        /// <param name="scoresToWin">The scores the player needs to make to win.</param>
        public GameSettings(int lives, int time, int scoresToWin)
        {
            this.PlayerLives = lives;
            this.TimerLengthSeconds = time;
            this.ScoresToWin = scoresToWin;
        }
    }
}
