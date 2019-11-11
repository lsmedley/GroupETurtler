// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.View.Sprites.PlayerSprites
{
    /// <summary>
    ///     Holds functionality for the second frame of the death animation.
    /// </summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class DeathSprite2
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeathSprite2" /> class.
        /// </summary>
        public DeathSprite2()
        {
            this.InitializeComponent();
            this.basePlayerSprite.Resources["BodyColor"] = this.Resources["BodyColor"];
            this.basePlayerSprite.Resources["SpotColor"] = this.Resources["SpotColor"];
            this.basePlayerSprite.Resources["ShellColor"] = this.Resources["ShellColor"];
        }

        #endregion
    }
}