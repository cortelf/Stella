using Microsoft.Extensions.DependencyInjection;
using Stella.Contracts;

namespace Stella.AspNetCore;

public static class ServicesExtensions
{
    public static void AddStella(this IServiceCollection services)
    {
        IControllerManager controllerManager = new TelegramControllerManager(new ControllerHandlersFetcher(),
            new FilterResolver());
        IControllersFetcher controllersFetcher = new ControllersFetcher();
        
        foreach (var c in controllersFetcher.GetControllers())
        {
            controllerManager.RegisterController(c);
            services.AddScoped(c);
        }
        
        services.AddSingleton(controllerManager);
    }
}