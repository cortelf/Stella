using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Stella
{
    public interface IControllerManager
    {
        Task ProcessUpdate(Update update, IServiceProvider scope);

        void RegisterController(Type controller);
    }
}
