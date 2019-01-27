using System;

using AsyncAwaitBestPractices;

using Xamarin.Essentials;

namespace VotingApp
{
    abstract class GraphQLSettings
    {
        #region Constant Fields
        readonly static WeakEventManager<Uri> _uriChangedEventHandler = new WeakEventManager<Uri>();
        #endregion

        #region Fields
        static Uri _uri;
        #endregion

        #region Events
        public static event EventHandler<Uri> UriChanged
        {
            add => _uriChangedEventHandler.AddEventHandler(value);
            remove => _uriChangedEventHandler.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public static Uri Uri
        {
            get => _uri ?? (_uri = new Uri(Preferences.Get(nameof(Uri), "https://graphqlplayground.azurewebsites.net/api/graphql")));
            set
            {
                _uri = value;
                Preferences.Set(nameof(Uri), value.ToString());
                OnUriChanged(value);
            }
        }

        public static bool ShouldUpdateChartAutomatically
        {
            get => Preferences.Get(nameof(ShouldUpdateChartAutomatically), true);
            set => Preferences.Set(nameof(ShouldUpdateChartAutomatically), value);
        }

        static void OnUriChanged(Uri uri) => _uriChangedEventHandler.HandleEvent(null, uri, nameof(UriChanged));
        #endregion
    }
}
