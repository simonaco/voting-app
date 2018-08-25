using Xamarin.Forms;

namespace BubbleWar
{
    public abstract class BaseContentPage<TViewModel> : ContentPage where TViewModel : BaseViewModel, new()
    {
        #region Constructors
        protected BaseContentPage() => BindingContext = ViewModel;
        #endregion

        #region Properties
        protected TViewModel ViewModel { get; } = new TViewModel();
        #endregion
    }
}
