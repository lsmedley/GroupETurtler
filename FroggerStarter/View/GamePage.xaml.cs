using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Controller;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;

        private readonly TextBlock livesTextBlock;
        private readonly TextBlock scoreTextBlock;
        private readonly TextBlock gameOverTextBlock;

        private static readonly Brush HudBrush = new SolidColorBrush(Colors.White);
        private static readonly Brush TitleBrush = new SolidColorBrush(Colors.YellowGreen);

        private readonly MediaPlayer soundPlayer;
        private StorageFile gameOverSound;
        private StorageFile gameWonSound;
        private StorageFile vehicleCollisionSound;
        private StorageFile scoreMadeSound;
        private StorageFile wallCollisionSound;
        private StorageFile timerDeathSound;

        private readonly MediaPlayer musicPlayer;
        private StorageFile Level1Music;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePage"/> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size
                {Width = this.applicationWidth, Height = this.applicationHeight};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                           .SetPreferredMinSize(new Size(this.applicationWidth, this.applicationHeight));

            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
            this.gameManager = new GameManager(this.applicationHeight, this.applicationWidth);
            this.gameManager.InitializeGame(this.canvas);

            this.livesTextBlock = new TextBlock {Text = $"Lives: {this.gameManager.Lives.ToString()}"};
            this.scoreTextBlock = new TextBlock {Text = $"Score: {this.gameManager.TotalScore.ToString()}"};
            var titleTextBlock = new TextBlock { Text = "Turtler" };
            this.gameOverTextBlock = new TextBlock {Text = "Game Over"};

            this.canvas.Children.Add(this.livesTextBlock);
            this.canvas.Children.Add(this.scoreTextBlock);
            this.canvas.Children.Add(titleTextBlock);

            this.livesTextBlock.FontSize = 15;
            Canvas.SetTop(this.livesTextBlock, 10);
            Canvas.SetLeft(this.livesTextBlock, 10);
            this.livesTextBlock.Foreground = HudBrush;

            this.scoreTextBlock.FontSize = 15;
            Canvas.SetTop(this.scoreTextBlock, 10);
            Canvas.SetLeft(this.scoreTextBlock, this.applicationWidth - 70);
            this.scoreTextBlock.Foreground = HudBrush;

            titleTextBlock.FontSize = 20;
            Canvas.SetTop(titleTextBlock, 10);
            Canvas.SetLeft(titleTextBlock, this.applicationWidth / 2 - 25);
            titleTextBlock.Foreground = TitleBrush;

            this.gameManager.LivesUpdated += this.onLivesUpdated;
            this.gameManager.ScoreUpdated += this.onScoreUpdated;
            this.gameManager.GameOver += this.onGameOver;

            this.soundPlayer = new MediaPlayer();
            this.musicPlayer = new MediaPlayer();
            this.setUpPlayers();
        }

        private async void setUpPlayers()
        {
            await this.setUpSoundPlayer();

            await this.setUpMusicPlayer();
        }

        private async Task setUpMusicPlayer()
        {
            Windows.Storage.StorageFolder musicFolder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"View\\Music");
            this.Level1Music = await musicFolder.GetFileAsync("Level1Song.wav");
            this.musicPlayer.AutoPlay = true;
            this.musicPlayer.IsLoopingEnabled = true;
            this.musicPlayer.Source = MediaSource.CreateFromStorageFile(this.Level1Music);
        }

        private async Task setUpSoundPlayer()
        {
            Windows.Storage.StorageFolder folder =
                await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"View\\SoundEffects");
            this.gameOverSound = await folder.GetFileAsync("GameOverSound.wav");
            this.gameWonSound = await folder.GetFileAsync("GameWonSound.wav");
            this.vehicleCollisionSound = await folder.GetFileAsync("CarCollisionSound.wav");
            this.wallCollisionSound = await folder.GetFileAsync("WallCollisionSound.wav");
            this.timerDeathSound = await folder.GetFileAsync("TimerDeathSound.wav");
            this.scoreMadeSound = await folder.GetFileAsync("ScoreMadeSound.wav");

            this.soundPlayer.AutoPlay = false;
        }

        private void onGameOver(object sender, SoundType sound)
        {
            this.initializeGameOverText();
            this.musicPlayer.Pause();
            if (sound == SoundType.GameLost)
            {
                this.playSoundEffect(this.gameOverSound);
            }
            else
            {
                this.playSoundEffect(this.gameWonSound);
            }
        }

        

        private void initializeGameOverText()
        {
            this.gameOverTextBlock.FontSize = 60;
            this.gameOverTextBlock.Foreground = HudBrush;
            Canvas.SetTop(this.gameOverTextBlock, this.applicationHeight / 2 - 40);
            Canvas.SetLeft(this.gameOverTextBlock, this.applicationWidth / 2 - 150);
            this.canvas.Children.Add(this.gameOverTextBlock);

        }

        private void onScoreUpdated(object sender, EventArgs e)
        {
            this.scoreTextBlock.Text = $"Score: {this.gameManager.TotalScore.ToString()}";
            this.playSoundEffect(this.scoreMadeSound);
        }

        private void onLivesUpdated(object sender, SoundType sound)
        {
            this.livesTextBlock.Text = $"Lives: {this.gameManager.Lives.ToString()}";
            if (sound.Equals(SoundType.VehicleDeath))
            {
                this.playSoundEffect(this.vehicleCollisionSound);
            } else if (sound.Equals(SoundType.WallDeath))
            {
                this.playSoundEffect(this.wallCollisionSound);
            } else if (sound.Equals(SoundType.TimeDeath))
            {
                this.playSoundEffect(this.timerDeathSound);
            }
        }

        private void playSoundEffect(Windows.Storage.StorageFile sound)
        {
            this.soundPlayer.Source = MediaSource.CreateFromStorageFile(sound);
            this.soundPlayer.Play();
        }



        #endregion

        #region Methods

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.gameManager.MovePlayerLeft();
                    break;
                case VirtualKey.Right:
                    this.gameManager.MovePlayerRight();
                    break;
                case VirtualKey.Up:
                    this.gameManager.MovePlayerUp();
                    break;
                case VirtualKey.Down:
                    this.gameManager.MovePlayerDown();
                    break;
            }
        }

        #endregion
    }
}