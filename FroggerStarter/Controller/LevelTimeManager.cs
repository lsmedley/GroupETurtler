using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FroggerStarter.Controller
{
    /// <summary>
    /// Holds logic and functionality for the player's timer.
    /// </summary>
    class LevelTimeManager
    {
        public event EventHandler<EventArgs> TimeUp;
        /// <summary>
        /// The maximum time before the player dies.
        /// </summary>
        public readonly int MaxTime;
        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>
        /// The current time.
        /// </value>
        public int CurTime { get; private set; }
        private DispatcherTimer levelTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelTimeManager"/> class.
        /// </summary>
        /// <param name="maxTime">The maximum time.</param>
        public LevelTimeManager(int maxTime)
        {
            this.MaxTime = maxTime;
            this.CurTime = 0;
            this.setUpTimer();
        }

        private void setUpTimer()
        {
            this.levelTime = new DispatcherTimer();
            this.levelTime.Tick += this.timerOnTick;
            this.levelTime.Interval = new TimeSpan(0, 0, 0, 1, 0);
            this.levelTime.Start();
        }

        private void timerOnTick(object sender, object e)
        {
            this.CurTime++;
            if (this.CurTime >= this.MaxTime)
            {
                this.onTimeUp();
                this.CurTime = 0;
            }
        }

        /// <summary>
        /// Resets the timer.
        /// </summary>
        public void Reset()
        {
            this.CurTime = 0;
        }

        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void Pause()
        {
            this.levelTime.Stop();
        }

        /// <summary>
        /// Resumes the timer.
        /// </summary>
        public void UnPause()
        {
            this.levelTime.Start();
        }

        private void onTimeUp()
        {
            this.TimeUp?.Invoke(this, EventArgs.Empty);
        }
    }
}
