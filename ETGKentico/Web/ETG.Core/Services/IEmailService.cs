using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Core.Services
{
    public interface IEmailService
    {
        void SendEmail(string templateCode, string to, Dictionary<string, string> replacements, bool isHtml = true, string from = "",
            string cc = "", string bcc = "", string subject="");
    }
}
