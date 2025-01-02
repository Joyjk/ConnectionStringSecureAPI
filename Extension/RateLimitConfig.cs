using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;

namespace ConnectionStringSecureAPI.Extension
{
    public static class RateLimitConfig
    {
        public static void AddCustomRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            // Add memory cache
            services.AddMemoryCache();

            // Configure IP rate limiting options
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.RealIpHeader = "X-Real-IP";
                options.ClientIdHeader = "X-ClientId";
                options.HttpStatusCode = 429;
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "GET:/api/v2/WeatherForecast",
                        Period = "1m",
                        Limit = 1
                    },
                    new RateLimitRule
                    {
                        Endpoint = "POST:/api/login",
                        Period = "10m",
                        Limit = 10
                    }
                };
            });

            // Add the policy and counter stores
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
            services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }
    }
}
