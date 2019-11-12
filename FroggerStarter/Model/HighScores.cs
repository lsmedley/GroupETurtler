using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FroggerStarter.Utils;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Manages high score tasks
    /// </summary>
    public class HighScores
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the scores.
        /// </summary>
        /// <value>
        ///     The scores.
        /// </value>
        [XmlArray("Scores")]
        [XmlArrayItem(typeof(HighScore), ElementName = "HighScore")]
        public List<HighScore> Scores { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScores" /> class.
        /// </summary>
        public HighScores()
        {
            this.Scores = new List<HighScore>();
            this.populateHighScores();
        }

        #endregion

        #region Methods

        private void populateHighScores()
        {
            this.Scores = Task.Run(ScoreSerializer.Deserialize).Result;
            this.SortByScoreNameLevel();
            this.Scores = this.Scores.Take(10).ToList();
        }

        /// <summary>
        ///     Sorts by score, then name, then level
        /// </summary>
        public void SortByScoreNameLevel()
        {
            var sortedScores = this.Scores.OrderByDescending(score => score.Score)
                                   .ThenBy(score => score.PlayerName)
                                   .ThenByDescending(score => score.LevelCompleted).ToList();

            this.Scores = sortedScores;
        }

        /// <summary>
        ///     Sorts by name, then score, then level
        /// </summary>
        public void SortByNameScoreLevel()
        {
            var sortedScores = this.Scores.OrderBy(score => score.PlayerName)
                                   .ThenByDescending(score => score.Score)
                                   .ThenByDescending(score => score.LevelCompleted).ToList();

            this.Scores = sortedScores;
        }

        /// <summary>
        ///     Sorts by level, then score, then name
        /// </summary>
        public void SortByLevelScoreName()
        {
            var sortedScores = this.Scores.OrderByDescending(score => score.LevelCompleted)
                                   .ThenByDescending(score => score.Score)
                                   .ThenBy(score => score.PlayerName).ToList();

            this.Scores = sortedScores;
        }

        #endregion
    }
}