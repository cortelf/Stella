using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stella
{
    public class ControllersFetcher : IControllersFetcher
    {
        public IList<Type> GetControllers()
        {
            var domain = AppDomain.CurrentDomain;
            var assembies = domain.GetAssemblies();
            var types = assembies.SelectMany(x => x.GetTypes());
            var withAttrs = types.Where(x => x.GetCustomAttributes(typeof(BotControllerAttribute), true).Any());
            return withAttrs.ToList();
        }
    }
}
