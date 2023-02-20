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
        public StellaApp() { }
        public void AddControllers()
        {
            foreach (var c in ControllersFetcher.GetControllers())
            {
                ControllerManager.RegisterController(c);
            }
        }
        public async Task ProcessUpdate(Telegram.Bot.Types.Update update, ITelegramHandlerScope scope)
        {
            await ControllerManager.ProcessUpdate(update, scope);
        }
    }
}
