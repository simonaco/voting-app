using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Forms;

namespace BubbleWar
{
    public abstract class GraphQLService : BaseGraphQLService
    {
        #region Constant Fields
        readonly static Lazy<HttpClient> _clientHolder = new Lazy<HttpClient>(() => CreateHttpClient(TimeSpan.FromSeconds(10)));
        #endregion

        #region Properties
        static HttpClient Client => _clientHolder.Value;
        static string BubbleWarUrl => GraphQLSettings.Uri.ToString();
        #endregion

        #region Methods
        public static async Task<List<TeamScore>> GetTeamScoreList()
        {
            var query = @"
                query {
                    teams {
                        name
                        points
                    }
                }";

            var response = await PostObjectToAPI<TeamsResponse>(BubbleWarUrl, query).ConfigureAwait(false);
            return response.Data.Teams;
        }

        public static Task<HttpResponseMessage> VoteForTeam(TeamColor teamType)
        {
            var mutation = @"
                mutation {
                    incrementPoints(id:" + (int)teamType + @") {
                        id
                        name
                        points
                    }
                }";

            return PostObjectToAPI(BubbleWarUrl, mutation);
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
        #endregion

        #region Classes
        class TeamsResponse
        {
            [JsonProperty("data")]
            public TeamsData Data { get; set; }
        }

        class TeamsData
        {
            [JsonProperty("teams")]
            public List<TeamScore> Teams { get; set; }
        }
        #endregion
    }
}
