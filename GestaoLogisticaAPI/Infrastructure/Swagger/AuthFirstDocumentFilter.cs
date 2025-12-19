using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GestaoLogisticaAPI.Infrastructure.Swagger;

public class AuthFirstDocumentFilter : IDocumentFilter
{
    private static readonly string[] Prefixes = ["/auth", "/login", "/register"];
        
    public void Apply(OpenApiDocument? swaggerDoc, DocumentFilterContext context)
    {
        if (swaggerDoc?.Paths == null) return;

        // seleciona todos os caminhos que contenham "/auth" (case-insensitive)
        var authPaths = swaggerDoc.Paths
            .Where(kv => Prefixes.Any(p => kv.Key.Contains(p, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (authPaths.Count == 0) return;

        var ordered = new OpenApiPaths();

        // adiciona os endpoints de auth primeiro (mantendo a ordem encontrada)
        foreach (var kv in authPaths)
        {
            ordered.Add(kv.Key, kv.Value);
        }

        // adiciona os demais paths na ordem original, excluindo os já adicionados
        foreach (var kv in swaggerDoc.Paths.Where(kv => authPaths.All(a => a.Key != kv.Key)))
        {
            ordered.Add(kv.Key, kv.Value);
        }

        swaggerDoc.Paths = ordered;
    }
}