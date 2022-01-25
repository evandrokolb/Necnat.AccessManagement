using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Necnat.Server.Helpers.Swagger
{
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths();

            foreach (var entry in swaggerDoc.Paths)
            {
                updatedPaths.Add(
                    entry.Key.Replace("v{version}", $"v{swaggerDoc.Info.Version}"),
                    entry.Value);
            }

            swaggerDoc.Paths = updatedPaths;
        }
    }
}
