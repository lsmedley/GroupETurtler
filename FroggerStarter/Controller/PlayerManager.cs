using FroggerStarter.Model;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages a player, including its lives and score.
    /// </summary>
    public class PlayerManager
    {
        /// <summary>
        /// Gets the lives the player has left.
        /// </summary>
        /// <value>
        /// The lives the player has left.
        /// </value>
        public int Lives { get; private set; }
        /// <summary>
        /// Gets the score of the player.
        /// </summary>
        /// <value>
        /// The score of the player.
        /// </value>
        public int Score { get; private set; }
        /// <summary>
        /// The player gameObject.
        /// </summary>
        public readonly Turtle Player;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerManager"/> class.
        /// Postcondition: this.Player is a new instance of <see cref="Turtle"/> class, this.Lives == 3,
        /// this.Score == 0.
        /// </summary>
        /// <param name="lives"></param>
        public PlayerManager(int lives)
        {
            this.Player = new Turtle();
            this.Lives = lives;
            this.Score = 0;
        }

        /// <summary>
        /// Sets the location of this.Player.
        /// Postcondition: this.Player.X == x, this.Player.Y == y.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public void SetLocation(double x, double y)
        {
            this.Player.X = x;
            this.Player.Y = y;
        }

        /// <summary>
        /// Loses a life.
        /// </summary>
        public void LoseLife()
        {
            this.Lives--;
        }

        /// <summary>
        /// Moves the player to the left if the player is within bounds and the game is still going.
        /// </summary>
        public void MoveLeft()
        {
            if (this.Player.X > 0 && this.Lives > 0 && this.Score < 3)
            {
                this.Player.MoveLeft();
            }
        }

        /// <summary>
        /// Moves the player to the right if the player is within bounds and the game is still going.
        /// </summary>
        /// <param name="maxRight">The maximum distance right.</param>
        public void MoveRight(double maxRight)
        {
            if (this.Player.X < maxRight - this.Player.Width && this.Lives > 0 && this.Score < 3)
            {
                this.Player.MoveRight();
            }
        }

        /// <summary>
        /// Moves the player up if the player is within bounds and the game is still going.
        /// </summary>
        /// <param name="topOfGameOffset">The highest point that the player can go.</param>
        public void MoveUp(int topOfGameOffset)
        {
            if (this.Player.Y > topOfGameOffset && this.Lives > 0 && this.Score < 3)
            {
                this.Player.MoveUp();
            }
        }

        /// <summary>
        /// Moves the player down if the player is within bounds and the game is still going.
        /// </summary>
        /// <param name="roadHeight">Maximum distance that the player can move down.</param>
        public void MoveDown(double roadHeight)
        {
            if (this.Player.Y < roadHeight && this.Lives > 0 && this.Score < 3)
            {
                this.Player.MoveDown();
            }
        }

        /// <summary>
        /// Checks if the player has won a point.
        /// Postcondition: if player.Y > winHeight, score == @prev score+1
        /// </summary>
        /// <param name="winHeight">Height above which the player has won a point.</param>
        /// <returns>true if the player won a point, false otherwise.</returns>
        public bool CheckWin(int winHeight)
        {
            var isWin = false;
            if (this.Player.Y <= winHeight)
            {
                this.Score++;
                isWin = true;
            }

            return isWin;
        }
    }
}
