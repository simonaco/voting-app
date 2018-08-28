using System;

using Xamarin.Forms;

namespace BubbleWar
{
    public static class AppCenterConstants
    {
        const string AppCenterApiKey_iOS = "";
        const string AppCenterApiKey_Android = "";

        public static string AppCenterApiKey => GetAppCenterApiKey();

        static string GetAppCenterApiKey()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return AppCenterApiKey_iOS;
                case Device.Android:
                    return AppCenterApiKey_Android;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
