using System;
using Windows.Foundation;
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
        }

        private void onGameOver(object sender, EventArgs e)
        {
            this.initializeGameOverText();
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
        }

        private void onLivesUpdated(object sender, EventArgs e)
        {
            this.livesTextBlock.Text = $"Lives: {this.gameManager.Lives.ToString()}";
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