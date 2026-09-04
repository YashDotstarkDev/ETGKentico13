using ETG.Data.Models.Global;
using System.Collections.Generic;

namespace ETG.Data.Global
{
    public interface ILinkRepository
    {
        List<LinkModel> GetLinks(string path);
    }
}
