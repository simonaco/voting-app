using Xamarin.Forms;

namespace BubbleWar
{
	public class App : Application
    {
        public App() => MainPage = new TabbedPage
        {
            Children = 
            {
                new VotingPage(),
                new SettingsPage()
            }
        };
    }
}
