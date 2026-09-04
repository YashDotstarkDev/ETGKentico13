using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETG.Data.Tour.Data;

namespace ETG.Data.Tour.Repositories
{
    public class CruiseTypeRepository : ICruiseTypeRepository
    {
        public KeyValuePair<string, string> GetCruiseType(int cruiseTypeId)
        {
            return TourData.GetCruiseType(cruiseTypeId);
        }

        public KeyValuePair<int, string> GetCruiseType(string cruiseTypeName)
        {
            return TourData.GetCruiseType(cruiseTypeName);
        }
    }
}
