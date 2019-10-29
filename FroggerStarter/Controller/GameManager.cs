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
    ///     Manages all aspects of the game play including moving the player,
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
        private const int TopOfGameOffset = 100;
        private const int TileHeight = 50;

        private readonly double roadHeight;
        private readonly double backgroundWidth;

        private Canvas gameCanvas;

        private DispatcherTimer timer;
        private DispatcherTimer deathTimer;
        private LevelTimeManager levelTimer;
        private int TimeLeft => Math.Abs(this.levelTimer.CurTime - this.levelTimer.MaxTime);
        private ProgressBar timerBar;

        private RoadManager rm;
        private PlayerManager player;
        private HomeManager homes;
        private IList<TurtleSprite> takenTokens;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the lives the player has left.
        /// </summary>
        /// <value>
        ///     The lives.
        /// </value>
        public int Lives => this.player.Lives;

        /// <summary>
        ///     Gets the scores made by the player.
        /// </summary>
        /// <value>
        ///     The number of scores made by the player.
        /// </value>
        public int ScoresMade => this.player.ScoresMade;

        /// <summary>
        /// Gets the total score.
        /// </summary>
        /// <value>
        /// The total score.
        /// </value>
        public int TotalScore => this.player.TotalScore;

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
        /// <param name="gameSet">The game settings.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage, GameSettings gameSet)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createHomeManager(gameSet.ScoresToWin);
            this.createTakenTokens(gameSet.ScoresToWin);
            this.createAndPlacePlayer(gameSet.PlayerLives, gameSet.ScoresToWin);
            this.createRoadManager();
            this.setUpTimers(gameSet.TimerLengthSeconds);
        }

        private void createTakenTokens(int scoresToWin)
        {
            this.takenTokens = new List<TurtleSprite>();
            foreach (var home in this.homes)
            {
                var tToken = new TurtleSprite();
                tToken.RenderAt(home.X, home.Y);
                this.takenTokens.Add(tToken);
            }

            foreach (var tToken in this.takenTokens)
            {
                this.gameCanvas.Children.Add(tToken);
                tToken.Visibility = Visibility.Collapsed;
            }
        }

        private void createHomeManager(int homeNum)
        {
            this.homes = new HomeManager(TopOfGameOffset - TileHeight + 5, this.backgroundWidth, homeNum);
            foreach (var home in this.homes)
            {
                this.gameCanvas.Children.Add(home.Sprite);
            }

        }

        private void createRoadManager()
        {
            var traffic = new List<(int, VehicleType)> {
                (3, VehicleType.Car),
                (2, VehicleType.Bus),
                (4, VehicleType.Car),
                (3, VehicleType.Bus),
                (5, VehicleType.Car)
            };
            var flow = new List<Direction> {
                Direction.Left,
                Direction.Right,
                Direction.Left,
                Direction.Left,
                Direction.Right
            };
            var speeds = new List<int> {
                1,
                3,
                5,
                7,
                9
            };
            LaneSettings laneset = new LaneSettings(traffic, flow, speeds);
            this.rm = new RoadManager(laneset, this.roadHeight, this.backgroundWidth);
            this.initializeRoad();
        }

       
        private void initializeRoad()
        {
            foreach (var vehicle in this.rm)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }
            this.rm.CarAdded += this.onCarAdded;
        }

        private void createAndPlacePlayer(int lives, int score)
        {
            this.player = new PlayerManager(lives, score);
            this.gameCanvas.Children.Add(this.player.Player.Sprite);
            foreach(var sp in this.player.PlayerSprites)
            {
                this.gameCanvas.Children.Add(sp);
                sp.Visibility = Visibility.Collapsed;
            }

            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.player.SetLocation(this.backgroundWidth / 2 - this.player.Player.Sprite.Width / 2, this.roadHeight);
        }

        private void setUpTimers(int timerLen)
        {
            this.setupGameClock();
            this.setupDeathClock();
            this.setUpLevelClock(timerLen);
        }

        private void setUpLevelClock(int tLen)
        {
            this.levelTimer = new LevelTimeManager(tLen);
            this.levelTimer.TimeUp += this.onTimeUp;

            this.initializeTimerBar();
        }

        private void initializeTimerBar()
        {
            this.timerBar = new ProgressBar();
            this.timerBar.Value = this.TimeLeft;
            this.gameCanvas.Children.Add(this.timerBar);

            this.timerBar.Height = 10;
            Canvas.SetTop(this.timerBar, TopOfGameOffset / 2 - this.timerBar.Height);
            this.timerBar.Width = this.backgroundWidth;
            this.timerBar.Visibility = Visibility.Visible;
            this.timerBar.Background = new SolidColorBrush(Colors.Transparent);
            this.timerBar.Foreground = new SolidColorBrush(Colors.YellowGreen);
        }

        private void setupGameClock()
        {
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.timerOnTick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            this.timer.Start();
        }

        private void setupDeathClock()
        {
            this.deathTimer = new DispatcherTimer();
            this.deathTimer.Tick += this.deathTimerOnTick;
            this.deathTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
        }


        private void timerOnTick(object sender, object e)
        {
            this.rm.OnTick(this.backgroundWidth);
            this.timerBar.Value = 5 * this.TimeLeft;
            if (this.rm.CheckCollision(this.player.Player))
            {
                this.onCollision();
            }
        }

        private void deathTimerOnTick(object sender, object e)
        {
            this.player.Player.Sprite.Visibility = Visibility.Collapsed;
            this.player.MoveToNextSprite();
            this.player.SyncSpriteToLocation();
            this.player.Player.Sprite.Visibility = Visibility.Visible;
            if (this.player.Player.Sprite.Equals(this.player.PlayerSprites[0]))
            {
                this.deathTimer.Stop();
                if (this.Lives > 0)
                {
                    this.timer.Start();
                    this.player.Disabled = false;
                    this.setPlayerToCenterOfBottomLane();
                    this.levelTimer.UnPause();
                }
                else
                {
                    this.player.Player.Sprite.Visibility = Visibility.Collapsed;
                    this.player.MoveToDeadSprite();
                    this.player.SyncSpriteToLocation();
                    this.player.Player.Sprite.Visibility = Visibility.Visible;
                }
            }
        }

        private void onCollision()
        {
            this.player.Disabled = true;
            this.timer.Stop();
            this.levelTimer.Pause();
            this.deathTimer.Start();

            this.player.LoseLife();
            this.onLivesUpdated();
            this.levelTimer.Reset();
            if (this.Lives <= 0)
            {
                this.onGameOver();
            }
            else
            {
                this.resetRoad();
            }

        }

        private void resetRoad()
        {
            foreach (var v in this.rm)
            {
                this.gameCanvas.Children.Remove(v.Sprite);
            }

            this.rm.SetUpLanes(this.roadHeight, this.backgroundWidth);
            foreach (var v in this.rm)
            {
                this.gameCanvas.Children.Add(v.Sprite);
            }
        }

        /// <summary>
        ///     Moves the player to the left.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev - player.Width
        /// </summary>
        public void MovePlayerLeft()
        {
            this.player.MoveLeft();
        }

        /// <summary>
        ///     Moves the player to the right.
        ///     Precondition: none
        ///     Postcondition: player.X = player.X@prev + player.Width
        /// </summary>
        public void MovePlayerRight()
        {
            this.player.MoveRight(this.backgroundWidth);
        }

        /// <summary>
        ///     Moves the player up.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev - player.Height
        /// </summary>
        public void MovePlayerUp()
        {
            this.player.MoveUp(TopOfGameOffset);

            this.checkVictory();
        }

        /// <summary>
        ///     Moves the player down.
        ///     Precondition: none
        ///     Postcondition: player.Y = player.Y@prev + player.Height
        /// </summary>
        public void MovePlayerDown()
        {
            this.player.MoveDown(this.roadHeight);
        }

        private void checkVictory()
        {
            //this.player.CheckWin(TopOfGameOffset + 1)
            var collidedHome = this.homes.CheckCollision(this.player.Player);
            if ( collidedHome != -1)
            {
                this.setPlayerToCenterOfBottomLane();
                this.player.HasScored(this.TimeLeft);
                this.onScoreUpdated();
                this.updateHomes(collidedHome);
                this.levelTimer.Reset();
            }
            else if (this.player.Player.Y < TopOfGameOffset + 1)
            {
                this.onCollision();
            }

            
            if (this.ScoresMade < this.player.ScoresToWin)
            {
                return;
            }

            this.onGameOver();
        }

        private void updateHomes(int homeTaken)
        {
            this.takenTokens[homeTaken].Visibility = Visibility.Visible;
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
            this.GameOver?.Invoke(this, EventArgs.Empty);
        }

        private void onTimeUp(object sender, EventArgs e)
        {
            this.onCollision();
        }

        private void onCarAdded(object sender, EventArgs e)
        {
            foreach (var vehicle in this.rm)
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