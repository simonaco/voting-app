using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BubbleWar.iOS.TabbedPageCustomRenderer))]
namespace BubbleWar.iOS
{
    public class TabbedPageCustomRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            TabBar.TintColor = Color.Black.ToUIColor();
            TabBar.Translucent = true;
        }
    }
}
