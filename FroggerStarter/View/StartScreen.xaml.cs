using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using FroggerStarter.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FroggerStarter.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartScreen
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="StartScreen" /> class.
        /// </summary>
        public StartScreen()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.replaceStartScreenWith(new GamePage());
        }

        private void ViewHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            this.replaceStartScreenWith(new HighScoreBoard());
        }

        private void replaceStartScreenWith(UIElement element)
        {
            this.canvas.Children.Clear();
            this.canvas.Children.Add(element);
        }

        private async void ResetHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            var messageDialog = new MessageDialog("Are you sure you want to reset the high score board?");

            messageDialog.Commands.Add(new UICommand(
                "Yes",
                resetHighScores));
            messageDialog.Commands.Add(new UICommand(
                "No"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 1;

            await messageDialog.ShowAsync();
        }

        private static async void resetHighScores(IUICommand command)
        {
            var folder = ApplicationData.Current.LocalFolder;

            await folder.CreateFileAsync(GameSettings.HighScoresFilename, CreationCollisionOption.ReplaceExisting);

            var messageDialog = new MessageDialog("High score board has been reset.");

            messageDialog.Commands.Add(new UICommand(
                "OK"));
            messageDialog.CancelCommandIndex = 0;

            await messageDialog.ShowAsync();
        }

        #endregion
    }
}