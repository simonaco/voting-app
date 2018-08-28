using System;

using Xamarin.Essentials;

namespace BubbleWar
{
    abstract class GraphQLSettings
    {
        #region Fields
        static Uri _uri;
        #endregion

        #region Properties
        public static Uri Uri
        {
            get => _uri ?? (_uri = new Uri(Preferences.Get(nameof(Uri), "https://graphqlplayground.azurewebsites.net/api/graphql")));
            set
            {
                _uri = value;
                Preferences.Set(nameof(Uri), value.ToString());
            }
        }
        #endregion
    }
}
