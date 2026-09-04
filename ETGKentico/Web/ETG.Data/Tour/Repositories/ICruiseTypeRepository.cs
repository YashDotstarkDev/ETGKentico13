using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Tour.Repositories
{
    public interface ICruiseTypeRepository
    {
        KeyValuePair<string, string> GetCruiseType(int cruiseTypeId);
        KeyValuePair<int, string> GetCruiseType(string cruiseTypeName);
    }
}
