using Microsoft.AspNetCore.Mvc;

namespace GestaoLogisticaAPI.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Route("/")]
public class HomeController(IConfiguration config, IHostEnvironment env) : AbstractController
{
    [HttpGet]
    public IActionResult Get()
    {
        var version = config["Api:Version"] ?? "1.0.0";
        var title = config["Api:Title"] ?? null;
        var description = config["Api:Description"] ?? null;
        var authors = config.GetSection("Api:Authors").Get<string[]>() ?? [];
        var docUrl = config["Api:DocumentationUrl"] ?? null;

        var response = new Dictionary<string, object?>();
        
        if (title != null) response["title"] = title;
        if (description != null) response["description"] = description;
        
        response["version"] = version;
        
        if (env.IsDevelopment() && docUrl != null) response["documentationUrl"] = docUrl;
        if (authors.Length > 0) response["authors"] = authors;
        
        return Ok(response);
    }
}