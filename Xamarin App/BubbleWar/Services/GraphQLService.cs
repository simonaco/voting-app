using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BubbleWar
{
    public abstract class GraphQLService : BaseGraphQLService
    {
        const string _url = "https://graphqlplayground.azurewebsites.net/api/VotingApp";
        readonly static Lazy<HttpClient> _clientHolder = new Lazy<HttpClient>(() => CreateHttpClient(TimeSpan.FromSeconds(10)));

        static HttpClient Client => _clientHolder.Value;

        public static async Task<List<Team>> GetTeams()
        {
            var requestContent = @"
                query {
                    teams {
                        name
                        points
                    }
                }";

            var response = await PostObjectToAPI<TeamsResponse>(_url, requestContent).ConfigureAwait(false);
            return response.Data.Teams;
        }

        static HttpClient CreateHttpClient(TimeSpan timeout)
        {
            HttpClient client;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                case Device.Android:
                    client = new HttpClient();
                    break;
                default:
                    client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip });
                    break;
            }

            client.Timeout = timeout;
            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
