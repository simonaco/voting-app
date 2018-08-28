using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BubbleWar
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            var settingsNavigationPage = new Xamarin.Forms.NavigationPage(new SettingsPage())
            {
                Icon = "Settings",
                Title = "Settings",
                BarBackgroundColor = Xamarin.Forms.Color.White,
                BarTextColor = Xamarin.Forms.Color.Black
            };

            settingsNavigationPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);

            var tabbedPage = new Xamarin.Forms.TabbedPage
            {
                Children =
                {
                    new VotePage(),
                    settingsNavigationPage
                }
            };

            tabbedPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            MainPage = tabbedPage;
        }
    }
}
