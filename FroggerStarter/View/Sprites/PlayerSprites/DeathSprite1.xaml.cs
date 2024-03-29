﻿// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.View.Sprites.PlayerSprites
{
    /// <summary>
    ///     Holds functionality for the first frame of the death animation.
    /// </summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class DeathSprite1
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeathSprite1" /> class.
        /// </summary>
        public DeathSprite1()
        {
            var myColor = new SolidColorBrush(Colors.Red);
            Resources["BodyColor"] = myColor;
            this.InitializeComponent();
            this.basePlayerSprite.Resources["BodyColor"] = Resources["BodyColor"];
            this.basePlayerSprite.Resources["SpotColor"] = Resources["SpotColor"];
        }

        #endregion
    }
}