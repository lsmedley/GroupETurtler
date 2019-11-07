using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Model;
using FroggerStarter.View.Sprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the playerManager,
    ///     the Vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        /// <summary>
        ///     Occurs when [lives updated].
        /// </summary>
        public event EventHandler<EventArgs> LivesUpdated;
        /// <summary>
        ///     Occurs when [score updated].
        /// </summary>
        public event EventHandler<EventArgs> ScoreUpdated;
        /// <summary>
        ///     Occurs when [game over].
        /// </summary>
        public event EventHandler<EventArgs> GameOver;

        private const int BottomLaneOffset = 5;
        private const int TimerBlockWidth = 5;
        private const int TimerBarHeight = 10;
        private const int TopOfGameOffset = 100;
        private const int TileHeight = 50;

        private readonly double roadHeight;
        private readonly double backgroundWidth;

        private Canvas gameCanvas;

        private DispatcherTimer timer;
        private DispatcherTimer deathTimer;
        private LevelTimeManager levelTimer;
        private int TimeLeft => Math.Abs(this.levelTimer.CurrTime - this.levelTimer.MaxTime);
        private ProgressBar timerBar;

        private RoadManager roadManager;
        private PlayerManager playerManager;
        private HomeManager homes;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the lives the playerManager has left.
        /// </summary>
        /// <value>
        ///     The lives the playerManager has left.
        /// </value>
        public int Lives => this.playerManager.Lives;

        /// <summary>
        ///     Gets the scores made by the playerManager.
        /// </summary>
        /// <value>
        ///     The number of scores made by the playerManager.
        /// </value>
        public int ScoresMade => this.playerManager.ScoresMade;

        /// <summary>
        /// Gets the total score.
        /// </summary>
        /// <value>
        /// The total score of the playerManager.
        /// </value>
        public int TotalScore => this.playerManager.TotalScore;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="backgroundHeight">Height of the background.</param>
        /// <param name="backgroundWidth">Width of the background.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     backgroundHeight &lt;= 0
        ///     or
        ///     backgroundWidth &lt;= 0
        /// </exception>
        public GameManager(double backgroundHeight, double backgroundWidth)
        {
            if (backgroundHeight <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundHeight));
            }

            if (backgroundWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(backgroundWidth));
            }

            this.backgroundWidth = backgroundWidth;
            this.roadHeight = backgroundHeight - BottomLaneOffset - TileHeight;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the game working with appropriate classes to play frog
        /// and vehicle on game screen.
        /// Precondition: background != null
        /// Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createHomeManager(GameSettings.ScoresToWin);
            this.createAndPlacePlayer(GameSettings.PlayerLives, GameSettings.ScoresToWin);
            this.createRoadManager();
            this.setUpTimers(GameSettings.TimerLengthSeconds);
        }

        

        private void createHomeManager(int numHomes)
        {
            this.homes = new HomeManager(TopOfGameOffset - TileHeight + 5, this.backgroundWidth, numHomes);
            foreach (var home in this.homes)
            {
                this.gameCanvas.Children.Add(home.Sprite);
                this.gameCanvas.Children.Add(home.TakenSprite);
                Canvas.SetTop(home.TakenSprite, home.Y);
                Canvas.SetLeft(home.TakenSprite, home.X);
            }
        }

        private void createRoadManager()
        {
            this.roadManager = new RoadManager(GameSettings.LaneSettingsCollection, this.roadHeight, this.backgroundWidth);
            this.initializeRoad();
        }

       
        private void initializeRoad()
        {
            foreach (var vehicle in this.roadManager)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
            this.roadManager.CarAdded += this.onCarAdded;
        }

        private void createAndPlacePlayer(int lives, int score)
        {
            this.playerManager = new PlayerManager(lives, score);
            foreach(var sprite in this.playerManager.PlayerSprites)
            {
                this.gameCanvas.Children.Add(sprite);
                if (sprite.GetType() != typeof(PlayerSprite))
                {
                    sprite.Visibility = Visibility.Collapsed;
                }
            }

            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.playerManager.SetLocation(this.backgroundWidth / 2 - this.playerManager.Player.Sprite.Width / 2, this.roadHeight);
        }

        private void setUpTimers(int timerLen)
        {
            this.setupGameTimer();
            this.setupDeathTimer();
            this.setUpLevelTimer(timerLen);
        }

        private void setUpLevelTimer(int tLen)
        {
            this.levelTimer = new LevelTimeManager(tLen);
            this.levelTimer.TimeUp += this.onTimeUp;

            this.initializeTimerBar();
        }

        private void initializeTimerBar()
        {
            this.timerBar = new ProgressBar {Value = this.TimeLeft};
            this.gameCanvas.Children.Add(this.timerBar);

            this.timerBar.Height = TimerBarHeight;
            Canvas.SetTop(this.timerBar, (double)TopOfGameOffset / 2 - this.timerBar.Height);
            this.timerBar.Width = this.backgroundWidth;
            this.timerBar.Visibility = Visibility.Visible;
            this.timerBar.Background = new SolidColorBrush(Colors.Transparent);
            this.timerBar.Foreground = new SolidColorBrush(Colors.YellowGreen);
        }

        private void setupGameTimer()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.gameTimerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
        }

        private void setupDeathTimer()
        {
            this.deathTimer = new DispatcherTimer();
            this.deathTimer.Tick += this.deathTimerOnTick;
            this.deathTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
        }


        private void gameTimerOnTick(object sender, object e)
        {
            this.roadManager.OnTick(this.backgroundWidth);
            this.timerBar.Value = TimerBlockWidth * this.TimeLeft;
            if (this.roadManager.CheckCollision(this.playerManager.Player, this.playerManager.Disabled))
            {
                this.onPlayerDeath();
            }
        }

        private void deathTimerOnTick(object sender, object e)
        {
            this.animateDeath();
            if (this.playerManager.Player.Sprite.Equals(this.playerManager.PlayerSprites[0]))
            {
                this.deathTimer.Stop();
                if (this.Lives > 0)
                {
                    this.resumePlay();
                }
                else
                {
                    this.onGameOver();
                }
            }
        }

        private void resumePlay()
        {
            this.playerManager.Disabled = false;
            this.setPlayerToCenterOfBottomLane();
            this.levelTimer.UnPause();
            this.resetRoad();
        }

        private void setPlayerGameOverSprite()
        {
            this.playerManager.Player.Sprite.Visibility = Visibility.Collapsed;
            this.playerManager.MoveToDeadSprite();
            this.playerManager.Player.Sprite.Visibility = Visibility.Visible;
        }

        private void animateDeath()
        {
            this.playerManager.Player.Sprite.Visibility = Visibility.Collapsed;
            this.playerManager.MoveToNextSprite();
            this.playerManager.Player.Sprite.Visibility = Visibility.Visible;
        }

        private void onPlayerDeath()
        {
            this.playerManager.Disabled = true;
            this.levelTimer.Pause();
            this.deathTimer.Start();

            this.playerManager.LoseLife();
            this.onLivesUpdated();
            this.levelTimer.Reset();

        }

        private void resetRoad()
        {
            foreach (var v in this.roadManager)
            {
                this.gameCanvas.Children.Remove(v.Sprite);
            }

            this.roadManager.SetUpLanes(this.roadHeight, this.backgroundWidth);
            foreach (var v in this.roadManager)
            {
                this.gameCanvas.Children.Add(v.Sprite);
            }
        }

        /// <summary>
        ///     Moves the playerManager to the left.
        ///     Precondition: none
        ///     Postcondition: playerManager.X = playerManager.X@prev - playerManager.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.playerManager.MoveLeft();
        }

        /// <summary>
        ///     Moves the playerManager to the right.
        ///     Precondition: none
        ///     Postcondition: playerManager.X = playerManager.X@prev + playerManager.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.playerManager.MoveRight(this.backgroundWidth);
        }

        /// <summary>
        ///     Moves the playerManager up.
        ///     Precondition: none
        ///     Postcondition: playerManager.Y = playerManager.Y@prev - playerManager.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.playerManager.MoveUp(TopOfGameOffset);

            this.checkVictory();
        }

        /// <summary>
        ///     Moves the playerManager down.
        ///     Precondition: none
        ///     Postcondition: playerManager.Y = playerManager.Y@prev + playerManager.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.playerManager.MoveDown(this.roadHeight);
        }

        private void checkVictory()
        {
            var collidedHome = this.homes.CheckCollision(this.playerManager.Player);
            if ( collidedHome)
            {
                this.setPlayerToCenterOfBottomLane();
                this.playerManager.HasScored(this.TimeLeft);
                this.onScoreUpdated();
                this.levelTimer.Reset();
            }
            else if (this.playerManager.Player.Y < TopOfGameOffset + 1)
            {
                this.onPlayerDeath();
            }

            if (this.ScoresMade >= this.playerManager.ScoresToWin)
            {
                this.onGameOver();
            }
        }

        private void onLivesUpdated()
        {
            this.LivesUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void onScoreUpdated()
        {
            this.ScoreUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void onGameOver()
        {
            this.timer.Stop();
            this.levelTimer.Pause();
            if (this.Lives <= 0)
            {
                this.setPlayerGameOverSprite();
            }
            this.GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void onTimeUp(object sender, EventArgs e)
        {
            this.onPlayerDeath();
        }

        private void onCarAdded(object sender, EventArgs e)
        {
            foreach (var vehicle in this.roadManager)
            {
                if (!this.gameCanvas.Children.Contains(vehicle.Sprite))
                {
                    this.gameCanvas.Children.Add(vehicle.Sprite);
                }
            }
        }


        #endregion
    }
}