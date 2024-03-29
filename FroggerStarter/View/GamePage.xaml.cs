﻿using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using FroggerStarter.Controller;
using FroggerStarter.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private static readonly Brush HudBrush = new SolidColorBrush(Colors.White);
        private static readonly Brush TitleBrush = new SolidColorBrush(Colors.YellowGreen);

        private readonly double applicationHeight = (double) Application.Current.Resources["AppHeight"];
        private readonly double applicationWidth = (double) Application.Current.Resources["AppWidth"];
        private readonly GameManager gameManager;

        private readonly TextBlock livesTextBlock;
        private readonly TextBlock levelTextBlock;
        private readonly TextBlock scoreTextBlock;

        private readonly MediaPlayer soundPlayer;
        private StorageFile levelWonSound;
        private StorageFile gameOverSound;
        private StorageFile gameWonSound;
        private StorageFile vehicleCollisionSound;
        private StorageFile scoreMadeSound;
        private StorageFile wallCollisionSound;
        private StorageFile waterCollisionSound;
        private StorageFile timerDeathSound;
        private StorageFile timerPowerupSound;

        private readonly MediaPlayer musicPlayer;
        private StorageFile level1Music;
        private StorageFile level2Music;
        private StorageFile level3Music;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
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
            var titleTextBlock = new TextBlock {Text = "Turtler"};
            this.levelTextBlock = new TextBlock {Text = $"Level: {this.gameManager.CurrentLevel.ToString()}"};

            this.canvas.Children.Add(this.livesTextBlock);
            this.canvas.Children.Add(this.levelTextBlock);
            this.canvas.Children.Add(this.scoreTextBlock);
            this.canvas.Children.Add(titleTextBlock);

            this.livesTextBlock.FontSize = 15;
            Canvas.SetTop(this.livesTextBlock, 10);
            Canvas.SetLeft(this.livesTextBlock, 10);
            this.livesTextBlock.Foreground = HudBrush;

            this.levelTextBlock.FontSize = 15;
            Canvas.SetTop(this.levelTextBlock, 10);
            Canvas.SetLeft(this.levelTextBlock, 80);
            this.levelTextBlock.Foreground = HudBrush;

            this.scoreTextBlock.FontSize = 15;
            Canvas.SetTop(this.scoreTextBlock, 10);
            Canvas.SetLeft(this.scoreTextBlock, this.applicationWidth - 80);
            this.scoreTextBlock.Foreground = HudBrush;

            titleTextBlock.FontSize = 20;
            Canvas.SetTop(titleTextBlock, 10);
            Canvas.SetLeft(titleTextBlock, this.applicationWidth / 2 - 25);
            titleTextBlock.Foreground = TitleBrush;

            this.gameManager.LivesUpdated += this.onLivesUpdated;
            this.gameManager.ScoreUpdated += this.onScoreUpdated;
            this.gameManager.LevelUpdated += this.onLevelUpdated;
            this.gameManager.SlowDownEnded += this.onSlowDownEnded;
            this.gameManager.GameOver += this.onGameOver;
            this.gameManager.PowerUpActivated += this.onPowerUp;

            this.soundPlayer = new MediaPlayer();
            this.musicPlayer = new MediaPlayer();
            this.setUpPlayers();
        }

        #endregion

        #region Methods

        private void onSlowDownEnded(object sender, EventArgs e)
        {
            this.musicPlayer.PlaybackSession.PlaybackRate = 1;
        }

        private async void setUpPlayers()
        {
            await this.setUpSoundPlayer();

            await this.setUpMusicPlayer();
        }

        private async Task setUpMusicPlayer()
        {
            var musicFolder =
                await Package.Current.InstalledLocation.GetFolderAsync(@"View\\Music");
            this.level1Music = await musicFolder.GetFileAsync("Level1Song.wav");
            this.level2Music = await musicFolder.GetFileAsync("Level2Song.wav");
            this.level3Music = await musicFolder.GetFileAsync("Level3Song.wav");
            this.musicPlayer.AutoPlay = true;
            this.musicPlayer.IsLoopingEnabled = true;
            this.setMusic(this.level1Music);
        }

        private void setMusic(IStorageFile music)
        {
            this.musicPlayer.Source = MediaSource.CreateFromStorageFile(music);
        }

        private async Task setUpSoundPlayer()
        {
            var folder =
                await Package.Current.InstalledLocation.GetFolderAsync(@"View\\SoundEffects");
            this.levelWonSound = await folder.GetFileAsync("LevelWonSound.wav");
            this.gameOverSound = await folder.GetFileAsync("GameOverSound.wav");
            this.gameWonSound = await folder.GetFileAsync("GameWonSound.wav");
            this.vehicleCollisionSound = await folder.GetFileAsync("CarCollisionSound.wav");
            this.wallCollisionSound = await folder.GetFileAsync("WallCollisionSound.wav");
            this.waterCollisionSound = await folder.GetFileAsync("WaterDeathSound.wav");
            this.timerDeathSound = await folder.GetFileAsync("TimerDeathSound.wav");
            this.timerPowerupSound = await folder.GetFileAsync("PowerupSound.wav");
            this.scoreMadeSound = await folder.GetFileAsync("ScoreMadeSound.wav");

            this.soundPlayer.AutoPlay = false;
        }

        private void onGameOver(object sender, SoundType sound)
        {
            this.playGameOverSound(sound);
            this.nameBoxLabel.Visibility = Visibility.Visible;
            this.playerNameTextBox.Visibility = Visibility.Visible;
            this.nameSubmitButton.Visibility = Visibility.Visible;
            this.gameOverTextBlock.Visibility = Visibility.Visible;
        }

        private void playGameOverSound(SoundType sound)
        {
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

        private void onScoreUpdated(object sender, EventArgs e)
        {
            this.scoreTextBlock.Text = $"Score: {this.gameManager.TotalScore.ToString()}";
            this.playSoundEffect(this.scoreMadeSound);
        }

        private void onLivesUpdated(object sender, SoundType sound)
        {
            this.livesTextBlock.Text = $"Lives: {this.gameManager.Lives.ToString()}";
            switch (sound)
            {
                case SoundType.VehicleDeath:
                    this.playSoundEffect(this.vehicleCollisionSound);
                    break;
                case SoundType.WallDeath:
                    this.playSoundEffect(this.wallCollisionSound);
                    break;
                case SoundType.WaterDeath:
                    this.playSoundEffect(this.waterCollisionSound);
                    break;
                case SoundType.TimeDeath:
                    this.playSoundEffect(this.timerDeathSound);
                    break;
            }
        }

        private void onLevelUpdated(object sender, EventArgs eventArgs)
        {
            this.levelTextBlock.Text = $"Level: {this.gameManager.CurrentLevel.ToString()}";
            this.playSoundEffect(this.levelWonSound);
            switch (this.gameManager.CurrentLevel)
            {
                case 2:
                    this.setMusic(this.level2Music);
                    break;
                case 3:
                    this.setMusic(this.level3Music);
                    break;
            }
        }

        private void onPowerUp(object sender, SoundType e)
        {
            this.playSoundEffect(this.timerPowerupSound);
            if (e == SoundType.VehiclePowerUp)
            {
                this.musicPlayer.PlaybackSession.PlaybackRate = .5;
            }
        }

        private void playSoundEffect(IStorageFile sound)
        {
            this.soundPlayer.Source = MediaSource.CreateFromStorageFile(sound);
            this.soundPlayer.Play();
        }

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

        private async void NameSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.playerNameTextBox.Text))
            {
                var messageDialog = new MessageDialog("Please enter your name");
                messageDialog.Commands.Add(new UICommand(
                    "OK"));
                messageDialog.CancelCommandIndex = 0;
                await messageDialog.ShowAsync();
            }
            else
            {
                this.nameBoxLabel.Visibility = Visibility.Collapsed;
                this.playerNameTextBox.Visibility = Visibility.Collapsed;
                this.nameSubmitButton.Visibility = Visibility.Collapsed;
                this.gameOverTextBlock.Visibility = Visibility.Collapsed;
                await this.gameManager.SaveHighScore(this.playerNameTextBox.Text);
            }
        }

        #endregion
    }
}