using System;

using Xamarin.Forms;

namespace BubbleWar
{
    public static class AppCenterConstants
    {
        public const string VotingButtonTapped = "Voting Button Tapped";
        public const string VotePageAppeared = "Vote Page Appeared";
        public const string SettingsPageAppeared = "Settings Page Appered";
        public const string CreatedByLabelTapped = "Created By Label Tapped";
        public const string Method = "Method";
        public const string LaunchedTwitterProfile = "Launched Twitter Profile";
        public const string TwitterApp = "Twitter App";
        public const string AndroidCustomTabs = "Android Custom Tabs";
        public const string SFSafariViewController = nameof(SFSafariViewController);

        const string AppCenterApiKey_iOS = "b0d6809e-0a39-41fc-a741-7229e865b17e";
        const string AppCenterApiKey_Android = "71aa3c68-65be-4fc5-a602-696f4002a2a1";

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
