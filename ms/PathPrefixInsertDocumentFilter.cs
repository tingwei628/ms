using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ms
{
    public class PathPrefixInsertDocumentFilter : IDocumentFilter
    {
        private readonly string _pathPrefix;

        public PathPrefixInsertDocumentFilter(string prefix)
        {
            this._pathPrefix = prefix;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths();
            foreach (var (key, value) in swaggerDoc.Paths)
            {
                var newKey = $"{_pathPrefix}{key}";
                updatedPaths[newKey] = value;
            }
            swaggerDoc.Paths = updatedPaths;
        }
    }
}
