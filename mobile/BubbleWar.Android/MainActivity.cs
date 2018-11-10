using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

using BubbleWar.Shared;

using Plugin.CurrentActivity;

namespace BubbleWar.Droid
{
    [Activity(Label = "BubbleWar", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState); 
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SyncfusionServices.InitializeSyncfusion();

            LoadApplication(new App());
        }
    }
}