using System.Threading.Tasks;
using Refit;

namespace VotingApp
{
    [Headers("Accept-Encoding: gzip",
                "Accept: application/json")]
    interface IGraphQLAPI
    {
        [Post("")]
        Task<GraphQLResponse<TeamsQueryDataResponse>> TeamsQuery([Body] GraphQLRequest request);

        [Post("")]
        Task<GraphQLResponse<IncrementPointsDataResponse>> IncrementPoints([Body] GraphQLRequest request);
    }
}
