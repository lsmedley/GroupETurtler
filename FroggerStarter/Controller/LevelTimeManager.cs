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
        ///     Gets the amount of time used.
        /// </summary>
        /// <value>
        ///     The amount of time used.
        /// </value>
        public int TimeUsed { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LevelTimeManager" /> class.
        /// </summary>
        /// <param name="maxTime">The maximum time the player has to score.</param>
        public LevelTimeManager(int maxTime)
        {
            this.MaxTime = maxTime;
            this.TimeUsed = 0;
            this.setUpTimer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when no time is remaining
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
            this.TimeUsed++;
            if (this.TimeUsed >= this.MaxTime)
            {
                this.onTimeUp();
                this.TimeUsed = 0;
            }
        }

        /// <summary>
        ///     Resets the timer.
        /// </summary>
        public void Reset()
        {
            this.TimeUsed = 0;
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

        /// <summary>
        ///     Reduces the time used by the given amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public void ReduceTimeUsedBy(int amount)
        {
            this.TimeUsed -= amount;
        }

        #endregion
    }
}