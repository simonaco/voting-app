using Android.App;
using Android.Content.PM;
using Android.OS;

using BubbleWar.Shared;
using Plugin.CurrentActivity;

namespace BubbleWar.Droid
{
    [Activity(Label = "BubbleWar", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SyncfusionServices.InitializeSyncfusion();

            LoadApplication(new App());
        }
    }
}