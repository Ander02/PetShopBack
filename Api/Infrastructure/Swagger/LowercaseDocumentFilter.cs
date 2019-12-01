using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Api.Infrastructure.Swagger
{
    public class LowercaseDocumentFilter : IDocumentFilter
    {
        private static string LowercaseEverythingButParameters(string key)
            => string.Join('/', key.Split('/').Select(x => x.Contains("{") ? x : x.ToLower()));

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var item in swaggerDoc.Paths.ToDictionary(entry => LowercaseEverythingButParameters(entry.Key), entry => entry.Value))
                paths.Add(item.Key, item.Value);

            swaggerDoc.Paths = paths;
        }
    }
}
