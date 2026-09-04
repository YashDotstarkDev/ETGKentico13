using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Web.Validation
{
    public interface IValidator<T>
    {
        bool Validate(T item);
    }
}
