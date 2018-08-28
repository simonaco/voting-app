using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BubbleWar
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
    {
        #region Constructors
        protected BaseContentPage()
        {
            BindingContext = ViewModel;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
        #endregion

        #region Properties
        protected TViewModel ViewModel { get; } = new TViewModel();
        #endregion
    }
}
