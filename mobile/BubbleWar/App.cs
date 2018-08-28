using Xamarin.Forms;
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
                BarBackgroundColor = Color.White,
                BarTextColor = Color.Black
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
            tabbedPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarItemColor(Color.Gray);
            tabbedPage.On<Xamarin.Forms.PlatformConfiguration.Android>().SetBarSelectedItemColor(Color.Black);

            MainPage = tabbedPage;
        }

        protected override void OnStart()
        {
            base.OnStart();

            AppCenterService.Start();
        }
    }
}
