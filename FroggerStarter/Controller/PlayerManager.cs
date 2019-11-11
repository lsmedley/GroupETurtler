using System.Collections.Generic;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PlayerSprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages a player, including its lives and score.
    /// </summary>
    public class PlayerManager
    {
        #region Data members

        /// <summary>
        ///     The player gameObject.
        /// </summary>
        public readonly Player Player;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the lives the player has left.
        /// </summary>
        /// <value>
        ///     The lives the player has left.
        /// </value>
        public int Lives { get; private set; }

        /// <summary>
        ///     Gets the Scores made by the player.
        /// </summary>
        /// <value>
        ///     The Scores made by the player.
        /// </value>
        public int ScoresMade { get; internal set; }

        /// <summary>
        ///     Gets the Scores to win.
        /// </summary>
        /// <value>
        ///     The Scores to win.
        /// </value>
        public int ScoresToWin { get; }

        /// <summary>
        ///     Gets the total score.
        /// </summary>
        /// <value>
        ///     The total score.
        /// </value>
        public int TotalScore { get; private set; }

        /// <summary>
        ///     Gets the player sprites.
        /// </summary>
        /// <value>
        ///     The player sprites.
        /// </value>
        public IList<BaseSprite> PlayerSprites { get; }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Controller.PlayerManager" /> is able to move.
        /// </summary>
        /// <value>
        ///     <c>true</c> if unable to move; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Controller.PlayerManager" /> class.
        ///     Postcondition: this.Player is a new instance of <see cref="Model.Player" /> class, this.Lives == 3,
        ///     this.ScoresMade == 0.
        /// </summary>
        /// <param name="lives">The number of lives this player starts with</param>
        /// <param name="winScore">
        ///     The number of times that this player needs to score in order
        ///     to win.
        /// </param>
        public PlayerManager(int lives, int winScore)
        {
            this.Disabled = false;
            this.Lives = lives;
            this.ScoresToWin = winScore;
            this.ScoresMade = 0;
            this.TotalScore = 0;
            this.PlayerSprites = new List<BaseSprite>();
            this.Player = new Player();
            this.setUpSprites();
            this.Player.ChangeSprite(this.PlayerSprites[0]);
        }

        #endregion

        #region Methods

        private void setUpSprites()
        {
            this.PlayerSprites.Add(new PlayerSprite());
            this.PlayerSprites.Add(new DeathSprite1());
            this.PlayerSprites.Add(new DeathSprite2());
            this.PlayerSprites.Add(new DeathSprite3());
            this.PlayerSprites.Add(new DeathSprite4());
        }

        /// <summary>
        ///     Sets the location of this.Player.
        ///     Postcondition: this.Player.X == x, this.Player.Y == y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void SetLocation(double x, double y)
        {
            this.Player.X = x;
            this.Player.Y = y;
            this.syncSpriteToLocation();
        }

        private void syncSpriteToLocation()
        {
            this.Player.Sprite.RenderAt(this.Player.X, this.Player.Y);
        }

        /// <summary>
        ///     Loses a life.
        /// </summary>
        public void LoseLife()
        {
            this.Lives--;
        }

        /// <summary>
        ///     Moves the player to the left if the player is within bounds, enabled, and if the game is still going.
        /// </summary>
        public void MoveLeft()
        {
            if (this.Player.X > 0 && this.Lives > 0 && this.ScoresMade < this.ScoresToWin && !this.Disabled)
            {
                this.Player.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player to the right if the player is within bounds, enabled, and if the game is still going.
        /// </summary>
        /// <param name="maxRight">The maximum distance right.</param>
        public void MoveRight(double maxRight)
        {
            if (this.Player.X < maxRight - this.Player.Width && this.Lives > 0 && this.ScoresMade < this.ScoresToWin &&
                !this.Disabled)
            {
                this.Player.MoveRight();
            }
        }

        /// <summary>
        ///     Moves the player up if the player is within bounds, enabled, and if the game is still going.
        /// </summary>
        /// <param name="topOfGameOffset">The highest point that the player can go.</param>
        public void MoveUp(int topOfGameOffset)
        {
            if (this.Player.Y > topOfGameOffset && this.Lives > 0 && this.ScoresMade < this.ScoresToWin &&
                !this.Disabled)
            {
                this.Player.MoveUp();
            }
        }

        /// <summary>
        ///     Moves the player down if the player is within bounds, enabled, and if the game is still going.
        /// </summary>
        /// <param name="roadHeight">Maximum distance that the player can move down.</param>
        public void MoveDown(double roadHeight)
        {
            if (this.Player.Y < roadHeight && this.Lives > 0 && this.ScoresMade < this.ScoresToWin && !this.Disabled)
            {
                this.Player.MoveDown();
            }
        }

        /// <summary>
        ///     Updates score and ScoresMade .
        /// </summary>
        /// <param name="timeLeft">The time left to score</param>
        public void HasScored(int timeLeft)
        {
            this.ScoresMade++;
            this.TotalScore += 5 + timeLeft;
        }

        /// <summary>
        ///     Moves to next sprite in this.PlayerSprites.
        ///     Postcondition: this.Sprite == the next sprite in this.PlayerSprites, or the first sprite
        ///     if already on the last sprite in the list.
        /// </summary>
        public void MoveToNextSprite()
        {
            var current = this.PlayerSprites.IndexOf(this.Player.Sprite);
            if (current == this.PlayerSprites.Count - 1)
            {
                this.Player.ChangeSprite(this.PlayerSprites[0]);
            }
            else
            {
                this.Player.ChangeSprite(this.PlayerSprites[current + 1]);
            }

            this.syncSpriteToLocation();
        }

        /// <summary>
        ///     Changes player sprite to dead sprite.
        ///     Postcondition: this.Player.sprite == the final frame of the death animation.
        /// </summary>
        public void MoveToDeadSprite()
        {
            this.Player.ChangeSprite(this.PlayerSprites[this.PlayerSprites.Count - 1]);
            this.syncSpriteToLocation();
        }

        #endregion
    }
}