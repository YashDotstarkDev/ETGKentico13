using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.ContactManagement;

namespace ETG.Data.Services
{
    public interface IConsentService
    {
        string GetConsentText();
        void Agree(ContactInfo contact);
        void Decline(ContactInfo contact);
    }
}
