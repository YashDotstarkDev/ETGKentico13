using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Services
{
    public interface IShareLinksService
    {
        string GetFacebookShareLink();

        string GetTwitterShareLink();

        string GetLinkedInShareLink();

        string GetPinterestShareLink(string imagePath, string description="");

        string GetCopyLink();
    }
}
