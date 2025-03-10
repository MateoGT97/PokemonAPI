
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;

namespace PokemonAPI.Policies;
public static class HttpCLientPolicies
{
    /// <summary>
    /// Defines a retry policy to be used on the named HTTPClient
    /// </summary>
    /// <returns>Defined retry policy</returns>
    public static IAsyncPolicy<HttpResponseMessage> DefineRetryPolicy()
    {
        return HttpPolicyExtensions.HandleTransientHttpError()
            .OrResult(message => message.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay: TimeSpan.FromSeconds(5), retryCount: 10));
    }

    /// <summary>
    /// Defines a Circuit Breaker Policy to be used on the named HTTPClient
    /// </summary>
    /// <returns>Defined Circuit breaker policy</returns>
    public static IAsyncPolicy<HttpResponseMessage> DefineCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions.HandleTransientHttpError()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(45));
    }
}