using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ETG.Core.Forms;

namespace ETG.Data.Forms
{
    public class CompetitionAgentBizformEntry : BizformEntry<CompetitionAgentsItem>
    {
        private readonly IBizformEmailSender _emailSender;
        public CompetitionAgentBizformEntry(IBizformEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

    }
}
