using System;
using ETG.Data.Destination.Models;
using System.Collections.Generic;

namespace ETG.Data.Destination.Repositories
{
    public interface IDestinationRepository 
    {
        DestinationModel GetDestination(string path);
        bool IsDestinationUnpublished(string path);
    }
}
