using System;
using System.Xml.Serialization;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Contains a score with the associated player name and level completed
    /// </summary>
    [XmlRoot("HighScores")]
    public class HighScore
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        ///     Gets or sets the level completed.
        /// </summary>
        /// <value>
        ///     The level completed.
        /// </value>
        public int LevelCompleted { get; set; }

        /// <summary>
        ///     Gets or sets the name of the player.
        /// </summary>
        /// <value>
        ///     The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Overrides ToString
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{this.PlayerName}: Level {this.LevelCompleted}, {this.Score} points";
        }

        #endregion
    }
}