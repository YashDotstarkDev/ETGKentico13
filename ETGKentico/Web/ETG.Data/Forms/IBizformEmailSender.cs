using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Base;
using CMS.OnlineForms;

namespace ETG.Data.Forms
{
    public interface IBizformEmailSender
    {
        void SendNotificationEmail(BizFormItem bizformItem, string email);
    }
}
