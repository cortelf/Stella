using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stella
{
    public interface ITelegramHandlerScope
    {
        void Add<T>(T value) where T : class;
        T? Get<T>() where T : class;
    }
}
