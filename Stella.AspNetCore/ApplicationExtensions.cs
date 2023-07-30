using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Stella.Contracts;
using System.Diagnostics.CodeAnalysis;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace Stella.AspNetCore;

public static class ApplicationExtensions
{
    public static void UseStella(this IEndpointRouteBuilder app, [StringSyntax("Route")] string pattern)
    {
        app.MapPost(pattern, async (ctx) =>
        {
            var manager = ctx.RequestServices.GetRequiredService<IControllerManager>();

            using var body = new StreamReader(ctx.Request.Body);
            string postData = await body.ReadToEndAsync();

            var update = JsonConvert.DeserializeObject<Update>(postData);
            await manager.ProcessUpdate(update, ctx.RequestServices);
        });
    }
}