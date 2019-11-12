using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Model;
using FroggerStarter.Utils;
using FroggerStarter.View;
using FroggerStarter.View.Sprites.PlayerSprites;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Manages all aspects of the game play including moving the playerManager,
    ///     the Vehicles as well as lives and score.
    /// </summary>
    public class GameManager
    {
        #region Data members

        private readonly double roadHeight;
        private readonly double riverHeight;
        private readonly double backgroundWidth;

        private Canvas gameCanvas;

        private DispatcherTimer timer;
        private DispatcherTimer deathTimer;
        private DispatcherTimer movementAnimationTimer;
        private LevelTimeManager levelTimer;
        private ProgressBar timerBar;

        private Road road;
        private River river;
        private PlayerManager playerManager;
        private HomeManager homes;
        private PowerupManager powerUpManager;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current level.
        /// </summary>
        /// <value>
        ///     The current level.
        /// </value>
        public int CurrentLevel { get; private set; }

        private int TimeLeft => Math.Abs(this.levelTimer.CurrTime - this.levelTimer.MaxTime);

        /// <summary>
        ///     Gets the lives the playerManager has left.
        /// </summary>
        /// <value>
        ///     The lives the playerManager has left.
        /// </value>
        public int Lives => this.playerManager.Lives;

        /// <summary>
        ///     Gets the Scores made by the playerManager.
        /// </summary>
        /// <value>
        ///     The number of Scores made by the playerManager.
        /// </value>
        public int ScoresMade
        {
            get => this.playerManager.ScoresMade;
            set => this.playerManager.ScoresMade = value;
        }

        /// <summary>
        ///     Gets the total score.
        /// </summary>
        /// <value>
        ///     The total score of the playerManager.
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
            this.roadHeight = backgroundHeight - GameSettings.BottomLaneOffset - GameSettings.TileHeight;
            this.riverHeight = backgroundHeight - GameSettings.TopOfGameOffset -
                               GameSettings.RiverLaneSettingsCollection.Count * GameSettings.TileHeight;
            this.CurrentLevel = 1;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [lives updated].
        /// </summary>
        public event EventHandler<SoundType> LivesUpdated;

        /// <summary>
        ///     Occurs when [level updated].
        /// </summary>
        public event EventHandler<EventArgs> LevelUpdated;

        /// <summary>
        ///     Occurs when [score updated].
        /// </summary>
        public event EventHandler<EventArgs> ScoreUpdated;

        /// <summary>
        ///     Occurs when [game over].
        /// </summary>
        public event EventHandler<SoundType> GameOver;

        /// <summary>
        ///     Occurs when [power up activated].
        /// </summary>
        public event EventHandler<SoundType> PowerUpActivated;

        /// <summary>
        ///     Occurs when [slow down ended].
        /// </summary>
        public event EventHandler<EventArgs> SlowDownEnded;

        /// <summary>
        ///     Initializes the game working with appropriate classes to play frog
        ///     and vehicle on game screen.
        ///     Precondition: background != null
        ///     Postcondition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="gamePage">The game page.</param>
        /// <exception cref="ArgumentNullException">gameCanvas</exception>
        public void InitializeGame(Canvas gamePage)
        {
            this.gameCanvas = gamePage ?? throw new ArgumentNullException(nameof(gamePage));
            this.createHomeManager(GameSettings.ScoresToWin);
            this.createRiver();
            this.createAndPlacePlayer(GameSettings.PlayerLives, GameSettings.ScoresToWin);
            this.createRoadManager();
            this.createPowerupManager();
            this.setUpTimers(GameSettings.TimerLengthSeconds);
        }

        private void createPowerupManager()
        {
            this.powerUpManager = new PowerupManager();
            this.gameCanvas.Children.Add(this.powerUpManager.GetTimeSprite());
            this.gameCanvas.Children.Add(this.powerUpManager.GetVehicleSprite());
        }

        private void createHomeManager(int numHomes)
        {
            this.homes = new HomeManager(GameSettings.TopOfGameOffset - GameSettings.TileHeight + 5,
                this.backgroundWidth, numHomes);
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
            this.road = new Road(GameSettings.LaneSettingsCollection, this.roadHeight, this.backgroundWidth);
            this.initializeRoad();
        }

        private void initializeRoad()
        {
            foreach (var vehicle in this.road)
            {
                this.gameCanvas.Children.Add(vehicle.Sprite);
            }

            this.road.CarAdded += this.onCarAdded;
            this.road.SlowdownEnded += this.onSlowDownEnded;
        }

        private void createRiver()
        {
            this.river = new River(GameSettings.RiverLaneSettingsCollection, this.riverHeight, this.backgroundWidth);
            this.initializeRiver();
        }

        private void initializeRiver()
        {
            foreach (var debris in this.river)
            {
                this.gameCanvas.Children.Add(debris.Sprite);
            }
        }

        private void createAndPlacePlayer(int lives, int score)
        {
            this.playerManager = new PlayerManager(lives, score);
            foreach (var sprite in this.playerManager.PlayerSprites)
            {
                this.gameCanvas.Children.Add(sprite);
                if (sprite.GetType() != typeof(PlayerSprite))
                {
                    sprite.Visibility = Visibility.Collapsed;
                }
            }

            this.gameCanvas.Children.Add(this.playerManager.MovingSprite);
            this.playerManager.MovingSprite.Visibility = Visibility.Collapsed;

            this.setPlayerToCenterOfBottomLane();
        }

        private void setPlayerToCenterOfBottomLane()
        {
            this.playerManager.SetLocation(this.backgroundWidth / 2 - this.playerManager.Player.Sprite.Width / 2,
                this.roadHeight);
            this.playerManager.TurnPlayer(Direction.Up);
        }

        private void setUpTimers(int timerLen)
        {
            this.setupGameTimer();
            this.setupDeathTimer();
            this.setupAnimationTimer();
            this.setUpLevelTimer(timerLen);
        }

        private void setupAnimationTimer()
        {
            this.movementAnimationTimer = new DispatcherTimer();
            this.movementAnimationTimer.Tick += this.movementTimerOnTick;
            this.movementAnimationTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
        }

        private void movementTimerOnTick(object sender, object e)
        {
            this.playerManager.Player.Sprite.Visibility = Visibility.Collapsed;
            this.playerManager.ToggleMovingSprite();
            this.playerManager.Player.Sprite.Visibility = Visibility.Visible;
            if (this.playerManager.Player.Sprite.Equals(this.playerManager.PlayerSprites[0]))
            {
                this.movementAnimationTimer.Stop();
            }
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

            this.timerBar.Height = GameSettings.TimerBarHeight;
            Canvas.SetTop(this.timerBar, (double) GameSettings.TopOfGameOffset / 2 - this.timerBar.Height);
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
            this.updateRoad();
            this.updateRiver();
            this.timerBar.Value = GameSettings.TimerBlockWidth * this.TimeLeft;
            this.updatePowerUps();
        }

        private void updateRiver()
        {
            this.playerManager.Player.X += this.river.MoveRiver(this.backgroundWidth, this.playerManager.Player);
            if (this.playerManager.Player.Y <= this.riverHeight - GameSettings.TileHeight &&
                !this.river.CheckCollision(this.playerManager.Player, this.playerManager.Disabled) &&
                !this.playerManager.Disabled)
            {
                this.onPlayerDeath(SoundType.WaterDeath);
            }
        }

        private void updatePowerUps()
        {
            this.powerUpManager.OnTick(this.backgroundWidth, GameSettings.TopOfGameOffset, this.roadHeight);
            if (this.powerUpManager.CheckTimeCollision(this.playerManager.Player, this.playerManager.Disabled))
            {
                this.levelTimer.AddTime(GameSettings.TimePowerUpAmount);
                this.onPowerUp(SoundType.TimePowerUp);
            }

            if (this.powerUpManager.CheckVehicleCollision(this.playerManager.Player, this.playerManager.Disabled))
            {
                this.road.SlowDownVehicles(GameSettings.VehicleSlowDownTickLength);
                this.onPowerUp(SoundType.VehiclePowerUp);
            }
        }

        private void updateRoad()
        {
            var speedToAdd = 0;
            if (this.CurrentLevel == 2)
            {
                speedToAdd = GameSettings.Level2AdditionalSpeedOnTick;
            }
            else if (this.CurrentLevel == 3)
            {
                speedToAdd = GameSettings.Level3AdditionalSpeedOnTick;
            }

            if (this.road.CheckCollision(this.playerManager.Player, this.playerManager.Disabled))
            {
                this.onPlayerDeath(SoundType.VehicleDeath);
            }

            this.road.OnTick(this.backgroundWidth, speedToAdd);
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
                    this.endGame();
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

        private void onPlayerDeath(SoundType deathType)
        {
            this.playerManager.Disabled = true;
            this.levelTimer.Pause();
            this.movementAnimationTimer.Stop();
            this.deathTimer.Start();
            this.playerManager.LoseLife();
            this.onLivesUpdated(deathType);
            this.road.ResetSpeeds();
            this.levelTimer.Reset();
        }

        private void resetRoad()
        {
            foreach (var v in this.road)
            {
                this.gameCanvas.Children.Remove(v.Sprite);
            }

            this.road.SetUpLanes(this.roadHeight, this.backgroundWidth);
            foreach (var v in this.road)
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
            if (!this.playerManager.Disabled)
            {
                this.movementAnimationTimer.Start();
                this.playerManager.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the playerManager to the right.
        ///     Precondition: none
        ///     Postcondition: playerManager.X = playerManager.X@prev + playerManager.Width
        /// </summary>
        public void MovePlayerRight()
        {
            if (!this.playerManager.Disabled)
            {
                this.movementAnimationTimer.Start();
                this.playerManager.MoveRight(this.backgroundWidth);
            }
        }

        /// <summary>
        ///     Moves the playerManager up.
        ///     Precondition: none
        ///     Postcondition: playerManager.Y = playerManager.Y@prev - playerManager.Height
        /// </summary>
        public void MovePlayerUp()
        {
            if (!this.playerManager.Disabled)
            {
                this.playerManager.MoveUp(GameSettings.TopOfGameOffset);
                this.checkVictory();
                this.movementAnimationTimer.Start();
            }
        }

        /// <summary>
        ///     Moves the playerManager down.
        ///     Precondition: none
        ///     Postcondition: playerManager.Y = playerManager.Y@prev + playerManager.Height
        /// </summary>
        public void MovePlayerDown()
        {
            if (!this.playerManager.Disabled)
            {
                this.movementAnimationTimer.Start();
                this.playerManager.MoveDown(this.roadHeight);
            }
        }

        private void checkVictory()
        {
            var collidedHome = this.homes.CheckCollision(this.playerManager.Player);
            if (collidedHome)
            {
                this.processPlayerHome();
            }
            else
            {
                this.checkIfPlayerHitWall();
            }

            this.checkIfLevelComplete();
        }

        private void processPlayerHome()
        {
            this.setPlayerToCenterOfBottomLane();
            this.playerManager.HasScored(this.TimeLeft);
            this.onScoreUpdated();
            this.levelTimer.Reset();
        }

        private void checkIfLevelComplete()
        {
            if (this.ScoresMade >= this.playerManager.ScoresToWin)
            {
                this.onLevelComplete();
            }
        }

        private void onLevelComplete()
        {
            switch (this.CurrentLevel)
            {
                case 1:
                    this.changeToRound2();
                    break;
                case 2:
                    this.changeToRound3();
                    break;
                case 3:
                    this.endGame();
                    break;
            }
        }

        private void endGame()
        {
            this.timer.Stop();
            this.levelTimer.Pause();
            this.playerManager.Disabled = true;
            if (this.Lives <= 0)
            {
                this.setPlayerGameOverSprite();
                this.CurrentLevel--;
                this.GameOver?.Invoke(this, SoundType.GameLost);
            }
            else
            {
                this.GameOver?.Invoke(this, SoundType.GameWon);
            }
        }

        private void changeToRound3()
        {
            this.resetHomes();
            this.CurrentLevel = 3;
            this.onLevelUpdated();
            this.road.IncreaseMaxVehiclesBy(1);
            this.road.IncreaseStartSpeedBy(1);
            this.road.ResetSpeeds();
            this.playerManager.AddLife();
            this.onLivesUpdated(SoundType.GameWon);
        }

        private void changeToRound2()
        {
            this.resetHomes();
            this.CurrentLevel = 2;
            this.onLevelUpdated();
            this.playerManager.AddLife();
            this.onLivesUpdated(SoundType.GameWon);
        }

        private void resetHomes()
        {
            foreach (var home in this.homes)
            {
                home.TakenSprite.Visibility = Visibility.Collapsed;
                home.IsTaken = false;
            }

            this.ScoresMade = 0;
        }

        private void checkIfPlayerHitWall()
        {
            if (this.playerManager.Player.Y < GameSettings.TopOfGameOffset + 1)
            {
                this.onPlayerDeath(SoundType.WallDeath);
            }
        }

        private void onLivesUpdated(SoundType deathType)
        {
            this.LivesUpdated?.Invoke(this, deathType);
        }

        private void onScoreUpdated()
        {
            this.ScoreUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void onLevelUpdated()
        {
            this.LevelUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void onPowerUp(SoundType sound)
        {
            this.PowerUpActivated?.Invoke(this, sound);
        }

        private void onTimeUp(object sender, EventArgs e)
        {
            this.onPlayerDeath(SoundType.TimeDeath);
        }

        private void onCarAdded(object sender, EventArgs e)
        {
            foreach (var vehicle in this.road)
            {
                if (!this.gameCanvas.Children.Contains(vehicle.Sprite))
                {
                    this.gameCanvas.Children.Add(vehicle.Sprite);
                }
            }
        }

        private void onSlowDownEnded(object sender, EventArgs e)
        {
            this.SlowDownEnded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Saves the high score.
        /// </summary>
        /// <param name="name">The name.</param>
        public async Task SaveHighScore(string name)
        {
            var highScore = new HighScore
                {LevelCompleted = this.CurrentLevel, PlayerName = name, Score = this.TotalScore};

            var highScores = ScoreSerializer.Deserialize();
            highScores.Add(highScore);

            await ScoreSerializer.SerializeObject(highScores);

            this.gameCanvas.Children.Add(new HighScoreBoard());
        }

        #endregion
    }
}