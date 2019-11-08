using System;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    /// <summary>
    ///     Holds logic and functionality for the level's timer, measuring how much time the player
    ///     has left to score.
    /// </summary>
    public class LevelTimeManager
    {
        #region Data members

        /// <summary>
        ///     The maximum time before the player dies.
        /// </summary>
        public readonly int MaxTime;

        private DispatcherTimer levelTime;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the current time.
        /// </summary>
        /// <value>
        ///     The current time.
        /// </value>
        public int CurrTime { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelTimeManager" /> class.
        /// </summary>
        /// <param name="maxTime">The maximum time the player has to score.</param>
        public LevelTimeManager(int maxTime)
        {
            this.MaxTime = maxTime;
            this.CurrTime = 0;
            this.setUpTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [time up].
        /// </summary>
        public event EventHandler<EventArgs> TimeUp;

        private void setUpTimer()
        {
            this.levelTime = new DispatcherTimer();
            this.levelTime.Tick += this.timerOnTick;
            this.levelTime.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.levelTime.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            this.CurrTime++;
            if (this.CurrTime >= this.MaxTime)
            {
                this.onTimeUp();
                this.CurrTime = 0;
            }
        }

        /// <summary>
        ///     Resets the timer.
        /// </summary>
        public void Reset()
        {
            this.CurrTime = 0;
        }

        /// <summary>
        ///     Pauses the timer.
        /// </summary>
        public void Pause()
        {
            this.levelTime.Stop();
        }

        /// <summary>
        ///     Resumes the timer.
        /// </summary>
        public void UnPause()
        {
            this.levelTime.Start();
        }

        private void onTimeUp()
        {
            this.TimeUp?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// Increases the time left.
        /// </summary>
        /// <param name="increment">The increment.</param>
        public void AddTime(int increment)
        {
            this.CurrTime -= increment;
        }
    }
}