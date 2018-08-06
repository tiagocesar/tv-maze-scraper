using Polly;
using RestSharp;

namespace TvMazeScraper.Services.Utils
{
    public static class PolicyExtensions
    {
        public static IRestResponse ExecuteWithPolicy(this IRestClient client, IRestRequest request,
            Policy<IRestResponse> policy)
        {
            // capture the exception so we can push it though the standard response flow.
            var val = policy.ExecuteAndCapture(() => client.Execute(request));
            
            var rr = val.Result;

            if (rr != null) return rr;

            rr = new RestResponse
            {
                Request = request,
                ErrorException = val.FinalException
            };

            return rr;
        }
    }
}