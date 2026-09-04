using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Core.Forms;

namespace ETG.Data.Forms
{
    public class CompetitionBizformEntry : BizformEntry<CompetitionItem>
    {
        private readonly IBizformEmailSender _emailSender;
        public CompetitionBizformEntry(IBizformEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

    }
}
