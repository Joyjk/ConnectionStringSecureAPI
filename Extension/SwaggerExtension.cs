using ConnectionStringSecureAPI.Filters;

namespace ConnectionStringSecureAPI.Extension
{
    public static class SwaggerExtension 
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services) 
        {
            services.AddEndpointsApiExplorer();

            //Show Only Specific API

            services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<IncludeSpecificApisFilter>();
            });

            services.AddSwaggerGen(options =>
            {
                // Add Swagger docs for v1
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API v1",
                    Version = "v1",
                    Description = "Documentation for API version 1"
                });

                // Add Swagger docs for v2
                options.SwaggerDoc("v2", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API v2",
                    Version = "v2",
                    Description = "Documentation for API version 2"
                });

                // Add a document filter to group APIs by their version
                options.DocInclusionPredicate((version, apiDesc) =>
                {
                    if (apiDesc.RelativePath?.StartsWith($"api/{version}/") == true)
                    {
                        return true;
                    }
                    return false;
                });
            });

            return services;
        }
    }
}
