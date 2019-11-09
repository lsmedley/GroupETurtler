using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FroggerStarter.View.Sprites
{
    /// <summary>
    /// Holds sprite for the time powerup.
    /// </summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class TimePowerUpSprite
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimePowerUpSprite" /> class.
        /// </summary>
        public TimePowerUpSprite()
        {
            this.InitializeComponent();
        }
    }
}
