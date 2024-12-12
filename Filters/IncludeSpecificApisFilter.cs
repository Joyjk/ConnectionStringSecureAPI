using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace ConnectionStringSecureAPI.Filters
{
    public class IncludeSpecificApisFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathsToKeep = new[] { "/api/v1/ConnectionString/encrypt", "/api/v1/ConnectionString/encrypt-existing", "/api/v2/WeatherForecast" };
            var paths = swaggerDoc.Paths
                .Where(path => pathsToKeep.Contains(path.Key))
                .ToDictionary(path => path.Key, path => path.Value);

            swaggerDoc.Paths = new OpenApiPaths();
            foreach (var path in paths)
            {
                swaggerDoc.Paths.Add(path.Key, path.Value);
            }
        }
    }

}
