using ETG.Data.Models.Common;
using ETG.Data.Models.Forms;
using ETG.Data.Models.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Repositories
{
    public interface INewsletterRepository
    {
        UnsubscribePageModel GetUnsubscribePage(string path);
    }
}
