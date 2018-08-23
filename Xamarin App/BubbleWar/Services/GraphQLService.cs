using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BubbleWar
{
    public abstract class GraphQLSerqvice : BaseGraphQLService
    {
        const string _bubbleWarUrl = "https://graphqlplayground.azurewebsites.net/api/VotingApp";

        readonly static Lazy<HttpClient> _clientHolder = new Lazy<HttpClient>(() => CreateHttpClient(TimeSpan.FromSeconds(10)));

        static HttpClient Client => _clientHolder.Value;

        public static async Task<List<Team>> GetTeams()
        {
            var query = @"
                query {
                    teams {
                        name
                        points
                    }
                }";

            var response = await PostObjectToAPI<TeamsResponse>(_bubbleWarUrl, query).ConfigureAwait(false);
            return response.Data.Teams;
        }

        public static Task<HttpResponseMessage> VoteForRedTeam()
        {
            var mutation = @"
                mutation {
                    incrementPoints(id:1) {
                        id
                        name
                        points
                    }
                }";

            return PostObjectToAPI(_bubbleWarUrl, mutation);
        }

        public static Task<HttpResponseMessage> VoteForGreenTeam()
        {
            var mutation = @"
                mutation {
                    incrementPoints(id:2) {
                        id
                        name
                        points
                    }
                }";

            return PostObjectToAPI(_bubbleWarUrl, mutation);
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
