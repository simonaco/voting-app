using System.Threading.Tasks;
using Refit;

namespace BubbleWar
{
    [Headers("Accept-Encoding: gzip",
                "Accept: application/json")]
    interface IGraphQLAPI
    {
        [Post("")]
        Task<QueryResponse> Query([Body] QueryRequest request);

        [Post("")]
        Task<MutationResponse> Mutation([Body] MutationRequest request);
    }
}
