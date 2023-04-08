using Microsoft.Extensions.DependencyInjection;

namespace Stella.Polling;

public static class RegisterControllersExtension
{
    public static void AddBotControllers(this PollingApp app)
    {
        foreach (var c in app.ControllersFetcher.GetControllers())
        {
            app.ControllerManager.RegisterController(c);
            app.Services.AddScoped(c);
        }
    }
}