using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.ApplicationModel.Core;
using FroggerStarter.Annotations;
using FroggerStarter.Extensions;
using FroggerStarter.Model;
using FroggerStarter.Utils;

namespace FroggerStarter.ViewModel
{
    /// <summary>
    ///     Binds the high score view to model
    /// </summary>
    public class HighScoreBoardViewModel : INotifyPropertyChanged
    {
        #region Data members

        private ObservableCollection<HighScore> scores;

        private readonly HighScores highScores;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the Scores.
        /// </summary>
        /// <value>
        ///     The Scores.
        /// </value>
        public ObservableCollection<HighScore> Scores
        {
            get => this.scores;
            set
            {
                this.scores = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the sort Scores by score name level button command.
        /// </summary>
        /// <value>
        ///     The sort Scores by score name level button command.
        /// </value>
        public ICommand SortScoresByScoreNameLevelButtonCommand { get; set; }

        /// <summary>
        ///     Gets or sets the sort Scores by name score level button command.
        /// </summary>
        /// <value>
        ///     The sort Scores by name score level button command.
        /// </value>
        public ICommand SortScoresByNameScoreLevelButtonCommand { get; set; }

        /// <summary>
        ///     Gets or sets the sort Scores by level score name button command.
        /// </summary>
        /// <value>
        ///     The sort Scores by level score name button command.
        /// </value>
        public ICommand SortScoresByLevelScoreNameButtonCommand { get; set; }

        /// <summary>
        ///     Gets or sets the restart application button command.
        /// </summary>
        /// <value>
        ///     The restart application button command.
        /// </value>
        public ICommand RestartApplicationButtonCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="HighScoreBoardViewModel" /> class.
        /// </summary>
        public HighScoreBoardViewModel()
        {
            this.highScores = new HighScores();
            this.sortScoresByScoreNameLevel();
            this.SortScoresByScoreNameLevelButtonCommand =
                new RelayCommand(o => this.sortScoresByScoreNameLevel(), null);
            this.SortScoresByNameScoreLevelButtonCommand =
                new RelayCommand(o => this.sortScoresByNameScoreLevel(), null);
            this.SortScoresByLevelScoreNameButtonCommand =
                new RelayCommand(o => this.sortScoresByLevelScoreName(), null);
            this.RestartApplicationButtonCommand = new RelayCommand(o => this.restartApplication(), null);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns>the PropertyChanged event</returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private void sortScoresByScoreNameLevel()
        {
            this.highScores.SortByScoreNameLevel();
            this.Scores = this.highScores.Scores.ToObservableCollection();
        }

        private void sortScoresByNameScoreLevel()
        {
            this.highScores.SortByNameScoreLevel();
            this.Scores = this.highScores.Scores.ToObservableCollection();
        }

        private void sortScoresByLevelScoreName()
        {
            this.highScores.SortByLevelScoreName();
            this.Scores = this.highScores.Scores.ToObservableCollection();
        }

        private async void restartApplication()
        {
            await CoreApplication.RequestRestartAsync("");
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}