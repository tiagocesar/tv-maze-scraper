using System.Threading;
using System.Threading.Tasks;
using Polly;
using RestSharp;

namespace TvMazeScraper.Services.Helpers
{
    public static class RestClientHelpers
    {
        public static async Task<IRestResponse<T>> ExecuteTaskAsyncWithPolicy<T>(this IRestClient client,
            IRestRequest request, CancellationToken cancellationToken, Policy<IRestResponse<T>> policy)
        {
            var policyResult =
                await policy.ExecuteAndCaptureAsync(ct => client.ExecuteTaskAsync<T>(request, ct), cancellationToken);

            return (policyResult.Outcome == OutcomeType.Successful)
                ? policyResult.Result
                : new RestResponse<T>
                {
                    Request = request,
                    ErrorException = policyResult.FinalException
                };
        }
    }
}