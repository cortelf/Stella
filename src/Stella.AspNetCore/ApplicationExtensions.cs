using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Stella.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Stella.AspNetCore;

public static class ApplicationExtensions
{
    public static void UseStellaWebhookHandler(this IEndpointRouteBuilder app, [StringSyntax("Route")] string routePattern, string? exceptedSecretToken = null)
    {
        app.MapPost(routePattern, async (HttpContext ctx, IBotUpdateRouter router, [FromHeader(Name = "X-Telegram-Bot-Api-Secret-Token")] string? secretToken) =>
        {
            if (exceptedSecretToken != null)
            {
                if (exceptedSecretToken != secretToken)
                    return;
            }
            using var body = new StreamReader(ctx.Request.Body);
            var postData = await body.ReadToEndAsync();

            var update = JsonSerializer.Deserialize<Update>(postData, JsonBotAPI.Options);
            await router.RouteAsync(update!);
        }).AllowAnonymous();
    }
}