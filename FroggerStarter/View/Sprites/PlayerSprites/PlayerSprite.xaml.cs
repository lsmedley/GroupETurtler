// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace FroggerStarter.View.Sprites.PlayerSprites
{
    /// <summary>
    ///     Holds functionality for player sprites.
    /// </summary>
    /// <seealso cref="FroggerStarter.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public partial class PlayerSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        /// </summary>
        public PlayerSprite()
        {
            Resources["BodyColor"] = new SolidColorBrush(Colors.Green);

            this.InitializeComponent();
        }

        #endregion
    }
}