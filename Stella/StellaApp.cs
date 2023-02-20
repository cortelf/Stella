using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stella
{
    // Facade
    public class StellaApp
    {
        public IControllerManager ControllerManager { get; set; } = new TelegramControllerManager(new ControllerHandlersFetcher());
        public IControllersFetcher ControllersFetcher { get; set; } = new ControllersFetcher();
        public ContainerBuilder DependencyInjection { get; set; } = new ContainerBuilder();
        public IContainer? DependencyInjectionContainer { get; set; }

        public StellaApp() {}
        public void AddControllers()
        {
            foreach (var c in ControllersFetcher.GetControllers())
            {
                ControllerManager.RegisterController(c);
                DependencyInjection.RegisterType(c);
            }
        }
        public void Build()
        {
            DependencyInjectionContainer = DependencyInjection.Build();
        }
        public async Task ProcessUpdate(Telegram.Bot.Types.Update update)
        {
            if (DependencyInjectionContainer == null)
                throw new ArgumentNullException(nameof(DependencyInjectionContainer));

            await ControllerManager.ProcessUpdate(update, DependencyInjectionContainer);
        }
    }
}
