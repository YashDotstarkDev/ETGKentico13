using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Core.Forms;

namespace ETG.Data.Forms
{
    public class EnquireBizformEntry : BizformEntry<EnquireItem>
    {
        private readonly IBizformEmailSender _emailSender;
        public EnquireBizformEntry(IBizformEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public override void SendNotificationEmail()
        {
            if (!OverrideNotificationEmail.IsNullOrEmpty())
            {
                _emailSender.SendNotificationEmail(_bizformItem, OverrideNotificationEmail);
            }
            else
            {
                base.SendNotificationEmail();
            }
        }
    }
}
