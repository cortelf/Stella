using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stella
{
    public class HandlerScope : ITelegramHandlerScope
    {
        private readonly Dictionary<Type, object?> _values = new();
        public void Add<T>(T value) where T : class
        {
            _values[typeof(T)] = value;
        }

        public T? Get<T>() where T : class
        {
            return _values[typeof(T)] as T;
        }
    }
}
