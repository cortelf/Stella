using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stella
{
    public interface IControllersFetcher
    {
        IList<Type> GetControllers();
    }
}
